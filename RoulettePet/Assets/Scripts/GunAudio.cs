using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    private AudioSource parentAudioSource;
    public AudioClip shootSound;

    System.Random randomizer;

    // Start is called before the first frame update
    void Start()
    {
        randomizer = new System.Random();
        parentAudioSource = transform.parent.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShootSound()
    {
        //parentAudioSource.clip = shootSound;
        parentAudioSource.pitch = 1 + ((float)randomizer.Next(-100, 100) / 800);
        parentAudioSource.PlayOneShot(shootSound);
    }
}
