using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMouseFollowCamera : MonoBehaviour
{
    public Canvas myCanvas;
    void Start () {

    }
   
    void Update () {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, null, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
    }
}
