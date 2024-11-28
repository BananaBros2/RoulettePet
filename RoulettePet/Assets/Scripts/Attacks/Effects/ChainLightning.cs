using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    // Start is called before the first frame update
    public float zapRange = 2f;

    void Start()
    {
        //StartCoroutine(ContinueLightning(true, 10, 0.15f, Time.));
    }


    IEnumerator ContinueLightning(bool exponential, float baseDamage, float diminishAmount, float lightningID, float currentDiminish = 1, int totalHits = 3, int hitsRemaining = 3)
    {
        yield return new WaitForSeconds(0.3f);

        GameObject[] enemiesToZap = new GameObject[hitsRemaining];
        

        if (exponential) { enemiesToZap = LocateNewTarget("Enemy", hitsRemaining); }
        else { enemiesToZap = LocateNewTarget("Enemy", 1); }




    }


    public GameObject[] LocateNewTarget(string searchTag, int targetsRemaining, bool failed = false) // Mmm this function
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(searchTag);
        List<GameObject> suitableTargets = new List<GameObject>();
        List<float> suitableTargetDistance = new List<float>();

        foreach (GameObject selectedObject in potentialTargets)
        {
            float dist = Vector3.Distance(transform.position, selectedObject.transform.position);

            if (dist < zapRange && selectedObject != transform.gameObject)
            {

                for (int i = 0; i < targetsRemaining; i++)
                {

                    if (dist > suitableTargetDistance[i])
                    {

                        for (int j = suitableTargets.Count-1; j > i; j--)
                        {

                            suitableTargets[j-1] = suitableTargets[j];
                            suitableTargetDistance[j-1] = suitableTargetDistance[j];

                        }

                        suitableTargets[i] = selectedObject;
                        suitableTargetDistance[i] = dist;
                        break;
                    }

                }




            }
        }

        if (closestObject != null || failed)
        {
            return closestObject;
        }
        else if (closestObject == null)
        {
            return LocateNewTarget("Frenemy", targetsRemaining, true); // Slightly dangerous but shouldn't loop twice
        }

        return null;

    }



}
