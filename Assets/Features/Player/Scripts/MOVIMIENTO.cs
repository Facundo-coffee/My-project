using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Importante para el nuevo Input System

public class Movimiento2D : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 5f;

    public event Action<bool> EstadoCaminandoCambiado;
    public bool EstaCaminando { get; private set; }

    private readonly HashSet<object> bloqueosMovimiento = new HashSet<object>();

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private Vector2 entradaMovimiento;
    private bool mirandoDerecha = true;
    private InputSystem_Actions inputActions;
    private InputAction moveAction;

    public IDisposable SolicitarBloqueoMovimiento()
    {
        object identificador = new object();
        bloqueosMovimiento.Add(identificador);
        ActualizarMovimiento();
        return new BloqueoMovimiento(this, identificador);
    }

    private void LiberarBloqueoMovimiento(object identificador)
    {
        if (bloqueosMovimiento.Remove(identificador))
        {
            ActualizarMovimiento();
        }
    }

    private sealed class BloqueoMovimiento : IDisposable
    {
        private Movimiento2D movimiento;
        private readonly object identificador;

        public BloqueoMovimiento(Movimiento2D movimiento, object identificador)
        {
            this.movimiento = movimiento;
            this.identificador = identificador;
        }

        public void Dispose()
        {
            if (movimiento == null) return;

            movimiento.LiberarBloqueoMovimiento(identificador);
            movimiento = null;
        }
    }

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        moveAction = inputActions.Player.Move;
    }

    void OnEnable()
    {
        inputActions?.Player.Enable();
    }

    void OnDisable()
    {
        inputActions?.Player.Disable();
    }

    void OnDestroy()
    {
        inputActions?.Dispose();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        entradaMovimiento = moveAction.ReadValue<Vector2>();
        ActualizarMovimiento();

        // Comprobación de orientación
        if (bloqueosMovimiento.Count == 0 && entradaMovimiento.x > 0 && !mirandoDerecha)
        {
            Voltear();
        }
        else if (bloqueosMovimiento.Count == 0 && entradaMovimiento.x < 0 && mirandoDerecha)
        {
            Voltear();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento * velocidad * Time.fixedDeltaTime);
    }

    private void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1; // Invierte el eje X
        transform.localScale = escala;
    }

    private void ActualizarEstadoCaminando(bool estaCaminando)
    {
        if (EstaCaminando == estaCaminando) return;

        EstaCaminando = estaCaminando;
        EstadoCaminandoCambiado?.Invoke(estaCaminando);
    }

    private void ActualizarMovimiento()
    {
        bool movimientoBloqueado = bloqueosMovimiento.Count > 0;
        movimiento = movimientoBloqueado ? Vector2.zero : entradaMovimiento.normalized;
        ActualizarEstadoCaminando(!movimientoBloqueado && entradaMovimiento.sqrMagnitude > 0f);
    }
}