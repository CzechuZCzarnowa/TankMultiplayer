using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BulletControler : NetworkBehaviour
{
    Rigidbody2D rigi;
    private float speed;
    [SerializeField] private float lifeTime = 20;
    [SerializeField] private int damage = 20;
    PlayerShooting ps;
    float age;
    void Start()
    {
        ps = GetComponent<PlayerShooting>();
        speed = ps.currentWeapon.speed;
        rigi = GetComponent<Rigidbody2D>();
        rigi.velocity = transform.up * speed;
        Destroy(gameObject, 3);

    }
    [ServerCallback]
    private void Update()
    {
        age += Time.deltaTime;
        if(age>lifeTime)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D colInfo)
    {
        if (!isServer)
            return;

        Player player = colInfo.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            
        }
       // NetworkServer.Destroy(gameObject);
    }

}
