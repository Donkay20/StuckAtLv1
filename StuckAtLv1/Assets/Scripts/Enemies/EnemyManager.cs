using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    //[SerializeField] GameObject[] enemyPool;
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject player;
    [SerializeField] MapManager mapManager;
    [SerializeField] GameObject[] forestSpecialEnemies;
    float timer;
    int condition; bool specialCondition;

    //todo: add special separate timer for special enemy spawns during forest, then randomize the timer

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }

    private void SpawnEnemy() {
        int enemyID = -1; Vector3 position;

        if (specialCondition) {
        //special conditions are for events.
            switch (condition) {
            case 2: //Ruins Event 2
                enemyID = Random.Range(0, 3);
                break;
            case 8: //Ruins Event 5
                enemyID = Random.Range(3, 6);
                break;
            }
        } else {
            switch(mapManager.GetWorld()) {
                case 1:
                    enemyID = Random.Range(0, 9);
                    break;
                case 2:
                    enemyID = Random.Range(0, 9);
                    //enemyID = Random.Range(9, 17); //fix when world 2 enemies are in
                    break;
                case 3:
                    enemyID = Random.Range(17, 26);
                    break;
                case 4:
                    enemyID = Random.Range(0, 26);
                    break;
            }
        }
        
        switch (enemyID) {      //special conditions for position goes here dependent on enemy, otherwise just default to a random position
            /*
            case int n when n < 9:
                position = GenerateRandomPosition();
                position += player.transform.position;
                break;
            case int n when n > 9 && n < 17:
                //todo different spawn patterns
            */
            default:
                position = GenerateRandomPosition();
                position += player.transform.position;
                break;
        }
        
        GameObject newEnemy = EnemyPool.Instance.GetEnemy(enemyID); //change this value to test specific enemies
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

    public void SetSpawnTimer(float newTimer) {
        spawnTimer = newTimer;
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
