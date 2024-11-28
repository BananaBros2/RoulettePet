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


    IEnumerator ContinueLightning(bool exponential, float baseDamage, float diminishAmount, float lightningID, float currentDiminish = 1, int totalHits = 1, int hitsRemaining = 3)
    {
        yield return new WaitForSeconds(0.3f);

        List<GameObject> enemiesToZap = new List<GameObject>();


        if (exponential) { enemiesToZap = LocateNewTarget("Enemy", hitsRemaining); }
        else { enemiesToZap = LocateNewTarget("Enemy", 1); }




    }


    public List<GameObject> LocateNewTarget(string searchTag, int requiredTargets, bool failed = false) // Mmm this function
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(searchTag);
        List<GameObject> suitableTargets = new List<GameObject>();
        List<float> suitableTargetDistance = new List<float>();

        foreach (GameObject selectedObject in potentialTargets)
        {

            float dist = Vector3.Distance(transform.position, selectedObject.transform.position); // Get distance of object from this object
            if (dist < zapRange && selectedObject != transform.gameObject) // Only effect objects within range and NOT SELF
            {
                for (int i = 0; i < potentialTargets.Length; i++)
                {
                    if (dist > suitableTargetDistance[i]) // If distance is larger than the current object selected
                    {

                        for (int j = suitableTargets.Count - 1; j > i; j--)   // Shift values on lists to the right to make room for the new better target
                        {
                            suitableTargets[j - 1] = suitableTargets[j];
                            suitableTargetDistance[j - 1] = suitableTargetDistance[j];
                        }

                        suitableTargets[i] = selectedObject;
                        suitableTargetDistance[i] = dist;
                        break;
                    }
                }


            }
        }

        if (suitableTargets.Count < requiredTargets || failed)
        {
            return suitableTargets;
        }
        else
        {
            return LocateNewTarget("Frenemy", requiredTargets, true); // Slightly dangerous but shouldn't loop twice
        }

        return null;

    }



}
