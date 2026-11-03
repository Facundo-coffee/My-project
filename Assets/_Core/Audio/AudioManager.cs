using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Configuración de Mezclador")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Fuentes de Audio Internas")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        // Patrón Singleton para persistir entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ConfigurarFuentes();
    }

    private void ConfigurarFuentes()
    {
        // Configuramos la fuente de música como 2D en loop
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        musicSource.outputAudioMixerGroup = musicGroup;

        // Configuramos la fuente de efectos 2D
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f;
        sfxSource.outputAudioMixerGroup = sfxGroup;
    }

    /// <summary>
    /// Reproduce o transiciona una pista musical de fondo.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    /// <summary>
    /// Reproduce un efecto no posicional (salto, moneda, muerte).
    /// PlayOneShot permite superponer sonidos sin cortar el anterior.
    /// </summary>
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volumeScale);
    }

    /// <summary>
    /// Control de volumen por decibelios para sliders de UI (rango 0.0001 a 1).
    /// </summary>
    public void SetMusicVolume(float normalizedValue)
    {
        float db = Mathf.Log10(Mathf.Clamp(normalizedValue, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("MusicVol", db);
    }
}