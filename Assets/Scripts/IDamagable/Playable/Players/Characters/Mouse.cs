using UnityEngine;

public class Mouse : PlayableCharacter
{
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void TakeDamageFromTrap(IDamagable damagable) // take dmg from trap (?)
    {
        // maybe when passed through successfully, it can take damage from the character player and self destruct or die?
    } 
}
