using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        Idel,
        Run,
        Attack,
        Patrol,
        Hit,
        Die,
        Max
    };
    [SerializeField] GameObject m_hiteffectPrefab;
    [SerializeField] GameObject m_deatheffectPrefab;
    [SerializeField] PlayerController m_player;
    MonsterAnimController m_monAinm;
    NavMeshAgent m_navAgent;
    Status m_status;
    float m_time;
    public MonsterState m_monsterState;
    public float m_idleDuration = 1f;
    public float m_detectDist = 5f;
    float m_attactDist;

    public void InitMonster()
    {
        m_status = new Status(3);
    }
    protected void SetState(MonsterState state)
    {
        m_monsterState = state;
    }
    protected void SetDamage()
    {
        m_time = 0f;
        SetState(MonsterState.Hit);
        m_monAinm.Play(MonsterAnimController.AnimState.Hit);
        var effect = Instantiate(m_hiteffectPrefab);
        effect.transform.position = transform.position + Vector3.up;
        m_status.m_hp--;
        if(m_status.m_hp <= 0)
        {
            SetState(MonsterState.Die);
            var deatheffect = Instantiate(m_deatheffectPrefab);
            deatheffect.transform.position = transform.position;
            gameObject.SetActive(false);
            Targeting.Instance.m_unitList.Remove(this.gameObject);
            //Destroy(this.gameObject);
            Debug.Log(Targeting.Instance.m_unitList.Count);
            
        }
        else
        {
            StartCoroutine("Coroutine_SetIdle");
        }
        
    }
    protected void Attack()
    {
        m_time = 0f;
        var dir = m_player.transform.position - transform.position;
        transform.forward = dir;
        SetState(MonsterState.Attack);
        m_monAinm.Play(MonsterAnimController.AnimState.Attack);
        m_navAgent.ResetPath();
        
    }
    protected void AnimEvent_AttackFinished()
    {
        m_player.SetDamage();
        m_monsterState = MonsterState.Idel;
        m_monAinm.Play(MonsterAnimController.AnimState.Idle);
    }
    protected bool CheckArea(Vector3 target, float area) //공격가능거리 도달 체크
    {
        var dist = target - transform.position;
        if (Mathf.Approximately(dist.sqrMagnitude, area) || dist.sqrMagnitude < area)
        {
            return true;
        }
        return false;
    }
    protected bool FindTarget(Vector3 target)
    {
        Vector3 start = transform.position + Vector3.up * 1f;
        Vector3 end = target + Vector3.up * 1f;
        var dir = target - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(start, dir.normalized, out hit, m_detectDist, 1 << LayerMask.NameToLayer("BackGround") | 1 << LayerMask.NameToLayer("Player")))
        {
            if (hit.collider.CompareTag("Player")) //Player 맞았으면 트루반환
                return true;
        }
        return false; //배경 맞은경우
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            SetDamage();
        }
    }
    IEnumerator Coroutine_SetIdle()
    {
        while (m_monsterState == MonsterState.Hit)
        {
            yield return new WaitForSeconds(0.5f);
            m_monsterState = MonsterState.Idel;
            m_monAinm.Play(MonsterAnimController.AnimState.Idle);
        }
    }
    IEnumerator Coroutine_SetPatrol()
    {
        while (m_monsterState == MonsterState.Patrol)
        {
            yield return new WaitForSeconds(0.5f);
            m_monAinm.Play(MonsterAnimController.AnimState.Run);
            //m_navAgent.SetDestination(transform.position += Vector3.back);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_monAinm = GetComponent<MonsterAnimController>();
        m_navAgent = GetComponent<NavMeshAgent>();
        InitMonster();
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > m_idleDuration)
        {
            if (FindTarget(m_player.transform.position))
            {
                SetState(MonsterState.Run);
                m_monAinm.Play(MonsterAnimController.AnimState.Run);
                m_navAgent.SetDestination(m_player.transform.position);
                if (CheckArea(m_player.transform.position, Mathf.Pow(m_navAgent.stoppingDistance, 2f)))
                {
                    Attack();
                }
            }
            else
            {
                SetState(MonsterState.Patrol);
                StartCoroutine("Coroutine_SetPatrol");


            }

        }
    }
}
