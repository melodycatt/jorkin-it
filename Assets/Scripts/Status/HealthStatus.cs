using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthStatus : StatusEffect {
    public int triggers;
    public int op;
    public List<StatusEffect> stati;

    public enum Type {
        Hurt,
        Heal, 
        Both
    }
    public Type type;

    public HealthStatus(HealthManager on, Type triggers, Func<float, HealthManager, float> Invoke): base(on) {
        type = triggers;
        if (triggers == Type.Hurt) {
            on.OnHurtFunctions.Add(Invoke);
        }
        else if (triggers == Type.Heal) {
            on.OnHealFunctions.Add(Invoke);
        }
        else if (triggers == Type.Both) {
            on.OnHurtFunctions.Add(Invoke);
            on.OnHealFunctions.Add(Invoke);
        }
    }


    public void Invoke(float damage) {

    }
}

public static class HealthStatusBuilder {
    public static HealthStatus HealMultiplier(HealthManager on, float mult) {
        return new HealthStatus(on, HealthStatus.Type.Heal, (float d, HealthManager on) => mult * d);
    }
    public static HealthStatus HurtMultiplier(HealthManager on, float mult) {
        return new HealthStatus(on, HealthStatus.Type.Hurt, (float d, HealthManager on) => mult * d);
    }
    public static HealthStatus Multiplier(HealthManager on, float mult) {
        return new HealthStatus(on, HealthStatus.Type.Both, (float d, HealthManager on) => mult * d);
    }
    public static HealthStatus HealModifier(HealthManager on, float mod) {
        return new HealthStatus(on, HealthStatus.Type.Heal, (float d, HealthManager on) => d + mod);
    }
    public static HealthStatus HurtModifier(HealthManager on, float mod) {
        return new HealthStatus(on, HealthStatus.Type.Hurt, (float d, HealthManager on) => d + mod);
    }
    public static HealthStatus Modifier(HealthManager on, float mod) {
        return new HealthStatus(on, HealthStatus.Type.Both, (float d, HealthManager on) => d + mod);
    }
    public static HealthStatus Statifier(HealthManager on, StatusEffect status) {
        return new HealthStatus(on, HealthStatus.Type.Both, (float d, HealthManager on) => {on.statuses.Add(status); on.didItWork.Add("Yes!"); return d;});
    }  
}