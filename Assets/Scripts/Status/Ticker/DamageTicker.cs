using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTicker : TickingEffect {
    public float damage;
    public bool heal;

    public DamageTicker(HealthManager on, float tickDelay, float damage, bool heal): base(on, tickDelay) {
        this.damage = damage;
        this.heal = heal;
    }

    protected override IEnumerator Ticker() {
        while (true) {
            if (heal) {
                healthManager.Heal(damage);
            } else {
                healthManager.Hurt(damage);
            }
            if (tickDown) {
                Level -= 1;
            }
            yield return new WaitForSeconds(tickDelay);
        }
    }
}