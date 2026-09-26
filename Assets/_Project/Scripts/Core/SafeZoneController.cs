using Unity.Netcode;
using UnityEngine;
using FinalDrop.Networking;

namespace FinalDrop.Core
{
    /// <summary>
    /// Server-authoritative shrinking zone. Only the server moves/shrinks the
    /// zone and applies damage — clients only read the synced values to draw
    /// the zone circle on the minimap/world.
    /// </summary>
    public class SafeZoneController : NetworkBehaviour
    {
        [Header("Zone Phases")]
        [SerializeField] private float[] phaseRadii = { 500f, 350f, 220f, 130f, 70f, 30f, 10f };
        [SerializeField] private float[] phaseShrinkDuration = { 90f, 75f, 60f, 45f, 30f, 20f, 15f };
        [SerializeField] private float[] phaseWaitDuration = { 60f, 50f, 40f, 30f, 20f, 15f, 10f };
        [SerializeField] private float[] phaseDamagePerSecond = { 1f, 2f, 3f, 5f, 8f, 12f, 20f };

        private NetworkVariable<Vector3> _currentCenter = new NetworkVariable<Vector3>();
        private NetworkVariable<float> _currentRadius = new NetworkVariable<float>();
        private NetworkVariable<int> _currentPhase = new NetworkVariable<int>(-1);

        public Vector3 CurrentCenter => _currentCenter.Value;
        public float CurrentRadius => _currentRadius.Value;
        public int CurrentPhase => _currentPhase.Value;

        private float _phaseTimer;
        private bool _isShrinking;
        private Vector3 _shrinkStartCenter;
        private float _shrinkStartRadius;
        private Vector3 _nextCenter;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            _currentRadius.Value = phaseRadii.Length > 0 ? phaseRadii[0] * 1.5f : 600f;
            _currentCenter.Value = Vector3.zero;
            StartNextPhaseServer();
        }

        private void Update()
        {
            if (!IsServer) return;
            TickZoneServer();
            ApplyZoneDamageServer();
        }

        private void TickZoneServer()
        {
            if (_currentPhase.Value >= phaseRadii.Length - 1 && !_isShrinking) return;

            _phaseTimer -= Time.deltaTime;

            if (_isShrinking)
            {
                float duration = phaseShrinkDuration[_currentPhase.Value];
                float t = 1f - Mathf.Clamp01(_phaseTimer / duration);
                _currentRadius.Value = Mathf.Lerp(_shrinkStartRadius, phaseRadii[_currentPhase.Value], t);
                _currentCenter.Value = Vector3.Lerp(_shrinkStartCenter, _nextCenter, t);

                if (_phaseTimer <= 0f)
                {
                    _isShrinking = false;
                    StartWaitPhaseServer();
                }
            }
            else if (_phaseTimer <= 0f && _currentPhase.Value < phaseRadii.Length - 1)
            {
                StartNextPhaseServer();
            }
        }

        private void StartNextPhaseServer()
        {
            _currentPhase.Value++;
            _shrinkStartCenter = _currentCenter.Value;
            _shrinkStartRadius = _currentRadius.Value;

            // Pick a random point inside the current zone as the next center.
            Vector2 randomOffset = Random.insideUnitCircle * (_currentRadius.Value - phaseRadii[_currentPhase.Value]);
            _nextCenter = _currentCenter.Value + new Vector3(randomOffset.x, 0f, randomOffset.y);

            _phaseTimer = phaseShrinkDuration[_currentPhase.Value];
            _isShrinking = true;
        }

        private void StartWaitPhaseServer()
        {
            if (_currentPhase.Value < phaseWaitDuration.Length)
                _phaseTimer = phaseWaitDuration[_currentPhase.Value];
        }

        private void ApplyZoneDamageServer()
        {
            if (_currentPhase.Value < 0) return;
            float damage = phaseDamagePerSecond[Mathf.Min(_currentPhase.Value, phaseDamagePerSecond.Length - 1)];

            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                var playerObj = client.PlayerObject;
                if (playerObj == null) continue;

                float dist = Vector3.Distance(playerObj.transform.position, _currentCenter.Value);
                if (dist > _currentRadius.Value)
                {
                    var health = playerObj.GetComponent<PlayerHealth>();
                    health?.ApplyDamageServerRpc(damage * Time.deltaTime, 0);
                }
            }
        }
    }
}
