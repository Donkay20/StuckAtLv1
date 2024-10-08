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
    [SerializeField] private Button skipButton;

    void Start()
    {
        InitializeButtons();
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
        if (lines.Count <= 0) {
            Debug.Log("Ran out of lines.");
            Scene currentScene = gameObject.scene;
            Debug.Log("Current scene: " + currentScene.name);
            switch (currentScene.name) {
                //Intro
            case "OpeningScene":
                SceneManager.LoadScene("ArtifactIntro");
                break;
            case "ArtifactIntro":
                SceneManager.LoadScene("Tutorial");
                break;
            
            //Ruins
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
                SceneManager.UnloadSceneAsync("RuinsBossEnd");
                SceneManager.LoadScene("RuinsEnd", LoadSceneMode.Additive);
                break;
            case "RuinsEnd":
                SceneManager.LoadScene("ForestIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("RuinsEnd");
                break;

            //Forest
            case "ForestIntro":
                gameManager.ReceiveCommand("map");
                break;
            case "ForestMiniBossIntro":
                gameManager.ReceiveCommand("miniboss");
                break;
            case "ForestMiniBossEnd":
                gameManager.ReceiveCommand("map");    
                break;
            case "ForestBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "ForestBossEnd":
                SceneManager.LoadScene("ForestEnd", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("ForestBossEnd");
                break;
            case "ForestEnd":
                SceneManager.LoadScene("CaveIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("ForestEnd");
                break;

            //Catacombs
            case "CaveIntro":
                gameManager.ReceiveCommand("map");
                break;
            case "CaveMiniBossIntro":
                gameManager.ReceiveCommand("miniboss");
                break;
            case "CaveMiniBossEnd":
                gameManager.ReceiveCommand("map");    
                break;
            case "CaveBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "CaveBossEnd":
                SceneManager.LoadScene("CaveEnd", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("CaveBossEnd");
                break;
            case "CaveEnd":
                SceneManager.LoadScene("AbyssIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("CaveEnd");
                break;
            
            //Abyss
            case "AbyssIntro":
                gameManager.ReceiveCommand("map");
                break;
            case "AbyssMiniBossIntro":
                gameManager.ReceiveCommand("miniboss");
                break;
            case "AbyssMiniBossEnd":
                gameManager.ReceiveCommand("map");    
                break;
            case "AbyssBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "AbyssBossEnd":
                SceneManager.LoadScene("TiffBossIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("AbyssBossEnd");
                break;

            //Final Boss
            case "TiffBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "TiffBossEnd":
                SceneManager.LoadScene("FinalScene");
                break;
            case "FinalScene":
                SceneManager.LoadScene("TitleScreen");
                break;
            }
        }
        if (lines.Count > 0) {
            currentLine = lines.Dequeue();
        }
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

    private void SkipDialogue() {
        Scene currentScene = gameObject.scene;
        switch (currentScene.name) {
                //Intro
            case "OpeningScene":
                SceneManager.LoadScene("ArtifactIntro");
                break;
            case "ArtifactIntro":
                SceneManager.LoadScene("Tutorial");
                break;
            
            //Ruins
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
                SceneManager.UnloadSceneAsync("RuinsBossEnd");
                SceneManager.LoadScene("RuinsEnd", LoadSceneMode.Additive);
                break;
            case "RuinsEnd":
                SceneManager.LoadScene("ForestIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("RuinsEnd");
                break;

            //Forest
            case "ForestIntro":
                gameManager.ReceiveCommand("map");
                break;
            case "ForestMiniBossIntro":
                gameManager.ReceiveCommand("miniboss");
                break;
            case "ForestMiniBossEnd":
                gameManager.ReceiveCommand("map");    
                break;
            case "ForestBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "ForestBossEnd":
                SceneManager.LoadScene("ForestEnd", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("ForestBossEnd");
                break;
            case "ForestEnd":
                SceneManager.LoadScene("CaveIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("ForestEnd");
                break;

            //Catacombs
            case "CaveIntro":
                gameManager.ReceiveCommand("map");
                break;
            case "CaveMiniBossIntro":
                gameManager.ReceiveCommand("miniboss");
                break;
            case "CaveMiniBossEnd":
                gameManager.ReceiveCommand("map");    
                break;
            case "CaveBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "CaveBossEnd":
                SceneManager.LoadScene("CaveEnd", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("CaveBossEnd");
                break;
            case "CaveEnd":
                SceneManager.LoadScene("AbyssIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("CaveEnd");
                break;
            
            //Abyss
            case "AbyssIntro":
                gameManager.ReceiveCommand("map");
                break;
            case "AbyssMiniBossIntro":
                gameManager.ReceiveCommand("miniboss");
                break;
            case "AbyssMiniBossEnd":
                gameManager.ReceiveCommand("map");    
                break;
            case "AbyssBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "AbyssBossEnd":
                SceneManager.LoadScene("TiffBossIntro", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("AbyssBossEnd");
                break;

            //Final Boss
            case "TiffBossIntro":
                gameManager.ReceiveCommand("boss");
                break;
            case "TiffBossEnd":
                SceneManager.LoadScene("FinalScene");
                break;
            case "FinalScene":
                SceneManager.LoadScene("TitleScreen");
                break;
            }
    }

    private void InitializeButtons() {
        skipButton.onClick.AddListener(() => SkipDialogue());
    }
}
