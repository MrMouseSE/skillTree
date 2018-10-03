using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class MainSkillMaterial : MonoBehaviour
{

	public Texture2D _Texture2D;
	public Color _Color;

	private int x_pos;
	private int y_pos;
	private float tilling;
	// Use this for initialization
	void Start ()
	{
		x_pos = Random.Range(1, 9);
		y_pos = Random.Range(1, 9);
		tilling = 1.0f / 9.0f;
		
		var renderer = GetComponent<Renderer>();
		MaterialPropertyBlock matblock = new MaterialPropertyBlock();
		if (_Texture2D != null)
		{
			matblock.SetTexture("_MainTex", _Texture2D);
		}
		matblock.SetVector("_MainTex_ST", new Vector4(tilling,tilling,tilling*x_pos,tilling*y_pos));
		matblock.SetColor("_Color",_Color);
		renderer.SetPropertyBlock(matblock);
	}

	void SetColorToActive(Color color)
	{
		_Color = color;
		var renderer = GetComponent<Renderer>();
		MaterialPropertyBlock matblock = new MaterialPropertyBlock();
		if (_Texture2D != null)
		{
			matblock.SetTexture("_MainTex", _Texture2D);
		}
		matblock.SetVector("_MainTex_ST", new Vector4(tilling,tilling,tilling*x_pos,tilling*y_pos));
		matblock.SetColor("_Color",_Color);
		renderer.SetPropertyBlock(matblock);
	}

	public void SwitchToActive(Transform skill)
	{
		var renderers = GetComponentsInChildren<Renderer>();
		if (transform.name.Contains("Root") || transform.parent.GetComponent<MainSkillMaterial>()._Color == Color.white)
		{
			SetColorToActive(Color.white);

			foreach (var renderer in renderers)
			{
				var parrent = renderer.transform.parent;
				if (parrent == skill)
				{
					renderer.enabled = true;

				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
