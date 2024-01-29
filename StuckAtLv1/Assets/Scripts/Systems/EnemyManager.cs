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
    int condition; bool specialCondition;

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }

    private void SpawnEnemy() {
        int number = -1;

        if (specialCondition) {
        //special conditions are for events.
            switch (condition) {
            case 2: //Ruins Event 2
                number = 0;
                break;
            case 8: //Ruins Event 5
                number = 1;
                break;
            }
        } else {
            number = Random.Range(0, enemy.Length);
        }

        Vector3 position = GenerateRandomPosition();
        position += player.transform.position;
        GameObject newEnemy = Instantiate(enemy[number]); //change this value to test specific enemies
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

    public void SetCondition(int n) {
        if (n == -1) {
            specialCondition = false;
            condition = n;
        } else {
            specialCondition = true;
            condition = n;
        }
    }
}
