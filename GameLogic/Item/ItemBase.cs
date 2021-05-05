using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase
{
    public virtual ItemType Type { get; }
    public int Num { get; set; }
    public virtual string Description { get; }

    public virtual void Reset()
    {
        Num = 0;
    }
    public abstract void Buy(ItemManager itemManager);//買われた時の処理
    public abstract void Activate(ItemManager itemManager);//アイテムの効果が発動
    protected abstract void Process(ItemManager itemManager);//アイテムの効果の実際の処理. 共通する処理はActivateに実装.
}
