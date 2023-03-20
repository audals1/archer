using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheck : SingletonMonoBehaviour<RoomCheck>
{
    public List<GameObject> m_monsterinroom;
    bool m_playerenter;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
          Debug.Log("플레이어입장");
            m_playerenter = true;
            Targeting.Instance.m_unitList = new List<GameObject>(m_monsterinroom);
            Debug.Log("enter room! mob Count : " + Targeting.Instance.m_unitList.Count);
        }
        if (other.CompareTag("Monster"))
        {
            m_monsterinroom.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            m_monsterinroom.Remove(other.gameObject);
            Debug.Log(m_monsterinroom.Count);
        }
    }
}
