using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : SingletonMonoBehaviour<Targeting>
{
    public Transform m_firePos;
    public List<GameObject> m_unitList = new List<GameObject>();
    float m_currentDist = 0;
    float m_closetDist = 100f;
    float m_targetDist = 100f;
    int m_closeDistIndex = 0;
    int m_targetIndex = -1;
    float m_maxDist = 10f;
    Vector3 m_dir;
    
    public GameObject GetMonsterNearest(Vector3 target) //가장 가까운 몬스터를 리스트에서 찾음
    {
        if (m_unitList.Count == 0) return null;

        m_maxDist = (target - m_unitList[0].transform.position).sqrMagnitude;
        int index = 0;
        for (int i = 1; i < m_unitList.Count; i++)
        {
            var dist = (target - m_unitList[i].transform.position).sqrMagnitude;
            if (m_maxDist > dist)
            {
                m_maxDist = dist;
                index = i;
            }
        }
        return m_unitList[index];
    }
    public Vector3 GetTargetDir()
    {
        var target = GetMonsterNearest(transform.position);
        if (target == null || target.gameObject.activeSelf == false)
        {
            return Vector3.zero;
        }

        var dir = target.transform.position - transform.position;
        dir.y = 0f;
        return dir.normalized;
    }
    public Vector3 GetPadDir()
    {
        Vector3 dir = Vector3.zero;
        var tempAxis = MovePad.Instance.GetAxis();
        if (tempAxis.x < 0f)
        {
            dir += Vector3.left * Mathf.Abs(tempAxis.x);
        }
        if (tempAxis.x > 0f)
        {
            dir += Vector3.right * tempAxis.x;
        }
        if (tempAxis.y < 0f)
        {
            dir += Vector3.back * Mathf.Abs(tempAxis.y);
        }
        if (tempAxis.y > 0f)
        {
            dir += Vector3.forward * tempAxis.y;
        }
        return dir;
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < m_unitList.Count; i++)
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, m_unitList[i].transform.position - transform.position, out hit, m_maxDist, 1 << LayerMask.NameToLayer("Monster"));
            if (isHit && hit.collider.CompareTag("Monster"))
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawRay(transform.position, m_unitList[i].transform.position - transform.position);
        }
    }
    void Targetting()
    {
        if (m_unitList.Count != 0)
        {
            m_currentDist = 0f;
            m_closeDistIndex = 0;
            m_targetIndex = -1;

            for (int i = 0; i < m_unitList.Count; i++)
            {
                m_currentDist = Vector3.Distance(transform.position, m_unitList[i].transform.position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, m_unitList[i].transform.position - transform.position, out hit, 240f);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (m_targetDist >= m_currentDist)
                    {
                        m_targetIndex = i;
                        m_targetDist = m_currentDist;
                    }
                }

                if (m_closetDist >= m_currentDist)
                {
                    m_closeDistIndex = i;
                    m_closetDist = m_currentDist;
                }
            }

            if (m_targetIndex == -1)
            {
                m_targetIndex = m_closeDistIndex;
            }
            m_closetDist = 100f;
            m_targetDist = 100f;
        }
    }
}
