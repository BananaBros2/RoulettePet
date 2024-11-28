using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceCluball : MonoBehaviour
{
    public GameObject target;

    public Transform oriPos;
    private Transform parentObject;

    private Vector2 targeted;

    public Sprite normalSprite;
    public Sprite spinningSprite;

    public float hitRange = 0.22f;
    public float rotationSpeed = 20;


    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent;
        StartCoroutine(HitEnemies());
    }


    private void FixedUpdate()
    {
        if (transform.parent == null)
        {


            if (target == null)
            {
                targeted = oriPos.transform.position;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = normalSprite;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, oriPos.transform.rotation, rotationSpeed/2);
                transform.GetChild(0).transform.localRotation = Quaternion.RotateTowards(transform.GetChild(0).transform.localRotation, Quaternion.identity, rotationSpeed/4);
            }
            else
            {
                targeted = target.transform.position;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spinningSprite;
                transform.GetChild(0).transform.Rotate(0, 0, rotationSpeed);
            }


            

            float gravitation = 0.1f;

            transform.position = Vector2.MoveTowards(transform.position, targeted, gravitation);

            if (Vector2.Distance(transform.position, new Vector2(oriPos.position.x, oriPos.position.y)) < 0.01f)
            {
                transform.parent = parentObject;
                transform.localPosition = oriPos.transform.localPosition;
            }
        }

    }

    public IEnumerator HitEnemies()
    {
        while (true)
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= hitRange)
                {
                    parentObject.GetComponent<AttackScript>().HitTarget(target.GetComponent<EntityStatus>(), transform);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

    }
}
