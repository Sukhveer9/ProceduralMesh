using UnityEngine;
using System.Collections;

public class VShapeMesh : ShapeMesh
{
    private float angle = 30;
   // private float speed;
    private float[] m_fPercentages;
    private int incrementalLength;

	public VShapeMesh()
    {

    }

    public override void StartChange()
    {
        m_bChangeCompleted = false;
        m_ShapeState = m_ShapeState == SHAPE_STATE.SHAPE_STATE_CHANGED ? SHAPE_STATE.SHAPE_STATE_NORMAL : SHAPE_STATE.SHAPE_STATE_CHANGED;
        m_bChangeCompleted = false;
        incrementalLength = 0;
        for (int i = 0; i < m_fPercentages.Length; i++)
        {
            m_fPercentages[i] = 0;
        }
        Debug.Log("start change");
        Camera.main.transform.position = new Vector3(0, m_fCameraHeight, 0);
    }

    public override void Initialize(Vector3[] verticesList, int numOfVertices, int length, int iWidth)
    {
        m_fCameraHeight = 0.17f;
        m_CalculatedVertices = new Vector3[numOfVertices];
        m_OriginalVertices = new Vector3[numOfVertices];
        m_UpdatedVertices = new Vector3[numOfVertices];
        midArray = new Vector3[length + 1];
        m_fPercentages = new float[numOfVertices];

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

        base.Initialize(verticesList, numOfVertices, length, iWidth);
    }

    public override void CalculateFinalVertices(Vector3[] verticesList)
    {

        for (int i = 0; i < verticesList.Length; i++)
        {
            m_OriginalVertices[i] = verticesList[i];
            m_CalculatedVertices[i] = verticesList[i];
        }

        int counter = 0;
        for (int i = m_iWidth / 2; i <= verticesList.Length; i += m_iWidth + 1)
        {
            midArray[counter] = verticesList[i];
            counter++;
        }

        float calculatedAngle = 0;
         counter = 0;

         for (int i = 0; i <= m_iLength; i++)
        {
            for (int j = 0; j <= m_iWidth; j++)
            {
                calculatedAngle = angle * Mathf.Deg2Rad;
                if (m_CalculatedVertices[counter].x < midArray[i].x)
                {
                    calculatedAngle *= -1;
                }
                m_CalculatedVertices[counter].y = m_CalculatedVertices[counter].y + (m_CalculatedVertices[counter].x - midArray[i].x) * Mathf.Sin(calculatedAngle);
                counter++;
            }
        }
    }

    public override bool UpdateVertices()
    {
        bool updateCompleted = true;
        if (m_UpdatedVertices == null) return false;
        for (int i = 0; i < m_CalculatedVertices.Length; i++)
        {
            if (m_ShapeState == SHAPE_STATE.SHAPE_STATE_CHANGED)
            {
                m_UpdatedVertices[i] = Vector3.Lerp(m_OriginalVertices[i], m_CalculatedVertices[i], m_fPercentages[i]);

            }
            else if (m_ShapeState == SHAPE_STATE.SHAPE_STATE_NORMAL)
            {
                m_UpdatedVertices[i] = Vector3.Lerp(m_CalculatedVertices[i], m_OriginalVertices[i], m_fPercentages[i]);
            }
        }
                  
           /* if ( percentage < 1)
            {
                percentage += Time.fixedDeltaTime * speed;
                updateCompleted = false;
            }*/
        int counter = m_fPercentages.Length - incrementalLength;
        for (int i = counter; i < m_fPercentages.Length; i++)
        {
            if (m_fPercentages[i] <= 1)
            {
                m_fPercentages[i] += Time.deltaTime * m_fSpeed;
                updateCompleted = false;
            }
        }
        if (incrementalLength != m_fPercentages.Length)
        {
            incrementalLength += m_iWidth+1;
            updateCompleted = false;
        }

        return updateCompleted;
    }
}
