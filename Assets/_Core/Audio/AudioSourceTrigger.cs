using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceTrigger : MonoBehaviour
{
    [Serializable]
    public class EntradaSonido
    {
        [Tooltip("Identificador usado en el campo String del Animation Event PlaySound (ej. 'golpes', 'pasos').")]
        public string id;

        [Tooltip("Contenedor aleatorio (AudioRandomContainer) o AudioClip a reproducir.")]
        public AudioResource recurso;

        [Tooltip("Multiplicador de volumen relativo al volumen del AudioSource base del GameObject.")]
        [Range(0f, 1.5f)]
        public float volumen = 1f;
    }

    [Header("Sonido por Defecto (Retrocompatibilidad)")]
    [Tooltip("Opcional. Se reproduce cuando PlaySound se invoca sin nombre. Si queda vacío, usa el AudioResource asignado en el AudioSource del GameObject.")]
    [SerializeField] private AudioResource recursoPorDefecto;

    [Header("Catálogo de Sonidos por Clave")]
    [Tooltip("Lista de contenedores o clips identificables por nombre desde el evento PlaySound.")]
    [SerializeField] private List<EntradaSonido> sonidos = new List<EntradaSonido>();

    [Header("Superposición de Audio")]
    [Tooltip("Cantidad máxima de sonidos que pueden sonar superpuestos al mismo tiempo sin cortarse entre sí.")]
    [Range(1, 8)]
    [SerializeField] private int maxFuentesSimultaneas = 4;

    private AudioSource fuenteBase;
    private AudioResource recursoInicialFuente;
    private float volumenInicialFuente = 1f;

    private readonly Dictionary<string, EntradaSonido> mapaSonidos =
        new Dictionary<string, EntradaSonido>(StringComparer.OrdinalIgnoreCase);
    private readonly List<AudioSource> poolFuentes = new List<AudioSource>();
    private int indiceReemplazo;

    private void Awake()
    {
        InicializarFuentes();
        ReconstruirMapaSonidos(emitirAdvertencias: true);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (maxFuentesSimultaneas < 1)
            maxFuentesSimultaneas = 1;

        if (Application.isPlaying)
            ReconstruirMapaSonidos(emitirAdvertencias: false);
    }
#endif

    private void OnDestroy()
    {
        poolFuentes.Clear();
        mapaSonidos.Clear();
    }

    /// <summary>
    /// Reproduce el sonido asociado a <paramref name="nombreSonido"/> (ej. desde un Animation Event).
    /// Si <paramref name="nombreSonido"/> está vacío, reproduce el sonido por defecto o el recurso inicial del AudioSource.
    /// </summary>
    public void PlaySound(string nombreSonido = "")
    {
        if (fuenteBase == null)
        {
            InicializarFuentes();
            if (fuenteBase == null)
                return;
        }

        if (string.IsNullOrWhiteSpace(nombreSonido))
        {
            PlayDefaultSound();
            return;
        }

        string clave = nombreSonido.Trim();
        if (!mapaSonidos.TryGetValue(clave, out EntradaSonido entrada) || entrada == null)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"[AudioSourceTrigger] No existe un sonido registrado con la clave '{clave}' en '{gameObject.name}'.", this);
#endif
            return;
        }

        if (entrada.recurso == null)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"[AudioSourceTrigger] La entrada '{clave}' en '{gameObject.name}' no tiene un AudioResource asignado.", this);
#endif
            return;
        }

        ReproducirRecurso(entrada.recurso, entrada.volumen);
    }

    /// <summary>
    /// Reproduce explícitamente el sonido por defecto o el recurso inicial del AudioSource del GameObject.
    /// </summary>
    public void PlayDefaultSound()
    {
        if (fuenteBase == null)
        {
            InicializarFuentes();
            if (fuenteBase == null)
                return;
        }

        AudioResource recurso = recursoPorDefecto != null ? recursoPorDefecto : recursoInicialFuente;
        if (recurso == null)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"[AudioSourceTrigger] PlaySound se invocó sin clave en '{gameObject.name}', pero no hay Recurso Por Defecto ni AudioResource inicial en su AudioSource.", this);
