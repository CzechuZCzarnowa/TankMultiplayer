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

    private void Start()
    {
        if(isLocalPlayer)
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

        GameObject bullet_fired = Instantiate(currentWeapon.Bulletprefab, firePosition.position, firePosition.rotation, transform);

        NetworkServer.SpawnWithClientAuthority(bullet_fired, this.gameObject);
        Rpc_Client(bullet_fired);

    }
    [ClientRpc]
    public void Rpc_Client(GameObject ob)
    {
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
