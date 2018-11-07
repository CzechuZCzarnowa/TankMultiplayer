using UnityEngine;

public class PlayerSpawnCollider : MonoBehaviour {

    public bool isOccupied = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            isOccupied = true;
        }



    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            isOccupied = false;
        }


    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            isOccupied = true;
        }

    }
}
