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


    void Start()
    {
        //StartCoroutine(ContinueLightning(true, 10, 0.15f, Time.));
    }

    public void StartLightning(bool exponential, float baseDamage, float diminishAmount, List<GameObject> blacklistedObjects, float currentDiminish = 0, int hitsRemaining = 3)
    {
        StartCoroutine(ContinueLightning(exponential, baseDamage, diminishAmount, blacklistedObjects, 0, hitsRemaining = 3));
    }

    IEnumerator ContinueLightning(bool exponential, float baseDamage, float diminishAmount, List<GameObject> blacklistedObjects, float currentDiminish = 0, int hitsRemaining = 3)
    {
        yield return new WaitForSeconds(0.3f);



        if (diminishAmount * currentDiminish < 1)
        {
            List<GameObject> enemiesToZap = new List<GameObject>();


            if (exponential) { enemiesToZap = LocateNewTarget("Enemy", hitsRemaining); }
            else { enemiesToZap = LocateNewTarget("Enemy", 1); }


            for (int i = 0; i < (exponential ? hitsRemaining : 1); i++) // Add new Items that have been tagged to be zapped
            {
                if(i == enemiesToZap.Count) { break; }

                print(blacklistedObjects[i]);
                blacklistedObjects.Add(enemiesToZap[i]);
            }
            for (int i = 0; i < hitsRemaining; i++) // Add chainlightning script to new objects and transfer relevant data
            {
                if (i == enemiesToZap.Count) { break; }

                ChainLightning newZap = enemiesToZap[i].AddComponent<ChainLightning>();
                newZap.ContinueLightning(exponential, baseDamage, currentDiminish, blacklistedObjects, currentDiminish + 1, exponential ? hitsRemaining : hitsRemaining - 1);
            }
        }

        TransferDebuffs(transform.GetComponent<EntityStatus>());
        transform.GetComponent<EntityStatus>().TakeDamage(baseDamage * (1 - (diminishAmount * currentDiminish)), Color.yellow);

        yield return new WaitForSeconds(1f);
        Destroy(this);
    }


    public List<GameObject> LocateNewTarget(string searchTag, int requiredTargets, bool convertedObjects = false) // Mmm this function
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
                    if (dist > suitableTargetDistance[i-1]) // If distance is larger than the current object selected
                    {

                        for (int j = suitableTargets.Count - 1; j > i; j--)   // Shift values on lists to the right to make room for the new better target
                        {
                            suitableTargets[j - 1] = suitableTargets[j];
                            suitableTargetDistance[j - 1] = suitableTargetDistance[j];
                        }

                        suitableTargets[i] = selectedObject; // Add new Object to list
                        suitableTargetDistance[i] = dist;
                        break;
                    }

                }


            }
        }

        if (!convertedObjects)
        {
            suitableTargets.AddRange(LocateNewTarget("Frenemy", requiredTargets - suitableTargets.Count, true));
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
}
