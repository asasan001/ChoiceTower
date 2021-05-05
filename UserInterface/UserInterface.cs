using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using naichilab.EasySoundPlayer.Scripts;

public class UserInterface : MonoBehaviour, IUserInterface
{
    [SerializeField] private GameObject titleGO;
    [SerializeField] private Button startButton;

    [SerializeField] private GameObject goodEndingGO;
    [SerializeField] private GameObject badEndingGO;
    [SerializeField] private Button returnTitleButton;

    [SerializeField] private GameObject playerStatusGO;

    [SerializeField] private Text levelText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text expText;
    [SerializeField] private Text attackText;
    [SerializeField] private Text defenseText;

    [SerializeField] private GameObject choicePartGO;

    [SerializeField] private Image rightImg;
    [SerializeField] private Image leftImg;

    [SerializeField] private Text rightText;
    [SerializeField] private Text leftText;

    [SerializeField] private Button rightChoiceButton;
    [SerializeField] private Button leftChoiceButton;

    [SerializeField] private GameObject floorGO;
    [SerializeField] private Text floorText;

    [SerializeField] private GameObject explainGO;
    [SerializeField] private Button explainPanelButton;
    [SerializeField] private Text explainText;
    [SerializeField] private Button explainButton;

    [SerializeField] private GameObject itemInfoGO;
    [SerializeField] private Button itemInfoPanelButton;
    [SerializeField] private Image itemInfoImg;
    [SerializeField] private Text itemInfoText;
    [SerializeField] private Text itemNumText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button rightArrow;
    [SerializeField] private Button leftArrow;

    [SerializeField] private GameObject itemButtonsGO;
    [SerializeField] private Button PortionButton;
    [SerializeField] private Button InfinitePowerButton;
    [SerializeField] private Button ResurrectionButton;

    [SerializeField] private Sprite restSprite;
    [SerializeField] private Sprite levelUpSprite;
    [SerializeField] private Sprite portionSprite;
    [SerializeField] private Sprite infinitePowerSprite;
    [SerializeField] private Sprite resurrectionSprite;
    [SerializeField] private Sprite attackUpSprite;
    [SerializeField] private Sprite defenseUpSprite;

    [SerializeField] private List<Sprite> monsterSprites;

    private ItemManager itemManager;
    public ReactiveProperty<(int category, int pickedThing)> waitInput;//選択と持ち物の使用時に使う

    private bool isWaitExplain;

    private ItemType itemTypeOfItemInfo;

    public int CurrentStage { get; set; }

    public Phase CurrentPhase{ get; set; }//GameDirectorでセットする



