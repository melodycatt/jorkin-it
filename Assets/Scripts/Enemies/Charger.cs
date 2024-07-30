using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)

//an enemy that charges at you
public class Charger : Basic {
    //self explanatory
    public bool Charging;

    protected override void Update() {
        base.Update();
        if (AttackTrigger.OverlapPoint(Player.position)) {
            attackManager.Attack(Player.GetComponent<HealthManager>());
        } 
    }

    public override IEnumerator AI()
    {
        if (Charging) {
            yield break;
        }
        Charging = true;
        //do a little startled jump
        rb.velocity = new(0, 6);
        //wait until you land and then a bit more
        yield return new WaitUntil(() => rb.velocity.y == 0);
        yield return new WaitForSeconds(0.3f);

        //save current player pos so it charges past you if you jump over,
        //but given that it always overshoots anyways, im not sure i like this
        Vector3 ogPos = Player.position;
        //lower the follow range so it doesnt immediately charge again unless youre really close
        FollowTrigger.radius = FollowRange / 2;
        //permanent direction so it doesnt just turn around mid charge
        float dir = (ogPos.x - transform.position.x) / Math.Abs(ogPos.x - transform.position.x);
        //this seems yandere dev but i think theres a reason?
        if (ogPos.x > transform.position.x) {
            while(transform.position.x < ogPos.x) {
                rb.velocity = new Vector2(Speed, rb.velocity.y);
                yield return null;
            }
        } else {
            while(transform.position.x > ogPos.x) {
                rb.velocity = new Vector2(-Speed, rb.velocity.y);
                yield return null;
            }
        }
        //overshoot the charge for 30 frames
        for (int i = 0; i < 30; i++) {
            rb.velocity = new Vector2(Speed * dir, rb.velocity.y);
            yield return null;
        }
        Charging = false;
        Following = false;
        //take a second to recover and charge again (unless theyre close and youre already charging)
        yield return new WaitForSeconds(1);
        if (!Charging) {
            FollowTrigger.radius = FollowRange;
        }
    }
}