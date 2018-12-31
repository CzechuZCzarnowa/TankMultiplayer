
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
namespace Prototype.MyNetworkLobby
{
    public class JoinGame : MonoBehaviour
    {

        private MyLobby networkManager;
        private List<GameObject> roomList = new List<GameObject>();
        [SerializeField]
        private TextMeshProUGUI status;
        [SerializeField]
        private GameObject roomListItemPrefab;
        [SerializeField]
        private Transform roomListParent;
        private void Start()
        {
            networkManager = MyLobby.s_Singleton;
            if (networkManager.matchMaker == null)
            {
                networkManager.StartMatchMaker();
            }
            RefreshRoomList();
        }

        public void RefreshRoomList()
        {
            ClearRoomList();
            networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            status.text = "Loading";
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            status.text = "";
            if (roomList == null)
            {
                status.text = "Couldn't get room list";
                return;
            }

            foreach (MatchInfoSnapshot item in matches)
            {
                GameObject roomListItemGO = Instantiate(roomListItemPrefab);
                roomListItemGO.transform.SetParent(roomListParent);
                RoomListItem _roomListItem = roomListItemGO.GetComponent<RoomListItem>();
                if (_roomListItem != null)
                {
                    _roomListItem.Setup(item, JoinRoom);

                }
                roomList.Add(roomListItemGO);

            }

            if (roomList.Count == 0)
            {
                status.text = "No rooms at the moment";
            }

        }



        private void ClearRoomList()
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                Destroy(roomList[i]);
            }
            roomList.Clear();
        }
        public void JoinRoom(MatchInfoSnapshot _match)
        {
            networkManager.MMJoinMatch(_match);
            ClearRoomList();
            status.text = "Joining";
        }

    }
}