using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject Bulletprefab;
    public float speed;
    public int Damage = 10;

}
