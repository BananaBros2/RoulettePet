using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : MonoBehaviour
{
    public DebuffIconsScript debuffIconUpdator;
    public GameObject damageText;
    public float health = 100;

    public bool frostResistance = false;
    public int frostStack = 0;
    public float frostEffectiveness = 0;
    public float frostSlowPercentage = 1;
    public float frostTimeRemaining = 0;

    public bool poisonResistance = false;
    public int poisonStack = 0;
    public float poisonDamage = 0;
    public float poisonTimeRemaining = 0;


    public bool stunResistance = false;
    public bool stunned = false;
    public float stunTimeRemaining = 0;

    public bool conversionResistance = false;
    public bool converted = false;
    public float convertedTimeRemaining = 0;

    private void FixedUpdate()
    {
        frostTimeRemaining -= Time.fixedDeltaTime; ;
        if(frostTimeRemaining < 0 && frostStack > 0)
        {
            frostStack = 0;
            frostEffectiveness = 0;
            frostSlowPercentage = 1;

            debuffIconUpdator.UpdateFrostIcon(frostStack);
        }

        stunTimeRemaining -= Time.fixedDeltaTime; ;
        if (stunTimeRemaining < 0 && stunned)
        {
            stunned = false;

            debuffIconUpdator.UpdateStunIcon(stunned);
        }

        poisonTimeRemaining -= Time.fixedDeltaTime; ;
        if (poisonTimeRemaining < 0 && poisonStack > 0)
        {
            poisonStack = 0;
            poisonDamage = 0;

            StopCoroutine(PoisonDebuff());

            debuffIconUpdator.UpdatePoisonIcon(poisonStack);
        }

        convertedTimeRemaining -= Time.fixedDeltaTime;
        if (convertedTimeRemaining < 0 && converted)
        {
            converted = false;
            transform.tag = "Enemy";

            debuffIconUpdator.UpdateConvertedIcon(converted);
        }

    }


    public void TakeDamage(float damage, Color textColour)
    {
        health -= damage;

        DamageText newDamageText = Instantiate(damageText, transform.position + new Vector3(0.5f,0,0), Quaternion.identity).GetComponent<DamageText>();
        newDamageText.UpdateDamageText(-damage, textColour);

        if (health <= 0) { KillEntity(); }
    }

    public void KillEntity()
    {
        Destroy(this.gameObject);
    }
    public void Knockback(Transform origin, float power)
    {
        try
        {
            Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
            rb.AddForce((transform.position - origin.position) * power, ForceMode2D.Impulse);
        }
        catch { }
    }

    public void UpdateFrost(float duration, float frostSlowness)
    {
        frostStack = Math.Min(5, frostStack+1);
        frostEffectiveness = frostSlowness;
        frostSlowPercentage = 1 - frostEffectiveness * frostStack;

        frostTimeRemaining = Math.Max(duration, frostTimeRemaining);

        debuffIconUpdator.UpdateFrostIcon(frostStack);
    }

    public void UpdatePoison(float duration, float damage)
    {
        poisonStack = Math.Min(5, poisonStack+1);
        if(poisonStack >= 5) { StartCoroutine(PoisonDebuff()); }

        poisonDamage = Math.Max(poisonDamage, damage);
        poisonTimeRemaining = Math.Max(duration, poisonTimeRemaining);

        debuffIconUpdator.UpdatePoisonIcon(poisonStack);
    }
    
    IEnumerator PoisonDebuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            TakeDamage(poisonDamage, Color.green);
        }
    }

    public void UpdateStun(float duration)
    {
        stunned = true;
        stunTimeRemaining = Math.Max(duration, stunTimeRemaining);

        debuffIconUpdator.UpdateStunIcon(stunned);
    }

    public void UpdateConversion(float duration)
    {
        converted = true;
        convertedTimeRemaining = Math.Max(duration, convertedTimeRemaining);

        transform.tag = "Frenemy";

        debuffIconUpdator.UpdateConvertedIcon(converted);
    }


}
