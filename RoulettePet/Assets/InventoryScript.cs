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
    public float defaultScrollDelay = 0.1f;
    private float currentScrollDelay = 0.1f;

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
            currentScrollDelay = defaultScrollDelay;
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
            timeSinceLastMove = -0.1f;
            currentScrollDelay = defaultScrollDelay;
        }
        else if (Input.GetButton("Horizontal"))
        {
            timeSinceLastMove += Time.deltaTime;

            if (timeSinceLastMove > currentScrollDelay)
            {
                ChangeSelection();
                timeSinceLastMove = 0;
                currentScrollDelay = Mathf.Max(currentScrollDelay - 0.03f, 0.02f);
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