#endif
            return;
        }

        ReproducirRecurso(recurso, 1f);
    }

    private void InicializarFuentes()
    {
        fuenteBase = GetComponent<AudioSource>();
        if (fuenteBase == null)
            return;

        recursoInicialFuente = fuenteBase.resource;
        volumenInicialFuente = fuenteBase.volume;

        poolFuentes.Clear();
        poolFuentes.Add(fuenteBase);
        indiceReemplazo = 0;
    }

    private void ReconstruirMapaSonidos(bool emitirAdvertencias)
    {
        mapaSonidos.Clear();
        if (sonidos == null)
            return;

        for (int i = 0; i < sonidos.Count; i++)
        {
            EntradaSonido entrada = sonidos[i];
            if (entrada == null || string.IsNullOrWhiteSpace(entrada.id))
                continue;

            string clave = entrada.id.Trim();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (emitirAdvertencias && mapaSonidos.ContainsKey(clave))
            {
                Debug.LogWarning($"[AudioSourceTrigger] Clave de sonido duplicada '{clave}' en '{gameObject.name}'. Se usará la última entrada configurada.", this);
            }
#endif
            mapaSonidos[clave] = entrada;
        }
    }

    private void ReproducirRecurso(AudioResource recurso, float multiplicadorVolumen)
    {
        AudioSource destino = ObtenerFuenteDisponible();
        if (destino == null)
            return;

        SincronizarAjustesDesdeBase(destino, multiplicadorVolumen);
        destino.resource = recurso;
        destino.Play();
    }

    private AudioSource ObtenerFuenteDisponible()
    {
        // Limpiar posibles referencias nulas y buscar una fuente libre
        for (int i = 0; i < poolFuentes.Count; i++)
        {
            AudioSource candidata = poolFuentes[i];
            if (candidata != null && !candidata.isPlaying)
                return candidata;
        }

        // Si todas están ocupadas y aún no llegamos al tope, crear una fuente auxiliar
        int limite = Mathf.Max(1, maxFuentesSimultaneas);
        if (poolFuentes.Count < limite)
        {
            AudioSource nuevaFuente = gameObject.AddComponent<AudioSource>();
            nuevaFuente.hideFlags = HideFlags.DontSave;
            nuevaFuente.playOnAwake = false;
            nuevaFuente.loop = false;
            poolFuentes.Add(nuevaFuente);
            return nuevaFuente;
        }

        // Alcanzado el límite de polifonía: reciclar en orden circular sin crear nuevos componentes
        if (poolFuentes.Count == 0)
            return fuenteBase;

        indiceReemplazo = (indiceReemplazo + 1) % poolFuentes.Count;
        AudioSource reciclada = poolFuentes[indiceReemplazo];
        return reciclada != null ? reciclada : fuenteBase;
    }

    private void SincronizarAjustesDesdeBase(AudioSource destino, float multiplicadorVolumen)
    {
        if (destino != fuenteBase)
        {
            destino.outputAudioMixerGroup = fuenteBase.outputAudioMixerGroup;
            destino.mute = fuenteBase.mute;
            destino.bypassEffects = fuenteBase.bypassEffects;
            destino.bypassListenerEffects = fuenteBase.bypassListenerEffects;
            destino.bypassReverbZones = fuenteBase.bypassReverbZones;
            destino.priority = fuenteBase.priority;
            destino.pitch = fuenteBase.pitch;
            destino.panStereo = fuenteBase.panStereo;
            destino.spatialBlend = fuenteBase.spatialBlend;
            destino.reverbZoneMix = fuenteBase.reverbZoneMix;
            destino.dopplerLevel = fuenteBase.dopplerLevel;
            destino.spread = fuenteBase.spread;
            destino.rolloffMode = fuenteBase.rolloffMode;
            destino.minDistance = fuenteBase.minDistance;
            destino.maxDistance = fuenteBase.maxDistance;
            destino.spatialize = fuenteBase.spatialize;
            destino.spatializePostEffects = fuenteBase.spatializePostEffects;
            destino.ignoreListenerVolume = fuenteBase.ignoreListenerVolume;
            destino.ignoreListenerPause = fuenteBase.ignoreListenerPause;

            if (fuenteBase.rolloffMode == AudioRolloffMode.Custom)
            {
                destino.SetCustomCurve(
                    AudioSourceCurveType.CustomRolloff,
                    fuenteBase.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            }
        }

        destino.loop = false;
        destino.volume = Mathf.Clamp01(volumenInicialFuente * Mathf.Max(0f, multiplicadorVolumen));
    }
}