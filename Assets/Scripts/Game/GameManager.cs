using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

    [SyncVar]

    private int maxPlayers = 4;
    private bool isGame = false;
    [SyncVar]private float timer;
    [SerializeField] private float spawnBonusTime = 3f;
    [SerializeField] private float timeRound = 119;

    public int playerCount = 0;
    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI timerCountdown_text;
    public Canvas timerCountdown_Panel;
    public GameObject barrier;
    public GameObject[] bonus;
    public BonusColliderSpawn[] bonusSpawn;

    public List<Player> allPlayers;
    public List<TextMeshProUGUI> nameLabelText;
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

    private static GameManager instance;
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
        UpdateScoreboard();
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

    public void AddPlayer(Player player)
    {
        if (playerCount < maxPlayers)
        {
            allPlayers.Add(player);

            playerCount++;
        }
        UpdateScoreboard();
    }
    [ClientRpc]
    void RpcUpdateScoreboard(string[] playerNames)
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (playerNames[i] != null)
            {
                nameLabelText[i].text = playerNames[i];
            }

            
        }

    }


    public void UpdateScoreboard()
    {

        if (isServer)
        {
            string[] names = new string[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                names[i] = allPlayers[i].GetComponent<NamePlayer>().namePlayer.ToString();
                
            }

            RpcUpdateScoreboard(names);
        }
    }

    public void EndGame()
    {
        playerCount--;
        if (playerCount>1)
        {
            Debug.Log("jeszcze są zywi");
        }
        else
        {
            PlayerHealth[] ph = GameObject.FindObjectsOfType<PlayerHealth>();
            foreach (PlayerHealth item in ph)
            {
                if(!item.isDead)
                {
                    string s = item.GetComponent<NamePlayer>().m_playerName;
                    RpcEndPanel(s);
                    Invoke("CmdBackToLobby", 5f);
                }
            }
            //RpcEndPanel();
          
        }
    }
    [ClientRpc]
    private void RpcEndPanel(string s)
    {
        GameManager.Instance.timerCountdown_Panel.gameObject.SetActive(true);
        
        GameManager.Instance.timerCountdown_text.text = "KONIEC GRY Wygral gracz " + s;
       
    }
    [Command]
    private void CmdBackToLobby()
    {
        NetworkServer.Shutdown();
    }
    
}
