using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 4;
    private string roomName;
    private NetworkManager networkManager;
 
    //NetworkMatch matchMaker;
    //private void Awake()
    //{
    //    matchMaker = gameObject.AddComponent<NetworkMatch>();
    //}
    public void SetRoomName(string name)
    {
        roomName = name;
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + "with room for" + roomSize + " players");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "","","",0,0,networkManager.OnMatchCreate);
            
        }
       
    }

}
