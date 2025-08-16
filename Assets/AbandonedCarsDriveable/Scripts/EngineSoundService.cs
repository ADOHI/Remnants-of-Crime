using System.Collections;
using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class EngineSoundService : MonoBehaviour
    {
        [SerializeField] private AudioSource engineStartAudioSource;
        [SerializeField] private AudioSource idleAudioSource;
        [SerializeField] private AudioSource midRPMAudioSource;

        [SerializeField] private AudioClip engineStartClip;
        [SerializeField] private AudioClip idleClip;
        [SerializeField] private AudioClip midRPMClip;

        [Header("Settings")]
        [SerializeField] private float pitchMinRPM = 1.0f;
        [SerializeField] private float pitchMaxRPM = 2.0f;
        [SerializeField] private float volumeStart = 1.0f;
        [SerializeField] private float volumeIdle = 0.1f;
        [SerializeField] private float crossfadeSpeed = 3.0f;

        private bool isEngineStarted = false;

        public void PlayEngineStartSound()
        {
            if (engineStartAudioSource != null && engineStartClip != null)
            {
                engineStartAudioSource.clip = engineStartClip;
                engineStartAudioSource.volume = volumeStart;
                engineStartAudioSource.loop = false;
                engineStartAudioSource.Play();
                StartCoroutine(FadeToIdleWhileStartPlays(engineStartClip.length));
            }
        }

        public void Stop()
        {
            isEngineStarted = false;
            engineStartAudioSource.Stop();
            idleAudioSource.Stop();
            midRPMAudioSource.Stop();
        }
        private IEnumerator FadeToIdleWhileStartPlays(float delay)
        {
            if (idleAudioSource != null && idleClip != null)
            {
                idleAudioSource.clip = idleClip;
                idleAudioSource.volume = 0.0f;
                idleAudioSource.loop = true;
                idleAudioSource.Play();

                float fadeTime = 0.0f;
                while (fadeTime < delay)
                {
                    fadeTime += Time.deltaTime;
                    idleAudioSource.volume = Mathf.Lerp(0.0f, volumeIdle, fadeTime / delay);
                    yield return null;
                }
                isEngineStarted = true;
            }
        }

        public void UpdateEngineSoundWithSpeed(float speed, float maxSpeed)
        {
            if (!isEngineStarted) return;

            float idleVolume;
            if (speed <= 5f)
            {
                idleVolume = Mathf.Lerp(0.1f, 0.11f, speed / 5f);
            }
            else if (speed <= 15f)
            {
                idleVolume = Mathf.Lerp(0.11f, 0.01f, (speed - 5f) / 10f);
            }
            else
            {
                idleVolume = 0f;
            }
            float midRPMVolume = speed > 5f
                ? Mathf.Lerp(0.1f, 0.8f, (speed - 5f) / (maxSpeed - 5f))
                : 0f;

            float midRPMPitch = Mathf.Lerp(pitchMinRPM, pitchMaxRPM, speed / maxSpeed);
            idleAudioSource.volume = Mathf.MoveTowards(idleAudioSource.volume, idleVolume, crossfadeSpeed * Time.deltaTime * 0.5f);
            midRPMAudioSource.volume = Mathf.MoveTowards(midRPMAudioSource.volume, midRPMVolume, crossfadeSpeed * Time.deltaTime);
            midRPMAudioSource.pitch = midRPMPitch;
            if (speed > 5f)
            {
                if (!midRPMAudioSource.isPlaying)
                {
                    midRPMAudioSource.clip = midRPMClip;
                    midRPMAudioSource.volume = 0f;
                    midRPMAudioSource.loop = true;
                    midRPMAudioSource.Play();
                }
            }
            else
            {
                if (midRPMAudioSource.isPlaying)
                {
                    midRPMAudioSource.Stop();
                }
            }
        }
    }
}