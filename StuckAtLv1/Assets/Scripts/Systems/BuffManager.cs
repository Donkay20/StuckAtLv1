using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Movement movement;
    //Player-related scripts
    [Space]
    [SerializeField] private Image[] activeBuffImages = new Image[5];
    [SerializeField] private Image[] activeDebuffImage = new Image[5];
    [SerializeField] private TextMeshProUGUI[] activeBuffDuration = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI[] activeDebuffDuration = new TextMeshProUGUI[5];
    //Front-end
    [Space]
    [SerializeField] private Sprite[] buffIcons = new Sprite[5];
    [SerializeField] private Sprite[] debuffIcons = new Sprite[5];
    //Storage for all of the different types of icons
    private Buff[] activeBuffList = new Buff[5];
    private Debuff[] activeDebuffList = new Debuff[5];
    //Back-end
    private int position;
    //Variables

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            AddBuff("speed", 0.3f, 5f);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            AddBuff("power", 0.3f, 4f);
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            AddBuff("bloodsucker", 0.5f, 6f);
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            AddBuff("bulwark", 0.5f, 30f);
        }
        //Manual buffs for testing purposes
        UpdateDisplay();
    }

    public void AddBuff(string buff, float efficacy, float duration) {
        //Add a buff to the list. Instantiate a new buff object and populate it.
        Buff b = gameObject.AddComponent<Buff>();
        b.Identity = position; activeBuffList[position] = b; position++;
        b.Initialize(buff, efficacy, duration);
        RecalculateBuffs();
    }

    public void AddDebuff(string condition, int duration) {
        //Add a debuff to the list. Instantiate a new debuff object and populate it.
        //todo
        switch (condition) {
            case "anemia":
                break;
            case "slow":
                break;
            case "bleed":
                break;
            case "blind":
                break;
            case "stun":
                break;
        }
    }

    private void UpdateDisplay() {
        //Should constantly be running to update displays for all currently active buffs.
        foreach(Buff b in activeBuffList) {
            int i = 0;
            switch (b.BuffType) {
                case "speed":
                    i = 1;
                    break;
                case "power":
                    i = 2; 
                    break;
                case "bloodsucker":
                    i = 3;
                    break;
                case "bulwark":
                    i = 4;
                    break;
            }
            activeBuffDuration[b.Identity].text = b.Duration.ToString("0f");
            activeBuffImages[b.Identity].sprite = buffIcons[i];
        }

        foreach(Debuff d in activeDebuffList) {
            int i = 0;
            //todo
        }
    }

    public void BuffExpired(int id) {
        //Remove the buff and its effects, destroy the buff with that ID, then rearrange all of the other buffs
        Destroy(activeBuffList[id]);

        for (int i = id + 1; i < position; i++) {
            //todo, trying to shift all of the other stuff back
            if (activeBuffList[i + 1] != null) {
                activeBuffList[i] = activeBuffList[i + 1];
            }
        }
        activeBuffList[position] = null; activeBuffImages[position].sprite = buffIcons[0];
        position--;
        RecalculateBuffs();
    }

    public void DebuffExpired(int id) {
        //todo
    }

    private void RecalculateBuffs() {
        float speed = 0f, power = 0f;
        foreach (Buff b in activeBuffList) {
            switch (b.BuffType) {
                case "speed":
                    speed += b.Efficacy;
                    break;
                case "power":
                    power += b.Efficacy;
                    break;
            }
        }
        movement.SpeedModifier = speed;
        character.DamageModifier = power;
    }

    private void EndBattle() {
        foreach (Buff b in activeBuffList) {
            b.EndBuff();
        }
    }
}

/*
Buff Icon directory:
0 - nothing/blank
1 - speed
2 - damage
*/
