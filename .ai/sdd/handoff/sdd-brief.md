# SDD Handoff Brief: Disparador de Audio Multisonido en `AudioSourceTrigger`

> Status: Approved
> Readiness: Ready for release
> Updated: 2026-10-27

## Metadata

- **Spec ID:** `002-audio-trigger-multisonido`
- **Spec Path:** `.ai/sdd/specs/002-audio-trigger-multisonido/`
- **Current .status:** `review:done`
- **Source Inputs:** `.ai/sdd/PLAN.md` (`F05`), `.ai/sdd/specs/002-audio-trigger-multisonido/requirements.md`, `.ai/sdd/specs/002-audio-trigger-multisonido/design.md`, `.ai/sdd/specs/002-audio-trigger-multisonido/tasks.md`

## Product / Feature Summary

- **User / Audience:** Diseñador/animador en Unity Editor y jugador de *Juancito de Adrogué*.
- **Problem:** `AudioSourceTrigger` solo permitía reproducir el único recurso asignado en el `AudioSource` de `Juancito`, impidiendo disparar contenedores distintos (ej. golpes y pasos) desde `PlaySound` sin pisarse entre sí.
- **Outcome:** `Juancito` puede disparar múltiples contenedores (`AudioResource`) por nombre desde eventos `PlaySound` superponiéndose sin cortes y manteniendo compatibilidad con animaciones existentes.
- **Scope:** Evolución del componente `Assets/Scripts/Audio/AudioSourceTrigger.cs` listo para configurar en el Inspector.
- **Out of Scope:** Extracción manual de `.zip`, creación del contenedor de pasos y edición manual de clips `.anim` en Unity Editor; cambios en `AudioManager`.

## Requirements Summary

- **Key User Stories:** `US-001`, `US-002`, `US-003`
- **Must Have Functional Requirements:** `FR-001`, `FR-002`, `FR-003`, `FR-004` (y `FR-005` Should Have)
- **Important NFRs:** `NFR-001` (Inspector claro en español), `NFR-002` (pool acotado sin fugas), `NFR-003` (preservación de `.meta`/GUID)
- **Acceptance Notes:** Eventos `PlaySound` vacíos siguen disparando el recurso por defecto o inicial de `Juancito`; claves como `"pasos"` / `"golpes"` no distinguen mayúsculas ni espacios extremos.

## Design Summary

- **Approach:** Catálogo serializado `List<EntradaSonido>` indexado en `Dictionary<string, EntradaSonido>` (`OrdinalIgnoreCase`) + mini-pool acotado de `AudioSource` en el mismo GameObject heredando ajustes de `fuenteBase`.
- **Components / Modules:** `AudioSourceTrigger` y clase anidada serializable `EntradaSonido`.
- **Data / State:** `recursoPorDefecto`, `sonidos`, `maxFuentesSimultaneas`, `recursoInicialFuente`, `volumenInicialFuente`, `poolFuentes`.
- **APIs / Integrations:** `public void PlaySound(string nombreSonido = "")` y `public void PlayDefaultSound()`.
- **Technical Decisions:** `TD-001` (`AudioResource`), `TD-002` (firma única `PlaySound(string)`), `TD-003` (mini-pool acotado de `AudioSource`).
- **Risks / Constraints:** Preservar GUID `221b63f03b983bf48b39758f3672b7c6` y cachear recurso/volumen inicial de `fuenteBase`.

## Implementation Plan

- **Task Source:** `.ai/sdd/specs/002-audio-trigger-multisonido/tasks.md`
- **Recommended Order:** `T1`
- **Key Tasks:** `T1`
- **Likely Files / Areas:** `Assets/Scripts/Audio/AudioSourceTrigger.cs`

## Verification Plan

```text
Command: dotnet build Assembly-CSharp.csproj -nologo
Expected: Build succeeded, 0 Error(s), Exit code 0; GUID 221b63f03b983bf48b39758f3672b7c6 intact in AudioSourceTrigger.cs.meta
```

## Review / Release Notes

- **Review Artifact:** `.ai/sdd/specs/002-audio-trigger-multisonido/review.md`
- **Review Verdict:** Approved
- **Known Follow-ups:** Si se ejecuta más adelante la spec `001-reestructuracion-feature-folders`, mover `Assets/Scripts/Audio/AudioSourceTrigger.cs` junto con su `.meta` a `Assets/_Core/Audio/`.

## Handoff Readiness

- **Ready for Implementation:** yes
- **Ready for QA:** yes
- **Ready for Release:** yes
- **Blockers:** N/A
- **Recommended Next Action:** Feature `002-audio-trigger-multisonido` cerrada (`review:done`). Lista para hacer commit en Git.
