using UnityEngine.Networking;
using TMPro;

public class NamePlayer : NetworkBehaviour {


    [SyncVar]public string m_playerName;
    public TextMeshProUGUI namePlayer;

    void Start ()
    {
    namePlayer.text = m_playerName;
        
    }

}
