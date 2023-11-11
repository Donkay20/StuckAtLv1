using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite clickedCursor;
    public Sprite normalCursor;

    // Start is called before the first frame update
    void Start()
    {
        CursorOff();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      transform.position = cursorPos;  

      Pressed();
    }

    void CursorOff(){
      Cursor.visible = false;
    }

    void Pressed()
    {
      if(Input.GetMouseButtonDown(0))
      {
        Cursor.visible = false;
        rend.sprite = clickedCursor;
        Invoke(nameof(Unpress), 0.1f);
      }
    }

    void Unpress()
    {
      rend.sprite = normalCursor;
    }
}
