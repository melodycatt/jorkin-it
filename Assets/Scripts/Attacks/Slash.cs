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

    public float lifetime = 0.15f;
    public float recoilScale = 8;

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
            Player.GetComponent<PlayerMovement>().recoil = true;
            Player.GetComponent<PlayerMovement>().rb.velocity = transform.rotation * new Vector2(-recoilScale, 0);
        }
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
