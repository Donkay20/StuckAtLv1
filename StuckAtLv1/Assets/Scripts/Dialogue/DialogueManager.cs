using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> lines = new Queue<string>();
    private Queue<string> names = new Queue<string>();
    private Queue<string> emotions = new Queue<string>();
    public TMP_Text lineText;
    public TMP_Text nameText;
    public float textSpeed = 0.3f;
    public Dialogue dialogue;
    public DialogueCharacterList characters;
    public GameObject leftCharacter;
    public GameObject rightCharacter;
    private DialogueCharacter speaker;
    private bool messaging;
    private string currentEmotion;

    // Start is called before the first frame update
    void Start()
    {
        lineText.text = string.Empty;

        storeLines();
        displayNextSentence();
        displayNextName();
        displayNextEmotion();
        displaySpriteColours();
        if (speaker.location == "left") {
            displaySpriteLeft();
        }
        else {
            displaySpriteRight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                displayNextSentence();
                displayNextName();
                displaySpriteColours();
                if (speaker.location == "left") {
                    displaySpriteLeft();
                }
                else {
                    displaySpriteRight();
                }
            }
        }

    }

    void storeLines() {
        foreach (string s in dialogue.sentences) {
            lines.Enqueue(s);
        }
        foreach (string s in dialogue.names) {
            names.Enqueue(s);
        }
        foreach (string s in dialogue.emotions) {
            emotions.Enqueue(s);
        }
    }

    void displayNextName() {
        string nextName = names.Dequeue();
        setSpeaker(nextName);
        nameText.text = nextName;
    }
    void displayNextEmotion() {
        string nextEmotion = emotions.Dequeue();
        currentEmotion = nextEmotion;
    }

    void setSpeaker(string nextName) {
        //iterates through all the right characters to see if one of them is currently speaking, 
        //if not it will then check the left characters
        foreach (DialogueCharacter c in characters.charactersRight) {
            if (nextName == c.characterName) {
                speaker = c;
                speaker.location = "right";
            }
        }
        //checking left characters
        foreach (DialogueCharacter c in characters.charactersLeft) {
            if (nextName == c.characterName) {
                speaker = c;
                speaker.location = "left";
            }
        }
    }

    void displayNextSentence() {

         string currentLine = lines.Dequeue();
         if (messaging) {
                StopAllCoroutines();
            }
        StartCoroutine(TypeLines(currentLine));

    }

    void displaySpriteColours() {
        if (speaker.characterName == " ") {
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            Debug.Log("reached this point");
        }
        else {
            if (speaker.location == "right") {
                //Debug.Log("reached here!");
                rightCharacter.GetComponent<SpriteRenderer>().color = Color.white;
                leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            }
            else {
                rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
                leftCharacter.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    void displaySpriteRight() {
        /*if (speaker.characterName == " ") {
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else {
            if (speaker.location == "right") {
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.white;
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            }
            else {
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }/*/
        
        switch(currentEmotion) {
            case "neutral":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.neutralSprite;
                break;
            case "happy":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.happySprite;
                break;
            case "sad":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.sadSprite;
                break;
            case "thinking":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.thinkingSprite;
                break;
            case "mad":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.madSprite;
                break;
        }
       
    }
    void displaySpriteLeft() {

        /*if (speaker.location == "left") {
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.white;
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else {
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.white;
        }*/
        
        switch(currentEmotion) {
            case "neutral":
                leftCharacter.GetComponent<SpriteRenderer>().sprite = speaker.neutralSprite;
                break;
            case "happy":
                leftCharacter.GetComponent<SpriteRenderer>().sprite = speaker.happySprite;
                break;
            case "sad":
                leftCharacter.GetComponent<SpriteRenderer>().sprite = speaker.sadSprite;
                break;
            case "thinking":
                leftCharacter.GetComponent<SpriteRenderer>().sprite = speaker.thinkingSprite;
                break;
            case "mad":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.madSprite;
                break;
        }
    }

    IEnumerator TypeLines(string currentLine) {

        lineText.text = "";
       
        messaging = true;
        
        foreach (char c in currentLine.ToCharArray()) {
            lineText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        
        messaging = false;
    }
}
