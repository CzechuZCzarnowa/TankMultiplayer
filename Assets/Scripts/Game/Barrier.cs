using UnityEngine;

public class Barrier : MonoBehaviour {

    [SerializeField] private int damage = 10;
    private Player player;
    [SerializeField] private float force = 5f;
 
    private void OnCollisionEnter2D(Collision2D coll)
    {
        player = coll.gameObject.GetComponent<Player>();
        
        if (player != null)
        {
            PlayerHealth playerHealth = coll.gameObject.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Rigidbody2D rigi = coll.gameObject.GetComponent<Rigidbody2D>();
            if (rigi != null)
            {
                
                //Do poprawy - jak x>0 i y<0 odbija pod kątem 
                rigi.AddForce(new Vector2(-Mathf.Sign(coll.transform.position.x)*force, -Mathf.Sign(coll.transform.position.y) * force), ForceMode2D.Impulse);
            }

        }
    }


}
