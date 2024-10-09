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

    public GameDataScript gameData;
    AudioSource audioSrc;
    

    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();
    static bool gameStarted = false;

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
        audioSrc = Camera.main.GetComponent<AudioSource>();
        Cursor.visible = false;
        if (!gameStarted)
{
            gameStarted = true;
            if (gameData.resetOnStart)
                gameData.Reset();
        }
        level = gameData.level;
        SetMusic();
        StartLevel();
    }

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = transform.position;
        pos.x = mousePos.x;
        transform.position = pos;
        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }
        if (Input.GetKeyDown(KeyCode.S))
            gameData.sound = !gameData.sound;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
        string.Format(
        "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>" +
        " Score <b>{2}</b></size></color>",
        gameData.level, gameData.balls, gameData.points));
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
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(pointSound, 5);
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

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());
    }

    void SetMusic()
    {
        if (gameData.music)
            audioSrc.Play();
        else
            audioSrc.Stop();
    }
}
