using UnityEngine;

abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    
    // PlayableCharacter Variables / Ola 24.9.26
    [SerializeField] protected int speed;
    [SerializeField] protected int maxHp;
    protected int currentHp; //v 
    
    
    public virtual void ApplyDamage(int damage) //for now we dont know, maybe take 1 hp
    {
        currentHp -= damage;
    }

    // SpecialAbility()
    
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

    void Movement()
    {
        // Place holder for movment - using speed, decide on needed movement
    }

}
