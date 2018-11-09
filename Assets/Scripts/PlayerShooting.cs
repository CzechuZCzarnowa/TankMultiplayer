using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShooting : NetworkBehaviour {

    public Transform firePosition;
    public Weapon currentWeapon;
    public GameObject canvas;
    public Rigidbody2D bulletPrefab;
   
    private void Start()
    {
        bulletPrefab = currentWeapon.Bulletprefab.GetComponent<Rigidbody2D>();       
        if (isLocalPlayer)
        {
           canvas.SetActive(true);
        }
        else
        {
            return;
        }
        GetComponentInChildren<ShootButton>().Player = this;
    }

    [Command]
    public void CmdShootBullet(NetworkInstanceId id)
    {
        
        Rigidbody2D rbody = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation) as Rigidbody2D;

        if (rbody != null)
        {
            rbody.velocity = currentWeapon.speed * firePosition.transform.up;
            NetworkServer.Spawn(rbody.gameObject);
            RpcObject(id, rbody.gameObject);

            if(isServer)
            {
            var t = ClientScene.FindLocalObject(id);
            rbody.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();

            }
        }

    }

    [ClientRpc]
    void RpcObject(NetworkInstanceId id,GameObject g)
    {
        var t = ClientScene.FindLocalObject(id);
        g.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();
    }

   

}
