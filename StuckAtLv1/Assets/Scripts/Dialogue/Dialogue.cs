using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private Queue lines = new Queue();
    public TMP_Text lineText;
    public float textSpeed = 0.3f;
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
        }
    }

    void storeLines() {
        string line1 = "Good day Mother! To what do I owe the pleasure?";
        string line2 = "Jamp…I am so sorry to say, but as of right now, I am disowning you. Please pack your things and leave by tonight.";
        lines.Enqueue(line1);
        lines.Enqueue(line2);
    }

    void displayNextSentence() {

         string currentLine = (string) lines.Dequeue();
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
