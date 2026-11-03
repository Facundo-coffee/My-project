using UnityEngine;

public class BloqueoZonaCombate : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Zona de combate cuyo evento controlará la activación y desactivación de los bloqueos.")]
    [SerializeField] private ZonaCombate zonaCombate;

    [Header("Colliders de Bloqueo")]
    [Tooltip("Colliders físicos (no triggers) que impiden al jugador salir de la zona durante el combate.")]
    [SerializeField] private Collider2D[] collidersBloqueo;

    [Tooltip("Objetos adicionales (por ejemplo barreras visuales con collider) que se activarán durante el combate.")]
    [SerializeField] private GameObject[] objetosBloqueo;

    private void Awake()
    {
        if (zonaCombate == null)
        {
            zonaCombate = GetComponent<ZonaCombate>();
            if (zonaCombate == null)
            {
                zonaCombate = GetComponentInParent<ZonaCombate>();
            }
        }

        ConfigurarCollidersFisicos();
        ActualizarBloqueo(zonaCombate != null && zonaCombate.EnCombate);
    }

    private void OnEnable()
    {
        if (zonaCombate != null)
        {
            zonaCombate.EstadoCombateCambiado += ActualizarBloqueo;
            ActualizarBloqueo(zonaCombate.EnCombate);
        }
    }

    private void OnDisable()
    {
        if (zonaCombate != null)
        {
            zonaCombate.EstadoCombateCambiado -= ActualizarBloqueo;
        }
    }

    /// <summary>
    /// Activa o desactiva los colliders y objetos de bloqueo de movimiento del jugador.
    /// </summary>
    public void ActualizarBloqueo(bool bloquear)
    {
        if (collidersBloqueo != null)
        {
            for (int i = 0; i < collidersBloqueo.Length; i++)
            {
                if (collidersBloqueo[i] != null)
                {
                    collidersBloqueo[i].enabled = bloquear;
                }
            }
        }

        if (objetosBloqueo != null)
        {
            for (int i = 0; i < objetosBloqueo.Length; i++)
            {
                if (objetosBloqueo[i] != null)
                {
                    objetosBloqueo[i].SetActive(bloquear);
                }
            }
        }
    }

    public void ActivarBloqueo() => ActualizarBloqueo(true);

    public void DesactivarBloqueo() => ActualizarBloqueo(false);

    private void ConfigurarCollidersFisicos()
    {
        if (collidersBloqueo == null) return;

        for (int i = 0; i < collidersBloqueo.Length; i++)
        {
            if (collidersBloqueo[i] != null)
            {
                collidersBloqueo[i].isTrigger = false;
            }
        }
    }
}

