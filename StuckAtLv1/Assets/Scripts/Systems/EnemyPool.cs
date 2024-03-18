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
    private ObjectPool<GameObject> skeleton1, skeleton2, skeleton3, golem1, golem2, golem3, spider1, spider2, spider3;

    private void Awake() {
        Instance = this;
        InitializePools();
    }

    private void InitializePools() {
        skeleton1 = new ObjectPool<GameObject>(() => CreateEnemy(0), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        skeleton2 = new ObjectPool<GameObject>(() => CreateEnemy(1), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        skeleton3 = new ObjectPool<GameObject>(() => CreateEnemy(2), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        golem1 = new ObjectPool<GameObject>(() => CreateEnemy(3), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        golem2 = new ObjectPool<GameObject>(() => CreateEnemy(4), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        golem3 = new ObjectPool<GameObject>(() => CreateEnemy(5), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        spider1 = new ObjectPool<GameObject>(() => CreateEnemy(6), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        spider2 = new ObjectPool<GameObject>(() => CreateEnemy(7), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
        spider3 = new ObjectPool<GameObject>(() => CreateEnemy(8), OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
    }

    GameObject CreateEnemy(int ID) {
        var newEnemy = Instantiate(enemyPoolPrefab[ID]);
        newEnemy.SetActive(false);
        return newEnemy;
    }

    void OnTakeFromPool(GameObject enemy) {
        //enemy.GetComponent<Enemy>().SetHealth(enemy.GetComponent<Enemy>().maxHP);
        enemy.SetActive(true);
    }

    void OnReturnToPool(GameObject enemy) {
        enemy.GetComponent<Enemy>().Cleanse();
        enemy.SetActive(false);
    }

    public GameObject GetEnemy(int ID) {
        switch (ID) {
            case 0:
                return skeleton1.Get();
            case 1:
                return skeleton2.Get();
            case 2:
                return skeleton3.Get();
            case 3:
                return golem1.Get();
            case 4:
                return golem2.Get();
            case 5:
                return golem3.Get();
            case 6:
                return spider1.Get();
            case 7:
                return spider2.Get();
            case 8:
                return spider3.Get();
            //case 9+: 
                //forest enemies begin here
            default:
                return null;
        }
    }

    public void ReturnEnemy(GameObject enemy) {
        switch (enemy.tag) {
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
