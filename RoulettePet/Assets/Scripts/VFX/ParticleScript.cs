using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public float destroyTime = 0.5f;
    public bool onlyDestroyOnTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.TryGetComponent<Animator>(out Animator animator))
        {
            System.Random rand = new System.Random();
            animator.SetInteger("RandomAnimation", rand.Next(1, 3));
        }

        if (onlyDestroyOnTrigger) { return; }
        Invoke("DestroyParticle", destroyTime);
    }

    public void StartSpin(float speed)
    {
        StartCoroutine(SpinParticle(speed));
    }

    IEnumerator SpinParticle(float speed)
    {
        while(true)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + speed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartGrow(float growSpeed)
    {
        StartCoroutine(GrowParticle(growSpeed));
    }

    IEnumerator GrowParticle(float speed)
    {
        while (true)
        {
            transform.localScale += Vector3.one * speed;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DestroyParticle()
    {
        Destroy(this.gameObject);

        //StopAllCoroutines();
        //ShrinkToNothing(); // Messes up timing
    }

    //IEnumerator ShrinkToNothing()
    //{
    //    while(transform.localScale.x > 0)
    //    {
    //        yield return new WaitForSeconds(0.1f);
            
    //        transform.localScale += Vector3.one / ;
    //    }
    //    Destroy(this.gameObject);
    //}
}
