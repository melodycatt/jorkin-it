using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)

//a basic boss class, because bosses have very diff behaviour to Basic enemies
public abstract class Boss : Enemy {
    protected readonly System.Random rand = new();

    public float AttackDelay = 5;
    public float AttackCooldown;
    public bool Attacking;

    protected override void Update() {
        if (AttackCooldown <= 0) {
            Attacking = true;
            AttackCooldown = AttackDelay;
            StartCoroutine(AI());
        } else if (!Attacking) {
            AttackCooldown -= Time.deltaTime;
        };
    }

}