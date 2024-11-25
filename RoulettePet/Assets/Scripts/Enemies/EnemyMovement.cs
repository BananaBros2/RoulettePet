using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    private EntityStatus status;

    public float movementSpeed = 1;

    public GameObject target = null;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<EntityStatus>();
        player = GameObject.FindWithTag("Player");
    }

    
    void FixedUpdate()
    {
        if (status.stunned) { return; }


        if (status.converted && (target == null || target == player))
        {
            target = LocateNewTarget();
        }
        else if (!status.converted)
        {
            try
            {
                target = player;
                if (player.GetComponent<EntityStatus>().isDead){ target = null; return; }
                
            }
            catch
            {
                return;
            }
            
        }

        float speed = movementSpeed * status.frostSlowPercentage;

        try
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed);
        }
        catch
        {
            // Object has been destroyed or is not available
        }

    }

    public GameObject LocateNewTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");
        if (potentialTargets.Length == 0) { potentialTargets = GameObject.FindGameObjectsWithTag("Frenemy"); }

        float oldDistance = 100f;
        GameObject closestObject = null;

        foreach (GameObject selectedObject in potentialTargets)
        {
            float dist = Vector3.Distance(this.transform.position, selectedObject.transform.position);
            if (dist < oldDistance && selectedObject != transform.gameObject)
            {
                closestObject = selectedObject;
                oldDistance = dist;
            }
        }

        if (closestObject == null) { closestObject = player; }

        return closestObject;
    }



}
