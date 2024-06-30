using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTicker : TickingEffect {
    readonly Action<HealthManager> tick;

    public CustomTicker(HealthManager on, float tickDelay, Action<HealthManager> tick): base(on, tickDelay) {
        this.tick = tick;
    }

    protected override IEnumerator Ticker() {
        while (true) {
            tick(healthManager);
            if (tickDown) {
                Level -= 1;
            }
            yield return new WaitForSeconds(tickDelay);
        }
    }
}