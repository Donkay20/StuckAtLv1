using UnityEngine;

public class SandPillar : MonoBehaviour
{
    private readonly int PILLAR_BASE_DMG = 20;
    private readonly float PILLAR_BASE_DURATION = 4f;
    private Slot slot;
    private float duration;
    private int damage;
    private float size;
    private Vector2 mousePosition;
    private Camera mainCamera;
    void Start() {
        slot = FindAnyObjectByType<AttackSpawner>().GetParent();
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, PILLAR_BASE_DURATION);

        damage = asb.GetDamageBonus(slot, PILLAR_BASE_DMG);
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.collider.TryGetComponent<Enemy>(out var enemy)) {
            if (enemy.CompareTag("Sandworm")) {
                enemy.GetComponent<Sandworm>().Slowdown();
                Destroy(gameObject);
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage * 4);
            } else {
                FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
            }
        }
    }
}