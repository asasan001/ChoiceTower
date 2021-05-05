using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : InstantItem
{
    public override ItemType Type { get; } = ItemType.LevelUp;
    public override string Description { get; } = "急成長薬\nすぐにレベルが2上がるぞ";
    protected override void Process(ItemManager itemManager)
    {
        itemManager.playerStatus.ForceLevelUp(2);
    }
}
