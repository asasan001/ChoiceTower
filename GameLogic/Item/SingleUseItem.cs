using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleUseItem : ItemBase
{
    public override void Buy(ItemManager itemManager)
    {
        ++Num;
    }

    public override void Activate(ItemManager itemManager)
    {
        if (Num > 0)
        {
            --Num;
            Process(itemManager);
        }
        else {
            Debug.LogError("SingleUseItemのActivateがnumが0以下の時、呼ばれました");
        }
    }
}
