using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SoundFX
{
    public class AudioRandomizer : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.pitch = Random.Range(0.8f, 1.2f);
        }

        public void SetNextAudioClip()
        {
            if (audioClips == null) return;

            if (audioClips.Length > 1)
            {
                int nextIndex = Random.Range(0, audioClips.Length);

                audioSource.clip = audioClips[nextIndex];
            }
            audioSource.pitch = Random.Range(0.8f, 1.2f);

            audioSource.Play();
        }
    }
}
