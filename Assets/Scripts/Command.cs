using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

public static class Commands
{
    public interface ICommand
    {
        public void Execute();
    }

    class ExitCommand : ICommand
    {
        public void Execute()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    class MuteMusicCommand : ICommand
    {
        public void Execute()
        {
            AudioManager manager = AudioManager.Instance;
            Debug.Log("MuteMusicCommand");
            manager.MuteMusic();
        }
    }

    class MuteSoundEfectsCommand : ICommand
    {
        public void Execute()
        {
            AudioManager manager = AudioManager.Instance;
            Debug.Log("MuteSoundEfectsCommand");
            manager.MuteSoundEffect();
        }
    }

    public static ICommand GetMuteMusicCommand()
    {
        return new MuteMusicCommand();
    }

    public static ICommand GetMuteSoundEfectsCommand()
    {
        return new MuteSoundEfectsCommand();
    }

    public static ICommand GetExitCommand()
    {
        return new ExitCommand();
    }
}