    // Start is called before the first frame update
    void Start()
    {
        waitInput = new ReactiveProperty<(int category, int pickedThing)>();

        rightChoiceButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            OnRightChoice();
        });
        leftChoiceButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            OnLeftChoice();
        });


        explainPanelButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            isWaitExplain = false;
            explainGO.SetActive(false);
        });
        explainButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            isWaitExplain = false;
            explainGO.SetActive(false);
        });

        PortionButton.OnClickAsObservable().Subscribe(_ =>{
            SePlayer.Instance.Play(0);
            OnItemButton(ItemType.Portion);
        });
        InfinitePowerButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            OnItemButton(ItemType.InfinitePower);
        });
        ResurrectionButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            OnItemButton(ItemType.Resurrection);
        });
        itemInfoPanelButton.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            itemInfoGO.SetActive(false);
        });
        rightArrow.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            OnRightArrow();
        });
        leftArrow.OnClickAsObservable().Subscribe(_ => {
            SePlayer.Instance.Play(0);
            OnLeftArrow();
        });
        useButton.OnClickAsObservable().Subscribe(_ =>
        {
            SePlayer.Instance.Play(0);
            OnUseButton();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(ItemManager itemManager) {
        this.itemManager = itemManager;
    }

    public async UniTask TitleWaitInputAsync() {
        ShowMainGame(false);
        titleGO.SetActive(true);
        await startButton.OnClickAsync();
        SePlayer.Instance.Play(0);
        titleGO.SetActive(false);
        

    }

    public async UniTask EndingWaitInputAsync(bool isGoodEnding) {
        ShowMainGame(false);
        returnTitleButton.gameObject.SetActive(true);
        if (isGoodEnding)
        {
            goodEndingGO.SetActive(true);
        }
        else {
            badEndingGO.SetActive(true);
        }
        await returnTitleButton.OnClickAsync();
        SePlayer.Instance.Play(0);
        returnTitleButton.gameObject.SetActive(false);
        goodEndingGO.SetActive(false);
        badEndingGO.SetActive(false);
    }

    public async UniTask ExplainAsync(string explainString)
    {
        explainText.text = explainString;
        isWaitExplain = true;
        explainGO.SetActive(true);
        while (isWaitExplain) {
            await UniTask.DelayFrame(1);
        }
    }

    public async UniTask FirstExplainAsync()
    {
        string explain = "モンスターを倒して, 塔を登ろう\nHPが0になるとゲームオーバーです\n";
        explain += "右上のアイコンをクリックすると持っているアイテムを使うことができます\n";
        explain += "初めからポーションを1つ持ってます\n";
        await ExplainAsync(explain);
    }

    public async UniTask BattleExplainAsync(BattleSystem.OutBattle outBattle) {
        string text = "バトルした\n";
        if (outBattle.playerDamage == 0) {
            text += "ダメージを受けなかった\n";
        }
        else
        {
            text += outBattle.playerDamage.ToString() + "ダメージを受け, HPが" + outBattle.remainHp +
                "になった\n";
        }

        if (outBattle.remainHp==0) {
            if (outBattle.didUseResurrection)
            {
                text += "不死のブレスレットおかげで復活し, HPが30になった\n";
            }
            else {
                text += "おばけちゃんは力尽きた\n";
                await ExplainAsync(text);
                return;
            }
        }

        if (outBattle.didSuccess) {
            text += "モンスターを倒した\n";
            text += "経験値を" + outBattle.actualExp + "もらった\n";
        }
        else {
            text += "モンスターが逃げた\n";
            text += "少なめに経験値を" + outBattle.actualExp + "もらった\n";
        }
        if (outBattle.levelUp != 0) {
            text += "レベルが" + outBattle.levelUp+ "つ上がった";
        }
        await ExplainAsync(text);

    }

    public async UniTask<(int category, int pickedThing)> WaitInputAsync() {
        ShowMainGame(true);
        return await waitInput;
    }

    private void OnRightChoice() {
        waitInput.SetValueAndForceNotify((0, 0));
    }

    private void OnLeftChoice()
    {
        waitInput.SetValueAndForceNotify((0, 1));
    }

    private void OnItemButton(ItemType itemType) {
        itemTypeOfItemInfo = itemType;
        itemInfoGO.SetActive(true);

        itemInfoImg.sprite = GetItemSprite(itemType);

        var item = itemManager.FetchItem(itemType);
        itemInfoText.text = item.Description;

        itemNumText.text = item.Num.ToString() + "コ";

        //使うボタンが押せるかどうか
        if (item.Num == 0 || itemType == ItemType.Resurrection ||
            (itemType == ItemType.InfinitePower &&
            (!itemManager.CanUseInfinitePower || CurrentPhase != Phase.Battle))) {
            useButton.interactable = false;
        }else
        {
                useButton.interactable = true;
        }
    }

    private void OnRightArrow() {
        switch (itemTypeOfItemInfo) {
            case ItemType.Portion:
                OnItemButton(ItemType.InfinitePower);
                break;
            case ItemType.InfinitePower:
                OnItemButton(ItemType.Resurrection);
                break;
            case ItemType.Resurrection:
                OnItemButton(ItemType.Portion);
                break;
        }
    }

    private void OnLeftArrow()
    {
        switch (itemTypeOfItemInfo)
        {
            case ItemType.Portion:
                OnItemButton(ItemType.Resurrection);
                break;
            case ItemType.InfinitePower:
                OnItemButton(ItemType.Portion);
                break;
            case ItemType.Resurrection:
                OnItemButton(ItemType.InfinitePower);
                break;
        }
    }

    private void OnUseButton() {
        waitInput.SetValueAndForceNotify((1,(int)itemTypeOfItemInfo));
        itemInfoGO.SetActive(false);
    }



    public void UpdateFloor(int currentStage) {
        CurrentStage = currentStage;
        floorText.text = (CurrentStage + 1).ToString();
    }

    public void UpdatePlayerStatus(PlayerStatus player) {
        levelText.text = "レベル: " + player.Level;
        hpText.text = "HP: " + player.Hp + "/100";
        expText.text = "経験値: " + player.Exp + "/100";

        if (!player.IsInfinitePower)
        {
            attackText.text = "こうげき: " + player.Attack;
            defenseText.text = "ぼうぎょ: " + player.Defense;
        }
        else {
            attackText.text = "こうげき: 9999";
            defenseText.text = "ぼうぎょ: 9999";
        }
    }

    public void UpdateTwoMonsters(BattleSystem.OutSimulate rightOutSim, BattleSystem.OutSimulate leftOutSim) {
        var sprites = GetTwoMonsterSprites(CurrentStage);
        rightImg.sprite = sprites.m1;
        leftImg.sprite = sprites.m2;
        UpdateMonster(rightOutSim, rightText);
        UpdateMonster(leftOutSim, leftText);
    }
    private void UpdateMonster(BattleSystem.OutSimulate outSim, Text text) {
        text.text = "こうげき: " + outSim.attack + "\n";
        text.text += "ぼうぎょ: " + outSim.defense + "\n";
        text.text += "倒した時の経験値: " + outSim.idealExp + "\n";

        Debug.Log("こ:" + outSim.attack + " ぼ:" + outSim.defense + " だ:" +
            outSim.playerDamage + " か:" + outSim.successProb + " け:" + outSim.idealExp);
    }

    public void UpdateTwoItems(ItemBase rightItem, ItemBase leftItem) {
        UpdateItem(rightItem, rightImg, rightText);
        UpdateItem(leftItem, leftImg, leftText);
    }

    private void UpdateItem(ItemBase item, Image img,Text text) {
        img.sprite = GetItemSprite(item.Type);
        text.text = item.Description;
    }
    private void ShowMainGame(bool isShow) {
        playerStatusGO.SetActive(isShow);
        choicePartGO.SetActive(isShow);
        floorGO.SetActive(isShow);
        itemButtonsGO.SetActive(isShow);
    }

    private Sprite GetItemSprite(ItemType itemType) {
        Sprite sprite = null;
        switch (itemType) {
            case ItemType.Rest:
                sprite = restSprite;
                break;
            case ItemType.LevelUp:
                sprite = levelUpSprite;
                break;
            case ItemType.Portion:
                sprite = portionSprite;
                break;
            case ItemType.InfinitePower:
                sprite = infinitePowerSprite;
                break;
            case ItemType.Resurrection:
                sprite = resurrectionSprite;
                break;
            case ItemType.AttackUp:
                sprite = attackUpSprite;
                break;
            case ItemType.DefenseUp:
                sprite = defenseUpSprite;
                break;
        }
        return sprite;
    }

    private (Sprite m1, Sprite m2) GetTwoMonsterSprites(int stage) {
        int r1 = MathUtility.Rnd.Next(0, 5);
        int r2 = MathUtility.Rnd.Next(0, 4);
        if (r2 >= r1)
        {
            ++r2;
        }

        int i1 = 0;
        int i2 = 0;
        if (stage < 6)
        {
            i1 = r1;
            i2 = r2;
        }
        else if (stage < 13)
        {
            i1 = r1 + 5;
            i2 = r2 + 5;
        }
        else {
            i1 = r1 + 10;
            i2 = r2 + 10;
        }
        return (monsterSprites[i1], monsterSprites[i2]);
    }

}
