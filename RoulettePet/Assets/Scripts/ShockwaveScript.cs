using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShockwaveScript : MonoBehaviour
{
    private SpriteRenderer waveRenderer;

    public Sprite[] waveSprites;
    public LayerMask objectsToHit;

    public int waveRadius = 3;
    public float waveDamage = 1;

    public float frostDuration = 0;
    public float frostSlowdown = 0;
    public float stunDuration = 0;
    public float poisonDuration = 0;
    public float poisonDamage = 0;
    public float conversionDuration = 0;

    // Start is called before the first frame update
    void Start()
    {
        waveRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(ShockWave());
    }

    IEnumerator ShockWave()
    {
        List<GameObject> hitObjects = new List<GameObject>();

        foreach (Sprite sprite in waveSprites)
        {
            waveRenderer.sprite = sprite;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, sprite.textureRect.width / sprite.pixelsPerUnit / 2, Vector2.zero, 0, objectsToHit);
            foreach(RaycastHit2D hit in hits)
            {
                if (hitObjects.Contains(hit.transform.gameObject)) { continue; }
                hitObjects.Add(hit.transform.gameObject);

                if (hit.transform.TryGetComponent<EntityStatus>(out EntityStatus status))
                {
                    status.TakeDamage(waveDamage);
                    TransferDebuffs(status);
                }
            }

            yield return new WaitForSeconds(0.05f - (waveRadius * 0.005f));

            waveRadius--;
            if (waveRadius == 0) { yield return new WaitForSeconds(0.05f); break; }
        }

        DissipateWave();
    }


    public void TransferDebuffs(EntityStatus target)
    {
        if (frostDuration != 0) { target.UpdateFrost(frostDuration, frostSlowdown); }

        if (stunDuration != 0) { target.UpdateStun(stunDuration); }

        if (poisonDuration != 0) { target.UpdatePoison(poisonDuration, poisonDamage); }

        if (conversionDuration != 0) { target.UpdateConversion(conversionDuration); }
    }


    public void DissipateWave()
    {
        Destroy(this.gameObject);
    }
}
