using UnityEngine;

public class BonusScript : MonoBehaviour {




    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {

            BonusController bonusControler = GameManager.Instance.GetComponent<BonusController>();
            bonusControler.GiveBonus(this.tag, collision.gameObject);
                
         
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }

      

    }
}

