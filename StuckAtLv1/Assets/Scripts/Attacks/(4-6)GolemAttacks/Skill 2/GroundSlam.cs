using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    private readonly float GROUNDSLAM_BASE_DURATION = 0.3f;
    private readonly float BASE_TARGET_MULTIPLIER = 2f;
    private readonly int GROUNDSLAM_BASE_DAMAGE = 8;
    private float timer;
    private float initialSize, targetSize, timeModifier, maxTime;
    private int damage;
    Slot slot;

    /*
    Ground slam's initial AoE is affected by Size modifiers. The time it expands for is based off of Duration modifiers.
    Its damage based affected by damage modifiers.
    */
    
    void Start()
    {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();
        //set the parent before anything else, by grabbing the parent's relation to the slot

        initialSize = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(initialSize, initialSize); 
        //set the initial size of the aoe

        maxTime = asb.GetDurationBonus(slot, GROUNDSLAM_BASE_DURATION);

        timeModifier = asb.GetDurationBonus(slot, GROUNDSLAM_BASE_DURATION); //-GROUNDSLAM_BASE_DURATION ??
        targetSize = BASE_TARGET_MULTIPLIER * (initialSize + (timeModifier * 2));
        //the ground slam will keep expanding depending on the duration modifier; the max size will also increase in relation to that.

        damage = asb.GetDamageBonus(slot, GROUNDSLAM_BASE_DAMAGE);
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
        Enemy enemy = col.GetComponent<Enemy>();
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }
}
