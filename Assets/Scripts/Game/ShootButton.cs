using UnityEngine.Networking;

public class ShootButton : NetworkBehaviour {

    public PlayerShooting Player;

    public void TestSpawn()
    {
        if (Player.GetComponent<PlayerHealth>().isDead)
            return;
        var playerNetIdentity = Player.gameObject.GetComponent<NetworkIdentity>();  
        Player.GetComponent<PlayerShooting>().CmdShootBullet(playerNetIdentity.netId);
    }
}
