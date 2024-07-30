using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthStatus : StatusEffect {
    public int triggers;
    public int op;
    public List<StatusEffect> stati;

    private Func<int, HealthManager, int> InvokeFunc;

    public enum Type {
        Hurt,
        Heal, 
        Both
    }
    public Type type;

    public HealthStatus(HealthManager on, Type triggers, Func<int, HealthManager, int> Invoke): base(on) {
        type = triggers;
        InvokeFunc = Invoke;
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

    public override StatusEffect Copy() {
        return new HealthStatus(healthManager, type, InvokeFunc);
    }

    public void Invoke(int damage) {

    }
}

public static class HealthStatusBuilder {
    public static HealthStatus HealMultiplier(HealthManager on, float mult) {
        return new HealthStatus(on, HealthStatus.Type.Heal, (int d, HealthManager on) => (int)(mult * d));
    }
    public static HealthStatus HurtMultiplier(HealthManager on, float mult) {
        return new HealthStatus(on, HealthStatus.Type.Hurt, (int d, HealthManager on) => (int)(mult * d));
    }
    public static HealthStatus Multiplier(HealthManager on, float mult) {
        return new HealthStatus(on, HealthStatus.Type.Both, (int d, HealthManager on) => (int)(mult * d));
    }
    public static HealthStatus HealModifier(HealthManager on, int mod) {
        return new HealthStatus(on, HealthStatus.Type.Heal, (int d, HealthManager on) => d + mod);
    }
    public static HealthStatus HurtModifier(HealthManager on, int mod) {
        return new HealthStatus(on, HealthStatus.Type.Hurt, (int d, HealthManager on) => d + mod);
    }
    public static HealthStatus Modifier(HealthManager on, int mod) {
        return new HealthStatus(on, HealthStatus.Type.Both, (int d, HealthManager on) => d + mod);
    }
    public static HealthStatus Statifier(HealthManager on, StatusEffect status) {
        Debug.Log("aaa");
        return new HealthStatus(on, HealthStatus.Type.Both, (int d, HealthManager on) => {status.Copy().SetManager(on); on.didItWork.Add("Yes!"); return d;});
    }  
}