using System;
using UnityEngine;

public class AnimacionJugador : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Movimiento2D movimiento;
    [SerializeField] private CombateJugador combate;
    [SerializeField] private Animator animator;

    [Header("Parámetros de Animator")]
    [SerializeField] private string parametroEstaCaminando = "EstaCaminando";
    [SerializeField] private string triggerAtaque = "Ataque";

    private IDisposable bloqueoMovimiento;

    private void OnEnable()
    {
        if (movimiento != null)
        {
            movimiento.EstadoCaminandoCambiado += ActualizarEstadoCaminando;
            ActualizarEstadoCaminando(movimiento.EstaCaminando);
        }

        if (combate != null)
        {
            combate.AtaqueIniciado += ActivarTriggerAtaque;
        }
    }

    private void OnDisable()
    {
        if (movimiento != null)
        {
            movimiento.EstadoCaminandoCambiado -= ActualizarEstadoCaminando;
        }

        if (combate != null)
        {
            combate.AtaqueIniciado -= ActivarTriggerAtaque;
        }

        FinalizarAtaque();
    }

    private void ActualizarEstadoCaminando(bool estaCaminando)
    {
        if (animator != null && !string.IsNullOrEmpty(parametroEstaCaminando))
        {
            animator.SetBool(parametroEstaCaminando, estaCaminando);
        }
    }

    private void ActivarTriggerAtaque()
    {
        if (animator != null && !string.IsNullOrEmpty(triggerAtaque))
        {
            if (bloqueoMovimiento == null && movimiento != null)
            {
                bloqueoMovimiento = movimiento.SolicitarBloqueoMovimiento();
            }

            animator.SetTrigger(triggerAtaque);
        }
    }

    public void FinalizarAtaque()
    {
        bloqueoMovimiento?.Dispose();
        bloqueoMovimiento = null;
    }
}
