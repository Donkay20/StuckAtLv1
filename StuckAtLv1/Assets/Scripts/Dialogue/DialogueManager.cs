using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> lines = new Queue<string>();
    private Queue<string> names = new Queue<string>();
    private Queue<string> emotions = new Queue<string>();
    public TMP_Text lineText;
    public TMP_Text nameText;
    private float textSpeed = 0.03f;
    public Dialogue dialogue;
    public DialogueCharacterList characters;
    public GameObject leftCharacter;
    public GameObject rightCharacter;
    private DialogueCharacter speaker;
    private bool messaging;
    private string currentEmotion;
    private string currentLine;
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        lineText.text = string.Empty;
        gameManager = FindAnyObjectByType<GameManager>();
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

                if (messaging) {
                    lineText.text = currentLine;
                    StopAllCoroutines();
                    messaging = false;
                }
                else {
                    displayNextSentence();
                    displayNextName();
                    displayNextEmotion();
                    displaySpriteColours();
                    if (speaker.location == "left") {
                        //Debug.Log("hello");
                        displaySpriteLeft();
                    }
                    else {
                        displaySpriteRight();
                    }
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

         if (lines.Count == 0) {
            Scene currentScene = gameObject.scene;
            switch (currentScene.name) {
                case "OpeningScene":
                    SceneManager.LoadScene("ArtifactIntro");
                    break;
                case "ArtifactIntro":
                    SceneManager.LoadScene("RuinsIntro");
                    break;
                case "RuinsIntro":
                    SceneManager.LoadScene("MainGame");
                    break;
                case "RuinsMiniBossIntro":
                    gameManager.ReceiveCommand("miniboss");
                    break;
                case "RuinsMiniBossEnd":
                    gameManager.ReceiveCommand("map");    
                    break;
                case "RuinsBossIntro":
                    gameManager.ReceiveCommand("boss");
                    break;
                case "RuinsBossEnd":
                    gameManager.ReceiveCommand("map");
                    break;
            }
         }

        currentLine = lines.Dequeue();
         if (messaging) {
                StopAllCoroutines();
            }
        StartCoroutine(TypeLines(currentLine));

    }

    void displaySpriteColours() {
        if (speaker.characterName == " ") {
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            //Debug.Log("reached this point");
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
            case "shocked":
                rightCharacter.GetComponent<SpriteRenderer>().sprite = speaker.shockedSprite;
                break;
        }
       
    }
    void displaySpriteLeft() {
        
        //Debug.Log(currentEmotion);
        switch(currentEmotion) {
            case "neutral":
                //Debug.Log(currentEmotion);
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
                leftCharacter.GetComponent<SpriteRenderer>().sprite = speaker.madSprite;
                break;
            case "shocked":
                leftCharacter.GetComponent<SpriteRenderer>().sprite = speaker.shockedSprite;
                break;
        }
    }

    public string GetCurrentLine() {
        return currentLine;
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
