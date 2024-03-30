using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    public GameObject[] enemyPoolPrefab;
    private int initialPoolSize = 3;
    private int maxSize = 50;
    private bool collectionChecks = false;
    private ObjectPool<GameObject> skeleton1, skeleton2, skeleton3, golem1, golem2, golem3, spider1, spider2, spider3; //world 1
    private ObjectPool<GameObject> squirrel1, squirrel2, squirrel3, tree1, tree2, tree3, wolf1, wolf2, wolf3; //world 2

    private void Awake() {
        Instance = this;
        InitializePools();
    }

    private void InitializePools() {
        //World 1: Ruins
        skeleton1 = new ObjectPool<GameObject>(() => CreateEnemy(0), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        skeleton2 = new ObjectPool<GameObject>(() => CreateEnemy(1), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        skeleton3 = new ObjectPool<GameObject>(() => CreateEnemy(2), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        golem1 = new ObjectPool<GameObject>(() => CreateEnemy(3), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        golem2 = new ObjectPool<GameObject>(() => CreateEnemy(4), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        golem3 = new ObjectPool<GameObject>(() => CreateEnemy(5), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        spider1 = new ObjectPool<GameObject>(() => CreateEnemy(6), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        spider2 = new ObjectPool<GameObject>(() => CreateEnemy(7), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        spider3 = new ObjectPool<GameObject>(() => CreateEnemy(8), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);

        //World 2: Forest
        squirrel1 = new ObjectPool<GameObject>(() => CreateEnemy(9), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        squirrel2 = new ObjectPool<GameObject>(() => CreateEnemy(10), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        squirrel3 = new ObjectPool<GameObject>(() => CreateEnemy(11), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        tree1 = new ObjectPool<GameObject>(() => CreateEnemy(12), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        tree2 = new ObjectPool<GameObject>(() => CreateEnemy(13), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        tree3 = new ObjectPool<GameObject>(() => CreateEnemy(14), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        wolf1 = new ObjectPool<GameObject>(() => CreateEnemy(15), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        wolf2 = new ObjectPool<GameObject>(() => CreateEnemy(16), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        wolf3 = new ObjectPool<GameObject>(() => CreateEnemy(17), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
    }

    GameObject CreateEnemy(int ID) {
        var newEnemy = Instantiate(enemyPoolPrefab[ID]);
        newEnemy.SetActive(false);
        return newEnemy;
    }

    void OnTakeFromPool(GameObject enemy) {
        enemy.SetActive(true);
    }

    void OnReturnToPool(GameObject enemy) {
        enemy.GetComponent<Enemy>().Cleanse();
        enemy.SetActive(false);
    }

    public GameObject GetEnemy(int ID) {
        return ID switch {
            //ruins enemies
            0 => skeleton1.Get(),
            1 => skeleton2.Get(),
            2 => skeleton3.Get(),
            3 => golem1.Get(),
            4 => golem2.Get(),
            5 => golem3.Get(),
            6 => spider1.Get(),
            7 => spider2.Get(),
            8 => spider3.Get(),
            //forest enemies
            9 => squirrel1.Get(),
            10 => squirrel2.Get(),
            11 => squirrel3.Get(),
            12 => tree1.Get(),
            13 => tree2.Get(),
            14 => tree3.Get(),
            15 => wolf1.Get(),
            16 => wolf2.Get(),
            17 => wolf3.Get(),
            //todo: sewer
            _ => null,
        };
    }

    public void ReturnEnemy(GameObject enemy) {
        switch (enemy.tag) {
            //Ruins Enemies
            case "Skeleton1":
                skeleton1.Release(enemy);
                break;
            case "Skeleton2":
                skeleton2.Release(enemy);
                break;
            case "Skeleton3":
                skeleton3.Release(enemy);
                break;
            case "Golem1":
                golem1.Release(enemy);
                break;
            case "Golem2":
                golem2.Release(enemy);
                break;
            case "Golem3":
                golem3.Release(enemy);
                break;
            case "Spider1":
                spider1.Release(enemy);
                break;
            case "Spider2":
                spider2.Release(enemy);
                break;
            case "Spider3":
                spider3.Release(enemy);
                break;

            //Forest Enemies
            case "Squirrel1":
                squirrel1.Release(enemy);
                break;
            case "Squirrel2":
                squirrel2.Release(enemy);
                break;
            case "Squirrel3":
                squirrel3.Release(enemy);
                break;
            case "Tree1":
                tree1.Release(enemy);
                break;
            case "Tree2":
                tree2.Release(enemy);
                break;
            case "Tree3":
                tree3.Release(enemy);
                break;
            case "Wolf1":
                wolf1.Release(enemy);
                break;
            case "Wolf2":
                wolf2.Release(enemy);
                break;
            case "Wolf3":
                wolf3.Release(enemy);
                break;
                
            //Sewer Enemies
            //case "":
                //x.Release(enemy);
        }
    }

    public void WorldOneClear() {
        skeleton1.Dispose(); skeleton2.Dispose(); skeleton3.Dispose();
        golem1.Dispose(); golem2.Dispose(); golem3.Dispose();
        spider1.Dispose(); spider2.Dispose(); spider3.Dispose();
    }

    public void WorldTwoClear() {
        //todo
    }

    public void WorldThreeClear() {
        //todo
    }
}