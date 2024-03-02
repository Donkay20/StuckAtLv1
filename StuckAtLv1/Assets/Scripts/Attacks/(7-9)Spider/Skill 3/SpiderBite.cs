using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBite : MonoBehaviour
{
    [SerializeField] private Sprite spiderBite;
    private readonly int SPIDERBITE_BASE_DMG = 1;
    private readonly float SPIDERBITE_BASE_ANEMIA_DURATION = 5f; //duration will ALSO increase the duration of the anemic effect
    private readonly float SPIDERBITE_BASE_DURATION = 1f;
    private int damage; //damage will also affect the poison dmg
    private float timer;
    private float size;
    private Slot slot;
    private BuffManager buffManager;
    AttackSlotBonus asb;
    
    void Start() {
        FaceMouse();
        GetComponent<SpriteRenderer>().sprite = spiderBite;
        slot = GetComponentInParent<Slot>();
        buffManager = FindAnyObjectByType<BuffManager>();
        asb = FindAnyObjectByType<AttackSlotBonus>();

        buffManager.AddBuff("speed", 0.9f, asb.GetDurationBonus(slot, SPIDERBITE_BASE_DURATION)); //duration of spd buff affected by duration

        size = asb.GetSizeBonus(slot); transform.localScale = new Vector2(0.5f * size, 0.5f * size);
        timer = asb.GetDurationBonus(slot, SPIDERBITE_BASE_DURATION);
        damage = asb.GetDamageBonus(slot, SPIDERBITE_BASE_DMG);
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
            enemy.ApplyAnemia(damage / 2, asb.GetDurationBonus(slot, SPIDERBITE_BASE_ANEMIA_DURATION));
        }
        FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
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
