using UnityEngine;
using UnityEngine.UI;
using FinalDrop.Networking;
using FinalDrop.Core;

namespace FinalDrop.UI
{
    public class MatchHUDController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerHealth localPlayerHealth;
        [SerializeField] private SafeZoneController zone;

        [Header("UI Elements")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Text healthText;
        [SerializeField] private Text zonePhaseText;
        [SerializeField] private Text playersAliveText;

        private void Update()
        {
            UpdateHealthUI();
            UpdateZoneUI();
        }

        private void UpdateHealthUI()
        {
            if (localPlayerHealth == null) return;

            float pct = localPlayerHealth.CurrentHealth / localPlayerHealth.MaxHealth;
            if (healthBar != null) healthBar.value = pct;
            if (healthText != null)
                healthText.text = $"{Mathf.CeilToInt(localPlayerHealth.CurrentHealth)} / {localPlayerHealth.MaxHealth}";
        }

        private void UpdateZoneUI()
        {
            if (zone == null || zonePhaseText == null) return;
            zonePhaseText.text = $"Zone Phase {zone.CurrentPhase + 1}";
        }

        /// <summary>Call this from your IGameMode whenever the alive count changes.</summary>
        public void SetPlayersAlive(int count)
        {
            if (playersAliveText != null)
                playersAliveText.text = $"{count} remaining";
        }
    }
}
