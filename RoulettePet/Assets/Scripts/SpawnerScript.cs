using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnDelay = 5;
    public float initialDelay = 1;

    public float maxSpawns;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawningTimer());
    }

    IEnumerator SpawningTimer()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }

    }

}
