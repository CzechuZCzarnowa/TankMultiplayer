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
    public void CmdShootBullet()
    {
        BulletControler bullet = null;
        bullet = bulletPrefab.GetComponent<BulletControler>();
        
        Rigidbody2D rbody = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation,transform) as Rigidbody2D;
        
        if(rbody != null)
        {
           
            rbody.velocity = currentWeapon.speed * firePosition.transform.up;
            NetworkServer.Spawn(rbody.gameObject);
        }
            Rpc_Client(rbody.gameObject);

    }
    [ClientRpc]
    public void Rpc_Client(GameObject ob)
    {
        Debug.Log(ob);
        ob.transform.SetParent(this.transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        PickUpWeapon pick = collision.GetComponent<PickUpWeapon>();
        if(pick != null)
        {
            currentWeapon = pick.weapon;
            Destroy(collision.gameObject);
        }
    }

}
