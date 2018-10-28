﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GameManager : NetworkBehaviour {

    private static GameManager instance;
    private bool isGame = false;
    [SyncVar]private float timer;
    [SerializeField] private float randomTime = 15f;
    [SerializeField] private float timeRound;

    public GameObject barrier;
    public TextMeshProUGUI timer_text;
    public GameObject[] newWeapon;
    public BonusColliderSpawn[] bonusSpawn;
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
        isGame = true;
        timer = timeRound;
       
        StartCoroutine("RandomBonus",randomTime);
    }
    private void Update()
    {
        TimeUpdate();
    }
    private IEnumerator RandomBonus(float time)
    {
        while (isGame && isServer)
        {
            yield return new WaitForSeconds(time);
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
            return;
        }

        int randomWeaponIndex = Random.Range(0, newWeapon.Length);
        GameObject go = Instantiate(newWeapon[randomWeaponIndex], spawn, Quaternion.identity);
        NetworkServer.Spawn(go);

    }

    private BonusColliderSpawn  GetRandomSpawnVector()
    {
        if(bonusSpawn != null)
        {
        
            if(bonusSpawn.Length >0)
            {
         
                float timeOut = Time.time + 2f;
                for (int i = 0; i < bonusSpawn.Length; i++)
                {
                    BonusColliderSpawn bonus = bonusSpawn[Random.Range(0, bonusSpawn.Length)].GetComponent<BonusColliderSpawn>();

                    if (!bonus.isOccupied)   
                        return bonus;

                }
                return null;                
            }
        }
        return null;
    }

    private void TimeUpdate()
    {
        
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            timer = timeRound;
            barrierScale();
        }
        string minutes = ((int)timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timer_text.text = minutes + ":" + seconds;
        
    }

    private void barrierScale()
    {
        Vector3 scale = barrier.transform.localScale;
        scale.x /= 2;
        scale.y /= 2;
        barrier.transform.localScale = scale;
    }
}