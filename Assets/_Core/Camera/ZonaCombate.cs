using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ZonaCombate : MonoBehaviour
{
    [Header("Referencias de Cámara")]
    [Tooltip("Administrador de cámaras de Cinemachine. Si se deja vacío, se buscará automáticamente en la escena.")]
    [SerializeField] private DirectorCamaras directorCamaras;

    [Tooltip("Cámara estática de combate correspondiente a esta zona.")]
    [SerializeField] private CinemachineCamera camaraCombate;

    [Header("Configuración del Trigger")]
    [SerializeField] private string tagJugador = "Player";
    [Tooltip("Si está activo, la zona solo se activará una vez tras completarse.")]
    [SerializeField] private bool activarUnaSolaVez = true;

    [Header("Enemigos de la Zona")]
    [Tooltip("Lista de enemigos que deben ser derrotados para finalizar el combate automáticamente.")]
    [SerializeField] private List<SaludEnemigo> enemigosZona = new List<SaludEnemigo>();
    [SerializeField] private bool finalizarAlMorirTodos = true;

    [Header("Eventos (Inspector)")]
    [Tooltip("Se dispara con 'true' al iniciar el combate y con 'false' al finalizar.")]
    [SerializeField] private UnityEvent<bool> alCambiarEstadoCombate = new UnityEvent<bool>();
    [SerializeField] private UnityEvent alIniciarCombate = new UnityEvent();
    [SerializeField] private UnityEvent alFinalizarCombate = new UnityEvent();

    /// <summary>
    /// Evento C# al que se suscriben el bloqueo de colliders y otros sistemas.
    /// Recibe true cuando inicia el combate y false cuando termina.
    /// </summary>
    public event Action<bool> EstadoCombateCambiado;
    public event Action CombateIniciado;
    public event Action CombateFinalizado;

    public bool EnCombate { get; private set; }
    public bool CombateCompletado { get; private set; }

    private BoxCollider2D triggerZona;

    private void Reset()
    {
        ConfigurarTrigger();
    }

    private void Awake()
    {
        ConfigurarTrigger();

        if (directorCamaras == null)
        {
            directorCamaras = FindAnyObjectByType<DirectorCamaras>();
        }
    }

    private void OnEnable()
    {
        SuscribirEnemigos();
        EstadoCombateCambiado += NotificarDirectorCamaras;
    }

    private void OnDisable()
    {
        DesuscribirEnemigos();
        EstadoCombateCambiado -= NotificarDirectorCamaras;
    }

    private void Update()
    {
        if (!EnCombate || !finalizarAlMorirTodos || enemigosZona.Count == 0)
        {
            return;
        }

        if (TodosLosEnemigosDerrotados())
        {
            FinalizarCombate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (EnCombate) return;
        if (activarUnaSolaVez && CombateCompletado) return;
        if (!other.CompareTag(tagJugador)) return;

        IniciarCombate();
    }

    /// <summary>
    /// Inicia el combate, dispara el evento de bloqueo y transiciona a la cámara estática de combate.
    /// </summary>
    public void IniciarCombate()
    {
        if (EnCombate) return;
        if (activarUnaSolaVez && CombateCompletado) return;

        EnCombate = true;

        if (activarUnaSolaVez && triggerZona != null)
        {
            triggerZona.enabled = false;
        }

        EstadoCombateCambiado?.Invoke(true);
        CombateIniciado?.Invoke();

        alCambiarEstadoCombate?.Invoke(true);
        alIniciarCombate?.Invoke();

        if (finalizarAlMorirTodos && enemigosZona.Count > 0 && TodosLosEnemigosDerrotados())
        {
            FinalizarCombate();
        }
    }

    /// <summary>
    /// Finaliza el combate, desactiva los colliders de bloqueo y vuelve a la cámara principal.
    /// Puede llamarse manualmente o de forma automática al morir todos los enemigos de la zona.
    /// </summary>
    public void FinalizarCombate()
    {
        if (!EnCombate) return;

        EnCombate = false;
        CombateCompletado = true;

        EstadoCombateCambiado?.Invoke(false);
        CombateFinalizado?.Invoke();

        alCambiarEstadoCombate?.Invoke(false);
        alFinalizarCombate?.Invoke();
    }

    public void RegistrarEnemigo(SaludEnemigo enemigo)
    {
        if (enemigo == null || enemigosZona.Contains(enemigo)) return;

        enemigosZona.Add(enemigo);
        enemigo.AlMorir += ManejarMuerteEnemigo;
    }

    private void ConfigurarTrigger()
    {
        triggerZona = GetComponent<BoxCollider2D>();
        if (triggerZona != null)
        {
            triggerZona.isTrigger = true;
        }
    }

    private void NotificarDirectorCamaras(bool enCombate)
    {
        if (directorCamaras == null) return;

        if (enCombate)
        {
            directorCamaras.ActivarCamaraCombate(camaraCombate);
        }
        else
        {
            directorCamaras.VolverACamaraPrincipal();
        }
    }

    private void SuscribirEnemigos()
    {
        for (int i = 0; i < enemigosZona.Count; i++)
        {
            SaludEnemigo enemigo = enemigosZona[i];
            if (enemigo != null)
            {
                enemigo.AlMorir -= ManejarMuerteEnemigo;
                enemigo.AlMorir += ManejarMuerteEnemigo;
            }
        }
    }

    private void DesuscribirEnemigos()
    {
        for (int i = 0; i < enemigosZona.Count; i++)
        {
            SaludEnemigo enemigo = enemigosZona[i];
            if (enemigo != null)
            {
                enemigo.AlMorir -= ManejarMuerteEnemigo;
            }
        }
    }

    private void ManejarMuerteEnemigo(SaludEnemigo enemigoDerrotado)
    {
        if (enemigoDerrotado != null)
        {
            enemigoDerrotado.AlMorir -= ManejarMuerteEnemigo;
        }

        if (EnCombate && finalizarAlMorirTodos && TodosLosEnemigosDerrotados())
        {
            FinalizarCombate();
        }
    }

    private bool TodosLosEnemigosDerrotados()
    {
        for (int i = 0; i < enemigosZona.Count; i++)
        {
            SaludEnemigo enemigo = enemigosZona[i];
            if (enemigo != null && !enemigo.EstaMuerto)
            {
                return false;
            }
        }

        return true;
    }
}
