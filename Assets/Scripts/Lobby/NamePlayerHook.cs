using UnityEngine;
using UnityEngine.Networking;
using Prototype.MyNetworkLobby;

public class NamePlayerHook : MyLobbyHook
{

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager,
            GameObject lobbyPlayer,
            GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NamePlayer tank= gamePlayer.GetComponent<NamePlayer>();
        tank.m_playerName = lobby.name;
        
    }
}
