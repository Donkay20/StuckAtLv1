using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPool;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject player;
    float timer;
    int condition; bool specialCondition;

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }

    private void SpawnEnemy() {
        int enemyID = -1;

        if (specialCondition) {
        //special conditions are for events.
            switch (condition) {
            case 2: //Ruins Event 2
                enemyID = 0;
                break;
            case 8: //Ruins Event 5
                enemyID = 1;
                break;
            }
        } else {
            enemyID = Random.Range(0, enemyPool.Length);
        }

        Vector3 position = GenerateRandomPosition();
        position += player.transform.position;
        GameObject newEnemy = Instantiate(enemyPool[enemyID]); //change this value to test specific enemies
        newEnemy.transform.position = position;
        newEnemy.GetComponent<Enemy>().SetTarget(player);
        newEnemy.transform.parent = transform;
    }

    private Vector3 GenerateRandomPosition() {

        Vector3 position =  new Vector3();

        float f = Random.value > 0.5f ? -1f : 1f;

        if(Random.value > 0.5f) 
        {
            position.x = Random.Range(-spawnArea.x, spawnArea.x);
            position.y = spawnArea.y * f;
        } else {
            position.y = Random.Range(-spawnArea.y, spawnArea.y);
            position.x = spawnArea.x * f;

        }
        position.z = 0;
        return position;
    }

    public void SetCondition(int n) {
        if (n == -1) {
            specialCondition = false;
        } else {
            specialCondition = true;
        }
        condition = n;
    }
}
