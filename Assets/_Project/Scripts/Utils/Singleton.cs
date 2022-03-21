using UnityEngine;

namespace Bitszer
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance { get { return instance; } }

        public virtual void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
            else
                instance = this as T;
        }
    }

    public class SingletonPersistent<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance { get { return _instance; } }

        public virtual void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}