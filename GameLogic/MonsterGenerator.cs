using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator
{
    public static (MonsterStatus right, MonsterStatus left) GenerateTwoMonster(int level)
    {//確率による計算　あとで直す
        var monsterRight = GenerateOneMonster(level);
        var monsterLeft = GenerateOneMonster(level);
        var bmOut = MathUtility.BoxMuller(120, 30);//難易度に関わる


        int heighExp=0;
        int lowExp=0;
        if (bmOut.Z1 > bmOut.Z2)
        {
            heighExp = (int)(bmOut.Z1 > 50 ? bmOut.Z1 : 50);
            lowExp = (int)(bmOut.Z2 > 50 ? bmOut.Z2 : 50);
        }
        else {
            heighExp = (int)(bmOut.Z2 > 50 ? bmOut.Z2 : 50);
            lowExp = (int)(bmOut.Z1 > 50 ? bmOut.Z1 : 50);
        }

        int rightAmountStatus = monsterRight.attack + monsterRight.defense;
        int leftAmountStatus = monsterLeft.attack + monsterLeft.defense;

        if (rightAmountStatus > leftAmountStatus) {
            monsterRight.exp = heighExp;
            monsterLeft.exp = lowExp;
        }else
        {
            monsterRight.exp = lowExp;
            monsterLeft.exp = heighExp;
        }
        return (monsterRight, monsterLeft);
    }

    
    private static MonsterStatus GenerateOneMonster(int level) {
        MonsterStatus monster = new MonsterStatus();
        int basicPoint = 30;
        int basicCoef = level;
        float statusAve = basicPoint + basicCoef * (level - 1);

        var bmOut = MathUtility.BoxMuller(statusAve, statusAve/3f);//難易度に関わる
        monster.level = level;
        monster.attack = (int)(bmOut.Z1>10? bmOut.Z1:10);
        monster.defense = (int)(bmOut.Z2 > 10 ? bmOut.Z2 : 10);
        return monster;
    }
   
}
