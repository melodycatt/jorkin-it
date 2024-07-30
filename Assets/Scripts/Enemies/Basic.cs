using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)

public class Basic : Enemy {
    //these should be mostly self explanatory
    public float FollowRange;
    public bool Following;
    public float AttackRange;

    public float Speed;

    public BoxCollider2D AttackTrigger;
    public CircleCollider2D FollowTrigger;
    public BoxCollider2D Hitbox;
    
    //same signature mentionsed in Enemy.cs, as well as the `base.Start()`
    override protected void Start() {
        base.Start();
        //basic init i think
        Hitbox = GetComponents<BoxCollider2D>().ToList().Find((x) => !x.isTrigger);
        AttackTrigger = GetComponents<BoxCollider2D>().ToList().Find((x) => x.isTrigger);
        FollowTrigger = GetComponent<CircleCollider2D>();
        //these constant values (like 0.85 and 0.425) can, and probably should in some cases, be changed
        //TODO: add a collider to not follow or attack when player is not in line of sight
        AttackTrigger.size = new(AttackRange, AttackRange * 0.85f);
        AttackTrigger.offset = new(0, AttackRange * 0.425f - 1.375f);
        FollowTrigger.radius = FollowRange;
    }

    protected override void Update() {
        //ai stuff, following variable is for when you dont want AI() to trigger again for a bit
        //(usually until the current AI() finishes)
        if (FollowTrigger.OverlapPoint(Player.position)) {
            if (!Following) {
                Following = true;
                StartCoroutine(AI());
            }
        } else {
            Following = false;
        };
    }

    //just moves towards player
    public override IEnumerator AI()
    {
        rb.velocity = new(Speed * ((Player.position.x - transform.position.x) / Math.Abs(Player.position.x - transform.position.x)), 0);
        Following = false;
        yield return null;
    }

    //TODO: make class abstract bc this is pointless
    //ACTUALLY TODO: add basic hurt functuality so i dont have to put it in each enemy
    public override IEnumerator Hurt()
    {
        throw new NotImplementedException();
    }
}