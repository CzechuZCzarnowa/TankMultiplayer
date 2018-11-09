using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class Player : NetworkBehaviour {



    public GameObject deathEffect;

    NetworkStartPosition[] spawnPoints;
    PlayerHealth playerHealth;
    Vector3 orginalPosition;
    CharacterControler playerControler;
    GameObject[] t;

    private void Start()
    {
        playerControler = GetComponent<CharacterControler>();
        playerHealth = GetComponent<PlayerHealth>();
    }
    public override void OnStartClient()
    {
       
        
    }

    public override void OnStartLocalPlayer()
    {
        //gameObject.name = GetComponent<NetworkIdentity>().netId.ToString();
        
        spawnPoints = GameObject.FindObjectsOfType<NetworkStartPosition>();
        
       
        orginalPosition = transform.position;
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
    public void Die()
    {
        StartCoroutine("RespawnRoutine");
    }

    public IEnumerator RespawnRoutine()
    {
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
