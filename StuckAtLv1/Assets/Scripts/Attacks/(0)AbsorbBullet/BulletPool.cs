using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int initialPoolSize = 3;
    public int maxSize = 100;
    private ObjectPool<GameObject> bulletPool;
    private SlotManager slotManager;
    private bool collectionChecks = false;

    private void Awake() {
        slotManager = FindAnyObjectByType<SlotManager>();
        Instance = this;
        InitializePool();
    }

    private void InitializePool() {
        bulletPool = new ObjectPool<GameObject>(CreateBullet, OnTakeFromPool, OnReturnToPool, null, collectionChecks, initialPoolSize, maxSize);
    }

    GameObject CreateBullet() {
        var bullet = Instantiate(bulletPrefab, slotManager.gameObject.transform.position, Quaternion.identity, slotManager.transform);
        bullet.GetComponent<TrailRenderer>().enabled = false;
        bullet.SetActive(false);
        bullet.GetComponent<AbsorbBullet>().UpdateTimer();
        bullet.GetComponent<AbsorbBullet>().enabled = false;
        return bullet;
    }

    void OnTakeFromPool(GameObject bullet) {
        bullet.transform.position = FindAnyObjectByType<SlotManager>().transform.position;
        bullet.GetComponent<AbsorbBullet>().enabled = true;
        bullet.GetComponent<AbsorbBullet>().UpdateTimer();
        bullet.SetActive(true);
    }

    void OnReturnToPool(GameObject bullet) {
        bullet.GetComponent<AbsorbBullet>().UpdateTimer();
        bullet.GetComponent<AbsorbBullet>().enabled = false;
        bullet.SetActive(false);
        bullet.transform.position = FindAnyObjectByType<SlotManager>().transform.position;
    }

    public GameObject GetBullet() {
        return bulletPool.Get();
    }

    public void ReturnBullet(GameObject bullet) {
        bulletPool.Release(bullet);
    }
}
