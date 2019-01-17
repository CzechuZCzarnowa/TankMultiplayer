using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShooting : NetworkBehaviour {

    public Transform firePosition;
    public Weapon[] currentWeapon;
    public GameObject canvas;
    public Rigidbody2D bulletPrefab;
    private bool canShoot = true;
    public Button shotButton;
   [SyncVar] private int currentWeaponIndex = 0;
    private void Start()
    {
        bulletPrefab = currentWeapon[currentWeaponIndex].Bulletprefab.GetComponent<Rigidbody2D>();       
        if (isLocalPlayer)
        {
           canvas.SetActive(true);
        }
        else
        {
            return;
        }
        GetComponentInChildren<ShootButton>().pS = this;
    }

    [Command]
    public void CmdShootBullet(NetworkInstanceId id)
    {
        
        if (!canShoot)
            return;


        Rigidbody2D rbody = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation) as Rigidbody2D;

        if (rbody != null)
        {
            rbody.velocity = currentWeapon[currentWeaponIndex].speed * firePosition.transform.up;
            NetworkServer.Spawn(rbody.gameObject);
            RpcObject(id, rbody.gameObject);

            if(isServer)
            {
            var t = ClientScene.FindLocalObject(id);
            rbody.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();

            }
        }
        StartCoroutine(ReloadGun());
    }

    [ClientRpc]
    void RpcObject(NetworkInstanceId id,GameObject g)
    {
        var t = ClientScene.FindLocalObject(id);
        g.GetComponent<BulletControler>().playerShoot = t.GetComponent<PlayerShooting>();
    }

    public void IncrementWeaponIndex()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex == currentWeapon.Length)
        {
            currentWeaponIndex = currentWeapon.Length - 1;
        }
        bulletPrefab = currentWeapon[currentWeaponIndex].Bulletprefab.GetComponent<Rigidbody2D>();

    }
    public int ActualWeapon()
    {
        return currentWeaponIndex;
    }

    IEnumerator ReloadGun()
    {
        canShoot = false;
        shotButton.interactable = false;
      
        yield return new WaitForSeconds(currentWeapon[currentWeaponIndex].timeReload);
        shotButton.interactable = true;
        canShoot = true;
    }
    
}
