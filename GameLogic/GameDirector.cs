using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class GameDirector : MonoBehaviour
{
    private State state;
    private int maxStages = 20;
    private int currentStage = 0;//0から始まる
    private List<int> stages; //0: battle, 1: item store
    private bool canUpdate = true;

    [SerializeField] private IUserInterface userInterface;
    private IPhase battlePhase;
    private IPhase itemStorePhase;
    private ItemManager itemManager;
    private PlayerStatus playerStatus;

    private enum State
    {
        Title,
        MainGame,
        GoodEnding,
        BadEnding
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        EnterTitle();
    }

    // Update is called once per frame
    async UniTaskVoid Update()
    {
        if (!canUpdate) return;
        canUpdate = false;

        switch (state)
        {
            case State.Title:
                itemManager.ResetEveryGame();
                await userInterface.TitleWaitInputAsync();
                await userInterface.FirstExplainAsync();
                state = State.MainGame;
                break;

            case State.MainGame:
                itemManager.ResetEveryPhase();
                playerStatus.Reset();

                userInterface.UpdateFloor(currentStage);
                if (stages[currentStage] == 0)
                {
                    userInterface.CurrentPhase = Phase.Battle;
                    await battlePhase.BegineAsync(currentStage);
                }
                else if (stages[currentStage] == 1)
                {
                    userInterface.CurrentPhase = Phase.ItemStore;
                    await itemStorePhase.BegineAsync(currentStage);
                }

                if (playerStatus.Hp > 0)//やられてない時
                {
                    ++currentStage;
                    if (currentStage == maxStages)
                    {
                        state = State.GoodEnding;
                    }
                }
                else {//やられたとき
                    state = State.BadEnding;
                }
                break;

            case State.GoodEnding:
                await userInterface.EndingWaitInputAsync(true);
                EnterTitle();
                break;

            case State.BadEnding:
                await userInterface.EndingWaitInputAsync(false);
                EnterTitle();
                break;
        }

        canUpdate = true;
    }
    private void Setup()
    {
        userInterface = GameObject.Find("UserInterface").GetComponent<IUserInterface>();
        battlePhase = new BattlePhase();
        itemStorePhase = new ItemStorePhase();
        itemManager = new ItemManager();
        playerStatus = new PlayerStatus();

        userInterface.Setup(itemManager);
        battlePhase.Setup(userInterface, itemManager,playerStatus);
        itemStorePhase.Setup(userInterface, itemManager, playerStatus);
        itemManager.Setup(playerStatus);
    }


    private void EnterTitle() {
        state = State.Title;
        currentStage = 0;
        playerStatus.Initialize();
        BuildStage();
    }
    private void BuildStage() {//今適当に作ってる。あとでランダムに作成
        stages = new List<int>();

        stages.Add(0);
        List<int> tmpList = new List<int>() { 0,0,0,0,1,1};
        tmpList.Shuffle();
        stages.AddRange(tmpList);

        tmpList = new List<int>() { 0,  0, 0, 1, 1 };
        tmpList.Shuffle();
        stages.AddRange(tmpList);

        tmpList = new List<int>() { 0, 0, 0, 0, 1, 1 };
        tmpList.Shuffle();
        stages.AddRange(tmpList);
        stages.Add(0);
        stages.Add(0);
    }

    

}
