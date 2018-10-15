using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
public class NamePlayerHook : LobbyHook
{

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager,
            GameObject lobbyPlayer,
            GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NamePlayer tank= gamePlayer.GetComponent<NamePlayer>();

        tank.m_playerName = lobby.playerName;
    }
}
