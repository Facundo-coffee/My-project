using System.Collections;
using UnityEngine;

public abstract class TransicionEscena : MonoBehaviour
{
    public abstract IEnumerator EjecutarSalida();
}
