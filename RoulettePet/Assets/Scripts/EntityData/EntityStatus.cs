using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityStatus : MonoBehaviour
{
    private bool playerEntity;
    public DebuffIconsScript debuffIconUpdator;
    public GameObject damageText;
    public Image healthBarGreen;
    public Image healthBarWhite;
    public float maxHealth = 50;
    private float health = 50;
    private bool aboutToDie;
    public float enemyDamage = 15;

    public bool isDead = false;
    public float invincibilityFrames = 1;
    private float curInvincibilityFrames = 0;



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


    private void Start()
    {
        health = maxHealth;

        if(transform.TryGetComponent<Movement>(out Movement isPlayer))
        {
            playerEntity = true;
        }
    }

    private void FixedUpdate()
    {
        curInvincibilityFrames -= 1;

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerEntity && collision.transform.CompareTag("Enemy"))
        {
            TakeDamage(collision.transform.GetComponent<EntityStatus>().enemyDamage, Color.red);
        }
    }

    public void TakeDamage(float damage, Color textColour)
    {
        if(curInvincibilityFrames > 0) { return; }
        curInvincibilityFrames = invincibilityFrames;

        health -= damage;

        DamageText newDamageText = Instantiate(damageText, transform.position + new Vector3(0.5f,0,0), Quaternion.identity).GetComponent<DamageText>();
        newDamageText.UpdateDamageText(-damage, textColour);

        if (healthBarGreen != null)
        {
            healthBarGreen.fillAmount = health / maxHealth;
            if(healthBarWhite != null && !aboutToDie)
            {
                StopCoroutine(HealthbarDrain());
                StartCoroutine(HealthbarDrain());

                if (playerEntity) { return; }
            }
        }

        if (health <= 0) { KillEntity(); }
    }

    IEnumerator HealthbarDrain()
    {
        if(healthBarGreen.fillAmount <= 0)
        {
            aboutToDie = true;
            StartCoroutine(FlashTemporaryHealth());
        }

        yield return new WaitForSeconds(0.8f);

        while (healthBarWhite.fillAmount > healthBarGreen.fillAmount)
        {
            float drainMult = Mathf.Clamp(healthBarWhite.fillAmount*2 + 0.02f, 0.05f, 1);
            healthBarWhite.fillAmount = Mathf.Max(healthBarWhite.fillAmount - 0.01f * drainMult, healthBarGreen.fillAmount);
            yield return new WaitForSeconds(0.02f);
        }

        if (playerEntity && healthBarGreen.fillAmount <= 0) { KillEntity(); }

    }

    IEnumerator FlashTemporaryHealth()
    {
        while (health <= 0)
        {
            healthBarWhite.color = Color.yellow;
            yield return new WaitForSeconds(0.2f);

            healthBarWhite.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
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
