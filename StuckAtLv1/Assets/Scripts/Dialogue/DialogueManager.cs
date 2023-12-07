using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> lines = new Queue<string>();
    private Queue<string> names = new Queue<string>();
    public TMP_Text lineText;
    public TMP_Text nameText;
    public float textSpeed = 0.3f;
    public Dialogue dialogue;
    private bool messaging;

    // Start is called before the first frame update
    void Start()
    {
        lineText.text = string.Empty;
        storeLines();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            displayNextSentence();
            displayNextName();
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
        nameText.text = nextName;
    }

    void displayNextSentence() {

         string currentLine = lines.Dequeue();
         if (messaging) {
                StopAllCoroutines();
            }
        StartCoroutine(TypeLines(currentLine));

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
