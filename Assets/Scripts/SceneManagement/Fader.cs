using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup fader = null;
        Coroutine currentActiveFade = null;

        private void Awake()
        {
            fader = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            fader.alpha = 1;
        }

        IEnumerator FadeOutIn(float duration)
        {
            float inOutFadeTime = duration / 2;
            yield return FadeOut(inOutFadeTime);
            print("Faded out");
            yield return FadeIn(inOutFadeTime);
            print("Faded in");
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }

            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(fader.alpha, target))
            {
                fader.alpha = Mathf.MoveTowards(fader.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
