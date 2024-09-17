using GabrielBertasso.Debugger.Helpers;
using UnityEngine;

namespace GabrielBertasso.Debugger.Patterns
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = CreateInstance();
                }

                return s_instance;
            }
        }
        public static bool HasInstance => s_instance != null;

        protected static T s_instance;


        protected virtual void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
            }
            else if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected static T CreateInstance()
        {
            if (ApplicationManager.IsQuitting)
            {
                return null;
            }
            
            return ObjectHelper.InstantiateComponent<T>();
        }
    }
}
