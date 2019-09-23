using UnityEngine;
using System.Collections;
using System;

public class HexagonMesh
{
    public HexagonMesh()
    {
    }

    private Vector3[] m_OriginalVertices;
    private Vector3[] m_CalculatedVertices;
    private Vector3[] m_UpdatedVertices;


    private Vector3[][] m_VerticesGroup;
    private int m_iVerticesPerSide;

    int m_iWidth;
    int m_iLength;

    public void Initialize(Vector3[] verticesList, int iNumOfVertices, int iLength, int iWidth)
    {
        iWidth += 1;
        m_iWidth = iWidth;
        if (iWidth % 6 == 1)
        {
            //Debug.LogError("ERROR: Width vertices not even");
            //throw new Exception();
        }

        m_OriginalVertices = new Vector3[iNumOfVertices];
        m_OriginalVertices = verticesList;

        m_CalculatedVertices = new Vector3[iNumOfVertices];

        int iVerticesPerSide = iWidth / 6;
        m_VerticesGroup = new Vector3[6][];

        int iCounter = 0;
        for (int i = 0; i < 6; i++)
        {
            m_VerticesGroup[i] = new Vector3[iVerticesPerSide];
            for (int j = 0; j < iVerticesPerSide; j++)
            {
                m_VerticesGroup[i][j] = m_OriginalVertices[iCounter];
                iCounter++;
            }
        }
    }

    public void CalculateFinalVertices(Vector3[] verticesList, Vector3[] midVertices)
    {
        Vector3 newVector = Vector3.zero;
        Vector3 previousVector = Vector3.zero;
        float x;
        float y;
        float z;
        int counter = 0;

       
        for (int i = 0; i < verticesList.Length; i++)
        {
            m_OriginalVertices[i] = verticesList[i];
            m_CalculatedVertices[i] = verticesList[i];
        }

        int iVerticesPerSide = m_iWidth / 6;
        m_VerticesGroup = new Vector3[6][];

        int iCounter = 0;
        for (int i = 0; i < 6; i++)
        {
            m_VerticesGroup[i] = new Vector3[iVerticesPerSide];
            for (int j = 0; j < iVerticesPerSide; j++)
            {
                m_VerticesGroup[i][j] = m_OriginalVertices[iCounter];
                iCounter++;
            }
        }

        //SIDE 2
        counter = 1;
        while (counter < verticesList.Length)
        {
            newVector = verticesList[counter];
            previousVector = verticesList[counter + 1];
            x = previousVector.x;
            y = newVector.y + 0.1f;
            z = newVector.z;

            m_CalculatedVertices[counter] = new Vector3(x, y, z);

            counter += 7;
        }

        // SIDE 3
        counter = 0;
        while (counter < verticesList.Length)
        {
            newVector = verticesList[counter];
            previousVector = verticesList[counter + 1];
            x = m_CalculatedVertices[counter + 3].x;
            y = newVector.y + 0.1f;//(newVector.x - previousVector.x * Mathf.Sin(60 * Mathf.Deg2Rad));
            z = newVector.z;

            m_CalculatedVertices[counter] = new Vector3(x, y, z);

            counter += 7;
        }
        

        // SIDE 5
        counter = 5;
        while (counter < verticesList.Length)
        {
            newVector = verticesList[counter];
            previousVector = verticesList[counter - 1];
            x = previousVector.x;
            y = newVector.y + 0.1f;//(newVector.x - previousVector.x * Mathf.Sin(60 * Mathf.Deg2Rad));
            z = newVector.z;

            m_CalculatedVertices[counter] = new Vector3(x, y, z);

            counter += 7;
        }

        // SIDE 6
        counter = 6;
        while (counter < verticesList.Length)
        {
            newVector = verticesList[counter];
            previousVector = verticesList[counter - 1];
            x = m_CalculatedVertices[counter-3].x;
            y = newVector.y + 0.1f;//(newVector.x - previousVector.x * Mathf.Sin(60 * Mathf.Deg2Rad));
            z = newVector.z;

            m_CalculatedVertices[counter] = new Vector3(x, y, z);

            counter += 7;
        }

    }

    public Vector3[] GetFinalVertices()
    {
        return m_CalculatedVertices;
    }
}
