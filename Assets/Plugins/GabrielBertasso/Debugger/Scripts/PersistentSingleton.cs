using UnityEngine;

namespace GabrielBertasso.Debugger.Patterns
{
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
