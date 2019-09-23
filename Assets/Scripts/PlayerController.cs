using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject end;
	public float x;
	public float y;
    public float rate;
    public float maxCap;
	
	private GUILayer guiLayer;
	public Vector3 endInitialPosition;
	
	// Use this for initialization
	void Start () 
	{
        rate = 0.03f;
        maxCap = 3.0f;
		x = end.transform.position.x;
		y = end.transform.position.y;
		endInitialPosition = end.transform.position;

		guiLayer = Camera.main.GetComponent<GUILayer>();
	}
	
	// Update is called once per frame
	public void Update () 
	{
	
	}

    public void onClick()
    {
        ControlScript.ChangeTrigger();
    }
	
	void FixedUpdate()
	{
        if (ControlScript.shapeChanging)
            return;
        x = end.transform.position.x;
        y = end.transform.position.y;
		if(Input.GetAxis("Horizontal") != 0)
		{
            x += (Input.GetAxis("Horizontal") * rate);
			//transform.position = new Vector3(x, transform.position.y, tra
		}
		
		if(Input.GetAxis("Vertical") != 0)
		{
            y += (Input.GetAxis("Vertical") * rate);
            if (y >= maxCap) y = maxCap;
		}

        if (Input.GetKeyDown(KeyCode.N) && !ControlScript.shapeChanging)
        {
            ControlScript.ChangeTrigger();
        }
		
		if(guiLayer.HitTest(Input.mousePosition) && Input.GetMouseButton(0))
		{
			GUIElement gElement = guiLayer.HitTest(Input.mousePosition);
			if(gElement)
			{
				if(gElement.GetComponent<GUIText>().text == "RIGHT" || gElement.GetComponent<GUIText>().text == "LEFT")
                    x += (rate) * (gElement.GetComponent<GUIText>().text == "RIGHT" ? 1 : -1);
				
				else if(gElement.GetComponent<GUIText>().text == "TOP" || gElement.GetComponent<GUIText>().text == "BOTTOM")
                    y += (rate) * (gElement.GetComponent<GUIText>().text == "TOP" ? 1 : -1);
					
				else if(gElement.GetComponent<GUIText>().text == "Reset")
				{
					x = TestMesh2.endPosition.x;//endInitialPosition.x;
                    y = TestMesh2.endPosition.y;//endInitialPosition.y;
				}

                else if (gElement.GetComponent<GUIText>().text == "Change")
                {
                    ControlScript.ChangeTrigger();
                }

                if (y >= maxCap) y = maxCap;
                if (x >= maxCap) x = maxCap;

                if (x < 0 && x < maxCap * -1) x = maxCap * -1;
                if (y < 0 && y < maxCap * -1) y = maxCap * -1;
			}
            TestMesh2.updateVertices = true;
		}
		
		for(uint i = 0; i < Input.touches.Length; i++)
		{
			Touch touch = Input.touches[i];
			if(touch.phase == TouchPhase.Stationary && guiLayer.HitTest(touch.position))
			{
				GUIElement gElement = guiLayer.HitTest(touch.position);
				if(gElement)
				{
					if(gElement.GetComponent<GUIText>().text == "RIGHT" || gElement.GetComponent<GUIText>().text == "LEFT")
                        x += (rate) * (gElement.GetComponent<GUIText>().text == "RIGHT" ? 1 : -1);
					
					else if(gElement.GetComponent<GUIText>().text == "TOP" || gElement.GetComponent<GUIText>().text == "BOTTOM")
                        y += (rate) * (gElement.GetComponent<GUIText>().text == "TOP" ? 1 : -1);
					
					else if(gElement.GetComponent<GUIText>().text == "Reset")
					{
						x = endInitialPosition.x;
						y = endInitialPosition.y;
					}
				}
                TestMesh2.updateVertices = true;
			}
		}
		
		end.transform.position = new Vector3(x,y,end.transform.position.z);
	}
}
