using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BonusBase : MonoBehaviour
{
	public Color[] colors = { Color.yellow, Color.green, Color.red, Color.blue, Color.white, Color.black };
	public TMP_Text textComponent;
	private Rigidbody2D rb;
	public GameDataScript gameData;
    private const int pointsPerActivation = 100;
    protected PlayerScript _playerScript;
    protected Color color = Color.yellow;
    protected Color textColor = Color.black;
    protected String text = "+100";
    private const float deltaY = 0.02f;

    void initializeBonus()
    {
        initializeFields();
        gameObject.GetComponent<SpriteRenderer>().color = color;
        var textComponent = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.color = textColor;
    }

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(0f, -0.5f);
        _playerScript = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerScript>();
        // _audioSource = gameObject.GetComponent<AudioSource>();
        initializeBonus();
    }

	void Update()
    {
        if (Time.timeScale > 0)
        {
            var pos = transform.position;
            pos.y = gameObject.transform.position.y - deltaY;
            transform.position = pos;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("In");
            BonusActivate();
            Destroy(gameObject);
        }
        Debug.Log("Out");
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(gameObject);
	}

    protected virtual void BonusActivate()
    {
        _playerScript.AddPoints(pointsPerActivation);
    }

    protected virtual void initializeFields() { }


}
