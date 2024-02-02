using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI activeDurationText;
    [SerializeField] private Image activeBuffIcon;
    [SerializeField] private Sprite[] buffIcons;
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    [SerializeField] private string buffType;
    [SerializeField] private float efficacy;
    [SerializeField] private float duration;
    private bool buffActive;
    [SerializeField] private int identity;       public int Identity { get => identity; set => identity = value; }

    private void Awake() {
        character = FindAnyObjectByType<Character>();
        movement = FindAnyObjectByType<Movement>();        
    }

    void Update() {
        if(buffActive) {
            duration -= Time.deltaTime;
            if (duration > 0) {
                if (duration > 1) {
                    activeDurationText.text = duration.ToString("f0");
                } else {
                    activeDurationText.text = duration.ToString("f1");
                }
            } else {
                EndBuff();
            }
        }
    }

    public void Initialize(string b, float e, float d) {
        buffType = b;
        efficacy = e;
        duration = d;
        Debug.Log("Buff granted. Type: "+ buffType + ", Efficacy: "+ efficacy + ", Duration: " + duration);
        buffActive = true;
        AdjustBuff(true);   
    }

    public void AdjustBuff(bool x) {
    //If true: add buff. If false: remove buff.
        int i = 0;
        switch(buffType) {
            case "power":
                if (x) {
                    character.DamageModifier += efficacy; 
                    i = 1;
                } else {
                    character.DamageModifier -= efficacy;
                }
                break;
            case "speed":
                if (x) {
                    movement.SpeedModifier += efficacy; 
                    i = 2;
                } else {
                    movement.SpeedModifier -= efficacy;
                }
                break;
            case "bloodsucker": //todo
                if (x) {
                    i = 3;
                } else {

                }
                break;
            case "bulwark": //todo
                if (x) {
                    i = 4;
                } else {

                }
                break;
        }
        activeBuffIcon.sprite = buffIcons[i];
    }

    public void EndBuff() {
        //Communicate to the buff manager that this buff should be deleted, and reorganize the buffs.
        buffActive = false;
        AdjustBuff(false);
        FindAnyObjectByType<BuffManager>().BuffExpired(identity);
        Destroy(gameObject);
    }
}

/*
Icon directory:
0 - no icon
1 - power
2 - speed
3 - bloodsucker
4 - bulwark
*/
