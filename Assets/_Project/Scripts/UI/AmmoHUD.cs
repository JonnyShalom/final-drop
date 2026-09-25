using UnityEngine;
using UnityEngine.UI;
using FinalDrop.Weapons;

namespace FinalDrop.UI
{
    public class AmmoHUD : MonoBehaviour
    {
        [SerializeField] private WeaponController weapon;
        [SerializeField] private Text ammoText;

        private void Update()
        {
            if (weapon == null || ammoText == null) return;
            ammoText.text = weapon.IsReloading
                ? "Reloading..."
                : $"{weapon.CurrentAmmo} / {weapon.MagazineSize}";
        }
    }
}
