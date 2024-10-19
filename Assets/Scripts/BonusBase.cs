using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class BonusBase : MonoBehaviour
{
	public Color[] colors = { Color.yellow, Color.green, Color.red, Color.blue, Color.white, Color.black };
	public TMP_Text textComponent;
	private Rigidbody2D rb;
	public GameDataScript gameData;

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(0f, -0.5f);
	}

	void Update()
    {
        
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		BonusActivate(100);
		Destroy(gameObject);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(gameObject);
	}

	void BonusActivate(int points)
	{
		gameData.points += points;
	}

	public void build(GameDataScript gameData, string text)
	{
		textComponent = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
		this.gameData = gameData;
		switch (text)
		{
			case "+100":
				gameObject.GetComponent<SpriteRenderer>().color = colors[0];
				textComponent.text = text;
				textComponent.color = colors[5];
				break;
			case "Slow":

				break;
			case "Fast":

				break;
			case "Ball":

				break;
			case "+2":

				break;
			case "+10":

				break;
		}
	}
}
