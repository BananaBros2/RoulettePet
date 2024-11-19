 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class ScriptableGun : ScriptableObject
{
    [Header("Gun Type")]
    public ActivationType activationType;
    public ShootType shootType;


    [Header("Basic Stats")]
    public float damage = 1f;
    public float timeBeforeFiring = 0;
    public float timeBetweenShots = 0.2f;
    public float projectileSpeed = 10;
    public float hitDistance = 10;
    public float hitDelay = 0.2f;

    [Header("Ammo")]
    public bool infiniteAmmo;
    public int ammoMax = 120;
    public bool infiniteMag;
    public int magMax = 20;
    public float reloadTime = 2;
    public float fuelConsumptionRate = 0.5f;


    [Header("Special Attributes")]
    public bool overheatable;
    public bool explosiveAmmo;
    public bool shockWaves;
    public bool knockback;
    public bool ricochet;
    public bool multishot;
    public bool lightning;
    public bool goreyDeath;

    [Header("Overheat")]
    public float timeToOverheat = 10;
    public float timeToCool = 5;

    [Header("Explosive")]
    public float explosionArea = 3;

    [Header("Shockwaves")]
    [Range(3,23)] public int shockWaveSize = 5;

    [Header("Knockback")]
    public float knockbackPower = 10;

    [Header("Ricochet")]
    public int ricochetCount = 5;

    [Header("Multishot")]
    public int split = 3;
    public bool consumeExtraAmmo = true;
    public float angleDifference = 45;

    [Header("Lightning")]
    public bool exponentialLightning;
    public float flatDiminishPercentage = 0.15f;
    public int maxChain = 4;


    [Header("Debuffs")]
    public bool frost;
    public float frostDuration = 4;
    public float frostSlowdown = 0.15f;

    public bool stun;
    public float stunDuration = 2f;

    public bool poison;
    public float poisonDuration = 6;
    public float poisonDamage = 30;

    public bool enemyConversion;
    public float conversionDuration = 10;


    public enum ActivationType
    {
        Single,
        Held,
        HeldDistance
    }

    public enum ShootType
    {
        Single,
        ClosestSingle,
        Area,
        ConstantArea
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableGun))]
    class GunVariableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ScriptableGun self = (ScriptableGun)target;
            serializedObject.Update();

            string[] ignoreList;
            ignoreList = new string[25];

            //ignoreList[0] = ""

            if (self.shootType != ShootType.ClosestSingle) { ignoreList[1] = "hitDistance"; }
            if (self.shootType != ShootType.Single) { ignoreList[2] = "projectileSpeed"; }
            if (self.shootType != ShootType.ConstantArea) { ignoreList[3] = "hitDelay"; }

            if (!self.explosiveAmmo) { ignoreList[4] = "explosionArea"; }
            if (!self.shockWaves) { ignoreList[5] = "shockWaveSize"; }

            if (!self.overheatable) { ignoreList[6] = "timeToOverheat"; ignoreList[7] = "timeToCool"; }
            if (!self.knockback) { ignoreList[8] = "knockbackPower"; }
            if (!self.ricochet) { ignoreList[9] = "ricochetCount"; }

            if (!self.multishot) { ignoreList[10] = "split"; ignoreList[11] = "consumeExtraAmmo"; ignoreList[12] = "angleDifference"; }

            if (!self.lightning) { ignoreList[13] = "exponentialLightning"; ignoreList[14] = "flatDiminishPercentage"; ignoreList[15] = "maxChain"; }
            if (self.exponentialLightning) { ignoreList[15] = "maxChain"; }

            if (!self.frost) { ignoreList[16] = "frostDuration"; ignoreList[17] = "frostSlowdown"; }
            if (!self.stun) { ignoreList[18] = "stunDuration"; }
            if (!self.poison) { ignoreList[19] = "poisonDuration"; ignoreList[20] = "poisonDamage"; }
            if (!self.enemyConversion) { ignoreList[21] = "conversionDuration"; }

            if (self.infiniteAmmo) { ignoreList[22] = "ammoMax";}
            if (self.infiniteMag) { ignoreList[23] = "magMax"; ignoreList[24] = "reloadTime"; ignoreList[0] = "fuelConsumptionRate"; }


            DrawPropertiesExcluding(serializedObject, ignoreList);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
