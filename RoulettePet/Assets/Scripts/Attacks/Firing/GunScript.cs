using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

using static ScriptableGun;

public class GunScript : MonoBehaviour
{
    public GameObject gunObject;
    public ScriptableGun gunClass;
    public TMP_Text ammoCounterText;
    public GameObject reloadingText;

    //private int flip = 1;

    public float timeSinceLastShot = 0;
    public int ammoLeft = 0;
    public int ammoLeftInMag = 0;
    public bool reloading = false;

    System.Random randomizer;

    private float currentHeldValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        ammoLeft = gunClass.ammoMax;
        ammoLeftInMag = gunClass.magMax;
        UpdateAmmoCounter();

        randomizer = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        // Make gun turn to face mouse
        gunObject.transform.right = TurnToMouse(gunObject.transform);
        //if (gunObject.transform.eulerAngles.z < 90 || gunObject.transform.eulerAngles.z > 270)
        //{
        //    gunObject.transform.localScale = new Vector3(1, 1, gunObject.transform.localScale.z);
        //    flip = 1;
        //}
        //else
        //{
        //    gunObject.transform.localScale = new Vector3(-1, gunObject.transform.localScale.y, gunObject.transform.localScale.z);
        //    gunObject.transform.right *= -1;
        //    flip = -1;
        //}


        timeSinceLastShot += Time.deltaTime;

        if (Input.GetButtonDown("Reload"))
        {
            StartReload(gunClass);
        }

