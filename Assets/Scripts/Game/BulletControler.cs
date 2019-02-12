using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControler : NetworkBehaviour
{
    private Rigidbody2D rigi;
    private BoxCollider2D boxCollider;
    private int damage;
    private float lifetime = 0.7f;
    public string nameP;
    public Weapon currentWeapon;
    public PlayerShooting playerShoot;
    public ParticleSystem exlodeEfect;
    public ParticleSystem fireEfect;
    void Start()
    {      
        currentWeapon = playerShoot.currentWeapon[playerShoot.ActualWeapon()];
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        damage = currentWeapon.Damage;
        rigi = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine("SelfDestruct"); 
    }


    IEnumerator SelfDestruct()
    {
        Efect();
        yield return new WaitForSeconds(lifetime);
        
        Destruct();
    }

    private void Efect()
    {
        if (exlodeEfect != null)
        {
            fireEfect.transform.parent = null;
            fireEfect.Play();
        }
    }

    private void Destruct()
    {
        rigi.velocity = Vector2.zero;
        boxCollider.enabled = false;
        rigi.Sleep();
        
        if(exlodeEfect !=null)
        {
            exlodeEfect.transform.parent = null;
            exlodeEfect.Play();
        }
        BulletDestroy();
    }

    private void BulletDestroy()
    {
        if(isServer)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D colInfo)
    {


        if (exlodeEfect != null && !colInfo.CompareTag("SpawnPoint") && !colInfo.CompareTag("Spawn"))
        {
            exlodeEfect.transform.parent = null;
            exlodeEfect.Play();
        }
        if (!isServer)
            return;
       
        PlayerHealth playerHealth = colInfo.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, playerShoot);
            Destruct();

        }
       
    }

    
}
