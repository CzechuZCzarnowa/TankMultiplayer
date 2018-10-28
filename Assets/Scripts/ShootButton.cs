using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ShootButton : NetworkBehaviour {

    public PlayerShooting Player;

    // Use this for initialization

    public void TestSpawn()
    {
       
        var playerNetIdentity = Player.gameObject.GetComponent<NetworkIdentity>();
        Player.GetComponent<PlayerShooting>().CmdShootBullet(playerNetIdentity.netId);


    }
}
