using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private string[] dialogue; //the text that shows up for the narrative of each event
    [SerializeField] private string[] names; //the names/characters that show up for each line of dialogue
    [SerializeField] private string[] optionsToSelect; //the text to display for each button of the branching options.
    [SerializeField] private int[] skipToThisValueAfterChoices; //start at X line to grab the dialogue for each option
    [SerializeField] private int[] outcome; //the outcomes of each dialogue path; what to do afterwards

    public string GetTitle() {
        return title;
    }
    
    public string[] GetDialogue() {
        return dialogue;
    }

    public string[] GetNames() {
        return names;
    }

    public string[] GetOptions() {
        return optionsToSelect;
    }

    public int[] GetSkipEntry() {
        return skipToThisValueAfterChoices;
    }

    public int[] GetOutcome() {
        return outcome;
    }
}