using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHM : HealthManager {
    public List<Transform> hearts;
    public List<Transform> shatteredHearts;


    void Start() {
        Health = MaxHealth;
        attackManager = GetComponent<AttackManager>();
        Debug.Log("bb");
        //HealthStatusBuilder.Statifier(this, new DamageTicker(null, 3, 1, false));
        hearts = Camera.main.transform.Find("UI").Find("HUD").Find("HEALTH").GetComponentsInChildren<Transform>().Where((x) => x.gameObject.name == "Shard").ToList();
    }

    void Update() {
        if (genericIframes > 0) genericIframes -= Time.deltaTime;
        if (genericIframes < 0) genericIframes = 0;
    }

    public override void Hurt(int damage, bool ignoreIFrames = false) {
        if (genericIframes <= 0 || ignoreIFrames) {
            foreach (Func<int, HealthManager, int> function in OnHurtFunctions) {
                damage = function(damage, this);
            }
            for (int i = 1; i <= damage; i++) {
                shatteredHearts.Add(hearts[^i]);
                hearts[^i].Find("Full").gameObject.SetActive(false);
                hearts[^i].Find("Empty").gameObject.SetActive(true);
                hearts.Remove(hearts[^i]);
            }
            genericIframes = genericIframesLength;
            _Hurt(damage);
        }
    }

    public override void Heal(int health) {
        foreach (Func<int, HealthManager, int> function in OnHealFunctions) {
            health = function(health, this);
        }
        for (int i = 1; i <= health; i++) {
            hearts.Add(shatteredHearts[i]);
            shatteredHearts[i].Find("Full").gameObject.SetActive(true);
            shatteredHearts[i].Find("Empty").gameObject.SetActive(false);
            shatteredHearts.Remove(shatteredHearts[i]);
        }
        _Heal(health);
    }

    public override void Hurt(int damage, List<StatusEffect> statuses, string source, bool ignoreIFrames = false) {
        if (genericIframes <= 0 || ignoreIFrames) {
            print(source);
            foreach (Func<int, HealthManager, int> function in OnHurtFunctions) {
                damage = function(damage, this);
            }
            for (int i = 1; i <= damage; i++) {
                shatteredHearts.Add(hearts[^i]);
                hearts[^i].Find("Full").gameObject.SetActive(false);
                hearts[^i].Find("Empty").gameObject.SetActive(true);
                hearts.Remove(hearts[^i]);
            }
            genericIframes = genericIframesLength;
            _Hurt(damage);
        }
    }

    public override void Heal(int health, List<StatusEffect> statuses) {
        foreach (Func<int, HealthManager, int> function in OnHealFunctions) {
            health = function(health, this);
        }
        for (int i = 1; i <= health; i++) {
            hearts.Add(shatteredHearts[i]);
            shatteredHearts[i].Find("Full").gameObject.SetActive(true);
            shatteredHearts[i].Find("Empty").gameObject.SetActive(false);
            shatteredHearts.Remove(shatteredHearts[i]);
        }
        _Heal(health);
    }

    private void _Hurt(float damage) {
        if(Dead) {
            return;
        }
        Health -= damage;
        if (Health <= 0) {
            Health = 0;
            Dead = true;
        }
    }
    
    private void _Heal(float health) {
        if(Dead) {
            return;
        }
        Health += health;
        if (Health > MaxHealth) {
            Health = MaxHealth;
        }
    }
}