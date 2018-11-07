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
        gameObject.name = GetComponent<NetworkIdentity>().netId.ToString();
    }

    public void TakeDamage(int value)
    {
        if (!isServer || health <= 0)
            return;

        health -= value;

        if (health <= 0)
        {
           
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
        if (!isServer)
            return;

    

    }

    void BackToLobby()
    {
        FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }
    void DeActivateScripts()
    {
        GetComponent<CharacterControler>().enabled = false;
        GetComponent<PlayerShooting>().canvas.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        
        foreach(SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
        {
            r.enabled = false;
        }

        foreach (Canvas c in GetComponentsInChildren<Canvas>())
        {
            c.enabled = false;
        }


    }
}
