using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect {
    public string GUID;
    public HealthManager healthManager;
    public bool effective = true;
    public string whatThe = "hell";

    protected  int level;
    public virtual int Level {
        get => level;
        set {
            if (!effective) {
                throw new Exception("This status effect is not effective!");
            }
            if (value == 0) {
                healthManager.statuses.Remove(this);
                effective = false;
            }
            level = value;
        }
    }

    public StatusEffect(HealthManager on) {
        GUID = Guid.NewGuid().ToString();
        if (on != null) {
            healthManager = on;
            on.statuses.Add(this);
        }
    }

    public virtual StatusEffect SetManager(HealthManager on) {
        if (healthManager != null) {
            healthManager.statuses.Remove(this);
        }
        MonoBehaviour.print(on == null);
        healthManager = on;
        on.statuses.Add(this);
        return this;
    }

    public virtual StatusEffect Copy() {
        return new StatusEffect(healthManager);
    }
}