#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GabrielBertasso.Debugger
{
    public class DebuggerActions : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [Button]
#endif
        public static void PauseTime()
        {
            Time.timeScale = 0f;
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public static void ResetTimeSpeed()
        {
            Time.timeScale = 1f;
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public static void SpeedUpTime()
        {
            Time.timeScale += 0.2f;
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public static void SlowDownTime()
        {
            Time.timeScale -= 0.2f;
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            Debugger.AddAction(PauseTime, "Pause Time");
            Debugger.AddAction(ResetTimeSpeed, "Reset Time Speed");
            Debugger.AddAction(SpeedUpTime, "Speed Up Time");
            Debugger.AddAction(SlowDownTime, "Slow Down Time");
            Debugger.AddAction(ReloadScene, "Reload Scene");
        }
    }
}