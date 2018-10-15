using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {

    [SerializeField] private int damage = 5;
    [SerializeField] private float waitTime = .5f;
    Player player;
    // Update is called once per frame

    
    private void OnCollisionStay2D(Collision2D coll)
    {
        player = coll.gameObject.GetComponent<Player>();
        //Player player = coll.gameobject.GetComponent<Player>();
        if (player != null)
        {
            Invoke("HitPlayer", 0.5f);

        }
    }
    private void HitPlayer()
    {
        player.TakeDamage(damage);
    }

    //private void OnCollisionEnter2D(Collision2D coll)
    //{

    //    Player player = coll.gameObject.GetComponent<Player>();
    //    //Player player = coll.gameobject.GetComponent<Player>();
    //    if (player != null)
    //    {
    //        player.TakeDamage(damage);

    //    }
    //}
}
