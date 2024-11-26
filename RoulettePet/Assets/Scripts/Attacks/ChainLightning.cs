using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ContinueLightning(true, 10, 0.15f, Time.));
    }


    IEnumerator ContinueLightning(bool lightningType, float baseDamage, float diminishAmount, float lightningID, float currentDiminish = 1, int hitsRemaining = 3)
    {
        yield return new WaitForSeconds(1);


        GameObject.FindGameObjectsWithTag("Enemy");


    }




}
