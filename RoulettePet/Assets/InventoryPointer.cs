using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPointer : MonoBehaviour
{
    // Update is called once per frame
    public void UpdatePosition(Vector2 targetPosition)
    {
        transform.localPosition = targetPosition * 0.75f;

        transform.up = targetPosition - new Vector2(transform.localPosition.x, transform.localPosition.y);
    }
}
