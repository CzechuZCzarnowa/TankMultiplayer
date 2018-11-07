using UnityEngine;

public class Barrier : MonoBehaviour {

    [SerializeField] private int damage = 10;
    private Player player;

 
    private void OnCollisionEnter2D(Collision2D coll)
    {
        player = coll.gameObject.GetComponent<Player>();
        
        if (player != null)
        {
            player.TakeDamage(damage);
            Rigidbody2D rigi = coll.gameObject.GetComponent<Rigidbody2D>();
            if (rigi != null)
            {
                rigi.AddForce(new Vector2(-coll.transform.position.x/2, -coll.transform.position.y/2), ForceMode2D.Impulse);
            }

        }
    }
    private void HitPlayer()
    {
        player.TakeDamage(damage);
    }

}
