using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverUpgrade : MonoBehaviour
{
    private Image image;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite hoverSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverSprite;
        Debug.Log("Mouse Enter");
    }
}
