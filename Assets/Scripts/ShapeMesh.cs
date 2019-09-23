using UnityEngine;
using System.Collections;

public class ShapeMesh
{
    protected Vector3[] m_OriginalVertices;
    protected Vector3[] m_CalculatedVertices;
    protected Vector3[] m_UpdatedVertices;

    protected Vector3[] midArray;
    protected string m_sShapeName;

    protected int m_iWidth;
    protected int m_iLength;

    protected float m_fSpeed;
    protected bool m_bChangeCompleted;

    protected float m_fCameraHeight;

    public enum SHAPE_STATE
    {
        SHAPE_STATE_NORMAL,
        SHAPE_STATE_CHANGING,
        SHAPE_STATE_CHANGED
    };

    protected SHAPE_STATE m_ShapeState;

    public ShapeMesh()
    {
    }

    public virtual void Initialize(Vector3[] verticesList, int iNumOfVertices, int iLength, int iWidth)
    {
        m_iLength = iLength;
        m_iWidth = iWidth;

        m_OriginalVertices = new Vector3[iNumOfVertices];
        m_CalculatedVertices = new Vector3[iNumOfVertices];
        m_UpdatedVertices = new Vector3[iNumOfVertices];
        midArray = new Vector3[iLength + 1];

        m_ShapeState = SHAPE_STATE.SHAPE_STATE_NORMAL;
        m_bChangeCompleted = true;
    }

    public virtual void SetChangeSpeed(float fSpeed)
    {
        m_fSpeed = fSpeed;
    }

    public virtual void StartChange()
    {
    }

    public virtual void CalculateFinalVertices(Vector3[] vertices)
    {
    }

    public virtual bool UpdateVertices()
    {
        return false;
    }

    public virtual void Update()
    {
        if (UpdateVertices())
        {
            m_bChangeCompleted = true;
        }
    }

    public Vector3[] GetFinalVertices()
    {
        if (!m_bChangeCompleted)
        {
            return m_UpdatedVertices;
        }
        else
        {
            return m_ShapeState == SHAPE_STATE.SHAPE_STATE_NORMAL ? m_OriginalVertices : m_CalculatedVertices;
        }
    }

    public bool isChangeCompleted()
    {
        return m_bChangeCompleted;
    }
}
