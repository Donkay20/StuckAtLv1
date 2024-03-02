using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpit : MonoBehaviour
{
    [SerializeField] private Sprite triangle;
    AttackSlotBonus asb;
    private Slot slot;
    private readonly int VENOMSPIT_BASE_DAMAGE = 1;
    private readonly float VENOMSPIT_BASE_DURATION = 5; //duration will increase the duration of the anemic effect, rather than the attack's staying power itself.
    private float timer = 0.5f; //this is a constant that needs to be mutable
    private int damage; //damage will also affect the poison dmg
    private float size;

    void Start() {
        FaceMouse();
        slot = GetComponentInParent<Slot>();
        asb = FindAnyObjectByType<AttackSlotBonus>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(2 * size, 2 * size); //base size is 200%
        damage = asb.GetDamageBonus(slot, VENOMSPIT_BASE_DAMAGE);
    }

    
    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) { //applies anemia by default
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {                        
            enemy.ApplyAnemia(damage, asb.GetDurationBonus(slot, VENOMSPIT_BASE_DURATION));  
        }
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
    }

    private void FaceMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = -direction;
        float offsetDistance = 0.5f;
        Vector3 offsetPosition = transform.position + (Vector3) direction * offsetDistance;
        transform.position = offsetPosition;
    }
}
