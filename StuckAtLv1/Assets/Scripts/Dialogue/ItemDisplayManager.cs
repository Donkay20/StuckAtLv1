using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Sprite sprite;
    public string startLine;
    public string endLine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(dialogueManager.GetCurrentLine());
        if (dialogueManager.GetCurrentLine() == startLine) {
            //Debug.Log("reached here");
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        if (dialogueManager.GetCurrentLine() == endLine) {
            gameObject.SetActive(false);
        }
    }
}
