using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TestMesh2 : MonoBehaviour 
{
	private Mesh mesh = null;
	private MeshFilter meshFilter = null;
	
	public int length = 1;
	public int width = 1;
	
	private List<int> indices;
    private List<Vector3> verticesList;
	private List<Vector2> uvList;
	
	public float height = 1.0f;
    public float faceWidth = 0.25f;
    public float faceLength = 0.25f;

    private float currentFaceWidth = 0.25f;
    private float currentFaceLength = 0.25f;
    private float currentLength = 1;
    private float currentWidth = 1;
	
	public  GameObject start;
	public GameObject middle;
	public GameObject end;
	
	public bool turnOnCurve = false;

    private List<Vector3> midVertices;

    public static Vector3 startPosition;
    public static Vector3 middlePosition;
    public static Vector3 endPosition;

    public static bool updateVertices;
    private Vector3[] oringinalVertices;

    float fMidpointWidth;
    float fMidPointLength;

    //VShapeMesh vMesh = new VShapeMesh();
    ShapeMesh vMesh = new VShapeMesh();
    CircleMesh cMesh = new CircleMesh();

    public float lerpSpeed = 0.1f;
    private bool changeTriggerBool = false;
    private MeshCollider meshCollider;


	
	void BuildQuad(Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow)
	{
		verticesList.Add (position);
		uvList.Add (uv);
		
		if(buildTriangles)
		{
			int baseIndex = verticesList.Count-1;
			
			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex - vertsPerRow;
			int index3 = baseIndex - vertsPerRow - 1;
			indices.Add (index0); indices.Add (index2); indices.Add (index1);
			indices.Add (index2); indices.Add (index3); indices.Add(index1);
		}
	}

    void BuildMesh()
    {
        verticesList = new List<Vector3>();
        uvList = new List<Vector2>();
        indices = new List<int>();
        midVertices = new List<Vector3>();

        for (int i = 0; i <= length; i++)
        {
            float z = faceLength * (float)i;
            float v = (1.0f / width) * i;
            for (int j = 0; j <= width; j++)
            {
                float x = faceWidth * (float)j;
                Vector3 pos = new Vector3(x, Random.Range(0.0f, height), z);
                float u = (1.0f / width) * j;
                Vector2 uv = new Vector2(u, v);
                bool buildTriangles = i > 0 && j > 0;
                BuildQuad(pos, uv, buildTriangles, width + 1);
            }
        }

        
        fMidpointWidth = ((float)width * faceWidth) / 2;
        fMidPointLength = ((float)length * faceLength) / 2;

        for (int i = 0; i < verticesList.Count; i++)
        {
            verticesList[i] = new Vector3(verticesList[i].x - fMidpointWidth*2, verticesList[i].y, verticesList[i].z);
        }

        for (int i = 0; i <= length; i++)
        {
            midVertices.Add(new Vector3(0, 0, i * faceLength));
        }

        start.transform.position = new Vector3(fMidpointWidth, 0, 0);
         end.transform.position = new Vector3(fMidpointWidth, 0, faceLength * (float)(length));
         middle.transform.position = new Vector3(fMidpointWidth, 0, fMidPointLength);

         startPosition = start.transform.position;
         middlePosition = middle.transform.position;
         endPosition = end.transform.position;

        mesh.Clear();
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        Debug.Log("Mesh rebuiled");
        oringinalVertices = verticesList.ToArray();

        if (GetComponent<MeshCollider>() != null)
        {
            meshCollider = gameObject.GetComponent<MeshCollider>();
        }
        else
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
    }

	// Use this for initialization
	void Start () 
	{
        Application.targetFrameRate = 60;
		meshFilter = gameObject.AddComponent<MeshFilter>();
		mesh = meshFilter.mesh;

        ControlScript.AddShapeChange(StartVMesh);
       

	}

    void UpdateVerticesList()
    {
        float t = 0;
        Vector3 p0 = start.transform.position;
        Vector3 p1 = middle.transform.position;
        Vector3 p2 = end.transform.position;
        Vector3 position;

        for (int i = 0; i < midVertices.Count; i++)
        {
            t = i / (midVertices.Count - 1.0f);
            position = (1.0f - t) * (1.0f - t) * p0
                + 2.0f * (1.0f - t) * t * p1
                    + t * t * p2;
            if (turnOnCurve)
                midVertices[i] = position;
        }

        Vector3[] newList = new Vector3[verticesList.Count];
        int counter = 0;
        for (int i = 0; i <= length; i++)
        {
            for (int j = 0; j <= width; j++)
            {
                Vector3 newVector = new Vector3(verticesList[counter].x + midVertices[i].x,
                                                verticesList[counter].y + midVertices[i].y,
                                                verticesList[counter].z);
                newList[counter] = newVector;
                counter++;
            }
        }
        vMesh.CalculateFinalVertices(newList);
        if (turnOnCurve)
        {
            mesh.vertices = newList;
        }
        else
        {
            mesh.vertices = verticesList.ToArray();
        }
        updateVertices = false;
    }
	
	// Update is called once per frame
	void LateUpdate () 
	{
        if (faceLength != currentFaceLength || faceWidth != currentFaceWidth || length != currentLength || width != currentWidth)
        {
            currentFaceWidth = faceWidth;
            currentFaceLength = faceLength;
            currentWidth = width;
            currentLength = length;
            BuildMesh();
            vMesh.Initialize(verticesList.ToArray(), verticesList.Count, length, width);
            UpdateVerticesList();
        }

        if (updateVertices)
        {
            UpdateVerticesList();
           // meshCollider.enabled = false;
           // meshCollider.enabled = true;

        }
        if (!updateVertices && changeTriggerBool)
        {
            vMesh.SetChangeSpeed(lerpSpeed);
            vMesh.Update();
            if (vMesh.isChangeCompleted()&& changeTriggerBool)
            {
                changeTriggerBool = false;
                ControlScript.shapeChanging = false;
            }
        }
        mesh.vertices = vMesh.GetFinalVertices();
	}

    void FixedUpdate()
    {
        
        
    }

    public void StartVMesh()
    {
        Debug.Log("in start mesh");
        if (vMesh.isChangeCompleted())
        {
            ControlScript.shapeChanging = true;
            vMesh.StartChange();
            changeTriggerBool = true;
            Debug.Log("starting change");
        }
        else
        {
            Debug.Log("change is not completed yet");
        }
    }
}
