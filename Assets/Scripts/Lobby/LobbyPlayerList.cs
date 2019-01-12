using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototype.MyNetworkLobby
{
    public class LobbyPlayerList : MonoBehaviour
    {

        protected List<LobbyPlayer> _players = new List<LobbyPlayer>();
        public RectTransform playerListContentTransform;
        static public LobbyPlayerList _lobbyPlayerList;
        //public Transform addButtonRow;

        public void OnEnable()
        {
            _lobbyPlayerList = this;
        }


        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);
            GameObject.Find("MenuGame").GetComponent<StartGame>()._lobbyplayer = player;
            player.transform.SetParent(playerListContentTransform, false);
          


        }


    }
}