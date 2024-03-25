using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBomb : MonoBehaviour
{
    [SerializeField] private GameObject plant;
    [SerializeField] private GameObject explosion;
    [SerializeField] private CircleCollider2D explosionCollider;
    [SerializeField] private Sprite explosionImage;
    private readonly float TIME_TO_EXPLODE = 2f;
    private float timer;

    void Update() {
        timer += Time.deltaTime;
        float time = Mathf.Clamp01(timer/TIME_TO_EXPLODE);
        SetSize(time);

        if (timer >= TIME_TO_EXPLODE) {
            Explode();
        }
    }

    private void SetSize(float radius) {
        explosion.transform.localScale = new Vector2(radius, radius);
    }

    private void Explode() {
        Destroy(plant);
        explosion.GetComponent<SpriteRenderer>().sprite = explosionImage;
        explosionCollider.enabled = true;
    }
}
