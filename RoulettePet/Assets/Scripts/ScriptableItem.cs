using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableItem : ScriptableObject
{
    public ScriptableGun scriptableWeapon;

    public Sprite itemSprite;
    public Sprite selectedSprite;

    public string itemName = "NULL";
    public string description = "NULL";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
