using System;
using UnityEngine;
public class AddToReserveBalls : BonusBase
{
    protected override void initializeFields()
    {
        this.color = new Color(0.215f, 0.588f, 0);
        this.textColor = Color.white;
        this.text = "Ball";
    }

    protected override void BonusActivate()
    {
        _playerScript.addBallsToReserve(1);
        Debug.Log("Bonus");
    }
}

