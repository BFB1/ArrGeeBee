using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;
    public static readonly Color[] PossibleColors = {Color.blue, Color.red, Color.green};
    
    public Player player;
    public Transform enemy;

    public float enemySpawnRate = 0.01f;
    private float scoreSeconds;

    private void Start()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one active GameManager at a time!");
        }
    }

    private void Update()
    {
        if (Random.value < enemySpawnRate && player.enabled)
        {
            MakeEnemy();
        }

        if (Random.value < 0.1)
        {
            enemySpawnRate += 0.01f * Time.deltaTime;
        }

        scoreSeconds += Time.deltaTime;
    }

    private Vector2 RandomSpawnPosition(Vector2 center, float radius)
    {
        // Finnur staðsetningu ákveðna vegalengd frá miðju í worldspace
        float angle = Random.Range(0, 360);
        Vector2 newPosition;
        newPosition.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        newPosition.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return newPosition;
    }

    private void MakeEnemy()
    {
        Enemy newEnemy = Instantiate(enemy, RandomSpawnPosition(player.transform.position, 15), Quaternion.identity).GetComponent<Enemy>();
        newEnemy.target = player.transform;
    }

    public static Color RandomColor()
    {
        return PossibleColors[Random.Range(0, PossibleColors.Length)];
    }

    public void GameOver()
    {
        Score.Seconds = Mathf.RoundToInt(scoreSeconds);
        SceneManager.LoadScene(2);
    }
}
