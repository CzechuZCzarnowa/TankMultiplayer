using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControler : NetworkBehaviour
{
    private Rigidbody2D rigi;
    private BoxCollider2D boxCollider;
    private int damage;
    private Weapon ps;

    private float lifetime = 2f;
    void Start()
    {
        ps = transform.parent.GetComponent<PlayerShooting>().currentWeapon;
        transform.localScale = new Vector3(1, 1, 1);
        damage = ps.Damage;
        rigi = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine("SelfDestruct"); 
       

    }


    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifetime);
        Destruct();
    }

    private void Destruct()
    {
        rigi.velocity = Vector2.zero;
        boxCollider.enabled = false;
        rigi.Sleep();
        BulletDestroy();
    }

    private void BulletDestroy()
    {
        if(isServer)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
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
            Destruct();
        }
    }


}
