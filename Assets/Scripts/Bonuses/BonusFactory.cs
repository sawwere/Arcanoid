using System;
using static PlusTwoBonus;

public static class BonusFactory
{
    public static System.Type getBonusScript()
    {
        Random random = new Random();
        return random.Next(1, 6) switch
        {
            1 => typeof(SlowBonus),
            2 => typeof(FastBonus),
            3 => typeof(AddToReserveBalls),
            4 => typeof(PlusTwoBonus),
            5 => typeof(PlusTenBonus),
            _ => typeof(BonusBase)
        };
    }
}

