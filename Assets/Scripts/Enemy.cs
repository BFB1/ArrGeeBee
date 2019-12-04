using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public Color color;

    public Transform target;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        SetColor(GameMaster.RandomColor());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Ef þeir eiga ekki target stoppa þeir. Annars munu þeir fara í áttina að player
        if (!target)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Vector2 direction = target.position - transform.position;

        // Með þessum kóða er hægt að ýta þeim afturábak með því að skjóta þá.
        rb.velocity = Vector2.MoveTowards(rb.velocity, direction.normalized, 1f);
    }

    private void SetColor(Color newColor)
    {
        color = newColor;
        spriteRenderer.color = newColor;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().currentColor == this.color)
            {
                GameMaster.Instance.GameOver();
            }
        }
    }
}