        if (gunClass.activationType == ScriptableGun.ActivationType.Projectile && Input.GetButtonDown("Fire1") && !reloading)
        {
            if (!ActivationCheck(gunClass)) { return; }


            if (gunClass.multishot)
            {

                gunObject.GetComponent<GunAudio>().PlayShootSound();
                timeSinceLastShot = 0;
                MultishotMath(gunClass);
            }
            else
            {
                gunObject.GetComponent<GunAudio>().PlayShootSound();
                timeSinceLastShot = 0;
                ShootProjectile(gunClass);
            }
            

        }



    }


    private void FixedUpdate()
    {


        if (gunClass.activationType == ScriptableGun.ActivationType.Held && Input.GetButton("Fire1") && !reloading)
        {
            if (!ActivationCheck(gunClass)) { return; }


            if(gunClass.multishot)
            {
                gunObject.GetComponent<GunAudio>().PlayShootSound();
                timeSinceLastShot = 0;
                MultishotMath(gunClass);
            }
            else
            {
                gunObject.GetComponent<GunAudio>().PlayShootSound();
                timeSinceLastShot = 0;
                ShootProjectile(gunClass);
            }

        }


        if (gunClass.activationType == ScriptableGun.ActivationType.HeldDistance && Input.GetButton("Fire1") && ActivationCheck(gunClass))
        {
            if(currentHeldValue == 0)
            {
                currentHeldValue = 0.2f;
            }
            currentHeldValue = Mathf.Clamp(currentHeldValue + 0.02f, 0.1f, 1);
        }
        else if (currentHeldValue > 0 && ActivationCheck(gunClass))
        {
            if (gunClass.multishot)
            {
                timeSinceLastShot = 0;
                MultishotMath(gunClass, currentHeldValue);
            }
            else
            {
                timeSinceLastShot = 0;
                ShootProjectile(gunClass, 0, true, currentHeldValue);
            }
            currentHeldValue = 0;
        }
        else
        {
            currentHeldValue = 0;
        }


    }





    public Vector3 TurnToMouse(Transform tran)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        Vector3 lookAt = mouseWorldPosition - tran.position;
        return lookAt;
    }


    public bool ActivationCheck(ScriptableGun gun)
    {
        // Instantly fail if reloading
        if (reloading)
        {
            return false;
        }

        // Check if enough time has passed since last shot
        if (timeSinceLastShot <= gun.timeBetweenShots)
        {
            // Play unsuccessful click
            return false;
        }

        // Check if has ammo in mag, if not attempt to reload
        if (ammoLeftInMag <= 0)
        {
            print("Needs to reload");
            // Attempt to relod gun
            StartReload(gun);
            return false;
            // Play unsuccesful click
        }

        // Check if enemy is within range of being hit
        //if (gun.shootType == ScriptableGun.ShootType.ClosestSingle)
        //{
        //    Vector2 enemyPosition = new Vector3(10,0);

        //    if (Vector2.Distance(enemyPosition, transform.position) > 10)
        //    {
        //        //Enemy is out of range
        //        return false;
        //    }
        //}

        return true;
    }


    public void MultishotMath(ScriptableGun gun, float heldPower = -1)
    {
        int attackCount = gun.split; // Control how many bullets are shot
        if (gun.consumeExtraAmmo)
        {
            attackCount = math.min(gun.split, ammoLeftInMag); // Reduce to bullets left in mag
        }

        bool consumeAmmo = true;
        for (int b = 0; b < attackCount; b++) // Calculate and shoot each bullet
        {
            float shootAngle = (gun.angleDifference * b) - (0.5f * (attackCount - 1) * gun.angleDifference); // Yummy rotation math
            ShootProjectile(gun, shootAngle, consumeAmmo, heldPower);

            if (!gun.consumeExtraAmmo) { consumeAmmo = false; } // Only consume ammo on first shot
        }
    }

    public void ShootProjectile(ScriptableGun gun, float rotationOffset = 0, bool consumeAmmo = true, float heldPower = -1)
    {
        GameObject newBullet = Instantiate(gun.attackObject, gunObject.transform.GetChild(0).transform.position, gunObject.transform.rotation);

        float randomOffset = (float)randomizer.Next(-gun.projectileSpread/2, gun.projectileSpread / 2);
        newBullet.transform.Rotate(0, 0, rotationOffset + randomOffset);

        if (!gun.infiniteMag && consumeAmmo) // Do not consume if has infinite mag ammo or if trigged not to
        {
            ammoLeftInMag--;
            UpdateAmmoCounter();
        }


        float attackSpeed = (gun.shootType == ShootType.Single) ? gun.projectileSpeed : 0;
        AttackScript bulletProperties = newBullet.GetComponent<AttackScript>();
        bulletProperties.SetBasicProperties(attackSpeed, gun.damage, heldPower);

        TransferSpecials(gun, bulletProperties);
        TransferDebuffs(gun, bulletProperties);

        if (gunObject.TryGetComponent<GunAnimationController>(out GunAnimationController gunAnimation))
        {
            gunAnimation.StartEffects(rotationOffset + randomOffset);
        }

        if (gun.shootType == ShootType.ConstantArea || gun.shootType == ShootType.BurstArea)
        {
            bulletProperties.attackType = AttackScript.AttackType.InstantArea;
            if (gun.newAreaPoints.Length < 3) { return; }


            PolygonCollider2D newCollider = newBullet.GetComponent<PolygonCollider2D>();
            newCollider.points = gun.newAreaPoints;

            if(heldPower == -1) { return; } // Held power has not been changed is default
            for(int i = 0; i < newCollider.points.Length; i++)
            {
                newCollider.points[i] *= heldPower;
            }
        }
    }

    public void TransferSpecials(ScriptableGun gun, AttackScript bulletProperties)
    {
        bulletProperties.SetSpecialProperties(
            gun.shockWaves, gun.shockWaveSize,
            gun.knockback ? gun.knockbackPower : 0,
            gun.ricochet ? gun.ricochetCount : 0,
            gun.exponentialLightning, gun.lightning ? gun.flatDiminishPercentage : 2, gun.maxChain,
            gun.goreyDeath,
            gun.wavyProjectile ? gun.wavyness : 0,
            gun.loseVelocity ? gun.slowdownRate : 1);

    }

    public void TransferDebuffs(ScriptableGun gun, AttackScript bulletProperties)
    {
        bulletProperties.SetDebuffProperties(
            gun.frost ? gun.frostDuration : 0, gun.frostSlowdown,
            gun.stun ? gun.stunDuration : 0,
            gun.enemyConversion ? gun.conversionDuration : 0,
            gun.poison ? gun.poisonDuration : 0, gun.poisonDamage);

    }


    public void StartReload(ScriptableGun gun)
    {
        if (reloading) { return; }
        reloading = true;

        print("Reload starting");
        StartCoroutine(ReloadingTime(gun));
    }

    IEnumerator ReloadingTime(ScriptableGun gun)
    {
        reloadingText.SetActive(true);
        yield return new WaitForSeconds(gun.reloadTime);

        if (gun.infiniteAmmo)
        {
            print("Gun has infinite ammo");
            ammoLeftInMag = gun.magMax;

            reloading = false;
            yield break;
        }
        else if (ammoLeft <= 0)
        {
            print("No ammo left to reload");
            // No ammo left to reload

            reloading = false;
            yield break;
        }

        print("Regular Reload");
        // Regular ammo refill, any remaining ammo is automatically passed over
        ammoLeft += ammoLeftInMag;
        ammoLeftInMag = Mathf.Min(gun.magMax, ammoLeft);
        ammoLeft -= ammoLeftInMag;

        UpdateAmmoCounter();
        reloadingText.SetActive(false);

        reloading = false;
        yield break;
    }

    public void UpdateAmmoCounter()
    {
        ammoCounterText.text = (ammoLeftInMag.ToString() + "/" + ammoLeft.ToString());
    }




    public void ChangeWeapon(ScriptableGun newGun)
    {
        Destroy(gunObject.gameObject);
        gunObject = Instantiate(newGun.weaponPrefab, transform);
    }
}
