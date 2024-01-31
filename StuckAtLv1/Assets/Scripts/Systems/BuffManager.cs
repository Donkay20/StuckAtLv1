using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Image[] buffIcons = new Image[20];
    [SerializeField] private Image[] debuffIcons = new Image[20];
    //Storage for all of the different types of icons
    private readonly float TICK_RATE = 1f;
    //Constants
    //LinkedList<string> activeList = new LinkedList<string>();
    //LinkedList<int> activeCD = new LinkedList<int>();
    private string[] activeList = new string[5];
    private float[] activeCD = new float[5];
    //Back-end

    private void Update() {
        //todo, make Buff class, then make linkedlist with a link for each buff I guess
    }

    public void AddBuff(string buff, int duration) {
        switch(buff) {
            case "speed":
                break;
            case "damage":
                break;
            case "recovery":
                break;
            case "bloodsucker":
                break;
            case "bulwark":
                break;
        }
    }

    public void AddDebuff(string condition, int duration) {
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

    private void EndBattle() {
        StopAllCoroutines();
    }
}
