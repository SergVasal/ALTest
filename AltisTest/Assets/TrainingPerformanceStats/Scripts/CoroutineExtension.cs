using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AltisTests.TrainingPerformanceStats
{
    public class CoroutineExtensionBehavior : MonoBehaviour
    {
    }

    public static class CoroutineExtension
    {
        static CoroutineExtensionBehavior behaviour = null;

        static List<Coroutine> allCoroutines = new List<Coroutine>();

        public static Coroutine StartCoroutine(this IEnumerator iterator)
        {
            Initialize();

            Coroutine asyncCoroutine = behaviour.StartCoroutine(iterator);
            if (asyncCoroutine != null)
            {
                allCoroutines.Add(asyncCoroutine);
            }

            return asyncCoroutine;
        }

        public static void StopCoroutine(this Coroutine coroutine)
        {
            if ((coroutine != null) && (behaviour))
            {
                if (allCoroutines.Contains(coroutine))
                {
                    allCoroutines.Remove(coroutine);
                    behaviour.StopCoroutine(coroutine);
                }
            }
        }

        static void Initialize()
        {
            if (behaviour == null)
            {
                GameObject g = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(g);
                g.name = "CoroutineExtension";
                g.hideFlags = HideFlags.HideAndDontSave;

                behaviour = g.AddComponent<CoroutineExtensionBehavior>();
            }
        }
    }
}