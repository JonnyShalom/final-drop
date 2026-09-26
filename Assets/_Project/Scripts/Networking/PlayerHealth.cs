using Unity.Netcode;
using UnityEngine;

namespace FinalDrop.Networking
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField] private float maxHealth = 100f;

        private NetworkVariable<float> _currentHealth = new NetworkVariable<float>(
            100f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        public float CurrentHealth => _currentHealth.Value;
        public float MaxHealth => maxHealth;
        public bool IsDead => _currentHealth.Value <= 0f;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _currentHealth.Value = maxHealth;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ApplyDamageServerRpc(float amount, ulong attackerId)
        {
            if (IsDead) return;

            _currentHealth.Value = Mathf.Max(0f, _currentHealth.Value - amount);

            if (_currentHealth.Value <= 0f)
            {
                HandleDeathServer(attackerId);
            }
        }

        private void HandleDeathServer(ulong attackerId)
        {
            Debug.Log($"Player {OwnerClientId} was eliminated by {attackerId}");
        }
    }
}
