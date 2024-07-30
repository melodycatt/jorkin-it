using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AttackManager : MonoBehaviour {
    public List<Action<HealthManager, float, List<StatusEffect>>> OnAttackFunctions = new();
    public float Damage;
    public string nname;
    public int FinalDamage {
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
            return (int)Damage;
        }
    }
    public List<AttackStatus> Modifiers = new();
    public List<StatusEffect> Effects = new();

    public void Attack(HealthManager target) {
        target.Hurt(FinalDamage, Effects, source:nname);
        foreach (Action<HealthManager, float, List<StatusEffect>> function in OnAttackFunctions) {
            function(target, FinalDamage, Effects);
        }
    }
    public void Attack(HealthManager target, float mult) {
        target.Hurt((int)(FinalDamage * mult), Effects);
        foreach (Action<HealthManager, float, List<StatusEffect>> function in OnAttackFunctions) {
            function(target, FinalDamage * mult, Effects);
        }
    }
}