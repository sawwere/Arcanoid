using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 ballInitialForce;
    Rigidbody2D rb;
    GameObject playerObj;
    AudioSource audioSrc;
    public AudioClip hitSound;
    public AudioClip loseSound;

    float deltaX;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        audioSrc = Camera.main.GetComponent<AudioSource>();
        deltaX = transform.position.x;
    }
    void Update()
    {
        if (rb.isKinematic)
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("fire");
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

    void OnTriggerEnter2D(Collider2D other)
    {
        audioSrc.PlayOneShot(loseSound);
        Destroy(gameObject);
        playerObj.GetComponent<PlayerScript>().BallDestroyed();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        audioSrc.PlayOneShot(hitSound);
    }
}
