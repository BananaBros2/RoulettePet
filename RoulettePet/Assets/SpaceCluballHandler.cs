using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCluballHandler : MonoBehaviour
{
    public List<SpaceCluball> balls;

    public float targetRange = 2;


    void FixedUpdate()
    {
        foreach(SpaceCluball currentBall in balls)
        {
            if (currentBall.target == null)
            {
                currentBall.target = LocateNewTarget("Enemy");
                if(currentBall.target != null)
                {
                    currentBall.transform.parent = null;
                }


            }
            else if (Vector2.Distance(currentBall.target.transform.position, transform.position) > targetRange + 0.3f)
            {
                currentBall.target = null;

            }
        }

    }

    public GameObject LocateNewTarget(string searchTag, bool failed = false)
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(searchTag);

        float oldDistance = targetRange;
        GameObject closestObject = null;

        foreach (GameObject selectedObject in potentialTargets)
        {

            bool skipItem = false;
            foreach (SpaceCluball currentBall in balls)
            {
                if (selectedObject == currentBall.target) { skipItem = true; }
            }
            if(skipItem) { continue; }


            float dist = Vector3.Distance(transform.position, selectedObject.transform.position);

            if (dist < oldDistance && selectedObject != transform.gameObject)
            {
                closestObject = selectedObject;
                oldDistance = dist;
            }
        }

        if (closestObject != null || failed)
        {
            return closestObject;
        }
        else if (closestObject == null)
        {
            return LocateNewTarget("Frenemy", true); // Slightly dangerous but shouldn't loop twice
        }

        return null;

    }

}
