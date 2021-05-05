using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstantItem : ItemBase
{
    public override void Buy(ItemManager itemManager)
    {
        ++Num;
        Activate(itemManager);
    }

    public override void Activate(ItemManager itemManager)
    {
        Process(itemManager);
    }
}
