using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeLineLoading : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private readonly float LOADING_TIME = 4;
    private float timer;
    private void OnEnable() {
        timer = 0;
    }

    void Update() {
        if (timer < LOADING_TIME) {
            timer += Time.deltaTime;
            // Calculate direction to the target
            Vector3 direction = target.transform.position - transform.position;
            direction.z = 0f; // Ensure only rotation around Z-axis

            // Calculate angle from direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation around Z-axis
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

            transform.Rotate(Vector3.forward, angle);
        }
        
        float barwidthPercentage = Mathf.Clamp01(timer/LOADING_TIME);
        transform.localScale = new Vector2(barwidthPercentage, 1);
    }
}
