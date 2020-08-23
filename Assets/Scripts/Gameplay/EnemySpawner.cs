using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy alienPrefab;
    private Enemy enemy;
    private Vector3 screenDimensions;
    private Vector3 randomPos;

    public void EnemySpawn()
	{
        enemy = Instantiate(alienPrefab, transform);
        enemy.transform.position = GenerateRandomPosition();
    }

    Vector3 GenerateRandomPosition()
    {
      screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
      randomPos = new Vector3(screenDimensions.x, Random.Range(-screenDimensions.y + 2, screenDimensions.y - 2), 0);
      return randomPos;
    }


   
}
