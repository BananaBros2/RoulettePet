using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceCluball : MonoBehaviour
{
    public GameObject target;

    public Transform oriPos;
    private Transform parentObject;

    public bool ballReturning;

    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent;
    }


    private void FixedUpdate()
    {
        if (transform.parent == null)
        {
            Vector2 targeted = (target == null) ? oriPos.transform.position : target.transform.position;

            float gravitation = 0.1f;

            transform.position = Vector2.MoveTowards(transform.position, targeted, gravitation);

            if (Vector2.Distance(transform.position, new Vector2(oriPos.position.x, oriPos.position.y)) < 0.01f)
            {
                transform.parent = parentObject;
                transform.localPosition = oriPos.transform.localPosition;
            }
        }

    }

}
