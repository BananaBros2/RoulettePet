using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public ScriptableItem currentItem;
    public Vector2 originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectItem(bool selected)
    {
        if (selected) 
        { 
            transform.GetComponent<SpriteRenderer>().sprite = currentItem.selectedSprite;
            transform.GetComponent<SpriteRenderer>().sortingOrder = 2;

            if (currentItem.itemName == "NULL") { return; }

            transform.localPosition = originalPosition * 1.05f;
            //transform.localScale = Vector3.one * 2;

        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = currentItem.itemSprite;
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            transform.localPosition = originalPosition;
            //transform.localScale = Vector3.one;
        }
        

    }
}
