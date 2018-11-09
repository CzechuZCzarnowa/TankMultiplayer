using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusColliderSpawn : MonoBehaviour {

    public bool isOccupied = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
            if(collision.gameObject.tag == "BonusHealth")
        {

            isOccupied = true;
        }
        
     

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "BonusHealth")
        {

            isOccupied = false;
        }


    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            isOccupied = false;
        }

    }
}
