using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    private readonly float BASE_DURATION = 0.3f;
    private readonly float BASE_TARGET_MULTIPLIER = 2f;
    private readonly int BASE_DAMAGE = 8;
    private float timer;
    private float initialSize, targetSize, timeModifier, maxTime;
    private int damage;
    Slot parent;

    /*
    Ground slam's initial AoE is affected by Size modifiers. The time it expands for is based off of Duration modifiers.
    Its damage based affected by damage modifiers.
    */
    
    void Start()
    {
        parent = FindAnyObjectByType<AttackSpawner>().GetParent();
        //set the parent before anything else, by grabbing the parent's relation to the slot

        initialSize = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(initialSize, initialSize); 
        //set the initial size of the aoe

        maxTime = BASE_DURATION + parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f;
        //set the time it expands for

        timeModifier = parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f;
        targetSize = BASE_TARGET_MULTIPLIER * (initialSize + (timeModifier*2));
        //the ground slam will keep expanding depending on the duration modifier; the max size will also increase in relation to that.

        damage = (int)(BASE_DAMAGE * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        //set damage the attack deals
    }

    void Update()
    {
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
        if (enemy != null) {
            enemy.TakeDamage(damage);   
            //if a modifier increases damage, it will call back to the parent slot and acquire the modifier for calculation
        }
    }
}
