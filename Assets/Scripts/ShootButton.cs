using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class ShootButton : MonoBehaviour {

    public PlayerShooting Player;
    public Transform playerT;
    // Use this for initialization

    public void TestSpawn()
    {
        Player.GetComponent<PlayerShooting>().CmdShootBullet();

    }
}
