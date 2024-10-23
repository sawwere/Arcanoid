using System;
using UnityEngine;

    public class PlusTwoBonus : BonusBase
    {
        protected override void initializeFields()
        {
            this.color = Color.blue;
            this.textColor = Color.white;
            this.text = "+2";
        }

        protected override void BonusActivate()
        {
            _playerScript.addBallsToGame(2);
        }
    }



