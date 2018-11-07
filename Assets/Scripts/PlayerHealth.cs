using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class PlayerHealth : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "ChangeHpText")] private int health = maxHealth;
    public TextMeshProUGUI HpUnitText;

    [SyncVar]
    public bool isDead = false;
    private void Start()
    {
        Reset();
    }

    public override void OnStartClient()
    {
        ChangeHpText(health);
    }


    public void TakeDamage(int value)
    {
        if (!isServer || health <= 0)
            return;

        health -= value;
        ChangeHpText(health);
        if (health <= 0 && !isDead)
        {
            
            isDead = true;
            RpcDied();

        }
    }

    public void ChangeHpText(int health)
    {
        HpUnitText.text = health.ToString();
    }

    [ClientRpc]
    public void RpcDied()
    {

        gameObject.SendMessage("Die");
    }

    public void Reset()
    {
        health = maxHealth;
        isDead = false;
    }
}
