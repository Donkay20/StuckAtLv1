using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBranch : MonoBehaviour
{
    [SerializeField] private GameObject bonusBranchL, bonusBranchR;
    [SerializeField] private GameObject bonusBranchSpawnPosition;
    [SerializeField] private bool originalBranch;
    private readonly int BASE_TREE_BRANCH_DAMAGE = 3;
    private readonly float BASE_TREE_BRANCH_DURATION = 1f;
    private int damage;
    private float duration;
    private float size;
    private Slot slot;
    private Vector2 direction;
    private bool bonusBranchSpawned;

    void Start() {
        if (originalBranch) {
            RotateTowardsMouse();
            slot = FindAnyObjectByType<AttackSpawner>().GetParent();
            ActivateBranch(slot);
        }
    }

    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0) {
            Destroy(gameObject);
        }
    }

    public void ActivateBranch(Slot s) {
        slot = s;
        AttackSlotBonus asb = FindAnyObjectByType<AttackSlotBonus>();

        size = asb.GetSizeBonus(slot);
        transform.localScale = new Vector2(size, size);

        duration = asb.GetDurationBonus(slot, BASE_TREE_BRANCH_DURATION);

        damage = asb.GetDamageBonus(slot, BASE_TREE_BRANCH_DAMAGE);
    }

    private void RotateTowardsMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.TryGetComponent<Enemy>(out var enemy)) {
            if (!bonusBranchSpawned) {
                GameObject b2 = Instantiate(bonusBranchR, bonusBranchSpawnPosition.transform.position, Quaternion.identity);
                b2.GetComponent<TreeBranch>().ActivateBranch(slot);
                bonusBranchSpawned = true;
            }
            FindAnyObjectByType<OnHitBonus>().ApplyDamageBonus(slot, enemy, damage);
        }
    }
}