using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BulletControler : NetworkBehaviour
{
    Rigidbody2D rigi;
    private float speed;
    [SerializeField] private float lifeTime = 20;
    private int damage;
    Weapon ps;
    float age;
    void Start()
    {
        ps = transform.parent.GetComponent<PlayerShooting>().currentWeapon;
        speed = ps.speed;
        damage = ps.Damage;
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
        if(colInfo.tag != "Ammo")
        {
        DeleteGameObject(gameObject);
        }
    }

    [ServerCallback]
    private void DeleteGameObject(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

}
