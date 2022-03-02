using UnityEngine;
using Utility.Logging;

namespace Managers
{
    public sealed class BattleEnvironmentsManager : MonoBehaviour
    {
        private readonly ILog _log = LogManager.GetLogger<BattleEnvironmentsManager>();
        
        [Header("Configuration")]
        [SerializeField] public GameObject[] Environments = null;
        
        /*
         * Mono Behavior.
         */

        private void Start()
        {
            if (Environments == null || Environments.Length == 0)
                _log.Error("Environments not configured.");
        }
        
        /*
         * Public.
         */

        public void EnableEnvironment(int levelId)
        {
            DisableAll();
            Environments[levelId < 4 ? 0 : 1].SetActive(true);
        }

        public void DisableAll()
        {
            foreach (var environment in Environments)
                environment.SetActive(false);
        }
    }
}