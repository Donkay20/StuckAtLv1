using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMouseFollowCamera : MonoBehaviour
{
    private Canvas myCanvas;

    void Start () {
        Cursor.visible = false;
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
   
    void Update () {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, null, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
        Pressed();
    }

    void Pressed()
    {
      if(Input.GetMouseButtonDown(0))
      {
        Cursor.visible = false;
      }
    }
}
