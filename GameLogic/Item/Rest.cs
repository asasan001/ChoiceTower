using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : InstantItem
{
    public override ItemType Type { get; } = ItemType.Rest;
    public override string Description { get; } = "薬草たっぷりスープ\nすぐにHPを全回復するぞ";
    protected override void Process(ItemManager itemManager)
    {
        itemManager.playerStatus.SetMaxHp();
    }
}
