using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationController : MonoBehaviour
{
    public bool shootProjectiles;
    public GameObject shotenParticle;
    public int projectileOffset = 0;
    public float projectileGrowSpeed = 0;

    public bool jabber;
    private GameObject spriteHolder;

    System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        if (jabber)
        {
            spriteHolder = transform.GetChild(0).gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartEffects(float shotOffset)
    {
        if (shootProjectiles)
        {
            print("GOT");
            //StopCoroutine(ShootParticles(shotOffset));
            StartCoroutine(ShootParticles(shotOffset));
        }
        
        if (jabber)
        {
            StartCoroutine(JabbingMotion());
        }
    }


    public void StartShootingParticles()
    {
        //StartCoroutine(ShootParticles());
    }
    public void StopShootingParticles()
    {
        //StopCoroutine(ShootParticles());
    }

    IEnumerator ShootParticles(float shotOffset)
    {
        while(true)
        {
            print("looping");
            yield return new WaitForSeconds(0.02f);
            GameObject newParticle = Instantiate(shotenParticle, transform.position, transform.rotation);

            float randomOffset = (float)rand.Next(-projectileOffset / 2, projectileOffset / 2);
            print(shotOffset + randomOffset);
            newParticle.transform.Rotate(0, 0, shotOffset + randomOffset);
            newParticle.GetComponent<Rigidbody2D>().velocity = newParticle.transform.right * 5f;

            newParticle.GetComponent<ParticleScript>().StartSpin(rand.Next(-100, 100) / 10);
            newParticle.GetComponent<ParticleScript>().StartGrow(projectileGrowSpeed);
        }
    }

    IEnumerator JabbingMotion()
    {
        int motionAmount = 0;
        while(motionAmount < 5)
        {
            spriteHolder.transform.position += spriteHolder.transform.right / 12;
            motionAmount++;
            yield return new WaitForSeconds(0.01f);
        }

        motionAmount = 0;
        while (motionAmount < 5)
        {
            spriteHolder.transform.position -= spriteHolder.transform.right / 12;
            motionAmount++;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
