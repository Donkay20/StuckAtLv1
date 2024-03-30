using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    private readonly int PHEROMONE_BASE_DAMAGE = 2;
    private readonly float PHEROMONE_BASE_DURATION = 0.5f;
    private readonly float BASE_TARGET_MULTIPLIER = 2f;
    private float timer;
    private float initialSize, targetSize, timeModifier, maxTime;
    private int damage;
    Slot slot;
    void Start() {
        //Identical code to the Ground Slam thingy
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        initialSize = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(initialSize, initialSize); 
        //set the initial size of the aoe

        maxTime = asb.GetDurationBonus(slot, PHEROMONE_BASE_DURATION);

        timeModifier = asb.GetDurationBonus(slot, PHEROMONE_BASE_DURATION); 
        targetSize = BASE_TARGET_MULTIPLIER * (initialSize + (timeModifier * 2));

        damage = asb.GetDamageBonus(slot, PHEROMONE_BASE_DAMAGE);
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= maxTime) {
            Destroy(gameObject);
        }

        float time = Mathf.Clamp01(timer/maxTime);
        float currentRadius = Mathf.Lerp(initialSize, targetSize, time);
        SetSize(currentRadius);
    }

    private void SetSize(float radius) {
        transform.localScale = new Vector2(radius, radius);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            if (enemy.CompareTag("DeerNymph") && enemy != null) {
                enemy.gameObject.GetComponent<DeerNymph>().RaiseAnger();
            }
        }
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
