using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform playerTransform;
    public List<GameObject> objectsToSpawn;
    public float despawnDistance = 20f;

    public float timer = 0;

    private GameObject spawnedObject;

    public void SpawnObject()
    {
        if (objectsToSpawn != null && objectsToSpawn.Count > 0)
        {
            playerTransform = FindAnyObjectByType<PlayerManager>().transform;

            int randomIndex = Random.Range(0, objectsToSpawn.Count);
            GameObject objectToSpawn = objectsToSpawn[randomIndex];
            Vector3 spawnDirection = playerTransform.right;
            Vector3 spawnPosition = playerTransform.position + spawnDirection * 10f;

            if (!spawnedObject)
                spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Lista de objetos para serem instanciados está vazia ou nula!");
        }
    }

    private void Update()
    {
        if (timer >= 10)
        {
            timer = 0;
            SpawnObject();
        }
        else if(!spawnedObject)
        {
            timer += Time.deltaTime;
        }

        if (spawnedObject != null)
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, spawnedObject.transform.position);
            if (distanceToPlayer > despawnDistance)
            {
                Destroy(spawnedObject);
            }
        }
    }
}
