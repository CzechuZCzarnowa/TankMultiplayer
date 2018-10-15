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

    public override void OnStartLocalPlayer()
    {

       // GameObject.Find("bs").GetComponent<ShootButton>().Player = this;
 
    }


    [Command]
    public void CmdShootBullet()
    {

            GameObject bullet_fired = Instantiate(currentWeapon.Bulletprefab, firePosition.position, firePosition.rotation);
            NetworkServer.SpawnWithClientAuthority(bullet_fired, this.gameObject);

    }

}
