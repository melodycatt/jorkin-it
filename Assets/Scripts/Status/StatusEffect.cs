using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect {
    public HealthManager healthManager;
    public bool effective = true;

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
        healthManager = on;
        on.statuses.Add(this);
    }
}