using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup fader = null;

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
            while (fader.alpha < 1)
            {
                fader.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (fader.alpha > 0)
            {
                fader.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
