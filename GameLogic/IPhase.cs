using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IPhase 
{
    UniTask BegineAsync(int currentStage);
    void Setup(IUserInterface userInterface, ItemManager itemManager, PlayerStatus playerStatus);
}
