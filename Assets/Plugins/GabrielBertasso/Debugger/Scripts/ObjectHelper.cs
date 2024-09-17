using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GabrielBertasso.Debugger.Helpers
{
    public static class ObjectHelper
    {
        public static T[] FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return Object.FindObjectsOfType<MonoBehaviour>(includeInactive).OfType<T>().ToArray();
        }

        public static void DestroyOnLoad(GameObject targetObject)
        {
            SceneManager.MoveGameObjectToScene(targetObject, SceneManager.GetActiveScene());
        }
        
        public static T InstantiateComponent<T>(Transform parent = null) where T : Component
        {
            GameObject gameObject = new GameObject(typeof(T).Name);
            gameObject.transform.parent = parent;
            return gameObject.AddComponent<T>();
        }
    }
}