using UnityEngine;

public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    
    // PlayableCharacter Variables / Ola 24.9.26
    [SerializeField] public float speed;
    [SerializeField] public int maxHp;
    [SerializeField] public int currentHp; //v 
    
    
    public virtual void ApplyDamage(int damage) //for now we dont know, maybe take 1 hp
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }
    // "die()" will probably move to the player itself, this one is just an abstract class for playable characters
    public virtual void Die()
    {
        // Place holder - decide what happens on death (respawn, game over, animation)
    }

    public virtual void Movement()
    {
        // Implementation for player movement
    }

    public virtual void SpecialAbility()
    {
        // Implementation for special ability
    }
    
    //  Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set hp and maybe speed to default state?
        currentHp = maxHp;
    }

    //  Update is called once per frame
    void Update()
    {
        Movement();
    }


}
