using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMesh 
{
    private Mesh m_Mesh = null;

    private List<int> m_Indices;
    private List<Vector3> m_VerticesList;
    private List<Vector2> m_uvList;
    private List<Vector3> m_midVertices;

    private Vector3[] m_OringinalVertices;

    public float m_fFaceWidth = 0.25f;
    public float m_fFaceLength = 0.25f;

    private float m_fCurrentFaceWidth = 0.25f;
    private float m_fCurrentFaceLength = 0.25f;
    private float m_fCurrentLength = 1;
    private float m_fCurrentWidth = 1;

    float m_fMidpointWidth;
    float m_fMidPointLength;

    public int m_iLength = 1;
    public int m_iWidth = 1;


    public GameObject m_start;
    public GameObject m_middle;
    public GameObject m_end;

    public static Vector3 m_StartPosition;
    public static Vector3 m_MiddlePosition;
    public static Vector3 m_endPosition;

    public GameMesh()
    {

    }

    public void Initialize()
    {
        if (m_Mesh == null)
        {
            m_Mesh = new Mesh();
        }
    }

    void BuildQuad(Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow)
    {
        m_VerticesList.Add(position);
        m_uvList.Add(uv);

        if (buildTriangles)
        {
            int baseIndex = m_VerticesList.Count - 1;

            int index0 = baseIndex;
            int index1 = baseIndex - 1;
            int index2 = baseIndex - vertsPerRow;
            int index3 = baseIndex - vertsPerRow - 1;
            m_Indices.Add(index0); m_Indices.Add(index2); m_Indices.Add(index1);
            m_Indices.Add(index2); m_Indices.Add(index3); m_Indices.Add(index1);
        }
    }


    void BuildMesh()
    {
        m_VerticesList = new List<Vector3>();
        m_uvList = new List<Vector2>();
        m_Indices = new List<int>();
        m_midVertices = new List<Vector3>();

        for (int i = 0; i <= m_iLength; i++)
        {
            float z = m_fFaceLength * (float)i;
            float v = (1.0f / m_iWidth) * i;
            for (int j = 0; j <= m_iWidth; j++)
            {
                float x = m_fFaceWidth * (float)j;
                Vector3 pos = new Vector3(x, Random.Range(0.0f, 0), z);
                float u = (1.0f / m_iWidth) * j;
                Vector2 uv = new Vector2(u, v);
                bool buildTriangles = i > 0 && j > 0;
                BuildQuad(pos, uv, buildTriangles, m_iWidth + 1);
            }
        }

        m_OringinalVertices = new Vector3[m_VerticesList.Count];


        m_fMidpointWidth = ((float)m_iWidth * m_fFaceWidth) / 2;
        m_fMidPointLength = ((float)m_iLength * m_fFaceLength) / 2;

        for (int i = 0; i < m_VerticesList.Count; i++)
        {
            m_VerticesList[i] = new Vector3(m_VerticesList[i].x - m_fMidpointWidth * 2, m_VerticesList[i].y, m_VerticesList[i].z);
        }

        for (int i = 0; i <= m_iLength; i++)
        {
            m_midVertices.Add(new Vector3(0, 0, i * m_fFaceLength));
        }

        m_start.transform.position = new Vector3(m_fMidpointWidth, 0, 0);
        m_end.transform.position = new Vector3(m_fMidpointWidth, 0, m_fFaceLength * (float)(m_iLength));
        m_middle.transform.position = new Vector3(m_fMidpointWidth, 0, m_fMidPointLength);

        m_StartPosition = m_start.transform.position;
        m_MiddlePosition = m_middle.transform.position;
        m_endPosition = m_end.transform.position;

        m_Mesh.Clear();
        m_Mesh.vertices = m_VerticesList.ToArray();
        m_Mesh.triangles = m_Indices.ToArray();
        m_Mesh.uv = m_uvList.ToArray();
        m_Mesh.RecalculateBounds();
        m_Mesh.RecalculateNormals();
        Debug.Log("Mesh rebuiled");
        for (int i = 0; i < m_VerticesList.Count; i++)
        {
            m_OringinalVertices[i] = m_VerticesList[i];
        }
           // m_OringinalVertices = m_VerticesList.ToArray();

       /* if (GetComponent<MeshCollider>() != null)
        {
            meshCollider = gameObject.GetComponent<MeshCollider>();
        }
        else
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }*/
    }

}
