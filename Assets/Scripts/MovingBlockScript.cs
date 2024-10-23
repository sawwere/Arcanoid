using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovingBlockScript : MonoBehaviour
{
    public GameObject textObject;
    private TMP_Text textComponent;
    private PlayerScript playerScript;
    private GameObject BonusObject;
    private GameObject bonus;
    public GameDataScript gameData;
    public int hitsToDestroy;
    public int points;

    Rigidbody2D rb;
    public Vector2 velocity;
    [SerializeField]
    public int speed;
    [SerializeField]
    public float minX;
    [SerializeField]
    public float maxX;
    private int c = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<TMP_Text>();
            textComponent.text = hitsToDestroy.ToString();
        }
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        BonusObject = GameObject.Find("Bonus");

        velocity = rb.velocity;
        velocity.x = speed;
        rb.velocity = velocity;

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            var pos = wall.transform.position;
            if (pos.y == 0)
            {
                if (pos.x > 0 && maxX > pos.x - 1.5f)
                {
                    Debug.Log("yes");
                    maxX = pos.x - 1.5f;
                }
                if (pos.x < 0 && minX < pos.x + 1.5f)
                {
                    Debug.Log("yes");
                    minX = pos.x + 1.5f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        var pos = transform.position;
        if ((pos.x <= minX || pos.x >= maxX) && c == 10)
        {
            Debug.Log(rb.velocity.x);
            velocity = rb.velocity;
            velocity.x = -1 * velocity.x;
            rb.velocity = velocity;
            c = 0;
        }
        else if (c < 10)
        {
            c += 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hitsToDestroy--;
            if (hitsToDestroy == 0)
            {
                Destroy(gameObject);
                playerScript.BlockDestroyed(points);

                if (gameObject.name == "Green Block Variant(Clone)")
                {
                    bonus = Instantiate(BonusObject, gameObject.transform.position, gameObject.transform.rotation);
                    bonus.AddComponent<BonusBase>();
                    bonus.GetComponent<BonusBase>().build(gameData, "+100");
                }
            }
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }
}
