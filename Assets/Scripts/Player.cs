using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class Player : NetworkBehaviour {


    public const int maxHealth = 100;
    public GameObject deathEffect;


    public TextMeshProUGUI HpUnitText;

    
    [SyncVar(hook = "ChangeHpText") ] private int health= maxHealth ;



    public override void OnStartClient()
    {
        ChangeHpText(health);
    }
    public void TakeDamage(int value)
    {
        if (!isServer || health <= 0)
            return;

        health -= value;

        if (health <= 0)
        {
            RpcDied();
            Invoke("BackToLobby",3f);
        }
    }


    public void ChangeHpText(int health)
    {
        
        HpUnitText.text = health.ToString();
    }
    [ClientRpc]
    public void RpcDied()
    {
        if (!isServer)
            return;
        // instatiate deathEffect;

    }



    void BackToLobby()
    {
        FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }
}
