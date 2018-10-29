using UnityEngine.Networking;

public class ShootButton : NetworkBehaviour {

    public PlayerShooting Player;

    public void TestSpawn()
    {     
        var playerNetIdentity = Player.gameObject.GetComponent<NetworkIdentity>();
        Player.GetComponent<PlayerShooting>().CmdShootBullet(playerNetIdentity.netId);
    }
}
