using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
namespace Prototype.MyNetworkLobby
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {

        [SyncVar(hook = "OnMyName")]
        public string name = "";
        [SerializeField] Text nameText;


        [HideInInspector]
        public int _playerNumber = 0;



        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();



            LobbyPlayerList._lobbyPlayerList.AddPlayer(this);


            if (isServer)
            {

                SetupLocalPlayer();
            }
            else
            {

                SetupOtherPlayer();
            }

            OnMyName(name);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            SendReadyToBeginMessage();



            SetupLocalPlayer();
        }
        void SetupOtherPlayer()
        {
            if (name == "")
                CmdNameChanged("host" + (LobbyPlayerList._lobbyPlayerList.playerListContentTransform.childCount - 1));

        }

        void SetupLocalPlayer()
        {
            if (name == "")
                CmdNameChanged("Player" + (LobbyPlayerList._lobbyPlayerList.playerListContentTransform.childCount - 1));



        }
        public void OnReadyClicked()
        {
            Debug.Log("ready");
            SendReadyToBeginMessage();
        }
        [ClientRpc]
        public void RpcDiactivePanel()
        {
            MyLobby.s_Singleton.PanelDiactive();
        }
        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            MyLobby.s_Singleton.countdownText.text = countdown.ToString();

        }
        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {





            }
            else
            {





            }

        }
        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }
        public void OnMyName(string _name)
        {
            name = _name;
            nameText.text = name;
        }

        [Command]
        public void CmdNameChanged(string _name)
        {
            name = _name;
            nameText.text = name;
        }

    }
}