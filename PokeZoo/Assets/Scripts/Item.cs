using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("HideInInspector")]
    [HideInInspector] private HudManager hudManager;
    [SerializeField] private float timeToDestroy = 5f;

    void Start()
    {
        hudManager = FindObjectOfType<HudManager>();
    }
    private void OnMouseDown()
    {
        hudManager.HealDamage(hudManager.player);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (timeToDestroy <= 0)
        {
            hudManager.HealDamage(hudManager.animal);
            Destroy(gameObject);
        }
        else
        {
            timeToDestroy -= Time.deltaTime;
        }
    }
}
