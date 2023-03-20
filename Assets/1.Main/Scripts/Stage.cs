using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    int m_stageNumber;
    bool m_isClear;
    [SerializeField]GameObject m_gate;
    [SerializeField] PlayerController m_player;
    void ClearCheck()
    {
        if(Targeting.Instance.m_unitList.Count == 0)
        {
            m_isClear = true;
            Debug.Log(m_stageNumber);
            m_gate.gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_stageNumber++;
            Debug.Log("next stage");
            m_gate.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_stageNumber++;
        Debug.Log("Stage Number :" + m_stageNumber);
    }

    // Update is called once per frame
    void Update()
    {
        ClearCheck();
    }
}
