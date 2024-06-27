using System.Collections;
using UnityEngine;

//the base enemy class, has util values
//abstract means it has declarations of methods (just with no functionality), and any class extending this should give functionality
public abstract class Enemy : MonoBehaviour {

    //all stuff that every enemy would use
    public int Health;
    public int MaxHealth;

    public Rigidbody2D rb;

    public Transform Player;
    public SpriteRenderer Sprite;


    //virtual is a memory thing hard to explain without detail (if you dont know it)
    //just make sure every enemy start function uses the signature `protected override void Start()`
    //and probably uses `base.Start();` (unless you are trying to completely rewrite the start functionality)
    protected virtual void Start() {
        Player = GameObject.Find("Player  Stuff").transform.Find("Player");
        Health = MaxHealth;
        rb = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    //if you want to make another general class like this one which doesnt have its own ai or hurt functionality,
    //make sure the class is abstract and the function is abstract
    public abstract IEnumerator AI();

    public abstract IEnumerator Hurt();
}