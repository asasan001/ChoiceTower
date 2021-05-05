using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePower : SingleUseItem
{
    public override ItemType Type { get; } = ItemType.InfinitePower;
    public override string Description { get; } = "鬼人薬\n無敵になる。バトル前に使えるぞ";
    protected override void Process(ItemManager itemManager) {
        itemManager.playerStatus.IsInfinitePower = true;
        itemManager.CanUseInfinitePower = false;
    }
}
