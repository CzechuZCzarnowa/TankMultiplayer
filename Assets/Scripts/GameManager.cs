using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    static GameManager instance;
    public GameObject[] newWeapon;
    public Transform[] bonusSpawn;

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
        yield return new WaitForSeconds(time);
        CmdRandomBonus();
    }

    [Command]
    private void CmdRandomBonus()
    {
        int randomWeaponIndex = Random.Range(0, newWeapon.Length);
        int randomSpawnIndex = Random.Range(0, newWeapon.Length);
        GameObject go = Instantiate(newWeapon[randomWeaponIndex],
                         bonusSpawn[randomSpawnIndex].position,
                         bonusSpawn[randomSpawnIndex].rotation);
        NetworkServer.Spawn(go);

    }

}
