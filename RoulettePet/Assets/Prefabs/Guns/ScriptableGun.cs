 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class ScriptableGun : ScriptableObject
{
    [Header("Gun Type")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Gun Type | = = = = = = = = = = = = #
    [Tooltip("How the gun will be activated by the player")]                                public ActivationType activationType;
    [Tooltip("Method on which the gun will create attacks")]                                public ShootType shootType;
    [Tooltip("Object spawned when using weapon, can be projectile or attack area")]         public GameObject attackObject;

    [Header("Basic Stats")] // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Basic Stats  | = = = = = = = = = = #
    [Tooltip("Base damage dealt to targets")]                                               public float damage = 1f;
    [Tooltip("Time in seconds that the player must hold before firing")]                    public float timeBeforeFiring = 0;
    [Tooltip("Time in seconds on which bullets fire, works with single shots")]             public float timeBetweenShots = 0.2f;
    [Tooltip("Speed which projectiles travel (If relevant)")]                               public float projectileSpeed = 10;
    [Tooltip("Distance on which instant hits can hit from")]                                public float hitDistance = 10;
    [Tooltip("Time in seconds of the delay between area based attacks")]                    public float hitDelay = 0.2f;

    [Header("Ammo")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Ammo | = = = = = = = = = = = = = = #
    [Tooltip("Infinite ammo that will not be drained, reloading still required")]           public bool infiniteAmmo;
    [Tooltip("Max amount of ammo that can be stored at once")]                              public int ammoMax = 120;
    [Tooltip("Infinite mag that does not need to reload")]                                  public bool infiniteMag;
    [Tooltip("Max amount of ammo that can be loaded at once")]                              public int magMax = 20;
    [Tooltip("Time it takes to reload in seconds")]                                         public float reloadTime = 2;


    [Header("Special Attributes")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = | Special Attributes | = = = = = = = #
    [Tooltip("The gun will stop firing when overheated and must be cooled over time")]      public bool overheatable;
    [Tooltip("")]public bool explosiveAmmo;
    [Tooltip("Rings that spawn when hitting objects, will transfer damage and debuffs")]    public bool shockWaves;
    [Tooltip("Attacks will push objects away from the position of the attack")]             public bool knockback;
    [Tooltip("Projectile attacks will bounce off surfaces and objects")]                    public bool ricochet;
    [Tooltip("Attacks will be created in unison with the given angle difference")]          public bool multishot;
    [Tooltip("Attacks will create a chain of lighting which bounces between targets")]      public bool lightning;
    [Tooltip("Attacks will automatically do their effects after a set period of time")]     public bool selfDestruct;
    [Tooltip("Targets will viscerally explode on death")]                                   public bool goreyDeath;

    [Header("Overheat")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Overheat | = = = = = = = = = = = = #
    [Tooltip("Duration of time/amount of shots it takes to overheat the gun")]              public float timeToOverheat = 10;
    [Tooltip("Time in seconds in which it takes to cool the gun")]                          public float timeToCool = 5;

    [Header("Explosive")] // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Explosions | = = = = = = = = = = = #
    [Tooltip("")] public float explosionArea = 3;                                           

    [Header("Shockwaves")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Shockwaves | = = = = = = = = = = = #
    [Tooltip("Size of shockwave, each number adds an extra larger ring")] [Range(3,23)]     public int shockWaveSize = 5;

    [Header("Knockback")] // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Knockback  | = = = = = = = = = = = #
    [Tooltip("Power of knockback when hitting a target")]                                   public float knockbackPower = 10;

    [Header("Ricochet")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Ricochet | = = = = = = = = = = = = #
    [Tooltip("Amount of times the projectile can bounce of surfaces/targets")]              public int ricochetCount = 5;

    [Header("Multishot")] // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Multishot  | = = = = = = = = = = = #
    [Tooltip("Amount of attacks fired at once")]                                            public int split = 3;
    [Tooltip("Whether each shot consumes ammo or only 1 is spent per group")]               public bool consumeExtraAmmo = true;
    [Tooltip("Angle between each attack, groups of attacks are centered")]                  public float angleDifference = 45;

    [Header("Lightning")] // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Lightning  | = = = = = = = = = = = #
    [Tooltip("Lighting chains either hits X enemies total or targets X other enemies ")]    public bool exponentialLightning;
    [Tooltip("Additive percentage that reduces lightning chain damage, will not continue if <= 0")]public float flatDiminishPercentage = 0.15f;
    [Tooltip("Max targets the lightning will travel between")]                              public int maxChain = 4;

    [Header("Self Destruction")]  // = = = = = = = = = = = = = = = = = = = = = = = = = = = | Self Destruction | = = = = = = = = #
    [Tooltip("Time in seconds that the object will self-destruct after")]                   public float selfDestructTime = 3;
    [Tooltip("Only destroy on self-destruct, projectile will lose all speed if no ricochet")]public bool overrideNormalHit = false;

    [Header("Debuffs")] // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = | Debuffs  | = = = = = = = = = = = = #
    [Tooltip("Attacks will slow hit targets reducing movement speed and attacks")]          public bool frost;
    [Tooltip("Duration on which the debuff is activate after hit")]                         public float frostDuration = 4;
    [Tooltip("Amount of slowdown from a single stack")]                                     public float frostSlowdown = 0.15f;

    [Tooltip("Enemies are unable to move or attack")]                                       public bool stun;
    [Tooltip("Duration on which the debuff is activate after hit")]                         public float stunDuration = 2f;

    [Tooltip("Enemies will rapidly lose health when at the max stack of 5")]                public bool poison;
    [Tooltip("Duration on which the debuff is activate after hit")]                         public float poisonDuration = 6;
    [Tooltip("Amount of damage enemies will take every 0.2 seconds")]                       public float poisonDamage = 30;

    [Tooltip("Enemies will change target based on: Non-converted > Converted > Player")]    public bool enemyConversion;
    [Tooltip("Duration on which the debuff is activate after hit")]                         public float conversionDuration = 10;

    [Tooltip("Freeze enemies into a one-shot entity")]                                      public bool freeze = false;
    [Tooltip("Duration on which the debuff is activate after hit")]                         public float freezeDuration = 3;



    public enum ActivationType
    {
        Single,
        Held,
        HeldDistance
    }

    public enum ShootType
    {
        Single,
        DirectlyAtTarget,
        BurstArea,
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
            ignoreList = new string[29];

            if (self.shootType != ShootType.DirectlyAtTarget) { ignoreList[0] = "hitDistance"; }
            if (self.shootType != ShootType.Single) { ignoreList[1] = "projectileSpeed"; }
            if (self.shootType != ShootType.ConstantArea) { ignoreList[2] = "hitDelay"; }

            if (self.infiniteAmmo) { ignoreList[3] = "ammoMax"; }
            if (self.infiniteMag) { ignoreList[4] = "magMax"; ignoreList[5] = "reloadTime"; ignoreList[6] = "fuelConsumptionRate"; }

            if (!self.explosiveAmmo) { ignoreList[7] = "explosionArea"; }
            if (!self.shockWaves) { ignoreList[8] = "shockWaveSize"; }

            if (!self.overheatable) { ignoreList[9] = "timeToOverheat"; ignoreList[10] = "timeToCool"; }
            if (!self.knockback) { ignoreList[11] = "knockbackPower"; }
            if (!self.ricochet) { ignoreList[12] = "ricochetCount"; }

            if (!self.multishot) { ignoreList[13] = "split"; ignoreList[14] = "consumeExtraAmmo"; ignoreList[15] = "angleDifference"; }

            if (!self.lightning) { ignoreList[16] = "exponentialLightning"; ignoreList[17] = "flatDiminishPercentage"; ignoreList[18] = "maxChain"; }
            if (self.exponentialLightning) { ignoreList[19] = "maxChain"; }

            if (!self.selfDestruct) { ignoreList[21] = "selfDestructTime"; ignoreList[22] = "overrideNormalHit"; }

            if (self.shootType != ShootType.Single) 
            {
                self.selfDestruct = false; ignoreList[20] = "selfDestruct"; ignoreList[21] = "selfDestructTime"; ignoreList[22] = "overrideNormalHit";
                ignoreList[23] = "ricochet"; ignoreList[12] = "ricochetCount";
            }

            if (!self.frost) { ignoreList[22] = "frostDuration"; ignoreList[23] = "frostSlowdown"; }
            if (!self.stun) { ignoreList[24] = "stunDuration"; }
            if (!self.poison) { ignoreList[25] = "poisonDuration"; ignoreList[26] = "poisonDamage"; }
            if (!self.enemyConversion) { ignoreList[27] = "conversionDuration"; }
            if (!self.freeze) { ignoreList[28] = "freezeDuration"; }

            DrawPropertiesExcluding(serializedObject, ignoreList);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
