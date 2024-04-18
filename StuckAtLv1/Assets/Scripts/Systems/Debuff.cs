using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debuff : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI activeDurationText;
    [SerializeField] private Image activeDebuffIcon;
    [SerializeField] private Sprite[] debuffIcons;
    private Character character;
    private Movement movement;
    private String debuffType;
    private float severity;
    private float duration;
    private bool debuffActive;
    private float tickRate;
    private int identity;   public int Identity { get => identity; set => identity = value; }
    
    void Awake() {
        character = FindAnyObjectByType<Character>();
        movement = FindAnyObjectByType<Movement>(); 
    }

    void Update() {
        if (debuffActive) {
            duration -= Time.deltaTime;
            tickRate -= Time.deltaTime;
            if (duration > 0) {
                if (duration > 1) {
                    activeDurationText.text = duration.ToString("f0");
                } else {
                    activeDurationText.text = duration.ToString("f1");
                }
            } else {
                EndDebuff();
            }

            if (tickRate <= 0) {
                switch (debuffType) {
                    case "anemia":
                        if (character.currentHp > 1) {
                            character.TakeDamage((int)Math.Ceiling(character.currentHp * severity));
                            tickRate = 1;
                        }
                        break;
                }
            }
        }
    }

    public void Initialize(string dt, float s, float dur) {
        debuffType = dt;
        severity = s;
        duration = dur;
        Debug.Log("Debuff inflicted. Type: "+ debuffType + ", Efficacy: "+ severity + ", Duration: " + duration);
        debuffActive = true;
        AdjustDebuff(debuffActive);
    }

    public void AdjustDebuff(bool x) {
    //If true: turn the debuff on. If false: turn the debuff off.
        int i = 0;
        switch(debuffType) {
            case "slow": //Reduces the speed modifier. Higher % = higher slow.
                i = 1;
                if (x) {
                    movement.SpeedDebuff *= severity; 
                } else {
                    movement.SpeedDebuff /= severity;
                }
                break;
            case "bleed": //If the character has overhealing, the drain rate is increased.
                i = 2;
                if (x) {
                    character.DrainTimerModifier *= severity;
                } else {
                    character.DrainTimerModifier /= severity;
                }
                break;
            case "anemia": //Deals [efficacy]% of the character's current health per second. Always deals at least 1 damage. Anemia handled in Update method.
                i = 3;
                tickRate = 1;
                break;
            //todo: other debuffs
        }
        activeDebuffIcon.sprite = debuffIcons[i];
    }

    public (string, float, float) GetDebuffInfo() {
        return (debuffType, severity, duration);
    }

    public void EndDebuff() {
        debuffActive = false;
        AdjustDebuff(debuffActive);
        FindAnyObjectByType<BuffManager>().DebuffExpired(identity);
        Destroy(gameObject);
    }
}

/*
Icon directory:
0 - No Icon / blank
1 - Slow
2 - Bleed
3 - Anemia
4 - 
5 - 
*/
