using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class ItemStorePhase: IPhase
{

    private IUserInterface userInterface;
    private ItemManager itemManager;
    private PlayerStatus playerStatus;


    public async UniTask BegineAsync(int currentStage)
    {
        var items = itemManager.PickUpTwoItems();
        userInterface.UpdatePlayerStatus(playerStatus);
        userInterface.UpdateTwoItems(items.right, items.left);

        while (true)
        {
            var input = await userInterface.WaitInputAsync();
            if (input.category == 0)//アイテムを取得
            {
                if (input.pickedThing == 0)
                {
                    items.right.Buy(itemManager);
                }
                else if (input.pickedThing == 1)
                {
                    items.left.Buy(itemManager);
                }
                else
                {
                    Debug.LogError("誤った選択肢が選ばれています");
                }
                userInterface.UpdatePlayerStatus(playerStatus);
                break;
            }
            else if (input.category == 1)//アイテム使用
            {
                itemManager.UseItem((ItemType)input.pickedThing);
                userInterface.UpdatePlayerStatus(playerStatus);
                continue;
            }
        }

    }

    public void Setup(IUserInterface userInterface, ItemManager itemManager, PlayerStatus playerStatus) {
        this.userInterface = userInterface;
        this.itemManager = itemManager;
        this.playerStatus = playerStatus;
    }

}
