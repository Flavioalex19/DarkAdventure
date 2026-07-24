using UnityEngine;
using System.Collections;
public class SFXManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource ambienceAudioSource;
    [Header("Fade Settings")]
    public float fadeDuration = 1.2f;
    public void PlayAmbience(AudioClip newClip)
    {
        if (ambienceAudioSource == null || newClip == null) return;

        StartCoroutine(FadeToNewAmbience(newClip));
    }

    IEnumerator FadeToNewAmbience(AudioClip newClip)
    {
        // Se já estiver tocando algo, faz fade out
        if (ambienceAudioSource.isPlaying)
        {
            yield return StartCoroutine(FadeVolume(ambienceAudioSource.volume, 0f));
        }

        // Troca o clipe
        ambienceAudioSource.clip = newClip;
        ambienceAudioSource.Play();

        // Faz fade in até o volume normal (1)
        yield return StartCoroutine(FadeVolume(0f, 1f));
    }

    IEnumerator FadeVolume(float fromVolume, float toVolume)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            ambienceAudioSource.volume = Mathf.Lerp(fromVolume, toVolume, t);
            yield return null;
        }

        ambienceAudioSource.volume = toVolume;
    }
}
