using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Types;
using Prototype.NetworkLobby;

public class GameManager : NetworkBehaviour
    {

        [SyncVar]
        private int maxPlayers = 4;
        [SyncVar(hook = "PlayerCountChange")] public int playerCount = 0;
        private bool isGame = false;
        [SyncVar] private float timer;
        [SerializeField] private float spawnBonusTime = 3f;
        [SerializeField] private float timeRound = 119;

      
        public TextMeshProUGUI timer_text;
        public TextMeshProUGUI panelInfoText;
        public Canvas panelInfo;
        public GameObject barrier;
        public GameObject[] bonus;
        public BonusColliderSpawn[] bonusSpawn;

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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
           
            StartCoroutine("RandomBonus", spawnBonusTime);

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

            if (bs != null)
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

        private BonusColliderSpawn GetRandomSpawnVector()
        {
            if (bonusSpawn != null)
            {
                if (bonusSpawn.Length > 0)
                {                  
                    for (int i = 0; i < bonusSpawn.Length; i++)
                    {
                        BonusColliderSpawn bonus = bonusSpawn[Random.Range(0, 
                                                   bonusSpawn.Length)].GetComponent<BonusColliderSpawn>();
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
            if (timer <= 0)
            {
                timer = timeRound;
                StartCoroutine(BarierMove());


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

        private IEnumerator BarierMove()
        {
            float progress = 0;
            Vector3 actualTransform = barrier.transform.localScale;
            Vector3 newLocal = new Vector3(actualTransform.x - 0.3f, actualTransform.y - 0.3f, actualTransform.z);

            while (progress <= 1)
            {
                progress += Time.deltaTime * Time.timeScale;
                barrier.transform.localScale = Vector3.Lerp(actualTransform, newLocal, progress);
                yield return null;

            }
            barrier.transform.localScale = newLocal;
            SpawnPlayerLocalScale(new Vector3(newLocal.x - 0.76f, newLocal.y - 1, newLocal.z));

        }

    
        public void PlayerCountChange(int index)
        {
        playerCount--;
        }
        public void EndGame(string nameLastAttacker)
        {
            
            if (playerCount > 1)
            {
                RpcInformationAboutCountPlayers();
            }
            else
            {

                StartCoroutine(EndGameMassage(nameLastAttacker));

            }
        }

        IEnumerator EndGameMassage(string nameLastAttacker)
        {
            yield return new WaitForSeconds(3f);
            RpcEndPanel(nameLastAttacker);
            yield return new WaitForSeconds(3f);
            StartCoroutine(ChangeScene());
        }
        
        
        IEnumerator RpcInformationAboutCountPlayers()
        {
        RpcCountPlayersText("zostało " + playerCount + "graczy",true);
        yield return new WaitForSeconds(3);
        RpcCountPlayersText("", true);
        }

        [ClientRpc]
        private void RpcCountPlayersText(string text,bool active)
        {
        GameManager.Instance.panelInfo.gameObject.SetActive(active);
        GameManager.Instance.panelInfoText.text = text;
        }
        [ClientRpc]
        private void RpcEndPanel(string nameLastAttacker)
        {
            GameManager.Instance.panelInfo.gameObject.SetActive(true);

            GameManager.Instance.panelInfoText.text = "KONIEC GRY Wygral gracz " + nameLastAttacker;   
        }
    IEnumerator ChangeScene()
    {
        while (Network.connections.Length > 0)
                yield return null;

        Disconnect();
    }

    public void Disconnect()
    {
        Prototype.MyNetworkLobby.MyLobby netManager = NetworkManager.singleton as Prototype.MyNetworkLobby.MyLobby;
        if (isServer)
        {
            netManager.StopHost();
            Destroy(NetworkManager.singleton.gameObject);
        }
        else
        {
            NetworkClient.ShutdownAll();
            netManager.StopClient();
            Destroy(gameObject);
        }
    }


}
    

