using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffIconsScript : MonoBehaviour
{
    private Transform stunIcon;
    public Sprite stunSprite;
    public Sprite stunResistSprite;

    private Transform poisonIcon;
    public Sprite[] poisonSprites;
    public Sprite poisonResistSprite;

    private Transform frostIcon;
    public Sprite[] frostSprites;
    public Sprite frostResistSprite;

    private Transform convertedIcon;
    public Sprite convertSprite;
    public Sprite convertResistSprite;

    // Start is called before the first frame update
    void Start()
    {
        stunIcon = transform.GetChild(0);
        poisonIcon = transform.GetChild(1);
        frostIcon = transform.GetChild(2);
        convertedIcon = transform.GetChild(3);

        AdjustPositions();
    }

    public void UpdateStunIcon(bool stunned)
    {
        stunIcon.GetComponent<SpriteRenderer>().enabled = stunned;

        AdjustPositions();
    }

    public void UpdatePoisonIcon(int poisonedLevel)
    {
        SpriteRenderer poisonRenderer = poisonIcon.GetComponent<SpriteRenderer>();
        if (poisonedLevel <= 0) 
        {
            poisonRenderer.enabled = false;

            AdjustPositions();
            return;
        }
        poisonRenderer.enabled = true;

        poisonRenderer.sprite = poisonSprites[poisonedLevel-1];

        AdjustPositions();
    }

    public void UpdateFrostIcon(int frostLevel)
    {
        SpriteRenderer frostRenderer = frostIcon.GetComponent<SpriteRenderer>();
        if (frostLevel <= 0)
        {
            frostRenderer.enabled = false;

            AdjustPositions();
            return;
        }
        frostRenderer.enabled = true;

        frostRenderer.sprite = frostSprites[frostLevel-1];

        AdjustPositions();
    }

    public void UpdateConvertedIcon(bool converted)
    {
        convertedIcon.GetComponent<SpriteRenderer>().enabled = converted;

        AdjustPositions();
    }


    public void AdjustPositions()
    {
        List<Transform> activeDebuffs = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<SpriteRenderer>().enabled)
            {
                activeDebuffs.Add(transform.GetChild(i));
            }
        }

        for (int i = 0; i < activeDebuffs.Count; i++)
        {
            activeDebuffs[i].localPosition = new Vector2((0.1875f * i) - (0.5f * (activeDebuffs.Count - 1) * 0.1875f), 0);
        }


    }
}
