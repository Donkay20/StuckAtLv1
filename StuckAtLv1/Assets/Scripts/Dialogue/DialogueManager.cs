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
    public TMP_Text lineText;
    public TMP_Text nameText;
    public float textSpeed = 0.3f;
    public Dialogue dialogue;
    public DialogueCharacterList characters;
    public GameObject leftCharacter;
    public GameObject rightCharacter;
    private bool messaging;

    // Start is called before the first frame update
    void Start()
    {
        lineText.text = string.Empty;
        storeLines();
        displayNextSentence();
        displayNextName();
        displaySprites();
    }

    // Update is called once per frame
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                displayNextSentence();
                displayNextName();
                displaySprites();
            }
        }

    }

    void storeLines() {
        /*string line1 = "Good day Mother! To what do I owe the pleasure?";
        string line2 = "Jamp…I am so sorry to say, but as of right now, I am disowning you. Please pack your things and leave by tonight.";
        lines.Enqueue(line1);
        lines.Enqueue(line2);*/
        foreach (string s in dialogue.sentences) {
            lines.Enqueue(s);
        }
        foreach (string s in dialogue.names) {
            names.Enqueue(s);
        }
    }

    void displayNextName() {
        string nextName = names.Dequeue();
        
        foreach (DialogueCharacter c in characters.characters) {
            if (nextName == c.characterName) {
                c.isSpeaking = true;
            }
            else {
                c.isSpeaking = false;
            }
        }

        nameText.text = nextName;
    }

    void displayNextSentence() {

         string currentLine = lines.Dequeue();
         if (messaging) {
                StopAllCoroutines();
            }
        StartCoroutine(TypeLines(currentLine));

    }

    void displaySprites() {
        if (characters.characters[0].isSpeaking == true) { //the first character listed will always be Jamp
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else {
            leftCharacter.GetComponent<SpriteRenderer>().color = Color.grey;
            rightCharacter.GetComponent<SpriteRenderer>().color = Color.white;
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
