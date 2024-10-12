using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    const int maxLevel = 0;

    public GameDataScript gameData;
    public Button exitButton;
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void Awake()
    {
        if (gameData.level > maxLevel)
        {
            exitButton.enabled = true;
            exitButton.onClick.AddListener(() => Commands.GetExitCommand().Execute());
        }
    }

    private void Start()
    {
        if (gameData.resetOnStart)
            gameData.Load();
        
    }
}
