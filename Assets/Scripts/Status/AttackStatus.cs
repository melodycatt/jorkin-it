using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackStatus : StatusEffect {
    public float mult;
    public int op;
    public List<StatusEffect> stati;

    public AttackStatus(HealthManager on): base(on) {
        on.attackManager.Effects = on.attackManager.Effects.Concat(stati).ToList();
        on.attackManager.Modifiers.Add(this); 
    }

    public override StatusEffect Copy() {
        return new AttackStatus(healthManager);
    }
}