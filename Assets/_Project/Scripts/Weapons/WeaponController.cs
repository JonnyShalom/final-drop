using UnityEngine;

namespace FinalDrop.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponData data;
        [SerializeField] private Transform muzzlePoint;
        [SerializeField] private LayerMask hitMask;

        private int _currentAmmo;
        private float _lastFireTime;
        private bool _isReloading;

        public bool FireHeld { get; set; }
        public bool ReloadPressed { get; set; }

        private void Awake()
        {
            _currentAmmo = data.magazineSize;
        }

        private void Update()
        {
            if (ReloadPressed && !_isReloading && _currentAmmo < data.magazineSize)
            {
                StartCoroutine(ReloadRoutine());
                ReloadPressed = false;
            }

            if (FireHeld && CanFire())
            {
                Fire();
                if (!data.isAutomatic) FireHeld = false;
            }
        }

        private bool CanFire()
        {
            if (_isReloading || _currentAmmo <= 0) return false;
            float fireInterval = 1f / data.fireRate;
            return Time.time - _lastFireTime >= fireInterval;
        }

        private void Fire()
        {
            _lastFireTime = Time.time;
            _currentAmmo--;

            Vector3 spread = Random.insideUnitSphere * data.bulletSpreadDegrees * 0.01f;
            Vector3 direction = (muzzlePoint.forward + spread).normalized;

            if (Physics.Raycast(muzzlePoint.position, direction, out RaycastHit hit, data.effectiveRange, hitMask))
            {
                float distanceRatio = hit.distance / data.effectiveRange;
                float falloff = data.damageFalloff.Evaluate(distanceRatio);
                float finalDamage = data.damagePerHit * falloff;

                Debug.Log($"{data.weaponName} hit {hit.collider.name} for {finalDamage:F1} dmg");
            }
        }

        private System.Collections.IEnumerator ReloadRoutine()
        {
            _isReloading = true;
            yield return new WaitForSeconds(data.reloadTime);
            _currentAmmo = data.magazineSize;
            _isReloading = false;
        }

        public int CurrentAmmo => _currentAmmo;
        public int MagazineSize => data.magazineSize;
        public bool IsReloading => _isReloading;
    }
}
