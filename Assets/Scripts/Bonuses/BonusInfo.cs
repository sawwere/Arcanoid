using System;
using UnityEngine;

public abstract class BonusInfo : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Color BackgroundColor { get; private set; }
    [field: SerializeField] public Color TextColor { get; private set; } = Color.white;

    public abstract void ExecuteAction(IBallController ballController);
}


