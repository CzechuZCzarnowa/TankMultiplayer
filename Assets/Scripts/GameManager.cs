using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    static GameManager instance;
    public GameObject[] newWeapon;
    public BonusColliderSpawn[] bonusSpawn;

    [SerializeField] private float RandomTime = 15f;
    private bool isGame = true;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    instance = new GameObject().AddComponent<GameManager>();
                }

            }
            return instance;
        }
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine("RandomBonus",RandomTime);
    }

    IEnumerator RandomBonus(float time)
    {
        while (isGame && isServer)
        {
            yield return new WaitForSeconds(5f);
            CmdRandomBonus();
        }

    }

    [Command]
    private void CmdRandomBonus()
    {
        Vector3 spawn = new Vector3();
        BonusColliderSpawn bs = GetRandomSpawnVector();
        if( bs != null)
        {
            spawn = bs.gameObject.transform.position;
        }
        else
        {
            Debug.Log("brak miejsc");
            return;
        }

        //if (oldSpawn != null)
        //{
        //    oldSpawn.isOccupied = false;
        //}

        int randomWeaponIndex = Random.Range(0, newWeapon.Length);
 
        GameObject go = Instantiate(newWeapon[randomWeaponIndex],
                         spawn,
                         Quaternion.identity);
        NetworkServer.Spawn(go);
       

    }




    BonusColliderSpawn  GetRandomSpawnVector()
    {
        if(bonusSpawn != null)
        {
        
            if(bonusSpawn.Length >0)
            {
                bool foundSpawner = false;

                //w razie nieskończonej pętli
                float timeOut = Time.time + 2f;
                for (int i = 0; i < bonusSpawn.Length; i++)
                {
                    BonusColliderSpawn bonus = bonusSpawn[Random.Range(0, bonusSpawn.Length)].GetComponent<BonusColliderSpawn>();

                    if (bonus.isOccupied == false)
                    {
                        //foundSpawner = true;
                        return bonus;

                    }

                }
               // while(!foundSpawner)
               // {        
      
                    
                 //   if (Time.time > timeOut)
                  //  {
                  //      foundSpawner = true;
                  //      return null;
                   // }
               // }
                return null;
                
            }
        }
        return null;
    }

   
}
