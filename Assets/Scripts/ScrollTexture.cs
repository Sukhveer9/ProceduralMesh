using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour 
{
	public Vector2 uvOffset = Vector2.zero;
	public float offsetY = 0.5f;
	
	public GameObject speedGUI;
	private GUIText speedGUIText;
	
	private GUILayer guiLayer;
	// Use this for initialization
	void Start () 
	{
		//renderer = GetComponent<MeshRenderer>();
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex", uvOffset);
		speedGUIText = speedGUI.GetComponent<GUIText>();
		guiLayer = Camera.main.GetComponent<GUILayer>();
	}
	
	void Update()
	{
		speedGUIText.text = offsetY.ToString();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//float v = 0;
		
		GUIElement gElement;
		
		if(guiLayer.HitTest(Input.mousePosition) && Input.GetMouseButton(0))
		{
			gElement = guiLayer.HitTest(Input.mousePosition);
			if(gElement.GetComponent<GUIText>().text == "+")
				offsetY+= 0.01f;
			else if(gElement.GetComponent<GUIText>().text == "-")
				offsetY-= 0.01f;
		}
		
		for(uint i = 0; i < Input.touches.Length; i++)
		{
			Touch touch = Input.touches[i];
			if(touch.phase == TouchPhase.Stationary && guiLayer.HitTest(touch.position))
			{
				gElement = guiLayer.HitTest(touch.position);
				if(gElement.GetComponent<GUIText>().text == "+")
					offsetY+= 0.01f;
				else if(gElement.GetComponent<GUIText>().text == "-")
					offsetY-= 0.01f;
			}
		}
		
		float v = offsetY * Time.deltaTime % 1;
		uvOffset += new Vector2(0.0f, v);// * Time.deltaTime;
		GetComponent<Renderer>().material.SetTextureOffset("_MainTex",uvOffset);
	}
}
