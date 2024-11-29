using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ChainLightning : MonoBehaviour
{
    // Start is called before the first frame update
    public float zapRange = 2f;

    public float frostDuration = 0;
    public float frostSlowdown = 0;
    public float stunDuration = 0;
    public float poisonDuration = 0;
    public float poisonDamage = 0;
    public float conversionDuration = 0;



    public void StartLightning(bool exponential, float baseDamage, float diminishAmount, List<GameObject> blacklistedObjects, int currentDiminish, int hitsRemaining = 3)
    {
        StartCoroutine(ContinueLightning(exponential, baseDamage, diminishAmount, blacklistedObjects, currentDiminish, hitsRemaining = 3));
    }

    IEnumerator ContinueLightning(bool exponential, float baseDamage, float diminishAmount, List<GameObject> blacklistedObjects, int currentDiminish, int hitsRemaining = 3)
    {
        yield return new WaitForSeconds(0.2f);


        if (diminishAmount * currentDiminish < 1)
        {
            List<GameObject> enemiesToZap = new List<GameObject>();

            if (exponential) { enemiesToZap = LocateNewTarget("Enemy", hitsRemaining, blacklistedObjects); }
            else { enemiesToZap = LocateNewTarget("Enemy", 1, blacklistedObjects); }

            //foreach (GameObject zap in enemiesToZap)
            //{
            //    print(zap.name);

            //    //ChainLightning newZap = CreateLightning(zap);
            //    //newZap.StartLightning(exponential, baseDamage, diminishAmount, blacklistedObjects, currentDiminish + 1, exponential ? hitsRemaining : hitsRemaining - 1);
            //}

            for (int i = 0; i < hitsRemaining; i++) // Add chainlightning script to new objects and transfer relevant data
            {
                if (i == enemiesToZap.Count) { break; }

                blacklistedObjects.Add(enemiesToZap[i]);

                ChainLightning newZap = CreateLightning(enemiesToZap[i]);
                newZap.StartLightning(exponential, baseDamage, diminishAmount, blacklistedObjects, currentDiminish+1, exponential ? hitsRemaining : hitsRemaining - 1);
                print(diminishAmount + " " + currentDiminish + 1);
            }
        }
        TransferDebuffs(transform.GetComponent<EntityStatus>());
        transform.GetComponent<EntityStatus>().TakeDamage(baseDamage * (1 - (diminishAmount * currentDiminish)), Color.yellow);

        yield return new WaitForSeconds(0.3f);
        Destroy(this);
    }


    public List<GameObject> LocateNewTarget(string searchTag, int requiredTargets, List<GameObject> blacklistedObjects, bool convertedObjects = false) // Mmm this function
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(searchTag);
        List<GameObject> suitableTargets = new List<GameObject>();
        List<float> suitableTargetDistance = new List<float>();

        foreach (GameObject selectedObject in potentialTargets)
        {

            float dist = Vector3.Distance(transform.position, selectedObject.transform.position); // Get distance of object from this object
            if (dist < zapRange && selectedObject != transform.gameObject && !blacklistedObjects.Contains(selectedObject)) // Only effect objects within range and NOT SELF
            {

                for (int i = 0; i < requiredTargets; i++)
                {
                    if (suitableTargets.Count < requiredTargets) 
                    {
                        //print("gabavb  " + selectedObject);
                        suitableTargets.Add(selectedObject); // Add new Object to list
                        suitableTargetDistance.Add(dist);
                        break;
                    }

                    //if (dist < suitableTargetDistance[i]) // If distance is smaller than the current object selected
                    //{
                    //    for (int j = suitableTargets.Count - 1; j > i; j--)   // Shift values on lists to the right to make room for the new better target
                    //    {
                    //        suitableTargets[j - 1] = suitableTargets[j];
                    //        suitableTargetDistance[j - 1] = suitableTargetDistance[j];
                    //    }
                    //    //print(suitableTargets);

                    //    suitableTargets[i] = selectedObject; // Add new Object to list
                    //    suitableTargetDistance[i] = dist;
                    //    break;

                    //}

                }


            }
        }

        if (!convertedObjects)
        {
            suitableTargets.AddRange(LocateNewTarget("Frenemy", requiredTargets - suitableTargets.Count, blacklistedObjects, true));
        }

        return suitableTargets;

    }


    public void TransferDebuffs(EntityStatus target)
    {
        if (frostDuration != 0) { target.UpdateFrost(frostDuration, frostSlowdown); }

        if (stunDuration != 0) { target.UpdateStun(stunDuration); }

        if (poisonDuration != 0) { target.UpdatePoison(poisonDuration, poisonDamage); }

        if (conversionDuration != 0) { target.UpdateConversion(conversionDuration); }
    }

    public ChainLightning CreateLightning(GameObject target)
    {
        ChainLightning newLightning = target.AddComponent<ChainLightning>();

        newLightning.frostDuration = frostDuration;
        newLightning.frostSlowdown = frostSlowdown;
        newLightning.stunDuration = stunDuration;
        newLightning.poisonDuration = poisonDuration;
        newLightning.poisonDamage = poisonDamage;
        newLightning.conversionDuration = conversionDuration;

        return newLightning;
    }
}
