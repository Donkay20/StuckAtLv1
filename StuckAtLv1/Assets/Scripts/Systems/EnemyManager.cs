using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject player;
    float timer;

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }

    private void SpawnEnemy() {
        int number = Random.Range(0, enemy.Length);
        Vector3 position = GenerateRandomPosition();
        position += player.transform.position;
        GameObject newEnemy = Instantiate(enemy[number]);
        newEnemy.transform.position = position;
        newEnemy.GetComponent<Enemy>().SetTarget(player);
        newEnemy.transform.parent = transform;
    }

    private Vector3 GenerateRandomPosition() {

        Vector3 position =  new Vector3();

        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;

        if(UnityEngine.Random.value > 0.5f) 
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            position.y = spawnArea.y * f;
        } else {
            position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
            position.x = spawnArea.x * f;

        }

        position.z = 0;

        return position;
    }
}
