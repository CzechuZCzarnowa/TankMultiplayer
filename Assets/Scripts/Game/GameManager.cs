using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Types;

public class GameManager : NetworkBehaviour
    {

        [SyncVar]
        private int maxPlayers = 4;
      
        private bool isGame = false;
        [SyncVar] private float timer;
        [SerializeField] private float spawnBonusTime = 3f;
        [SerializeField] private float timeRound = 119;

        public int playerCount = 0;
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
            if (timer <= 0)
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
                barrier.transform.localScale = Vector3.Lerp(actualTransform, newLocal, progress);
                yield return null;

            }
            barrier.transform.localScale = newLocal;
            SpawnPlayerLocalScale(new Vector3(newLocal.x - 0.76f, newLocal.y - 1, newLocal.z));

        }

        public void AddPlayer(Player player)
        {
            if (playerCount < maxPlayers)
            {
                playerCount++;
            }
          
        }

        public void EndGame(string nameLastAttacker)
        {
            playerCount--;
            if (playerCount > 1)
            {
              
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

        }

        [ClientRpc]
        private void RpcEndPanel(string nameLastAttacker)
        {
            GameManager.Instance.panelInfo.gameObject.SetActive(true);

            GameManager.Instance.panelInfoText.text = "KONIEC GRY Wygral gracz " + nameLastAttacker;

            StartCoroutine(ChangeScene());

            
        }
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);
        var lobby = Object.FindObjectOfType<Prototype.MyNetworkLobby.MyLobby>();
        float fTime = 3f;
        if(!NetworkServer.active)
        {
            yield return new WaitForSeconds(fTime);
            Object.Destroy(lobby.gameObject);

        }
        else
        {
            while (Network.connections.Length > 0)
                yield return null;
            
            NetworkServer.DisconnectAll();        
            Prototype.MyNetworkLobby.MyLobby.s_Singleton.StopHost();
            NetworkManager.Shutdown();
            Object.Destroy(lobby.gameObject);
        }

    }


}
    

