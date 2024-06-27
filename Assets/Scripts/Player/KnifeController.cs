using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class KnifeController : MonoBehaviour {
    //list of knives, and knives that are currently shooting
    public List<Knife> knives = new();
    public List<Knife> shootingKnives = new();

    //honestly this is pointless (just to separate it in the inpector)
    public _KnifeKeybinds<MouseButton, MouseButton> KnifeKeybinds = new _KnifeKeybinds<MouseButton, MouseButton>()
    {
        Spawn = KeyCode.R,
        Shoot = MouseButton.Left,
        ShootAll = MouseButton.Right
    };

    //even worse than above
    [SerializeField]
    public enum MouseButton {
        Left,
        Right,
        Middle
    }

    public GameObject Knife;
    public Transform Player;

    //for smooth movement of knives
    public Vector3 targetPosition;

    void Start() {
        Player = transform.parent.Find("Player");
        Knife = Resources.Load<GameObject>("Prefabs/Player/Knife");
    }

    void Update() {
        //like the smooth movement of knives
        targetPosition = Player.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Sigmoid(Vector3.Distance(transform.position, targetPosition) * Time.deltaTime));

        //spawn knives and shoot knives
        if (Input.GetKeyDown(KnifeKeybinds.Spawn) && knives.Count < 5) {
            Debug.Log(knives.Count);
            StartCoroutine(Spawn());
        } else if (Input.GetMouseButtonDown((int)KnifeKeybinds.Shoot) && knives.Count > 0) {
            //i think this is self explanatory?
            knives[0].Shooting = true;
            knives[0].transform.up = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            knives[0].transform.parent = null;
            shootingKnives.Add(knives[0]);
            knives.Remove(knives[0]);
            Arrange();
        }
    }

    //see Knife.cs
    float Sigmoid(float t) {
        return 1.05f / (1 + Mathf.Exp(-6 * (t*10 - 0.5f)));
    }


    IEnumerator Spawn() {
        //spawn multiple times with delay while spawn key is down
        for(int i = 0; i < 5 && Input.GetKey(KnifeKeybinds.Spawn) && knives.Count < 5; i++) {
            GameObject tempKnife = Instantiate(Knife);
            tempKnife.transform.parent = transform;//.parent.Find("Knife Holder"); // ignore that
            knives.Add(tempKnife.GetComponent<Knife>());
            tempKnife.GetComponent<Knife>().InstantArrange(knives.Count - 1, knives.Count);
            Arrange();
            yield return new WaitForSeconds(0.25f);
        }
    }

    //more pointlessness
    public struct _KnifeKeybinds<T1, T2> {
        public KeyCode Spawn;
        public T1 Shoot;
        public T2 ShootAll;
    }

    //call arrange on all non-shooting knives
    public void Arrange() {
        int i = 0;
        foreach (Knife x in knives) {
            x.Arrange(i, knives.Count);
            i++;
        }
    }
}