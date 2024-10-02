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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        {
            hitsToDestroy--;
            if (hitsToDestroy == 0)
            {
                Destroy(gameObject);
                playerScript.BlockDestroyed(points);
            }
                
            else if (textComponent != null)
                textComponent.text = hitsToDestroy.ToString();
        }
    }
}
