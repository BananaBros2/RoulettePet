using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackScript : MonoBehaviour
{
    public AttackType attackType = AttackType.Projectile;

    private Rigidbody2D rb;
    public Sprite[] randomSprite;

    public float speed = 0;
    public float damage = 1;

    public float explosiveArea = 0;
    public bool createShockwave = false;
    public int shockwavesSize = 0;
    public float knockbackPower = 0;
    public int ricochets = 0;
    public LayerMask ricochetLayers;
    
    public bool exponentialLightning;
    public float lightningDiminish = 0.15f;
    public int lightningMaxChain = 0;

    public bool gore = false;

    public float wavyness = 0;
    private float wavePosition = 90f;

    public float frostDuration = 0;
    public float frostSlowdown = 0;
    public float stunDuration = 0;
    public float poisonDuration = 0;
    public float poisonDamage = 0;
    public float conversionDuration = 0;

    public Vector2 directionalVelocity;

    public GameObject shockwaveObject;

    private float triggerEnterDelay;

    private List<Collider2D> triggeredObjectsList = new List<Collider2D>();

    public enum AttackType
    {
        Projectile,
        ConstantArea,
        InstantArea
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(randomSprite.Length > 0)
        {
            System.Random rand = new System.Random();
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = randomSprite[rand.Next(0,randomSprite.Length-1)];
        }

        if (attackType == AttackType.InstantArea)
        {
            Invoke("HitTriggerTargets", 0.1f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.right * speed;

        if (wavyness != 0)
        {
            wavePosition += speed/30;
            rb.velocity = rb.velocity + new Vector2(transform.up.x * Mathf.Sin(wavePosition) * speed * wavyness, transform.up.y * Mathf.Sin(wavePosition) * speed * wavyness);
        }

        triggerEnterDelay -= Time.fixedDeltaTime;
    }


    public void SetBasicProperties(float newSpeed, float newDamage)
    {
        speed = newSpeed;
        damage = newDamage;
    }
    public void SetSpecialProperties(bool newShockwaves, int newShockwavesSize,
                                     float newKnockbackPower, int newRicochetCount,
                                     bool newExponentiLightning, float newDiminishLightning, int newMaxChain,
                                     bool addGore, float newWavyness)
    {
        createShockwave = newShockwaves;
        shockwavesSize = newShockwavesSize;

        knockbackPower = newKnockbackPower;
        ricochets = newRicochetCount;

        exponentialLightning = newExponentiLightning;
        lightningDiminish = newDiminishLightning;
        lightningMaxChain = newMaxChain;

        gore = addGore;

        wavyness = newWavyness;
    }
    public void SetDebuffProperties(float newFrostDuration, float newFrostSlowdown,
                                    float newStunDuration, float newConversionDuration,
                                    float newPoisonDuration, float newPoisonDamage)
    {
        frostDuration = newFrostDuration;
        frostSlowdown = newFrostSlowdown;
        stunDuration = newStunDuration;
        poisonDuration = newPoisonDuration;
        poisonDamage = newPoisonDamage;
        conversionDuration = newConversionDuration;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackType == AttackType.Projectile)
        {
            try
            {
                EntityStatus target = collision.GetComponent<EntityStatus>();

                if (triggerEnterDelay < 0) { HitTarget(target); }

                if (ricochets > 0 && triggerEnterDelay < 0) { RicochetBounce(collision); }
                else if (ricochets < 1) { Destroy(this.gameObject); }

                triggerEnterDelay = 0.05f;
            }
            catch
            {
                if (collision.CompareTag("Wall"))
                {
                    if (createShockwave) { CreateShockwave(transform.position); }

                    if (ricochets > 0) { RicochetBounce(collision); }
                    else { Destroy(this.gameObject); }

                    triggerEnterDelay = 0.01f;
                }

            }
        }
        else if (attackType == AttackType.InstantArea )
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Frenemy"))
            {
                triggeredObjectsList.Add(collision);
            }
                
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (attackType == AttackType.InstantArea)
        {
            if (triggeredObjectsList.Contains(collision))
            {
                triggeredObjectsList.Remove(collision);
            }

        }
    }

    public void RicochetBounce(Collider2D collision)
    {
        ricochets--;

        Ray2D ray = new Ray2D(transform.position, transform.right);
        ContactFilter2D layerFilter = new();
        layerFilter.SetLayerMask(ricochetLayers);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, transform.right, Time.fixedDeltaTime * speed, ricochetLayers);
        if(hit)
        {

            Vector2 reflectDir = Vector2.Reflect(transform.right, hit.normal);

            float rot = 90 - Mathf.Atan2(reflectDir.x, reflectDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, rot);

        }

    }

    public void HitTriggerTargets()
    {

        for (int i = 0; i < triggeredObjectsList.Count; i++)
        {
            if(triggeredObjectsList[i] == null) { continue; }

            HitTarget(triggeredObjectsList[i].GetComponent<EntityStatus>());
        }

        Destroy(this.gameObject);
    }

    public void HitTarget(EntityStatus target)
    {
        target.TakeDamage(damage, Color.white);
        TransferDebuffs(target);

        if (knockbackPower != 0) { target.Knockback(transform, knockbackPower); }

        if (createShockwave)
        {
            CreateShockwave(target.transform.position);
        }


    }

    public void TransferDebuffs(EntityStatus target)
    {
        if (frostDuration != 0) { target.UpdateFrost(frostDuration, frostSlowdown); }
        
        if (stunDuration != 0) { target.UpdateStun(stunDuration); }

        if (poisonDuration != 0) { target.UpdatePoison(poisonDuration, poisonDamage); }

        if (conversionDuration != 0) { target.UpdateConversion(conversionDuration); }
    }

    public void CreateShockwave(Vector3 creationPosition)
    {
        ShockwaveScript newShockwave = Instantiate(shockwaveObject, creationPosition, Quaternion.Euler(0,0,0)).GetComponent<ShockwaveScript>();

        newShockwave.waveDamage = damage;
        newShockwave.waveRadius = shockwavesSize;

        newShockwave.frostDuration = frostDuration;
        newShockwave.frostSlowdown = frostSlowdown;
        newShockwave.stunDuration = stunDuration;
        newShockwave.poisonDuration = poisonDuration;
        newShockwave.poisonDamage = poisonDamage;
        newShockwave.conversionDuration = conversionDuration;



    }
}
