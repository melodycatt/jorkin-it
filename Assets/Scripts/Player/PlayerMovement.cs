using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    //do i really have to comment all these fields? figure it out 
    public SpriteRenderer Sprite;
    public HealthManager healthManager;
    public AttackManager attackManager;

    public float Speed;
    public float JumpSpeed;
    public Rigidbody2D rb;
    public bool Grounded;
    public bool Jumping;
    public bool JumpRequested;
    public LayerMask GroundLayer;

    public float fallMultiplier = 2f;
    public float lowJumpMultiplier = 2f;
    public Vector2Int Facing = new(0, 0);

    public bool airJump;

    private float _health;
    public float Health {
        get => _health;
        set {
            HealthMeter.text = value.ToString();
            _health = value;
        }
    }
    public TextMeshProUGUI HealthMeter;
    public float MaxHealth;
    public bool Hurting = false;

    // Start is called before the first frame update
    void Start()
    {
        attackManager = GetComponent<AttackManager>();
        healthManager = GetComponent<HealthManager>();
        Sprite = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        GroundLayer = 1 << LayerMask.NameToLayer("Ground");
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //set grounded and get keys
        Grounded = Physics2D.Raycast(transform.position, Vector2.down, Sprite.bounds.size.y / 2 + 0.1f, GroundLayer).collider != null;
        Tuple<int, Vector2Int> movement = GetMovement(); 

        //set facing direction
        Facing.y = movement.Item2.y;
        if (movement.Item2.x != 0) Facing.x = movement.Item2.x;

        //horizontal movment
        Vector2 velocity = Vector2.zero;
        velocity.x = Speed * movement.Item1;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        // if you touched the ground (i honestly have no idea at all why its checking for the jump key
        // but it breaks if you remove it)
        // this is my coconut.jpg
        if (Grounded && Jumping && !Input.GetKey(keybinds.Jump)) {
            airJump = false;
            Jumping = false;
            rb.gravityScale = 1;
        }
        if (Input.GetKeyDown(keybinds.Jump)) {
            StartCoroutine(Jump());
        }

        /*if(Input.GetKey(keybinds.Jump) && Grounded) {
            Grounded = false;
            Jumping = true;
            rb.velocity = new(0, JumpSpeed);
            StartCoroutine(Jump());
        }*/
    }
    IEnumerator Jump() {
        //coyote frames
        //you can press jump max 9 frames before you hit the ground and itll still jump
        for (int i = 0; i < 8; i++ ) {
            if(Grounded || airJump) {
                airJump = !airJump;
                rb.gravityScale = 1;
                rb.velocity = new(rb.velocity.x, JumpSpeed);
                Grounded = false;
                Jumping = true;
                StartCoroutine(LowJump());
                break;
            }
            yield return null;
        }
    }

    IEnumerator LowJump() {
        //delay so that tapping the key doesnt just make you do the worlds tiniest jump
        yield return new WaitForSeconds(0.1f);
        if(!Input.GetKey(keybinds.Jump)) {
            rb.gravityScale = lowJumpMultiplier;
            rb.velocity = new(rb.velocity.x, 0);
            yield break;
        }
        while (Jumping) {
            //fall a little slower if you never let go of the space key early,
            //and have an actual jump CURVE and not.. wave? spike? ratchet? idk
            if (rb.velocity.y <= 0) {
                rb.gravityScale = fallMultiplier;
                break;
            }
            //fast fall on letting go key
            else if(Input.GetKeyUp(keybinds.Jump) && rb.gravityScale == 1) {
                rb.gravityScale = lowJumpMultiplier;
                rb.velocity = new(rb.velocity.x, 0);
                break;
            } 
            yield return null;
        }
    }

    //similar dumbness to knifecontroller
    [Serializable]
    public struct Keybinds
    {
        public KeyCode Right;
        public KeyCode Left;
        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Jump;
    }
    public Keybinds keybinds = new() {
        Right = KeyCode.D,
        Left = KeyCode.A,
        Jump = KeyCode.Space,
    };

    //some of this is surely unneccessary but idc
    public IEnumerator Hurt(float damage) {
        if (!Hurting) {
            Hurting = true;
            Health -= damage;
            Sprite.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            Sprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            Hurting = false;
        }
    }

    //detached hurt mentioned in many other files
    public void DetatchedHurt(float damage) {
        StartCoroutine(Hurt(damage));
    }

    //get movement keys
    Tuple<int, Vector2Int> GetMovement() {
        int outputX = 0;
        Vector2Int outputDown = Vector2Int.zero;
        if (Input.GetKey(keybinds.Left)) outputX -= 1;
        if (Input.GetKeyDown(keybinds.Left)) outputDown.x -= 1;
        if (Input.GetKey(keybinds.Right)) outputX += 1;
        if (Input.GetKeyDown(keybinds.Right)) outputDown.x += 1;
        if (Input.GetKey(keybinds.Up)) outputDown.y += 1;
        if (Input.GetKey(keybinds.Down)) outputDown.y -= 1;

        return new Tuple<int, Vector2Int>(outputX, outputDown);
    }
}

