using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//this used to be swords / knives so thats why it says that stuff sometimes
class Fireball : MonoBehaviour {

    //references, self explanatory
    public Transform Player;
    public SpriteRenderer sprite;

    public float KnifeSpeed = 20;
    public float Velocity;
    public float Acceleration = 8;
    public float Delay = 0.75f;
    public bool Moving;

    // player layer mask = pLayerMask
    public LayerMask pLayerMask;

    void Start() {
        //basic init
        Player = transform.parent.GetComponent<Boss>().Player;
        sprite = GetComponent<SpriteRenderer>();
        pLayerMask = 1 << LayerMask.NameToLayer("Player");
        Velocity = KnifeSpeed * 0.4f;
        //stays still for a little bit
        StartCoroutine(MoveDelay());
    }

    IEnumerator MoveDelay() {
        yield return new WaitForSeconds(Delay);
        Moving = true;
    }

    void Update() {
        //btw, youll see these !checks with returns a lot. they make the code MUCh more readable (as opposed to putting the whole function inside an if)
        if(!Moving) {
            return;
        }
        //TODO: make this horizontal AND vertical (im lazy)
        if(IsOffScreenHorizontally()) {
            Destroy(gameObject);
            return;
        }
        //turn towards player (twice as fast as knife)
        transform.right = Vector3.MoveTowards(transform.right, Player.position - transform.position, Time.deltaTime);
        //accelerate and move
        if (Velocity < KnifeSpeed) {
            Velocity += Acceleration * Time.deltaTime;
        }
        transform.position += transform.rotation * new Vector3(Velocity, 0, 0) * Time.deltaTime;

        //check hits
        ContactFilter2D filter = new();
        filter.SetLayerMask(pLayerMask);
        List<Collider2D> results = new();
        if(GetComponent<Collider2D>().OverlapCollider(filter, results) > 0) {
            //this is the end goal of the TODO in knife.cs
            //this insures the the hurt coroutine isn't cut off
            Player.GetComponent<PlayerMovement>().DetatchedHurt(1);
        }
    }

    bool IsOffScreenHorizontally() {
        var bounds = sprite.bounds;

        var top = Camera.main.WorldToViewportPoint(bounds.max);
        var bottom = Camera.main.WorldToViewportPoint(bounds.min);
        return top.x < 0 || bottom.x > 1;    
    }
}