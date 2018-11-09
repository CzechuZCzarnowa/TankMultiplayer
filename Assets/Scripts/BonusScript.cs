using UnityEngine;

public class BonusScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {
            PlayerHealth healtScript = collision.GetComponent<PlayerHealth>();
            if (healtScript != null)
            {
                switch (this.tag)
                {
                    case "BonusHealth":

                        if (healtScript != null)
                        {
                            healtScript.BonusHp(10);
                        }

                        break;

                }
            }
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }

      

    }

}
