using Unity.Netcode;
using UnityEngine;
using FinalDrop.Player;

namespace FinalDrop.Networking
{
    /// <summary>
    /// Attach alongside PlayerMovementController and MobileInputManager.
    /// Disables input processing for every player EXCEPT the local one,
    /// so remote players' movement comes only from network sync, never
    /// from your own screen's touch input.
    /// </summary>
    public class NetworkPlayerSetup : NetworkBehaviour
    {
        [SerializeField] private MobileInputManager inputManager;
        [SerializeField] private GameObject touchUICanvas; // your on-screen controls

        public override void OnNetworkSpawn()
        {
            bool isLocalPlayer = IsOwner;

            if (inputManager != null)
                inputManager.enabled = isLocalPlayer;

            // Only show YOUR OWN touch controls, not a copy for every player
            if (touchUICanvas != null)
                touchUICanvas.SetActive(isLocalPlayer);
        }
    }
}
