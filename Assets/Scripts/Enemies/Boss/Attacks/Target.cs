using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    //references - target circle is inner circle, timer circle is outer circle, player is player, collider is this collider
    public Transform targetCircle;
    public Transform timerCircle;
    public Transform Player;
    new public CircleCollider2D collider;

    public float targetSize = 3;
    public float timerScale = 3;
    public float timer = 1;
    //movespeed not timer speed
    public float speed = 0.25f;

    public float timeLeft;

    public LayerMask pLayerMask;

    void Start() {
        //basic init i think
        targetCircle = transform.GetChild(0);
        timerCircle = transform.GetChild(1);
        
        pLayerMask = 1 << LayerMask.NameToLayer("Player");

        Player = transform.parent.GetComponent<Boss>().Player;

        timeLeft = timer;

        collider = GetComponent<CircleCollider2D>();
        collider.radius = targetSize / 2;

        targetCircle.localScale = new Vector3(targetSize, targetSize);
        timerCircle.localScale = new Vector3(targetSize + timerScale, targetSize + timerScale);
        StartCoroutine(Close());
    }

    IEnumerator Close() {
        //timer down every frame, timer cirle smaller every frame
        //multiplied by the timer circle's scale because if the circle is bigger it needs to travel a larger distance in the same time
        //divided by the timer because it needs to be slower to last longer
        while (timeLeft > 0) {
            timerCircle.localScale -= new Vector3(timerScale / timer * Time.deltaTime, timerScale / timer * Time.deltaTime);
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        //check hits
        ContactFilter2D filter = new();
        filter.SetLayerMask(pLayerMask);
        List<Collider2D> results = new();
        if(collider.OverlapCollider(filter, results) > 0) {
            targetCircle.GetComponent<SpriteRenderer>().enabled = false;
            timerCircle.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Attack());
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator Attack() {
        //freeze player physics, hurt (via the detached method for no cutoff) every .5 secs, unfreeze and die
        PlayerMovement pMov = Player.GetComponent<PlayerMovement>();
        pMov.rb.simulated = false;
        yield return new WaitForSeconds(0.5f);
        pMov.DetatchedHurt(0.5f);
        yield return new WaitForSeconds(0.5f);
        pMov.DetatchedHurt(0.5f);
        yield return new WaitForSeconds(0.5f);
        pMov.DetatchedHurt(1);
        pMov.rb.simulated = true;
        Destroy(gameObject);
        print("AAA");
    }

    // move veeerryyyy slowllyyyy to player
    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
    }
}