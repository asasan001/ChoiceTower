using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public interface IUserInterface
{
    Phase CurrentPhase { set; }
    void Setup(ItemManager itemManager);
    UniTask TitleWaitInputAsync();
    UniTask FirstExplainAsync();
    UniTask EndingWaitInputAsync(bool isGoodEnding);
    UniTask<(int category, int pickedThing)> WaitInputAsync();
    UniTask ExplainAsync(string explain);
    UniTask BattleExplainAsync(BattleSystem.OutBattle outBattle);
    void UpdateFloor(int floor);
    void UpdatePlayerStatus(PlayerStatus player);
    void UpdateTwoMonsters(BattleSystem.OutSimulate rightOutSim, BattleSystem.OutSimulate leftOutSim);
    void UpdateTwoItems(ItemBase rightItem, ItemBase leftItem);

}
