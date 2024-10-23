using System;
using UnityEngine;

public class PlusTenBonus : BonusBase
{
    protected override void initializeFields()
    {
        this.color = Color.blue;
        this.textColor = Color.white;
        this.text = "+10";
    }

    protected override void BonusActivate()
    {
        _playerScript.addBallsToGame(10);
    }
}


