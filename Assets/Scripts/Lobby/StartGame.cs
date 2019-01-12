using UnityEngine;
using TMPro;
namespace Prototype.MyNetworkLobby
{
    public class StartGame : MonoBehaviour
    {

        public string namePlayer;
        [SerializeField]
        Animator anim;
        public LobbyPlayer _lobbyplayer;
        public void TypeName(string name)
        {
            namePlayer = name;
        }

        public void StartAnim()
        {
            if (namePlayer != null)
            {
                if (anim == null)
                {
                    anim = GameObject.Find("MenuGame").GetComponent<Animator>();
                }

                anim.SetBool("join", true);
                _lobbyplayer.CmdNameChanged(namePlayer);
            }

        }
        public void JoinGameAnim()
        {
            anim.SetBool("join", true);
        }

    }
}
