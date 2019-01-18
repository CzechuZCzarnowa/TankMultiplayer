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
        public string playerName = "";
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

            OnMyName(playerName);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            SendReadyToBeginMessage();
            SetupLocalPlayer();
        }

        void SetupOtherPlayer()
        {

            if (playerName == "")
                CmdNameChanged("host" + (LobbyPlayerList._lobbyPlayerList.playerListContentTransform.childCount - 1));

        }

        void SetupLocalPlayer()
        {

            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._lobbyPlayerList.playerListContentTransform.childCount - 1));

        }
        public void OnReadyClicked()
        {         
            SendReadyToBeginMessage();
        }

        [ClientRpc]
        public void RpcDiactivePanel()
        {
            MyLobby.s_Singleton.PanelStatus(false);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            MyLobby.s_Singleton.countdownText.text = countdown.ToString();

        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnMyName(string _name)
        {
            playerName = _name;
            nameText.text = playerName;
        }

        [Command]
        public void CmdNameChanged(string _name)
        {
            playerName = _name;
            nameText.text = playerName;
        }

        public void OnDestroy()
        {
            LobbyPlayerList._lobbyPlayerList.RemovePlayer(this);
        }
    }
}