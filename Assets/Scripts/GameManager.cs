using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameManager : NetworkBehaviour {

    private static GameManager instance;
    private bool isGame = false;
    [SyncVar]private float timer;
    [SerializeField] private float spawnBonusTime = 3f;
    [SerializeField] private float timeRound;
    [SerializeField] private TextMeshProUGUI timer_text;
    
    public GameObject barrier;
    public GameObject[] bonus;
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
       
        StartCoroutine("RandomBonus",spawnBonusTime);
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

        int randomBonusIndex = Random.Range(0, bonus.Length);
        GameObject go = Instantiate(bonus[randomBonusIndex], spawn, Quaternion.identity);
        NetworkServer.Spawn(go);
    }

    private BonusColliderSpawn  GetRandomSpawnVector()
    {
        
        if (bonusSpawn != null)
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
            StartCoroutine(LocalLerp());


        }
        string minutes = ((int)timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timer_text.text = minutes + ":" + seconds;
        
    }

    private void SpawnPlayerLocalScale(Vector3 scale)
    {
        GameObject SpawnGO = GameObject.Find("Spawn");
        SpawnGO.transform.localScale = scale;
    }

    private IEnumerator LocalLerp()
    {
        float progress = 0;
        Vector3 actualTransform = barrier.transform.localScale;
        Vector3 newLocal = new Vector3(actualTransform.x - 0.3f, actualTransform.y - 0.3f, actualTransform.z);
        
        while (progress <= 1)
        {
            progress += Time.deltaTime * Time.timeScale;
            barrier.transform.localScale = Vector3.Lerp( actualTransform,newLocal,progress);
            yield return null;

        }
        barrier.transform.localScale = newLocal;
        SpawnPlayerLocalScale(new Vector3(newLocal.x - 0.76f, newLocal.y - 1, newLocal.z));

    }

}
