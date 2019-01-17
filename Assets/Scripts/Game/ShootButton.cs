using UnityEngine.Networking;

public class ShootButton : NetworkBehaviour {

    public PlayerShooting pS;

    public void Shot()
    {
        if (pS.GetComponent<PlayerHealth>().isDead)
            return;
        var playerNetIdentity = pS.gameObject.GetComponent<NetworkIdentity>();  
        pS.GetComponent<PlayerShooting>().CmdShootBullet(playerNetIdentity.netId);
       
    }
}
