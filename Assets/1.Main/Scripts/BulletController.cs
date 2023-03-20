using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    float m_speed = 15f;
    Vector3 m_bullDir;
    Rigidbody rigid;
    [SerializeField] MonsterController m_monster;
    
    public void Initialize(PlayerController player, MonsterController monster)
    {
        m_player = player;
        m_monster = monster;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BackGround"))
        {
            BulletManager.Instance.ReturnBullet(this);
        }
        if (other.CompareTag("Monster"))
        {
            BulletManager.Instance.ReturnBullet(this);
        }
    }
    void Start()
    {
         rigid = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

        //transform.position += transform.forward * m_speed * Time.deltaTime;
        m_bullDir = Targeting.Instance.GetTargetDir();
        if(m_bullDir == Vector3.zero)
        {
            transform.position += m_bullDir * m_speed * Time.deltaTime;
        }
        else
        {
            m_bullDir = m_player.GetPadDir();
            transform.position += m_bullDir * m_speed * Time.deltaTime;
        }

    }
}
