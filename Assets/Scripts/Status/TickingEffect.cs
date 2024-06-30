using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TickingEffect : StatusEffect {
    public float tickDelay;
    public bool tickDown;

    public override int Level {
        get => level;
        set {
            if (!effective) {
                throw new Exception("This status effect is not effective!");
            }
            if (value == 0) {
                healthManager.statuses.Remove(this);
                effective = false;
                healthManager.StopCoroutine(ticker);
            }
            level = value;
        }
    }

    protected Coroutine ticker;

    public TickingEffect(HealthManager on, float tickDelay): base(on) {
        ticker = on.StartCoroutine(Ticker());
        this.tickDelay = tickDelay;
    }

    protected abstract IEnumerator Ticker();
}