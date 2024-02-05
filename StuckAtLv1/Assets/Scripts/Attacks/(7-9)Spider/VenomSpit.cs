using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpit : MonoBehaviour
{
    private int damage; //damage will also affect the poison dmg
    private readonly float BASE_DURATION = 5; //duration will increase the duration of the anemic effect, rather than the attack's staying power itself.
    private float timer = 0.5f;
    private Slot parent;
    [SerializeField] private Sprite triangle;

    void Start() {
        GetComponent<SpriteRenderer>().sprite = triangle;
        FaceMouse();
        parent = GetComponentInParent<Slot>();

        float scalingFactor = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        transform.localScale = new Vector2(3 * scalingFactor, 3 * scalingFactor);

        damage = (int)(1 * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
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
            enemy.ApplyAnemia(damage, BASE_DURATION + parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
            enemy.TakeDamage(damage);   
        }
    }

    private void FaceMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = -direction;
        float offsetDistance = 0.5f;
        Vector3 offsetPosition = transform.position + (Vector3)direction * offsetDistance;
        transform.position = offsetPosition;
    }
}
