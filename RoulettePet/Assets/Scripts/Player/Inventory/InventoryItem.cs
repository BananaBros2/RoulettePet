using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
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

            if (currentItem.itemName == "NULL") { return; }

            transform.localPosition = originalPosition * 1.05f;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = currentItem.itemSprite;
            transform.localPosition = originalPosition;
        }
        

    }
}
