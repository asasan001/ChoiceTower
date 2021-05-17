using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerStatus
{
    public const int maxHp = 100;
    private const int levelUpExp = 100;
    private int _hp;
    public int Hp {
        set
        {
            if (value > maxHp) _hp = maxHp;
            else if (value < 0) _hp = 0;
            else _hp = value;
        }
        get {
            return _hp;
        }
    }
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int Attack { get; set; }
    public int Defense { get; set; }

    public int CumulativeMonsterAttack { get; set; }
    public int CumulativeMonsterDefense { get; set; }

    public bool IsInfinitePower { get; set; }

    public void Initialize()
    {
        Level = 1;
        Hp = 100;
        Exp = 0;
        CumulativeMonsterAttack = 0;
        CumulativeMonsterDefense = 0;
        RenewStatus();
    }

    public void Reset() {
        IsInfinitePower = false;
    }

    public void SetMaxHp() {
        Hp = maxHp;
    }
    public void GainExp(int exp,int monsterAttack, int monsterDefense) {
        if (monsterAttack > monsterDefense)
        {
            CumulativeMonsterAttack += 4;
            CumulativeMonsterDefense += 2;
        }
        else {
            CumulativeMonsterDefense += 3;//３から変更
            CumulativeMonsterAttack += 1;
        }
        int sumExp = exp + Exp;
        Level += (int)(sumExp / levelUpExp);
        Exp = sumExp % levelUpExp;
        RenewStatus();
    }
    public void ForceLevelUp(int level) {
        this.Level += level;
        RenewStatus();
    }

    private void RenewStatus() {
        int basicPoint = 30;
        int basicCoef = 2;
        Attack = (int)(basicPoint +
            (basicCoef + CumulativeMonsterAttack) *(Level - 1));
        Defense = (int)(basicPoint +
            (basicCoef + CumulativeMonsterDefense) * (Level - 1));
    }
}
