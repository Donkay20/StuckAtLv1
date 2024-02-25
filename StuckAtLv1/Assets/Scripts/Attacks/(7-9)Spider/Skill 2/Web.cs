using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Web : MonoBehaviour
{
    private Slot parent;
    private float ballTimer = 1f; private float webTimer = 2f;
    private float initialSize; private float expandedSize;
    private bool skillHit;
    Rigidbody2D rb;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private float speed = 5;
    private int damage;
    [SerializeField] private Sprite fullWeb;
    void Start() {
        skillHit = false;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        parent = FindAnyObjectByType<AttackSpawner>().GetParent();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        rb.velocity = direction.normalized * speed;

        initialSize = 1 + parent.GetCommonUpgrade(1)*0.2f + parent.GetRareUpgrade(1)*0.3f + parent.GetLegendaryUpgrade(1)*0.4f;
        expandedSize = initialSize * 5;
        transform.localScale = new Vector2(initialSize, initialSize);

        //apply duration bonus
        ballTimer *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        webTimer *= 1 + (parent.GetCommonUpgrade(2)*0.2f + parent.GetRareUpgrade(2)*0.4f + parent.GetLegendaryUpgrade(2)*0.6f);
        Debug.Log("ball timer: " + ballTimer + ", webtimer: " + webTimer);

        //apply damage bonus
        damage = (int)(1 * (1+(parent.GetCommonUpgrade(0)*0.2f + parent.GetRareUpgrade(0)*0.4f + parent.GetLegendaryUpgrade(0)*0.6f)));
        Debug.Log("damage: " + damage);
    }

    void Update() {
        if (!skillHit) {
            ballTimer -= Time.deltaTime;
            if (ballTimer <= 0) {
                Destroy(gameObject);
            }
        }

        if (skillHit) {
            webTimer -= Time.deltaTime;
            if(webTimer <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void TransformIntoWeb() {
        Debug.Log("transformed into web");
        GetComponent<SpriteRenderer>().sprite = fullWeb;
        transform.localScale = new Vector2(expandedSize, expandedSize);
        skillHit = true;
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            if(!skillHit) {
            TransformIntoWeb();
            }
            enemy.ApplyStun(1f);
            enemy.TakeDamage(damage);   //if a modifier increases damage, it would call back to the parent slot and acquire the modifier for calculation
        }
    }
}
