using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Item {
    string name;
    int count;
}

public struct TrinketItem : Item {
    //mana used per second as a mana well
    int mwps;
    int fragCost;

    bool achieved;
}