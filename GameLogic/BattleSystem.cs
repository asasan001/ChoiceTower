using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem 
{
    public static OutSimulate Simulate(PlayerStatus player, MonsterStatus monster) {
        OutSimulate outSimulate = new OutSimulate();

        if (!player.IsInfinitePower)
        {
            float tmpProb = 70f * ((float)player.Attack / monster.defense - 1f) + 60f;//難易度に関わる
            if (tmpProb > 90)
            {
                outSimulate.successProb = 90;
            }
            else if (tmpProb < 30)
            {
                outSimulate.successProb = 30;

            }
            else
            {
                outSimulate.successProb = (int)tmpProb;
            }
        }
        else {
            outSimulate.successProb = 100;
        }

        if (!player.IsInfinitePower)
        {
            float tmpDamage = 20f * ((float)monster.attack / player.Defense - 1f) + 25f;//難易度に関わる
            if (tmpDamage > 45)
            {
                outSimulate.playerDamage = 45;
            }
            else if (tmpDamage < 1)
            {
                outSimulate.playerDamage = 1;

            }
            else
            {
                outSimulate.playerDamage = (int)tmpDamage;
            }
        }
        else {
            outSimulate.playerDamage = 0;
        }
        outSimulate.attack = monster.attack;
        outSimulate.defense = monster.defense;
        outSimulate.idealExp = monster.exp;
        return outSimulate;
    }
    
    public static OutBattle Battle(OutSimulate outSimulate, ItemManager itemManager, PlayerStatus player) {
        OutBattle outBattle = new OutBattle();
        outBattle.playerDamage = outSimulate.playerDamage;
        outBattle.didUseResurrection = false;
        outBattle.didSuccess = false;
        outBattle.levelUp = 0;
        outBattle.actualExp = 0;

        int remainHp = player.Hp - outSimulate.playerDamage;
        outBattle.remainHp = remainHp > 0 ? remainHp : 0;

        if (remainHp <= 0)
        {
            player.Hp = 0;
            var resurrection = itemManager.FetchItem(ItemType.Resurrection);
            if (resurrection.Num > 0)
            {
                resurrection.Activate(itemManager);
                outBattle.didUseResurrection = true;
                
            }
        }
        else {
            player.Hp = remainHp;
        }

        int oldLevel = player.Level;
        if (player.Hp > 0) {
            int r = MathUtility.Rnd.Next(0, 100);
            if (r < outSimulate.successProb)
            {
                player.GainExp(outSimulate.idealExp,outSimulate.attack, outSimulate.defense);
                outBattle.didSuccess= true;
                outBattle.levelUp = player.Level - oldLevel;
                outBattle.actualExp = outSimulate.idealExp;
            }
            else {
                player.GainExp(outSimulate.idealExp/3, outSimulate.attack, outSimulate.defense);
                outBattle.didSuccess = false;
                outBattle.levelUp = player.Level - oldLevel;
                outBattle.actualExp = outSimulate.idealExp/3;
            }
        }
        return outBattle;
    }


    public struct OutSimulate {
        public int attack;
        public int defense;
        public int playerDamage;
        public int successProb;
        public int idealExp;
    }

    public struct OutBattle {
        public int playerDamage;
        public int remainHp;
        public bool didUseResurrection;
        public bool didSuccess;
        public int levelUp;
        public int actualExp;
    }
}
