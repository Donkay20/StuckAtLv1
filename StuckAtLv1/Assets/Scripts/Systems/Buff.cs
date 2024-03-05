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
    private Character character;
    private Movement movement;
    private string buffType;
    private float efficacy;
    private float duration;
    private bool buffActive;
    private int identity;   public int Identity { get => identity; set => identity = value; }

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
        buffType = b; efficacy = e; duration = d;
        //Debug.Log("Buff granted. Type: "+ buffType + ", Efficacy: "+ efficacy + ", Duration: " + duration);
        buffActive = true;
        AdjustBuff(buffActive);   
    }

    public void AdjustBuff(bool x) {
    //If true: add buff. 
    //If false: remove buff.
        int i = 0;
        switch(buffType) {
            case "power":
                i = 1;
                if (x) {
                    character.AdjustDamageModifier(efficacy); 
                } else {
                    character.AdjustDamageModifier(efficacy * -1);
                }
                break;
            case "speed":
                i = 2;
                if (x) {
                    movement.SpeedModifier += efficacy; 
                    i = 2;
                } else {
                    movement.SpeedModifier -= efficacy;
                }
                break;
            case "bloodsucker": //todo
                i = 3;
                if (x) {
                    FindAnyObjectByType<BuffManager>().SetBloodsuckerStatus(true);
                } else {
                    FindAnyObjectByType<BuffManager>().SetBloodsuckerStatus(false);
                }
                break;
            case "bulwark": //todo
                i = 4;
                if (x) {
                    character.ActivateBulwark();
                    FindAnyObjectByType<BuffManager>().SetBulwarkStatus(true);
                } else {
                    character.DeactivateBulwark();
                    FindAnyObjectByType<BuffManager>().SetBulwarkStatus(false);
                }
                break;
            case "critdmg": //todo
                i = 5;
                if (x) {
                    character.AdjustCriticalDamageModifier(efficacy);
                } else {
                    character.AdjustCriticalDamageModifier(efficacy * -1);
                }
                break;
        }
        activeBuffIcon.sprite = buffIcons[i];
    }

    public void EndBuff() {
        //Communicate to the buff manager that this buff should be deleted, and reorganize the buffs.
        buffActive = false;
        AdjustBuff(buffActive);
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
5 - crit dmg
*/
