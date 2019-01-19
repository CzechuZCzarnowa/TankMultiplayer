using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using TMPro;

using UnityEngine.SceneManagement;

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
        public TextMeshProUGUI placeHolder;
        protected MyLobbyHook _lobbyHooks;
        public LobbyPlayerList _lobbyPlayerList;
        public ulong _currentMatchID;

        private void Awake()
        {
          
            if (s_Singleton == null)
            {
                s_Singleton = this;
                
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            //NetworkManager.singleton = this;

            _lobbyHooks = GetComponent<Prototype.MyNetworkLobby.MyLobbyHook>();
      
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
                
                matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, OnMatchCreate);
            


            }
            else
            {
                placeHolder.text = "Wprowadz nazwe";

            }
        }


        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            _currentMatchID = (System.UInt64)matchInfo.networkId;
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
            
            ServerChangeScene(playScene);
        }



        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            Debug.Log("StopClient");
            StopClient();
            NetworkManager.singleton.StopMatchMaker();
            Destroy(this.gameObject);
        }
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            Debug.Log("StopHost");
            StopHost();
            NetworkManager.singleton.StopMatchMaker();
            _lobbyPlayerList.Remove();
            Destroy(this.gameObject);
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {

            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

            return true;
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {

            if(SceneManager.GetSceneAt(0).name != lobbyScene)
            {
                PanelStatus(false);
            }
            else
            {
                PanelStatus(true);
            }
      
        }

        public void PanelStatus(bool status)
        {
            for (int i = 0; i < active.Length; i++)
            {
                active[i].SetActive(status);
            }

        }
    }
    
   
}
