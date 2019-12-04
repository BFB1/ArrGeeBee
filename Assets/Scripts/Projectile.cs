using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Color color;

    private void Start()
    {
        StartCoroutine(DeSpawn());
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        GetComponent<SpriteRenderer>().color = newColor;
    }

    private IEnumerator DeSpawn()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<Enemy>().color == this.color)
            {
                Score.Kills++;
                Destroy(other.gameObject);
            }
        } else if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Player>().currentColor == this.color)
            {
                // Þetta olli böggum og er í raun óþarfi ¯\_(ツ)_/¯
            }
        } else if (other.gameObject.CompareTag("Projectile"))
        {
            if (other.gameObject.GetComponent<Projectile>().color == this.color)
            {
                Destroy(other.gameObject);
            }
        }
        Destroy(gameObject); // Eyðir alltaf kúlunni.
    }
}
