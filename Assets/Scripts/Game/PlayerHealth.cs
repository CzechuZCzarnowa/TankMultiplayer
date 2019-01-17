using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class PlayerHealth : NetworkBehaviour {

    public const int maxHealth = 100;
    [SyncVar(hook = "ChangeHpText")] private int health = maxHealth;
    public TextMeshProUGUI HpUnitText;
    public PlayerShooting lastAttacker = null;
    

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

    public void BonusHp(int amount)
    {
        health += amount;

    }
    string lastAttackerName;
    public void TakeDamage(int value, PlayerShooting ps = null)
    {
        
        if (!isServer)
            return;


        if (ps != null && ps!=this.GetComponent<PlayerShooting>())
        {
            
            lastAttacker = ps;
        }
        health -= value;
        ChangeHpText(health);
        if (health <= 0 && !isDead)
        {
            if(lastAttacker != null)
            {
                lastAttackerName = lastAttacker.GetComponent<NamePlayer>().m_playerName.ToString();
                lastAttacker.IncrementWeaponIndex();
                lastAttacker = null;
            }
    
            GameManager.Instance.UpdateScoreboard();
            RpcDied(lastAttackerName);
            GameManager.Instance.EndGame(lastAttackerName);

        }
    }

    public void ChangeHpText(int health)
    {
        HpUnitText.text = health.ToString();
    }

    [ClientRpc]
    public void RpcDied(string name)
    {
        SetState(false);
       
        gameObject.SendMessage("Die",name);
    }

    void SetState(bool state)
    {
        GetComponent<BoxCollider2D>().enabled = state;
        foreach (BoxCollider2D c in GetComponentsInChildren<BoxCollider2D>())
        {
            c.enabled = state;
        }
        
        GetComponent<SpriteRenderer>().enabled = state;
        foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.enabled = state;
        }
        foreach (Canvas r in GetComponentsInChildren<Canvas>())
        {
            r.enabled = state;
        }
    }



        public void Reset()
    {
        health = maxHealth;
        SetState(true);
        isDead = false;

    }
}
