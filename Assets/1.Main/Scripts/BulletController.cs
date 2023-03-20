using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    float m_speed = 15f;
    Vector3 m_targetDir;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody>().velocity = transform.forward * m_speed;
        transform.position += transform.forward * m_speed * Time.deltaTime;
        //m_targetDir = Targeting.Instance.GetTargetDir();
        //if(m_targetDir != Vector3.zero)
        //{
        //    transform.position += m_targetDir * m_speed * Time.deltaTime;
        //}
        //else
        //{
        //    RaycastHit hit;
        //    Physics.Raycast(transform.position, transform.forward - transform.position, out hit, 100f);
        //    m_targetDir = Targeting.Instance.GetPadDir();
        //    transform.position += m_targetDir * m_speed * Time.deltaTime;
        //}

    }
}
