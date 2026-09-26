using UnityEngine;
using UnityEngine.UI;
using FinalDrop.Inventory;

namespace FinalDrop.UI
{
    /// <summary>
    /// Attach to a UI panel with an interact prompt (e.g. "Tap to pick up").
    /// Call ShowPrompt/HidePrompt from LootPickup's trigger events.
    /// </summary>
    public class InteractPromptUI : MonoBehaviour
    {
        [SerializeField] private GameObject promptPanel;
        [SerializeField] private Text promptLabel;
        [SerializeField] private Button interactButton;

        private LootPickup _currentTarget;

        private void Awake()
        {
            if (promptPanel != null) promptPanel.SetActive(false);
            if (interactButton != null) interactButton.onClick.AddListener(OnInteractPressed);
        }

        public void ShowPrompt(LootPickup target, string label = "Tap to pick up")
        {
            _currentTarget = target;
            if (promptLabel != null) promptLabel.text = label;
            if (promptPanel != null) promptPanel.SetActive(true);
        }

        public void HidePrompt(LootPickup target)
        {
            if (_currentTarget != target) return;
            _currentTarget = null;
            if (promptPanel != null) promptPanel.SetActive(false);
        }

        private void OnInteractPressed()
        {
            _currentTarget?.TryPickup();
            if (promptPanel != null) promptPanel.SetActive(false);
        }
    }
}
