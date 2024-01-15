using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueCharacter", order = 1)]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public string location;
    public Sprite neutralSprite;
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite thinkingSprite;
    public Sprite madSprite;

}
