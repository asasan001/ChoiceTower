using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portion : SingleUseItem
{
    public override ItemType Type { get; } = ItemType.Portion;
    public override string Description { get; } = "ポーション\nHPを40回復する。いつでも使えるぞ";
    private int RecoveryPoint = 40;
    protected override void Process(ItemManager itemManager)
    {
        itemManager.playerStatus.Hp += RecoveryPoint;
    }

    public override void Reset()
    {
        Num = 1;
    }

}
