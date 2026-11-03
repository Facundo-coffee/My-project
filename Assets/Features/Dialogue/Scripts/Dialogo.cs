using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private TMP_Text texto;
    [SerializeField] private List<string> dialogos = new List<string>();
    [SerializeField, Min(0.1f)] private float caracteresPorSegundo = 30f;
    [SerializeField] private bool salteable = true;
    [SerializeField] private UnityEvent dialogoFinalizado = new UnityEvent();
    [Tooltip("Activa el modo de depuración para ver mensajes en la consola.")]
    [SerializeField] private bool debugMode = false;

    private int indiceActual;
    private int totalCaracteres;
    private float caracteresMostrados;
    private bool escribiendo;
    private bool finalizado;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions?.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions?.UI.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void Start()
    {
        if (texto == null || dialogos == null || dialogos.Count == 0)
        {
            FinalizarDialogo();
            return;
        }

        texto.text = string.Empty;
        MostrarLinea(0);
    }

    private void Update()
    {
        if (finalizado)
        {
            return;
        }

        bool clic = inputActions.UI.Click.WasPressedThisFrame() ||
                    inputActions.UI.RightClick.WasPressedThisFrame() ||
                    inputActions.UI.MiddleClick.WasPressedThisFrame();

        if (clic)
        {
            if (escribiendo && salteable)
            {
                CompletarEscritura();
            }

            return;
        }

        if (inputActions.UI.Submit.WasPressedThisFrame())
        {
            AvanzarLinea();
            return;
        }

        if (!escribiendo)
        {
            return;
        }

        caracteresMostrados += Mathf.Max(0.1f, caracteresPorSegundo) * Time.deltaTime;
        int visibles = Mathf.Min(Mathf.FloorToInt(caracteresMostrados), totalCaracteres);
        texto.maxVisibleCharacters = visibles;

        if (visibles >= totalCaracteres)
        {
            escribiendo = false;
        }
    }

    private void MostrarLinea(int indice)
    {
        indiceActual = indice;
        texto.text = dialogos[indiceActual] ?? string.Empty;
        texto.ForceMeshUpdate();
        totalCaracteres = texto.textInfo.characterCount;
        caracteresMostrados = 0f;
        texto.maxVisibleCharacters = 0;
        escribiendo = totalCaracteres > 0;
    }

    private void AvanzarLinea()
    {
        if (indiceActual + 1 >= dialogos.Count)
        {
            FinalizarDialogo();
            return;
        }

        MostrarLinea(indiceActual + 1);
    }

    private void CompletarEscritura()
    {
        texto.maxVisibleCharacters = totalCaracteres;
        escribiendo = false;
    }

    private void FinalizarDialogo()
    {
        finalizado = true;
        escribiendo = false;

        if (debugMode)
        {
            Debug.Log("Diálogo finalizado. ");
            Debug.Log($"Invocando evento: {dialogoFinalizado?.ToString()}");
        }

        dialogoFinalizado?.Invoke();
    }
}
