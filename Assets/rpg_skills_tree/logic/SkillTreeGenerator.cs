using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillTreeGenerator : MonoBehaviour
{
	public GameObject _Root;
	public GameObject _MainSkill;
	public Color _MainSkillColor;
	public GameObject _PassiveSkill;
	public Color _PassiveSkillColor;
	public GameObject _Link;
	public Color _LinkColor;
	public Vector2 _MaxBranches;
	public float _SkillDistance = 4;
	public int _TreeSteps = 2;
	public Vector2 _maxPassiveSkill;
	public float _SubSkillDistance = 2;

	private List<Transform> _NewSkillsList;
	private GameObject _RootObject;
	private const float Pi = 3.1415926f; 
	
	void Start()
	{
		_NewSkillsList = new List<Transform>();
		_RootObject = Instantiate(_Root);
		_NewSkillsList.Add(_RootObject.transform);		
		
		for (int i = 0; i < _TreeSteps; i++)
		{
			var buffer = new List<Transform>();

			foreach (var skill in _NewSkillsList)
			{
				buffer = SkillPlacer(skill,buffer);
			}
			_NewSkillsList = buffer;
		}
	}

	List<Transform> SkillPlacer(Transform currPos,List<Transform> buffer)
	{
		var branches = (Random.Range(_MaxBranches.x, _MaxBranches.y));
		var radialStep = 2 * Pi / branches;
		for (int i = 0; i < branches; i++)
		{
			var place = SkillPlacePosition(_MainSkill, currPos,_SkillDistance, 1, radialStep, i, 0, _MainSkillColor);
			if (place != null)
			{
				SubSkillPlacer(place);
				buffer.Add(place);
			}
		}
		return buffer;
	}

	void SubSkillPlacer(Transform place)
	{
		var subSkillCount = Random.Range(_maxPassiveSkill.x, _maxPassiveSkill.y);
		var radialStep = Pi / 4;
		for (int i = 0; i < subSkillCount; i++)
		{
			var multiplyer = Mathf.CeilToInt(radialStep * i / (2 * Pi)+0.1f);
			var offset = radialStep * 0.5f * (multiplyer-1);
			SkillPlacePosition(_PassiveSkill, place, _SubSkillDistance , _SubSkillDistance/2  * (1 + multiplyer), radialStep, i, offset, _PassiveSkillColor);
		}
	}
	
	Transform SkillPlacePosition(GameObject SkillObject, Transform currPos, float distance, float multiply, 
		float radialStep, int i, float offset,Color skillColor)
	{
		var width = 0.1f;
		var rotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * (radialStep*i - offset) + 45.0f, 0.0f);
		Vector3 position = new Vector3();
		position.x = currPos.position.x + (float) Math.Cos(radialStep*i+offset)*distance*multiply;
		position.z = currPos.position.z + (float) Math.Sin(radialStep*i+offset)*distance*multiply;
		
		if (SkillObject == _MainSkill)
		{
			width = 0.2f;
			rotation = Quaternion.Euler(0.0f,180.0f,0.0f);
			if (Physics.OverlapSphere(position, _SkillDistance-0.1f).Length>0)
			{
				return null;
			}
		}
		
		position.y = -width;
		var Skill = Instantiate(SkillObject);
		Skill.transform.position = position;
		Colorizer(Skill, skillColor);
		Skill.transform.rotation = rotation;
		Skill.transform.SetParent(currPos);
		GenerateLink(currPos,position,width);
		
		return Skill.transform;
	}

	void GenerateLink(Transform startPos, Vector3 endPos, float width)
	{
		var link = Instantiate(_Link).GetComponent<LineRenderer>();
		link.SetPosition(0,new Vector3(startPos.position.x, -5.0f-width, startPos.position.z));
		link.SetPosition(1,new Vector3(endPos.x, -5.0f-width, endPos.z));
		link.SetWidth(width,width);
		link.transform.SetParent(startPos);
		var color = new Color(_LinkColor.r - width*1.99f, _LinkColor.g - width*1.99f, _LinkColor.b - width*1.99f);
		Colorizer(link.gameObject, color);
	}
	
	void Colorizer(GameObject item, Color skillColor)
	{
		var skillMaterial = item.GetComponent<Renderer>();
		MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
		propertyBlock.SetColor("_Color", skillColor);
		skillMaterial.SetPropertyBlock(propertyBlock);
	}
	
	void Update()
	{
		if (Input.anyKeyDown)
		{
			DestroyImmediate(_RootObject);
			Start();
		}
	}
}
