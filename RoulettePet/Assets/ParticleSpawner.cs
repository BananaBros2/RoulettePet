using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particleObject;
    public float spawnFrequency = 0.2f;

    System.Random rand = new System.Random();


    void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        int numberObject = 0;

        while (true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            numberObject++;
            print(numberObject);

            GameObject newParticle = Instantiate(particleObject, transform.position, Quaternion.identity);
            newParticle.GetComponent<ParticleScript>().StartSpin(rand.Next(-100, 100) / 10);
        }
    }
}
