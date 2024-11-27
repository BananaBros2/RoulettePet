using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleScript : MonoBehaviour
{
    public float destroyTime = 0.5f;
    public bool onlyDestroyOnTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if (onlyDestroyOnTrigger) { return; }

        Invoke("DestroyParticle", destroyTime);
    }

    void DestroyParticle()
    {
        Destroy(this.gameObject);
    }
}
