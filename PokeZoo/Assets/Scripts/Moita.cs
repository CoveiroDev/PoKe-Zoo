using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moita : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (playerManager != null)
        {
            gameManager.FindFight();
            Destroy(gameObject);
        }
    }
}
