using UnityEngine;
using System.Collections;

public class CircleMesh : ShapeMesh
{

    private float m_fAngle;
    private float m_fFaceWidth;

    private float[] m_iPercentages;
    private int m_iIncrementalLength;

	public CircleMesh()
    {
    }    

    public override void Initialize(Vector3[] verticesList, int iNumOfVertices, int iLength, int iWidth)
    {
        m_fCameraHeight = 0.13f;
        m_iLength = iLength;
        m_iWidth = iWidth;

        m_OriginalVertices = new Vector3[iNumOfVertices];
        m_CalculatedVertices = new Vector3[iNumOfVertices];
        m_UpdatedVertices = new Vector3[iNumOfVertices];
        midArray = new Vector3[iLength + 1];
        m_iPercentages = new float[iNumOfVertices];


        for (int i = 0; i < verticesList.Length; i++)
        {
            m_OriginalVertices[i] = verticesList[i];

        }
        int counter = 0;
        for (int i = iWidth / 2; i <= m_OriginalVertices.Length; i += iWidth + 1)
        {
            midArray[counter] = m_OriginalVertices[i];
            counter++;
        }

        m_fFaceWidth = m_OriginalVertices[iWidth / 2].x;

        m_bChangeCompleted = true;
        m_iIncrementalLength = 0;
        base.Initialize(verticesList, iNumOfVertices, iLength, iWidth);
    }

    public override void CalculateFinalVertices(Vector3[] verticesList)
    {
        int counter = 0;
        for (int i = m_iWidth / 2; i <= verticesList.Length; i += m_iWidth + 1)
        {
            midArray[counter] = verticesList[i];
            counter++;
        }

        for (int i = 0; i < verticesList.Length; i++)
        {
            m_OriginalVertices[i] = verticesList[i];
        }

        float fAngleIncrement = 360 / m_iWidth;
        int iCounter = 0;

        for (int i = 0; i <= m_iLength; i++)
        {
            m_fAngle = 90;
            for (int j = 0; j <= m_iWidth; j++)
            {
                m_CalculatedVertices[iCounter].y = midArray[i].y + 0.1f * Mathf.Sin(m_fAngle * Mathf.Deg2Rad);
                m_CalculatedVertices[iCounter].x= midArray[i].x + 0.1f * Mathf.Cos(m_fAngle * Mathf.Deg2Rad);
                m_CalculatedVertices[iCounter].z= verticesList[iCounter].z;
                m_fAngle += fAngleIncrement;
                iCounter++;
            }
        }
    }

    public override void StartChange()
    {
        m_bChangeCompleted = false;
        m_ShapeState = m_ShapeState == SHAPE_STATE.SHAPE_STATE_CHANGED ? SHAPE_STATE.SHAPE_STATE_NORMAL : SHAPE_STATE.SHAPE_STATE_CHANGED;

        for (int i = 0; i < m_iPercentages.Length; i++)
        {
            m_iPercentages[i] = 0;
        }
        m_iIncrementalLength = 0;
    }

    public override bool UpdateVertices()
    {
        bool bUpdateCompleted = true;
        if (m_UpdatedVertices == null) return false;
        for (int i = 0; i < m_OriginalVertices.Length; i++)
        {
            if (m_ShapeState == SHAPE_STATE.SHAPE_STATE_CHANGED)
            {
                m_UpdatedVertices[i] = Vector3.Lerp(m_OriginalVertices[i], m_CalculatedVertices[i], m_iPercentages[i]);
            }

            else if (m_ShapeState == SHAPE_STATE.SHAPE_STATE_NORMAL)
            {
                m_UpdatedVertices[i] = Vector3.Lerp(m_CalculatedVertices[i], m_OriginalVertices[i], m_iPercentages[i]);
            }

           
        }

        int iCounter = m_iPercentages.Length - m_iIncrementalLength;
        for (int i = iCounter; i < m_iPercentages.Length; i++)
        {
            if (m_iPercentages[i] <= 1)
            {
                m_iPercentages[i] += Time.deltaTime * m_fSpeed;
                bUpdateCompleted = false;
            }
        }

        if (m_iIncrementalLength != m_iPercentages.Length)
        {
            m_iIncrementalLength += m_iWidth + 1;
            bUpdateCompleted = false;
        }
        return bUpdateCompleted;
    }

}
