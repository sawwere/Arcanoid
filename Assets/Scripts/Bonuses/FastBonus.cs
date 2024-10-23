using System;
using UnityEngine;
public class FastBonus : BonusBase
{
    protected override void initializeFields()
    {
        this.color = Color.red;
        this.textColor = Color.white;
        this.text = "Fast";
    }

    protected override void BonusActivate()
    {
        _playerScript.changeBallsVelocity(0.1f);
    }
}


