using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbsorbBullet : MonoBehaviour
{
    float timer = 1f;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            GetComponentInParent<Slot>().AbsorbBulletAvailable = true;
            Destroy(gameObject);
        }

    }
}
