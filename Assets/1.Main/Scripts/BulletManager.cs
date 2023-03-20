using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    GameObject[] m_bulletPrefabs;
    [SerializeField]
    Transform m_firePos;
    GameObjectPool<BulletController> m_bulletPool;
    [SerializeField]
    MonsterController m_monster;
    
    public void CreateBullet()
    {
        var bullet = m_bulletPool.Get();
        bullet.transform.position = m_firePos.position;
        bullet.gameObject.SetActive(true);
    }
    public void ReturnBullet(BulletController bullet)
    {
        bullet.gameObject.SetActive(false);
        m_bulletPool.Set(bullet);
    }
    // Start is called before the first frame update
    protected override void OnAwake()
    {
        m_bulletPrefabs = Resources.LoadAll<GameObject>("Prefab/Bullet/");
    }

    protected override void OnStart()
    {
        for (int i = 0; i < m_bulletPrefabs.Length; i++)
        {
            var prefab = m_bulletPrefabs[i];
            m_bulletPool = new GameObjectPool<BulletController>(5, () =>
            {
                var obj = Instantiate(prefab);
                obj.transform.SetParent(transform);
                var bullet = obj.GetComponent<BulletController>();
                bullet.Initialize(m_player,m_monster);
                obj.SetActive(false);
                return bullet;
            });
            
        }
        
    }
}
