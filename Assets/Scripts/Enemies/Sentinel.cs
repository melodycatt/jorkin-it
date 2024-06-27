using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)
//IF YOU ARE STARTING HERE GO TO ENEMY FIRST AND GO DOWN THE INHERITANCE CHAIN (the order of what classes extend what)

//boss 1
//use this as a ref for how i did stuff
public class Sentinel : Boss {
    //knife is actually a fireball and no i wont change that its funnt
    public GameObject Knife;
    public GameObject Target;

    public int nKnives = 3;
    public float knifeDelay = 0.75f;
    public float knifeSpeed = 25;

    override protected void Start() {
        //basic init
        base.Start();
        Knife = Resources.Load<GameObject>("Prefabs/Boss/Knife");
        Target = Resources.Load<GameObject>("Prefabs/Boss/Target");
        Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public override IEnumerator AI()
    {
        //stop moving
        rb.velocity = new(0, rb.velocity.y);
        //pick random attack
        int Attack = (int)Math.Round(rand.NextDouble());

        if (Attack == 0) {
            //dont let the knives fool you theyre not fireballs >:)
            for (int i = 0; i < nKnives; i++) {
                //spawn knife in dir of player, at sentinel y (with some offset)
                GameObject tempKnife = Instantiate(Knife);
                tempKnife.transform.position = new(transform.position.x + 2 * ((Player.position.x - transform.position.x) / Math.Abs(Player.position.x - transform.position.x)), transform.position.y + ((float)rand.NextDouble() - 0.5f), transform.position.z);

                //rotate knife to face player (this could be done better idc)
                //sometimes knives on the left would just shoot right, and it was because i didnt have Round() and it would be -0.99999
                //i hate floats
                if (Mathf.Round((Player.position.x - transform.position.x) / Math.Abs(Player.position.x - transform.position.x)) == -1) {
                    tempKnife.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                tempKnife.GetComponent<Fireball>().KnifeSpeed = knifeSpeed;
                tempKnife.transform.parent = transform;
                yield return new WaitForSeconds(knifeDelay);
            }
        } else {
            //that target trap attack i think its simple
            GameObject tempTarget = Instantiate(Target);
            tempTarget.GetComponent<Target>().timer = 1.5f;
            tempTarget.GetComponent<Target>().targetSize = 4;
            tempTarget.GetComponent<Target>().timerScale = 3;
            tempTarget.transform.position = Player.position;
            tempTarget.transform.parent = transform;
        }
        //dont move for a bit after the attack
        yield return new WaitForSeconds(0.5f);
        Attacking = false;
    }

    new void Update() {
        //base.Method() to keep functionality of parent but add to it
        base.Update();
        //move to player if theyre not too close
        if (!Attacking && Mathf.Abs(Player.position.x - transform.position.x) > 5) {
            rb.velocity = new(3 * ((Player.position.x - transform.position.x) / Math.Abs(Player.position.x - transform.position.x)), rb.velocity.y);
        }
    }

    public override IEnumerator Hurt() {
        Sprite.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        Sprite.color = Color.yellow;
    }
}