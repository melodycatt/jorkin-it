using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackStatus : StatusEffect {
    public float mult;
    public int op;
    public List<StatusEffect> stati;

    public AttackStatus(HealthManager on, List<StatusEffect> statuses, int op, float mult): base(on) {
        this.mult = mult;
        this.op = op;
        stati = statuses;
        on.attackManager.Effects = on.attackManager.Effects.Concat(stati).ToList();
        on.attackManager.Modifiers.Add(this); 
    }

    public override StatusEffect Copy() {
        return new AttackStatus(healthManager, stati, op, mult);
    }

    public static class AttackStatusBuilder {
        public static AttackStatus Adder(HealthManager on, float mult) {
            return new AttackStatus(on, null, 0, mult);
        }
        public static AttackStatus Multer(HealthManager on, float mult) {
            return new AttackStatus(on, null, 3, mult);
        }
        public static AttackStatus Divier(HealthManager on, float mult) {
            return new AttackStatus(on, null, 4, mult);
        }
        public static AttackStatus Statifier(HealthManager on, List<StatusEffect> stati) {
            return new AttackStatus(on, stati, 0, 0);
        }
    }
}