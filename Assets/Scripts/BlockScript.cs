using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    public GameObject textObject;
    private TMP_Text textComponent;
    private PlayerScript playerScript;
	private GameObject BonusObject;
	private GameObject bonus;
	public GameDataScript gameData;
    public bool isBonusBlock;
    public int hitsToDestroy;
    public int points;

    void Start()
    {
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<TMP_Text>();
            textComponent.text = hitsToDestroy.ToString();
		}
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
		//BonusObject = GameObject.Find("Bonus");
	}


    void OnCollisionEnter2D(Collision2D collision)
    {
        {
            hitsToDestroy--;
            if (hitsToDestroy == 0)
            {
                if (isBonusBlock)
                {
                    playerScript.SpawnBonus(transform.position);
                }
                Destroy(gameObject);
                playerScript.BlockDestroyed(points);

				//if (gameObject.name== "Green Block Variant(Clone)")
				//{
					//bonus = Instantiate(BonusObject, gameObject.transform.position, gameObject.transform.rotation);
					//bonus.AddComponent<BonusBase>();
                    //bonus.GetComponent<BonusBase>().build(gameData, "+100");
				//}

			}
                
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }
}
