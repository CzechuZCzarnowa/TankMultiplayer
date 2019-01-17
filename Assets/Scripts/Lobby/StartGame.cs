using UnityEngine;
using TMPro;
namespace Prototype.MyNetworkLobby
{
    public class StartGame : MonoBehaviour
    {
        public TextMeshProUGUI placeHolderNIck;
        public string namePlayer;
        [SerializeField]
        public Animator anim;
        public LobbyPlayer _lobbyplayer;
        public void TypeName(string name)
        {
            namePlayer = name;
        }

        public void StartAnim()
        {
            if (namePlayer != "")
            { 

                anim.SetBool("join", true);
                _lobbyplayer.CmdNameChanged(namePlayer);
            }
            else
            {
                placeHolderNIck.text = "Wprowadz nick";
            }

        }
        public void JoinGameAnim()
        {
            anim.SetBool("join", true);
        }

    }
}
