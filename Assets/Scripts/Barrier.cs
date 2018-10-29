using UnityEngine;

public class Barrier : MonoBehaviour {

    [SerializeField] private int damage = 5;
    [SerializeField] private float waitTime = .5f;
    private Player player;

 
    private void OnCollisionEnter2D(Collision2D coll)
    {
        player = coll.gameObject.GetComponent<Player>();
      
        if (player != null)
        {
            player.TakeDamage(damage);

        }
    }
    private void HitPlayer()
    {
        player.TakeDamage(damage);
    }

}
