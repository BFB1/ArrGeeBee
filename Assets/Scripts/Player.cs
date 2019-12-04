using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public int speedModifier = 1;
    
    public Transform projectileSpawn;
    
    [SerializeField]
    private SpriteRenderer colorIndicator;
    [SerializeField]
    private SpriteRenderer colorIndicatorAnimation;

    public Transform bullet;
    public Color currentColor = Color.red;
    
    private Rigidbody2D rb;
    private Animator animator;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        colorIndicator.color = currentColor;
        StartCoroutine(ColorSwitchTimer(Random.Range(5, 7)));
    }
    
    private void FixedUpdate()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        rb.velocity = Vector2.zero;
        
        Vector2 movement = new Vector2();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement.y += 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x += -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement.x += 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movement.y += -1;
        }

        rb.position += speedModifier * Time.deltaTime * movement.normalized;
        
        // Snúningur er reiknaður í "screen space". Þetta er í lagi vegna þess að ég nota orthographic myndavél
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        
        // Breyta úr vector2 í gráðuhorn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.SetRotation(Quaternion.AngleAxis(angle - 90, Vector3.forward));
    }

    private void Shoot()
    {
        if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse0)) return;
        
        // Eins og er getur spilarinn skotið um það bil 50 sinnum á sek því ég kalla á það úr FixedUpdate()
        // Mér finnst það ekki trufla leikinn neitt.
        
        Transform newBullet = Instantiate(bullet, projectileSpawn.position, transform.rotation);
            
        Rigidbody2D newBulletRb = newBullet.gameObject.GetComponent<Rigidbody2D>();
        newBullet.GetComponent<Projectile>().SetColor(currentColor);
            
        newBulletRb.AddForce(newBullet.up * 20, ForceMode2D.Impulse);
        newBulletRb.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse);

        rb.position -= (Vector2)transform.up * 0.1f; // Recoil
    }

    // Þessi kóði skiptir um lit á spilaranum á 5 til 15 sek fresti.
    
    public void OnColorSwitchAnimationEnd()
    {
        Color color = colorIndicatorAnimation.color;
        currentColor = color;
        colorIndicator.color = color;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(rb.position, 3);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                Destroy(enemy.gameObject);
            }
        }

        StartCoroutine(ColorSwitchTimer(Random.Range(5, 15)));
    }

    private IEnumerator ColorSwitchTimer(int time)
    {
        yield return new WaitForSeconds(time);
        
        Color newColor;
        while ((newColor = GameMaster.RandomColor()) == currentColor) {} // :3
        colorIndicatorAnimation.color = newColor;
        
        animator.Play("SwitchColor");
    }
}
