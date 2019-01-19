using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class Player : NetworkBehaviour {


    public GameObject deathEffect;
    NetworkStartPosition[] spawnPoints;
    PlayerHealth playerHealth;
    Vector3 orginalPosition;

    private void Start()
    {
 
   
        playerHealth = GetComponent<PlayerHealth>();
        orginalPosition = transform.position;
        if(isServer)
        {
            spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();
             RpcSet();
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
         spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();

    }


    [ClientRpc]
    void RpcSet()
    {
        spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();

    }
    public void Die(string nameLastAttacker)
    {
        if (isLocalPlayer)
        {
            
            GameManager.Instance.panelInfo.gameObject.SetActive(true);
            GameManager.Instance.panelInfoText.text = "JESTES MARTWY ";
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<PlayerHealth>().isDead = true;

        }

        
    }

    public IEnumerator RespawnRoutine()
    {
        if (isLocalPlayer)
        {
            GameManager.Instance.panelInfo.gameObject.SetActive(false);
        }

        PlayerSpawnCollider oldSpawn = GetNearestSpawnPoint();
        transform.position = GetRandomSpawnPosition();

        if (oldSpawn != null)
        {
            oldSpawn.isOccupied = false;
        }
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        yield return new WaitForSeconds(3f);
        playerHealth.Reset();
    }

    PlayerSpawnCollider GetNearestSpawnPoint()
    {
        Collider2D[] triggerColliders = Physics2D.OverlapCircleAll(transform.position, 2f, Physics2D.AllLayers);
        foreach (Collider2D col in triggerColliders)
        {
            PlayerSpawnCollider spawnPoint = col.GetComponent<PlayerSpawnCollider>();
            if (spawnPoint != null)
            {
                return spawnPoint;
            }
        }
        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        
        if (spawnPoints != null)
        {
            if (spawnPoints.Length > 0)
            {
                bool foundSpawner = false;
                Vector3 newStartPosition = new Vector3();
                float timeOut = Time.time + 2f;

                while (!foundSpawner)
                {
                    NetworkStartPosition startPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    PlayerSpawnCollider spawnPoint = startPoint.GetComponent<PlayerSpawnCollider>();

                    if (spawnPoint.isOccupied == false)
                    {
                        foundSpawner = true;
                        newStartPosition = startPoint.transform.position;
                    }

                    if (Time.time > timeOut)
                    {
                        foundSpawner = true;
                        newStartPosition = orginalPosition;
                    }
                }

                return newStartPosition;

            }
        }

       
        return orginalPosition;
    }


}
