using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{

    [SerializeField] int bonusHpAmount = 5;
    [SerializeField] int bonusSpeedAmount = 2;

    public void GiveBonus(string tag, GameObject player)
    {
        switch (tag)
        {
            case "BonusHealth":
                PlayerHealth healtScript = player.GetComponent<PlayerHealth>();
                if (healtScript != null)
                {

                    healtScript.BonusHp(bonusHpAmount);


                }
            break;

            case "BonusSpeed":
                CharacterControler playerControl = player.GetComponent<CharacterControler>();
                if (playerControl != null)
                {
                    playerControl.BonusSpeed(bonusSpeedAmount);
                }


            break;
        }
    }
}
