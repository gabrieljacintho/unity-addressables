using UnityEngine;

namespace GabrielBertasso.Debugger
{
    public static class ApplicationManager
    {
        public static bool IsQuitting { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            Application.quitting += OnQuitting;
        }
        
        private static void OnQuitting()
        {
            IsQuitting = true;
        }
    }
}