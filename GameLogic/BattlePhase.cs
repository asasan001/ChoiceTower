using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class BattlePhase
{

    private IUserInterface userInterface;
    private ItemManager itemManager;
    private PlayerStatus playerStatus;



    public async UniTask BegineAsync(int currentStage)
    {
        //モンスターの生成
        var monsters = MonsterGenerator.GenerateTwoMonster(currentStage + 1);
        var rightSim = BattleSystem.Simulate(playerStatus, monsters.right);
        var leftSim = BattleSystem.Simulate(playerStatus, monsters.left);

        userInterface.UpdatePlayerStatus(playerStatus);
        userInterface.UpdateTwoMonsters(rightSim, leftSim);



        BattleSystem.OutBattle outBattle=new BattleSystem.OutBattle();
        while (true)
        {
            var input = await userInterface.WaitInputAsync();

            if (input.category == 0)//アイテムを取得
            {
                
                if (input.pickedThing == 0)
                {
                    outBattle = BattleSystem.Battle(rightSim, itemManager, playerStatus);
                }
                else if (input.pickedThing == 1)
                {
                    outBattle = BattleSystem.Battle(leftSim, itemManager, playerStatus);
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
                rightSim = BattleSystem.Simulate(playerStatus, monsters.right);
                leftSim = BattleSystem.Simulate(playerStatus, monsters.left);

                userInterface.UpdatePlayerStatus(playerStatus);
                userInterface.UpdateTwoMonsters(rightSim, leftSim);//いらなくなるかも

                continue;
            }
        }
        await userInterface.BattleExplainAsync(outBattle);
    }

    public void Setup(IUserInterface userInterface, ItemManager itemManager, PlayerStatus playerStatus) {
        this.userInterface = userInterface;
        this.itemManager = itemManager;
        this.playerStatus = playerStatus;
    }
}
