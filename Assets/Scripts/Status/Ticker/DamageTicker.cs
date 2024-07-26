using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageTicker : TickingEffect {
    public float damage;
    public bool heal;

    public DamageTicker(HealthManager on, float tickDelay, float damage, bool heal): base(on, tickDelay) {
        this.damage = damage;
        this.heal = heal;
    }

    public override StatusEffect Copy() {
        Debug.Log("yo");
        return new DamageTicker(healthManager, tickDelay, damage, heal);
    }

    protected override IEnumerator Ticker() {
        yield return new WaitForSeconds(tickDelay);
        while (true) {
            if(healthManager == null) {
                whatThe = "fuck";
            }
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