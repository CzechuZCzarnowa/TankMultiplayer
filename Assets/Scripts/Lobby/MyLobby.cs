using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using TMPro;
using System.Collections;

namespace Prototype.MyNetworkLobby
{
    public class MyLobby : NetworkLobbyManager
    {

        static public MyLobby s_Singleton;


        [SerializeField]
        private uint roomSize = 4;
        private string roomName;
        [SerializeField]
        public TMP_InputField nameInput;
        [HideInInspector]
        public int _playerNumber = 0;
        public GameObject[] active;
        [SerializeField] Button startGameButton;
        public Text countdownText;
        public float prematchCountdown = 5.0f;

        protected MyLobbyHook _lobbyHooks;

        private void Awake()
        {
            s_Singleton = this;
        }


        // Use this for initialization
        void Start()
        {

            _lobbyHooks = GetComponent<Prototype.MyNetworkLobby.MyLobbyHook>();
            MMStart();
            MMListMatches();
            DontDestroyOnLoad(gameObject);
        }

        public void SetRoomName(string name)
        {
            roomName = name;

            if (matchMaker == null)
            {
                MMStart();
            }
        }

        void MMStart()
        {
            this.StartMatchMaker();
        }

        public void MMListMatches()
        {
            matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        }

        public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
        {
            base.OnMatchList(success, extendedInfo, matchList);

            if (!success)
            {
                Debug.Log("list failed" + extendedInfo);

            }
            else
            {
                if (matchList.Count > 0)
                {
                    Debug.Log("Successfully matches list");

                }
                else
                    MMCreateMatch();
            }
        }

        public void MMJoinMatch(MatchInfoSnapshot firstMatch)
        {
            this.matchMaker.JoinMatch(firstMatch.networkId, "", "", "", 0, 0, OnMatchJoined);

        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            GameObject.Find("MenuGame").GetComponent<Animator>().SetBool("start", true);
            base.OnMatchJoined(success, extendedInfo, matchInfo);

        }
        public void MMCreateMatch()
        {

            if (roomName != "" && roomName != null)
            {
                GameObject.Find("MenuGame").GetComponent<Animator>().SetBool("start", true);
                Debug.Log("Creating Room: " + roomName + "with room for" + roomSize + " players");
                matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, OnMatchCreate);


            }
        }
        public override void OnLobbyServerPlayersReady()
        {
            bool allready = true;
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                    allready &= lobbySlots[i].readyToBegin;
            }

            if (allready)
            {
                Debug.Log("ready wszyscy");
            }
            else
            {
                startGameButton.gameObject.SetActive(true);
            }
        }
        public void OnPlayersNumberModified(int count)
        {
            _playerNumber += count;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;


        }
        public void GameReady()
        {
            //StartCoroutine("ServerCountdownCoroutine", prematchCountdown);
            ServerChangeScene(playScene);
        }


        public IEnumerator ServerCountdownCoroutine(float time)
        {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);


            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            Debug.Log("BUG" + lobbySlots[i]);
                            LobbyPlayer p = lobbySlots[i] as LobbyPlayer;
                            p.RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }




            ServerChangeScene(playScene);

        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //This hook allows you to apply state data from the lobby-player to the game-player
            //just subclass "LobbyHook" and add it to the lobby object.

            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

            return true;
        }
        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            //for (int i = 0; i < lobbySlots.Length; ++i)
            //{
            //    if (lobbySlots[i] != null)
            //    {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
            //        lobbySlots[i].GetComponent<LobbyPlayer>().RpcDiactivePanel();
            //        //LobbyPlayer p = lobbySlots[i] as LobbyPlayer;
            //        //p.RpcDiactivePanel();
            //    }
            //}
            PanelDiactive();
        }
        public void PanelDiactive()
        {
            for (int i = 0; i < active.Length; i++)
            {
                active[i].SetActive(false);
            }

        }
    }
}
