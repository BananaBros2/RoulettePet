using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    private Transform slotsHolder;
    public GunScript gunScript;
    public TMP_Text itemNameTMP;
    public GameObject pointer;

    public bool inventoryOpen;
    public int itemSelectingIndex = 0;
    private InventoryItem selectedItem;

    private float timeSinceLastMove = 0;
    public float scrollDelay = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        slotsHolder = transform.GetChild(0);
        slotsHolder.GetChild(itemSelectingIndex).transform.GetComponent<InventoryItem>().SelectItem(true);

        selectedItem = slotsHolder.GetChild(itemSelectingIndex).transform.GetComponent<InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("OpenInventory"))
        {

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            transform.GetComponent<SpriteRenderer>().enabled = true;

            inventoryOpen = true;
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }


            transform.GetComponent<SpriteRenderer>().enabled = false;

            inventoryOpen = false;
        }

        if (!inventoryOpen) { return; }

        if (Input.GetButtonDown("Enter") && selectedItem.currentItem.itemName != "NULL")
        {
            pointer.transform.GetComponent<InventoryPointer>().UpdatePosition(selectedItem.originalPosition);
            gunScript.gunClass = selectedItem.currentItem.scriptableWeapon;


            gunScript.ChangeWeapon(selectedItem.currentItem.scriptableWeapon);
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            ChangeSelection();
            timeSinceLastMove = -1;
        }
        else if (Input.GetButton("Horizontal"))
        {
            timeSinceLastMove += Time.fixedDeltaTime;

            if (timeSinceLastMove > scrollDelay)
            {
                ChangeSelection();
                timeSinceLastMove = 0;
            }
        }
        else
        {
            timeSinceLastMove = 0;
        }


    }




    public void ChangeSelection()
    {
        slotsHolder.GetChild(itemSelectingIndex).transform.GetComponent<InventoryItem>().SelectItem(false);

        itemSelectingIndex += Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        if (itemSelectingIndex > slotsHolder.childCount - 1) { itemSelectingIndex = 0; }
        else if (itemSelectingIndex < 0) { itemSelectingIndex = slotsHolder.childCount - 1; }

        selectedItem = slotsHolder.GetChild(itemSelectingIndex).transform.GetComponent<InventoryItem>();
        selectedItem.SelectItem(true);
        if (selectedItem.currentItem.itemName == "NULL")
        {
            itemNameTMP.text = " ";
            return;
        }

        itemNameTMP.text = selectedItem.currentItem.itemName;
    }

}
