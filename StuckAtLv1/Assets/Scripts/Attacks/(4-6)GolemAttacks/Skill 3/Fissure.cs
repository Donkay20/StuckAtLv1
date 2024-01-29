using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fissure : MonoBehaviour
{
    private readonly int BASE_DAMAGE = 3;
    private readonly float BASE_DURATION = 3f;
    private int damage;
    private float timer;
    private Slot parent;    

    void Start()
    {
        FaceMouse();

        parent = FindAnyObjectByType<AttackSpawner>().GetParent();
        //set the parent before anything else, by grabbing the parent's relation to the slot

        float size = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(size, size);
        //set the size of the fissure

        timer = BASE_DURATION + parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f;
        Debug.Log(timer);
        //set the time the fissure lasts

        damage = (int)(BASE_DAMAGE * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        //set damage the attack deals
    }
    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);   
            //if a modifier increases damage, it will call back to the parent slot and acquire the modifier for calculation
        }
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
            Debug.Log("time ran out");
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        //Inflict slow, todo
    }

    private void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = -direction;
    }
}
