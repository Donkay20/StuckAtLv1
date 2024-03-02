using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackCircle : MonoBehaviour
{
    private readonly float KNOCKBACK_CIRCLE_MAXTIME = 0.2f;
    float initialSize = 1;
    float maximumSize = 10;
    float timer;
    private Slot slot;
    void Start() {
        slot = GetComponentInParent<Slot>();
        maximumSize *= slot.GetCommonUpgrade(11);
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= KNOCKBACK_CIRCLE_MAXTIME) {
            Destroy(gameObject);
        }

        float time = Mathf.Clamp01(timer/KNOCKBACK_CIRCLE_MAXTIME);
        float currentRadius = Mathf.Lerp(initialSize, maximumSize, time);
        SetSize(currentRadius);
    }

    private void SetSize(float radius) {
        transform.localScale = new Vector2(radius, radius);
    }
}
