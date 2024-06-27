using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SlashController : MonoBehaviour {
    public int cooldown = 0;
    public float cooldownLength = 0.4f;

    //similar dumbness as before
    public _SlashKeybinds<MouseButton> KnifeKeybinds = new _SlashKeybinds<MouseButton>()
    {
        Slash = MouseButton.Left,
    };

    [SerializeField]
    public enum MouseButton {
        Left,
        Right,
        Middle
    }

    public GameObject Slash;
    public Transform Player;

    void Start() {
        Player = transform.parent.Find("Player");
        Slash = Resources.Load<GameObject>("Prefabs/Player/Slash");
    }

    void Update() {
        transform.position = Player.position;
        if (Input.GetMouseButtonDown((int)MouseButton.Left) && cooldown == 0) {
            StartCoroutine(Spawn(Player.GetComponent<PlayerMovement>().Facing));
        }
    }

    IEnumerator Spawn(Vector2Int dir) {
        GameObject tempSlash = Instantiate(Slash);
        tempSlash.transform.parent = transform;
        tempSlash.transform.localPosition = new(0, 0);

        //hit direction stuff
        if(dir.y != 0) {
            if (dir.y == -1 && !Player.GetComponent<PlayerMovement>().Grounded) tempSlash.transform.rotation = Quaternion.Euler(0, 0, -90);
            else if (dir.y == 1) tempSlash.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (dir.x == -1) {
            tempSlash.transform.rotation = Quaternion.Euler(0, 0, 180);
            print("sjdja");
        }
        tempSlash.transform.position += tempSlash.transform.rotation * new Vector3(1.5f, 0);

        cooldown = 1;
        yield return new WaitForSeconds(cooldownLength);
        cooldown = 0;
    }

    //dumbness
    public struct _SlashKeybinds<T> {
        public T Slash;
    }
}