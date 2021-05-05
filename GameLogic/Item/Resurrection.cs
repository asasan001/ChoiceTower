using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrection : SingleUseItem
{
    public override ItemType Type { get; } = ItemType.Resurrection;
    public override string Description { get; } = "不死のブレスレット\nHPが０になったときに自動的に使用される。HPが30で復活する";
    private int RecoveryPoint = 30;
    protected override void Process(ItemManager itemManager)
    {
        itemManager.playerStatus.Hp += RecoveryPoint;
    }
}
