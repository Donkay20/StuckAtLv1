using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] string[] dialogue; //the text that shows up for the narrative of each event
    [SerializeField] string[] names; //the names/characters that show up for each line of dialogue
    [SerializeField] string[] optionsToSelect; //the text to display for each button of the branching options.
    [SerializeField] int[] skipToThisValueAfterChoices; //start at X line to grab the dialogue for each option
    [SerializeField] int[] outcome; //the outcomes of each dialogue path; what to do afterwards
}
