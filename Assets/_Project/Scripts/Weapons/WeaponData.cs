using UnityEngine;

namespace FinalDrop.Weapons
{
    public enum WeaponCategory { AssaultRifle, SMG, Shotgun, Sniper, Pistol, LMG, Melee }

    [CreateAssetMenu(fileName = "NewWeapon", menuName = "FinalDrop/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Identity")]
        public string weaponName = "Vantar-7";
        public WeaponCategory category = WeaponCategory.AssaultRifle;

        [Header("Damage")]
        public float damagePerHit = 24f;
        public float headshotMultiplier = 2f;

        [Header("Fire")]
        public float fireRate = 8f;
        public int magazineSize = 30;
        public float reloadTime = 2.2f;
        public bool isAutomatic = true;

        [Header("Accuracy")]
        public float baseAccuracy = 0.85f;
        public float bulletSpreadDegrees = 1.5f;
        public AnimationCurve recoilPattern = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Range")]
        public float effectiveRange = 60f;
        public AnimationCurve damageFalloff = AnimationCurve.Linear(0, 1, 1, 0.4f);

        [Header("Movement")]
        [Range(0f, 1f)] public float adsMovementPenalty = 0.4f;

        [Header("Attachments")]
        public bool acceptsScope = true;
        public bool acceptsMag = true;
        public bool acceptsGrip = true;
        public bool acceptsSuppressor = true;
    }
}
