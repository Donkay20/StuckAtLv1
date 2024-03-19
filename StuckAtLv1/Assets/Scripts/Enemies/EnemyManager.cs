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
    [SerializeField] float timer, specialTimer;
    int condition; bool eventCondition;

    //todo: add special separate timer for special enemy spawns during forest, then randomize the timer
    private void OnEnable() {
        ResetSpecialTimer();
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            SpawnEnemy();
            timer = spawnTimer;
        }

        if (mapManager.GetWorld() > 1) {
            if (specialTimer > 0) {specialTimer -= Time.deltaTime;}
            if (specialTimer <= 0) {
                SpawnSpecialEnemy(mapManager.GetWorld());
                ResetSpecialTimer();
            }
        }
    }

    private void SpawnEnemy() {
        int enemyID = -1; //Vector3 position;
        if (eventCondition) {
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
                    enemyID = Random.Range(0, 9); //adjust for world 2 enemies
                    break;
                case 3:
                    enemyID = Random.Range(17, 26);
                    break;
                case 4:
                    enemyID = Random.Range(0, 26);
                    break;
            }
        }
        //position = GenerateRandomPosition() + player.transform.position;
        GameObject newEnemy = EnemyPool.Instance.GetEnemy(enemyID); //change this value to test specific enemies
        newEnemy.transform.position = GenerateRandomPosition() + player.transform.position;
        newEnemy.GetComponent<Enemy>().SetTarget(player);
        newEnemy.transform.parent = transform;
    }

    private void SpawnSpecialEnemy(int world) {
        switch (world) {
            case 2:
                GameObject newSpecialEnemy = Instantiate(forestSpecialEnemies[Random.Range(0, forestSpecialEnemies.Length)]);
                newSpecialEnemy.transform.position += player.transform.position;
                newSpecialEnemy.transform.parent = transform;
                break;
            //case 3:
                //break;
            //case 4:
                //break;
        }
    }

    private void ResetSpecialTimer() {
        specialTimer = Random.Range(10, 21);
        switch (mapManager.GetWorld()) {
            case 2:
                specialTimer -= 1;
                break;
            case 3:
                specialTimer -= 3;
                break;
            case 4:
                specialTimer -= 5;
                break;
        }
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
            eventCondition = false;
        } else {
            eventCondition = true;
            condition = n;
        }
    }
}
