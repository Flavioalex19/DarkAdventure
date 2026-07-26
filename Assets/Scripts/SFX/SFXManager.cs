using UnityEngine;
using System.Collections;
public class SFXManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource ambienceAudioSource;
    public AudioSource soundtrackAudioSource;
    [Header("Fade Settings")]
    public float fadeDuration = 1.2f;
    public float summaryFadeDuration = 0.6f;
    public void PlayAmbience(AudioClip newClip)
    {
        if (ambienceAudioSource == null || newClip == null) return;
        StartCoroutine(FadeToNewAmbience(newClip));
    }

    /// <summary>
    /// Toca a soundtrack e reduz o volume dos outros AudioSources
    /// </summary>
    public void PlaySoundtrack()
    {
        if (soundtrackAudioSource == null) return;
        StartCoroutine(PlaySoundtrackRoutine());
    }

    IEnumerator PlaySoundtrackRoutine()
    {
        print("Here");
        // Reduz o volume da ambientação
        if (ambienceAudioSource != null && ambienceAudioSource.isPlaying)
        {
            StartCoroutine(FadeVolume(ambienceAudioSource, ambienceAudioSource.volume, 0f));
        }

        // Toca a soundtrack (já tem o clipe) começando do volume 0
        soundtrackAudioSource.volume = 0f;
        if (!soundtrackAudioSource.isPlaying)
        {
            soundtrackAudioSource.Play();
        }

        // Aumenta o volume da soundtrack
        yield return StartCoroutine(FadeVolume(soundtrackAudioSource, 0f, 1f));
    }

    IEnumerator FadeToNewAmbience(AudioClip newClip)
    {
        if (ambienceAudioSource.isPlaying)
        {
            yield return StartCoroutine(FadeVolume(ambienceAudioSource, ambienceAudioSource.volume, 0f));
        }

        ambienceAudioSource.clip = newClip;
        ambienceAudioSource.Play();

        yield return StartCoroutine(FadeVolume(ambienceAudioSource, 0f, 1f));
    }

    IEnumerator FadeVolume(AudioSource source, float fromVolume, float toVolume)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            source.volume = Mathf.Lerp(fromVolume, toVolume, t);
            yield return null;
        }

        source.volume = toVolume;
    }
    public void StopSoundtrack()
    {
        StartCoroutine(StopSoundtrackRoutine());
    }

    IEnumerator StopSoundtrackRoutine()
    {
        // Fade out da soundtrack
        if (soundtrackAudioSource != null && soundtrackAudioSource.isPlaying)
        {
            yield return StartCoroutine(FadeVolume(soundtrackAudioSource, soundtrackAudioSource.volume, 0f, summaryFadeDuration));
            soundtrackAudioSource.Stop();
        }

        // Fade in da ambientação
        if (ambienceAudioSource != null)
        {
            if (!ambienceAudioSource.isPlaying && ambienceAudioSource.clip != null)
            {
                ambienceAudioSource.Play();
            }

            yield return StartCoroutine(FadeVolume(ambienceAudioSource, ambienceAudioSource.volume, 1f, summaryFadeDuration));
        }
    }
    IEnumerator FadeVolume(AudioSource source, float fromVolume, float toVolume, float duration = -1f)
    {
        if (duration < 0f) duration = fadeDuration;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            source.volume = Mathf.Lerp(fromVolume, toVolume, t);
            yield return null;
        }

        source.volume = toVolume;
    }
    public void TransitionToNewAmbience(AudioClip newAmbienceClip)
    {
        StartCoroutine(TransitionToNewAmbienceRoutine(newAmbienceClip));
    }

    IEnumerator TransitionToNewAmbienceRoutine(AudioClip newClip)
    {
        // 1. Só faz fade out da soundtrack
        if (soundtrackAudioSource != null && soundtrackAudioSource.isPlaying)
        {
            yield return StartCoroutine(FadeVolume(soundtrackAudioSource, soundtrackAudioSource.volume, 0f, summaryFadeDuration));
            soundtrackAudioSource.Stop();
        }

        // 2. Troca o clipe da ambientação (se quiser trocar o áudio)
        // mas NÃO mexe no volume dela
        if (ambienceAudioSource != null && newClip != null)
        {
            bool wasPlaying = ambienceAudioSource.isPlaying;
            float currentVolume = ambienceAudioSource.volume;

            ambienceAudioSource.clip = newClip;

            if (wasPlaying)
            {
                ambienceAudioSource.Play();
                ambienceAudioSource.volume = currentVolume; // mantém o volume que já estava
            }
        }
    }
}
