using Unity.Netcode;
using UnityEngine;
using FinalDrop.Player;

namespace FinalDrop.Networking
{
    public class NetworkPlayerSetup : NetworkBehaviour
    {
        [SerializeField] private MobileInputManager inputManager;
        [SerializeField] private GameObject touchUICanvas;

        public override void OnNetworkSpawn()
        {
            bool isLocalPlayer = IsOwner;

            if (inputManager != null)
                inputManager.enabled = isLocalPlayer;

            if (touchUICanvas != null)
                touchUICanvas.SetActive(isLocalPlayer);
        }
    }
}
