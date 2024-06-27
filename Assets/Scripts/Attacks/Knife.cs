using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;

public class Knife : MonoBehaviour {

    //values for the smooth movement of knives
    public Vector3 targetPosition;
    //this one is so that the wobble doesnt affect other movements AND adding the wobble doesnt change the base position so the wobble doesnt stack on itself
    public Vector3 realTargetPosition;

    //references to stuff - collider is this knife's collider, target is the targeted enemy, enemylayer is the sorting layer for enemies
    new public Collider2D collider;
    public Transform target;
    public LayerMask EnemyLayer;

    private readonly System.Random rand = new System.Random();

    //true if the knife is moving (as opposed to floating above player)
    public bool Shooting;

    //hardcoded position values for floating above head (i promise this isnt yanderedev it actually made sense)
    public static Vector3[][] positions = new Vector3[][] {
        new Vector3[] {
            new(0,2)
        },
        new Vector3[] {
            new(-0.74355707527f, 1.77091205131f),
            new(0.74355707527f, 1.77091205131f),
        },
        new Vector3[] {
            new(-1.1313708499f,1.41421356237f),
            new(0,2),
            new(1.1313708499f,1.41421356237f),
        },
        new Vector3[] {
            new(-1.52169042607f, 0.61803398875f),
            new(-0.74355707527f, 1.77091205131f),
            new(0.74355707527f, 1.77091205131f),
            new(1.52169042607f, 0.61803398875f),
        },
        new Vector3[] {
            new(-1.6f,0),
            new(-1.1313708499f,1.41421356237f),
            new(0,2),
            new(1.1313708499f,1.41421356237f),
            new(1.6f,0),
        },
    };

    void Start() {
        collider = GetComponent<Collider2D>();
        EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        //transform.localPosition = Vector3.zero;

        StartCoroutine(Wobble());

        //find target (if any enemies are close enough)
        //(closest enemy)
        Collider2D[] nearColliders = Physics2D.OverlapCircleAll(transform.position, 10, EnemyLayer);
        if(nearColliders.Length > 0) {
            Collider2D closest = nearColliders[0];
            float distance = Vector3.Distance(closest.transform.position, transform.position);
            foreach(Collider2D x in nearColliders) {
                if (Vector3.Distance(x.transform.position, transform.position) < distance) {
                    distance = Vector3.Distance(x.transform.position, transform.position);
                    closest = x;
                }
            }
            target = closest.transform;
        }
    }

    IEnumerator Wobble() {
        //this should be self explanatory i think
        int wobbleIndex = 0;
        Vector3[] wobbles = {
            new(0f, 0.05f, 0),
            new(0.05f, 0f, 0),
            new(0f, -0.05f, 0),
            new(-0.05f, 0f, 0),
        };
        while (true) {
            realTargetPosition = targetPosition + wobbles[wobbleIndex] + new Vector3((float)(rand.NextDouble() / 50), (float)(rand.NextDouble() / 50));
            wobbleIndex = (int)Math.Round(rand.NextDouble() * 3);
            yield return new WaitForSeconds(0.2f);
        }
    }

    bool IsOffScreen() {
        var bounds = collider.bounds;
        var top = Camera.main.WorldToViewportPoint(bounds.max);
        var bottom = Camera.main.WorldToViewportPoint(bounds.min);
        return top.x < 0 || bottom.x > 1 || top.y < 0 || bottom.y > 1;    
    }

    void Update() {
        if (!Shooting) {
            //interpolate position (move smoothly)
            transform.localPosition = Vector3.Lerp(transform.localPosition, realTargetPosition, Sigmoid(Vector3.Distance(transform.localPosition, realTargetPosition) * Time.deltaTime));
            return;
        }
        if(IsOffScreen()) {
            Destroy(gameObject);
        }

        //if target wasnt found on start, search for closest enemy when you start shooting
        if (target == null) {
            Collider2D[] nearColliders = Physics2D.OverlapCircleAll(transform.position, 10, EnemyLayer);
            if(nearColliders.Length > 0) {
                Collider2D closest = nearColliders[0];
                float distance = Vector3.Distance(closest.transform.position, transform.position);
                foreach(Collider2D x in nearColliders) {
                    if (Vector3.Distance(x.transform.position, transform.position) < distance) {
                        distance = Vector3.Distance(x.transform.position, transform.position);
                        closest = x;
                    }
                }
                target = closest.transform;
            }
        } else {
            //turn veeerryyy slowly towards target
            transform.up = Vector3.MoveTowards(transform.up, target.position - transform.position, Time.deltaTime / 2);
        }
        //move forward
        transform.position += 20 * Time.deltaTime * transform.up;

        //check for hits
        ContactFilter2D filter = new();
        filter.SetLayerMask(EnemyLayer);
        List<Collider2D> hits = new();
        int nhits = collider.OverlapCollider(filter, hits);
        foreach (Collider2D hit in hits) {
            //TODO: make the enemies start the coroutine, not the knife (because when the knife hits it destroys itself, ending the coroutine early)
            StartCoroutine(hit.GetComponent<Enemy>().Hurt());
        }
        if (nhits > 0) {
            Destroy(gameObject);
        }
    }

    //for interpolation. maths.
    float Sigmoid(float t) {
        return 1.05f / (1 + Mathf.Exp(-6 * (t*10 - 0.5f)));
    }

    // go to correct position above player
    public void Arrange(int index, int total) {
        targetPosition = positions[total - 1][index];
    }

    // go to correct position instantly
    public void InstantArrange(int index, int total) {
        transform.localPosition = positions[total - 1][index];
        targetPosition = transform.localPosition;
    }
}