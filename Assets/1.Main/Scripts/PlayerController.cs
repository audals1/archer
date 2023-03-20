using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Filed
    Vector3 m_dir;
    [SerializeField] float m_speed = 2f;
    [SerializeField] GameObject m_hitEffect;
    GameObject m_target;
    bool m_isAttack;
    PlayerAnimController m_animCtr;
    public PlayerAnimController.AnimState AnimState { get { return m_animCtr.State; } }
    #endregion
    #region Method

    void SetLocomotion(bool isAttack = false)
    {
        if (m_dir != Vector3.zero && !m_isAttack) //이동키 누른 상태이고 공격 안누름
        {
            if (AnimState == PlayerAnimController.AnimState.Idle)
                m_animCtr.Play(PlayerAnimController.AnimState.Run);
            //if (m_clickDir == Vector3.zero)
                //transform.forward = m_dir;
        }
        else //이동키 x 공격키 o
        {
            if (isAttack)
            {
                m_animCtr.Play(PlayerAnimController.AnimState.Idle);
                m_dir = Vector3.zero;
            }
            else
            {
                if (AnimState == PlayerAnimController.AnimState.Run)
                    m_animCtr.Play(PlayerAnimController.AnimState.Idle);
            }
        }
    }
    public void SetDamage()
    {
        var effect = Instantiate(m_hitEffect);
        var dummy = Util.FindChildObject(gameObject, "Dummy_Hit");
        effect.transform.position = dummy.transform.position;
        m_animCtr.Play(PlayerAnimController.AnimState.Damage);
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

    void ActionControl()
    {
        m_speed = 5f;
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        var padDir = GetPadDir();
        //m_characterController.Move(m_dir * m_speed * Time.deltaTime);
        //m_dir = new Vector3(MovePad.Instance.m_dir.x, m_dir.y, MovePad.Instance.m_dir.z);
        Transform m_tr = transform;
        if(padDir != Vector3.zero)
        {
            m_dir = padDir;
            //m_characterController.Move(m_dir * m_speed * Time.deltaTime);
            transform.position += m_dir * m_speed * Time.deltaTime;
            m_animCtr.Play(PlayerAnimController.AnimState.Run);
            m_tr.rotation = Quaternion.LookRotation(m_dir);
           
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_animCtr.Play(PlayerAnimController.AnimState.Attack1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_dir = Targeting.Instance.GetTargetDir();
            if(m_dir != Vector3.zero)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.forward, m_dir.normalized);
            }
            else
            {
                m_dir = padDir;
            }
            BulletManager.Instance.CreateBullet();
        }
        if (m_dir == Vector3.zero)
        {
            SetLocomotion();
        }
    }
    #endregion

    #region AnimEvent
    void AnimEvent_AttackFinished()
    {
        SetLocomotion(true);
    }
    void AnimEvent_Attack()
    {

    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        m_animCtr = GetComponent<PlayerAnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        ActionControl();
        

    }
}
