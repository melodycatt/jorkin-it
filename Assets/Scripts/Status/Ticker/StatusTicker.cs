using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTicker : TickingEffect {
    public StatusEffect status;

    public StatusTicker(HealthManager on, float tickDelay, StatusEffect status): base(on, tickDelay) {
        this.status = status;
    }

    protected override IEnumerator Ticker() {
        while (true) {
            healthManager.statuses.Add(status);
            if (tickDown) {
                Level -= 1;
            }
            yield return new WaitForSeconds(tickDelay);
        }
    }
}