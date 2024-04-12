using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector2 spawnArea;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject player;
    [SerializeField] MapManager mapManager;
    [SerializeField] GameObject[] forestSpecialEnemies;
    private List<Enemy> activeEnemyList = new List<Enemy>();
    private float timer, specialTimer;
    int condition; bool eventCondition;

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
            case 2:     //Ruins Event 2
                enemyID = Random.Range(0, 3);
                break;
            case 8:     //Ruins Event 5
                enemyID = Random.Range(3, 6);
                break;
            case 11:    //Forest Event 1
                enemyID = Random.Range(15, 18);
                break;
            case 12:    //Forest Event 3
                enemyID = Random.Range(12, 15);
                break;
            case 19:    //Forest Event 5
                enemyID = Random.Range(9, 12);
                break;
            }
        } else {
            switch(mapManager.GetWorld()) {
                case 1:
                    enemyID = Random.Range(0, 9);
                    break;
                case 2:
                    enemyID = Random.Range(9, 18);
                    break;
                case 3:
                    enemyID = Random.Range(0, 18); //todo for world 3 enemies
                    break;
                case 4:
                    enemyID = Random.Range(0, 26);
                    break;
            }
        }
        GameObject newEnemy = EnemyPool.Instance.GetEnemy(enemyID); //change this value to test specific enemies or attacks
        newEnemy.transform.parent = transform;
        newEnemy.transform.position = GenerateRandomPosition() + player.transform.position;
        newEnemy.GetComponent<Enemy>().SetTarget(player);
        AddEnemy(newEnemy.GetComponent<Enemy>());
    }

    private void SpawnSpecialEnemy(int world) {
        switch (world) {
            case 2:
                GameObject newSpecialEnemy = Instantiate(forestSpecialEnemies[Random.Range(0, forestSpecialEnemies.Length)]);
                newSpecialEnemy.transform.position += player.transform.position;
                newSpecialEnemy.transform.parent = transform;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    private void ResetSpecialTimer() {
        specialTimer = Random.Range(20, 31);
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

    public void AddEnemy(Enemy enemy) {
        activeEnemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        activeEnemyList.Remove(enemy);
    }

    public void ClearEnemies() {
        List<Enemy> enemiesToRemove = new(activeEnemyList); //make a new temporary list since we can't mess up with the original list
        foreach (Enemy enemy in enemiesToRemove) {
            enemy.SelfDestruct();
            activeEnemyList.Remove(enemy);
        }
        enemiesToRemove.Clear();
    }
}