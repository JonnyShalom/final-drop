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

        /// <summary>
        /// Call this from the WEAPON's server-side hit logic only.
        /// Never call this from client code — damage must originate on the server.
        /// </summary>
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
            // TODO Phase 7: trigger knockdown state, elimination counter, match stats
            Debug.Log($"Player {OwnerClientId} was eliminated by {attackerId}");
        }
    }
}
