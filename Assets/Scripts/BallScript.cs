using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 ballInitialForce;
    Rigidbody2D rb;
    GameObject playerObj;
    AudioSource audioSrc;
    //public AudioClip hitSound;
    //public AudioClip loseSound;

    public GameDataScript gameData;

    float deltaX;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        deltaX = transform.position.x;
    }
    void Update()
    {
        if (rb.isKinematic)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                rb.isKinematic = false;
                rb.AddForce(ballInitialForce);
            }
            else
            {
                var pos = transform.position;
                pos.x = playerObj.transform.position.x + deltaX;
                transform.position = pos;
            }
        }
        if (!rb.isKinematic && Input.GetKeyDown(KeyCode.J))
        {
            var v = rb.velocity;
            if (Random.Range(0, 2) == 0)
                v.Set(v.x - 0.1f, v.y + 0.1f);
            else
                v.Set(v.x + 0.1f, v.y - 0.1f);
            rb.velocity = v;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameData.sound)
        {
            //audioSrc.PlayOneShot(loseSound, gameData.soundVolume);
        }
        Destroy(gameObject);
        playerObj.GetComponent<PlayerScript>().BallDestroyed();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameData.sound)
        {
            //audioSrc.PlayOneShot(hitSound, gameData.soundVolume);
        }
    }
}
