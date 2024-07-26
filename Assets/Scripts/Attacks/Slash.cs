using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public AttackManager attackManager;
    //references - see Knife.cs (it has the same references)
    public new Collider2D collider;
    public LayerMask EnemyLayer;
    public Transform Player;

    //has this hit anything
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        //basic init
        collider = GetComponent<Collider2D>();
        EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        Player = transform.parent.parent.Find("Player");
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        //move slightly. ill probs remove this maybe
        transform.position += transform.rotation * new Vector3(0.01f, 0);

        //check for hits. TODO: keep track of what enemies are hit so you dont hit them a gazillion times in one slash
        ContactFilter2D filter = new();
        filter.SetLayerMask(EnemyLayer);
        List<Collider2D> hits = new();
        int nhits = collider.OverlapCollider(filter, hits);
        foreach (Collider2D hit in hits) {
            attackManager.Attack(hit.GetComponent<HealthManager>());
        }
        if (nhits > 0 && !hit) {
            //recoil. this doesnt work, and i dont know how to make it work
            hit = true;
            Player.GetComponent<PlayerMovement>().rb.velocity -= (Vector2)(transform.rotation * new Vector3(Player.GetComponent<PlayerMovement>().Speed, 0));
        }
    }

    IEnumerator Die() {
        for (int i = 0; i < 20; i++) {
            yield return null;
        }
        Destroy(gameObject);
    }
}
