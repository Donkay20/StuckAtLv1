using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    private Button button;
    private SpriteState st;
    private Image image;

    public Sprite common1, uncommon1, rare1;
    public Sprite common2, uncommon2, rare2;
    public Sprite common3, uncommon3, rare3;
    //public Sprite common4, uncommon4, rare4;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Checker();  
    }

    void SpriteChange()
    {
        button.spriteState = st;
    }

    void Checker()
    {
        if(image.sprite == common1)
        {
            st.pressedSprite = common3;
            st.highlightedSprite = common2;
            st.selectedSprite = common2;
            st.disabledSprite = common2;
            SpriteChange();
        }
        else if(image.sprite == uncommon1)
        {
            st.pressedSprite = uncommon3;
            st.highlightedSprite = uncommon2;
            st.selectedSprite = uncommon2;
            st.disabledSprite = uncommon2;
            SpriteChange();
        }
        else if(image.sprite == rare1)
        {
            st.pressedSprite = rare3;
            st.highlightedSprite = rare2;
            st.selectedSprite = rare2;
            st.disabledSprite = rare2;
            SpriteChange();
        }
        else if(image.sprite == null)
        {
            Debug.Log("No Upgrade Rarity Assigned");
        }
    }
}
