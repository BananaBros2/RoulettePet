using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject gunObject;
    public ScriptableGun gunClass;
    public TMP_Text ammoCounterText;
    public GameObject reloadingText;

    public float timeSinceLastShot = 0;
    public int ammoLeft = 0;
    public int ammoLeftInMag = 0;
    public bool reloading = false;

    public GameObject bullet;

    System.Random randomizer;

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

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetButtonDown("Reload"))
        {
            StartReload(gunClass);
        }

        if (gunClass.activationType == ScriptableGun.ActivationType.Single && Input.GetButtonDown("Fire1") && !reloading)
        {
            if (!ActivationCheck(gunClass)) { return; }


            if (gunClass.multishot)
            {
                timeSinceLastShot = 0;
                MultishotMath(gunClass);
            }
            else
            {
                timeSinceLastShot = 0;
                ShootProjectile(gunClass);
            }
            

        }



    }


    private void FixedUpdate()
    {


        if (gunClass.activationType != ScriptableGun.ActivationType.Single && Input.GetButton("Fire1") && !reloading)
        {
            if (!ActivationCheck(gunClass)) { return; }


            if(gunClass.multishot)
            {
                timeSinceLastShot = 0;
                MultishotMath(gunClass);
            }
            else
            {
                timeSinceLastShot = 0;
                ShootProjectile(gunClass);
            }

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


    public void MultishotMath(ScriptableGun gun)
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
            ShootProjectile(gun, shootAngle, consumeAmmo);

            if (!gun.consumeExtraAmmo) { consumeAmmo = false; } // Only consume ammo on first shot
        }
    }

    public void ShootProjectile(ScriptableGun gun, float rotationOffset = 0, bool consumeAmmo = true)
    {
        GameObject newBullet = Instantiate(gun.attackObject, gunObject.transform.GetChild(0).transform.position, gunObject.transform.rotation);

        float randomOffset = (float)randomizer.Next(-100, 100) / 100;
        print(randomOffset);
        newBullet.transform.Rotate(0, 0, rotationOffset + randomOffset);

        if (!gun.infiniteMag && consumeAmmo) // Do not consume if has infinite mag ammo or if trigged not to
        {
            ammoLeftInMag--;
            UpdateAmmoCounter();
        }

        AttackScript bulletProperties = newBullet.GetComponent<AttackScript>();
        bulletProperties.SetBasicProperties(gun.projectileSpeed, gun.damage);
        TransferSpecials(gun, bulletProperties);
        TransferDebuffs(gun, bulletProperties);
    }

    public void TransferSpecials(ScriptableGun gun, AttackScript bulletProperties)
    {
        bulletProperties.SetSpecialProperties(
            gun.shockWaves, gun.shockWaveSize,
            gun.knockback ? gun.knockbackPower : 0,
            gun.ricochet ? gun.ricochetCount : 0,
            gun.exponentialLightning, gun.flatDiminishPercentage, gun.maxChain,
            gun.goreyDeath);

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
