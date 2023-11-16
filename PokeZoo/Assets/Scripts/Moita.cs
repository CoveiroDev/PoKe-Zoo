using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moita : MonoBehaviour
{
    public Spawn spawn;
    private void OnEnable()
    {
        spawn = FindAnyObjectByType<Spawn>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (playerManager != null)
        {
            gameManager.FindFight();
            Destroy(spawn.spawnedObject);
        }
    }
}
