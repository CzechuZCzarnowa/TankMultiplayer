using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class PlayerShooting : NetworkBehaviour {

    public Transform firePosition;
    public Weapon currentWeapon;
    public GameObject canvas;
    public Rigidbody2D bulletPrefab;
   

    private void Start()
    {
        bulletPrefab = currentWeapon.Bulletprefab.GetComponent<Rigidbody2D>();
        
        if (isLocalPlayer)
        {
           canvas.SetActive(true);
        }
        else
        {
            return;
        }
        GetComponentInChildren<ShootButton>().Player = this;
    }



    [Command]
    public void CmdShootBullet(NetworkInstanceId id)
    {
        
        Rigidbody2D rbody = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation) as Rigidbody2D;

        if (rbody != null)
        {
            //var t = ClientScene.FindLocalObject(id);
            //Debug.Log(t);
            //rbody.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();
            rbody.velocity = currentWeapon.speed * firePosition.transform.up;
            NetworkServer.Spawn(rbody.gameObject);
            RpcObject(id, rbody.gameObject);

            if(isServer)
            {
            var t = ClientScene.FindLocalObject(id);
            rbody.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();

            }
        }

    }

    [ClientRpc]
    void RpcObject(NetworkInstanceId id,GameObject g)
    {
        var t = ClientScene.FindLocalObject(id);
        g.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUpWeapon pick = collision.GetComponent<PickUpWeapon>();
        if(pick != null)
        {
            currentWeapon = pick.weapon;
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(collision.gameObject);
        }
    }

}
