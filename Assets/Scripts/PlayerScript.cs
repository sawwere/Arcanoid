using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    const int maxLevel = 30;

    [Range(1, maxLevel)]
    public int level = 1;
    public float ballVelocityMult = 0.02f;

    [SerializeField]
    private GameObject bluePrefab;
    [SerializeField]
    private GameObject redPrefab;
    [SerializeField]
    private GameObject greenPrefab;
    [SerializeField]
    private GameObject yellowPrefab;
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private AudioClip pointSound;
    [SerializeField]
    private GameObject pauseMenu;

    public GameDataScript gameData;
    AudioManager audioManager;
    

    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();
    static bool gameStarted = false;

    int requiredPointsToBall
    {
        get { return 400 + (level - 1) * 20; }
    }

    void CreateBlocks(GameObject prefab, 
        float xMax, float yMax, 
        int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (int i = 0; i < count; i++)
            for (int k = 0; k < 20; k++)
            {
                var obj = Instantiate(prefab,
                    new Vector3((Random.value * 2 - 1) * xMax,
                    Random.value * yMax, 0),
                    Quaternion.identity);
                if (obj.GetComponent<Collider2D>().OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
                {
                    break;
                }
                Destroy(obj);
            }
    }
    void CreateBalls()
    {
        int count = 2;
        if (gameData.balls == 1)
            count = 1;
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(ballPrefab);
            var ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;
        }
    }
    void StartLevel()
    {
        SetBackground();
        var yMax = Camera.main.orthographicSize * 0.8f;
        var xMax = Camera.main.orthographicSize * Camera.main.aspect * 0.85f;
        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, 1 + level, 10);
        CreateBlocks(greenPrefab, xMax, yMax, 1 + level, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, 2 + level, 15);
        CreateBalls();
    }

    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<UnityEngine.UI.Image>();
        // why overrideSprite ?    ¯\(°_o)/¯
        bg.overrideSprite = Resources.Load(level.ToString("d2"), typeof(Sprite)) as Sprite;
    }

    void Start()
    {
        audioManager =  AudioManager.Instance;
        Cursor.visible = false;
        if (!gameStarted)
{
            gameStarted = true;
            if (gameData.resetOnStart) 
                gameData.Load();
            audioManager.ReStart();
        }
        level = gameData.level;
        StartLevel();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = transform.position;
            pos.x = mousePos.x;
            transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Commands.GetMuteMusicCommand().Execute();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Commands.GetMuteSoundEfectsCommand().Execute();
        }
            
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale > 0)
            {
                pauseMenu.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
            }
                    
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameStarted = false;
            gameData.Reset();
            SceneManager.LoadScene("MainScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Commands.GetExitCommand().Execute();
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
        string.Format(
        "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>" +
        " Score <b>{2}</b></size></color>",
        gameData.level, gameData.balls, gameData.points));
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(5, 14, Screen.width - 10, 100),
        string.Format(
            "<color=yellow><size=20><color=white>Space</color>-pause {0}" +
            " <color=white>N</color>-new" +
            " <color=white>J</color>-jump" +
            " <color=white>M</color>-music {1}" +
            " <color=white>S</color>-sound {2}" +
            " <color=white>Esc</color>-exit</size></color>",
            OnOff(Time.timeScale > 0), OnOff(!gameData.music),
            OnOff(!gameData.sound)), style
         );
    }

    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            if (level < maxLevel)
                gameData.level++;
            SceneManager.LoadScene("SampleScene");
        }
    }
    public void BlockDestroyed(int points)
    {
        gameData.points += points;
        audioManager.Play("point");
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;
            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }
        StartCoroutine(BlockDestroyedCoroutine());
    }

    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
            if (gameData.balls > 0)
                CreateBalls();
            else
            {
                gameData.Reset();
                SceneManager.LoadScene("SampleScene");
            }
    }

    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            audioManager.Play("point");
        }
    }

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());
    }

    string OnOff(bool boolVal)
    {
        return boolVal ? "on" : "off";
    }

    void OnApplicationQuit()
    {
        Debug.Log(gameData.music);
        gameData.Save();
    }


}
