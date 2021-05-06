using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public PlayerStatus playerStatus;

    //アイテム6個
    private Dictionary<ItemType, ItemBase> items;

    public bool CanUseInfinitePower;

    public void Setup(PlayerStatus playerStatus)
    {
        this.playerStatus = playerStatus;

        //アイテムのセットアップ
        items = new Dictionary<ItemType, ItemBase>();
        ItemBase item;

        item = new Rest();
        items.Add(item.Type, item);

        item = new LevelUp();
        items.Add(item.Type, item);

        item = new Portion();
        items.Add(item.Type, item);

        item = new InfinitePower();
        items.Add(item.Type, item);

        item = new Resurrection();
        items.Add(item.Type, item);

    }

    public void ResetEveryPhase() {
        CanUseInfinitePower = true;
    }
    public void ResetEveryGame() {
        foreach (var item in items)
        {
            item.Value.Reset();
        }
    }

    public (ItemBase right, ItemBase left) PickUpTwoItems() {//確率による計算
        int r1 = MathUtility.Rnd.Next(0, items.Count);
        int r2 = MathUtility.Rnd.Next(0, items.Count-1);
        if (r2 >= r1) {
            ++r2;
        }
        return (FetchItem((ItemType)r1), FetchItem((ItemType)r2));
    }

    public void UseItem(ItemType itemType) {
        switch (itemType) {
            case ItemType.Portion:
            case ItemType.InfinitePower:
            case ItemType.Resurrection:
                FetchItem(itemType).Activate(this);
                break;
            default:
                Debug.LogError("使用できないアイテムが選択されました");
                break;
        }
    }

    public ItemBase FetchItem(ItemType itemType) {
        return items[itemType];
    }
}
