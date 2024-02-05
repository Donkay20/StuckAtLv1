using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBite : MonoBehaviour
{
    private int damage; //damage will also affect the poison dmg
    private readonly float BASE_DURATION = 5; //duration will increase the duration of the anemic effect
    private float timer = 1f;
    private Slot parent;
    private BuffManager buffManager;
    [SerializeField] private Sprite spiderBite;
    void Start() {
        GetComponent<SpriteRenderer>().sprite = spiderBite;
        FaceMouse();
        parent = GetComponentInParent<Slot>();
        buffManager = FindAnyObjectByType<BuffManager>();
        buffManager.AddBuff("speed", 0.9f, 1f * (1 + parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f)); //duration of spd buff affected by duration

        float scalingFactor = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(0.5f * scalingFactor, 0.5f * scalingFactor);

        //apply duration bonus
        timer *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        Debug.Log("timer: " + timer);

        damage = (int)(1 * (5+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        Debug.Log("damage: " + damage);
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.ApplyAnemia(damage / 2, BASE_DURATION + parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
            enemy.TakeDamage(damage);   
        }
    }

    private void FaceMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
        float offsetDistance = 0.2f;
        Vector3 offsetPosition = transform.position + (Vector3)direction * offsetDistance;
        transform.position = offsetPosition;
    }
}
