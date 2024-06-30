using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {
    public List<Action<HealthManager, float, List<StatusEffect>>> OnAttackFunctions;
    public float Damage;
    public float FinalDamage {
        get {
            float d = Damage;
            foreach (AttackStatus mod in Modifiers) {
                if (mod.op == 0) {
                    d += mod.mult;
                }
                if (mod.op == 1) {
                    d -= mod.mult;
                }
                if (mod.op == 2) {
                    d *= mod.mult;
                }
                if (mod.op == 3) {
                    d /= mod.mult;
                }
            }
            return Damage;
        }
    }
    public List<AttackStatus> Modifiers;
    public List<StatusEffect> Effects;

    public void Attack(HealthManager target) {
        target.Hurt(FinalDamage, Effects);
        foreach (Action<HealthManager, float, List<StatusEffect>> function in OnAttackFunctions) {
            function(target, FinalDamage, Effects);
        }
    }
    public void Attack(HealthManager target, float mult) {
        target.Hurt(FinalDamage * mult, Effects);
        foreach (Action<HealthManager, float, List<StatusEffect>> function in OnAttackFunctions) {
            function(target, FinalDamage * mult, Effects);
        }
    }
}