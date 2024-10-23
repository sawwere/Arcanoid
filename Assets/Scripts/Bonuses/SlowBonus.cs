﻿using System;
using UnityEngine;
public class SlowBonus : BonusBase
{
    protected override void initializeFields()
    {
        this.color = new Color(0.215f, 0.588f, 0);
        this.textColor = Color.white;
        this.text = "Slow";
    }

    protected override void BonusActivate()
    {
        _playerScript.changeBallsVelocity(-0.1f);
    }
}

