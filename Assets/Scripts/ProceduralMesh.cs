using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralMesh : MonoBehaviour {

	// Use this for initialization
	public int length = 10;
	public int width = 10;
	public float spacing = 1.0f;
	public MeshFilter meshFilter = null;
	public Mesh mesh = null;
	
	void Start () 
	{
		meshFilter = gameObject.AddComponent<MeshFilter>();
		mesh = meshFilter.mesh;
		
		GenerateMesh();
	}
	
	void GenerateMesh()
	{
		List<Vector3[]> vertices = new List<Vector3[]>();
		List<int> triangles = new List<int>();
		List<Vector2> UVs = new List<Vector2>();
		
		for(int z = 0; z < width; z++)
		{
			vertices.Add (new Vector3[width]);
			
			for(int x = 0; x < width; x++)
			{
				Vector3 point = new Vector3();
				point.x = x * spacing;
				point.y = 0;
				point.z = z * spacing;
				
				vertices[z][x] = point;
				UVs.Add (new Vector2(x,z));
				
				if( x-1 <=0 || z <= 0 || x >=width)
				continue;
				triangles.Add (x + z*width);
				triangles.Add (x + (z-1)*width);
				triangles.Add ((x-1) + (z-1)*width);
				
				if (x-1 <= 0 || z <= 0)
				{
					continue;
				}
				triangles.Add(x + z*width);
				triangles.Add((x) + (z-1)*width);
				triangles.Add((x-1) + z*width);
			}
		}
		
		
		
		Vector3[] verticesList = new Vector3[width * width];
		int i = 0;
		foreach(Vector3[] v in vertices)
		{
			v.CopyTo(verticesList, i * width);
			i++;
		}
		mesh.vertices = verticesList;
		mesh.triangles = triangles.ToArray();
		mesh.uv = UVs.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
		GenerateMesh();
	}
}
