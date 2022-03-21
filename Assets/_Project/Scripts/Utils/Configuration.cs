using UnityEngine;

namespace Bitszer
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "GraphQlClient/Configuration")]
    public class Configuration : ScriptableObject
    {
        public string gameId;
    }
}