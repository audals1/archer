using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovePad : SingletonMonoBehaviour<MovePad>
{
    [SerializeField]
    GameObject m_padBG;
    [SerializeField]
    GameObject m_pad;
    public Vector3 m_dir;
    Vector3 m_firstPos;
    float m_radius;
    Vector3 m_resetPos;
    // Start is called before the first frame update
    void Start()
    {
        m_radius = m_padBG.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        m_resetPos = m_padBG.transform.position;
    }

    public void PutDown()
    {
        m_padBG.transform.position = Input.mousePosition;
        m_pad.transform.position = Input.mousePosition;
        m_firstPos = Input.mousePosition;
    }

    public void Drag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        Vector3 DragPos = pointerEventData.position;
        m_dir = (DragPos - m_firstPos).normalized;

        float padDist = Vector3.Distance(DragPos, m_firstPos);

        if(padDist < m_radius)
        {
            
            m_pad.transform.position = m_firstPos + m_dir * padDist;
        }
        else
        {
            
            m_pad.transform.position = m_firstPos + m_dir * m_radius;
        }
    }

    public void Drop()
    {
        m_dir = Vector3.zero;
        m_padBG.transform.position = m_resetPos;
        m_pad.transform.position = m_resetPos;
    }

    public Vector2 GetAxis()
    {
        return m_dir;
    }
}
