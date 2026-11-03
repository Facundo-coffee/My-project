using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class TransicionFade : TransicionEscena
{
    [SerializeField] private string triggerFadeOut = "Fade_Out";
    [SerializeField] private string estadoFadeOut = "Base Layer.Fade_Out";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override IEnumerator EjecutarSalida()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (string.IsNullOrWhiteSpace(triggerFadeOut) ||
            string.IsNullOrWhiteSpace(estadoFadeOut) ||
            !animator.HasState(0, Animator.StringToHash(estadoFadeOut)) ||
            !TieneTriggerFadeOut())
        {
            Debug.LogError("Configura el trigger y el estado Fade_Out en el Animator.", this);
            yield break;
        }

        animator.ResetTrigger(triggerFadeOut);
        animator.SetTrigger(triggerFadeOut);
        yield return null;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(estadoFadeOut))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(estadoFadeOut) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }

    private bool TieneTriggerFadeOut()
    {
        int triggerHash = Animator.StringToHash(triggerFadeOut);

        foreach (AnimatorControllerParameter parametro in animator.parameters)
        {
            if (parametro.nameHash == triggerHash && parametro.type == AnimatorControllerParameterType.Trigger)
            {
                return true;
            }
        }

        return false;
    }
}
