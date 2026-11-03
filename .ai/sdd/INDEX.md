# SDD Index: Juancito de Adrogué

## Upstream Handoffs

- `.ai/strategy/handoff/strategy-brief.md`: No aplicable (iniciado desde GDD High Concept y repositorio local existente).

## Plan

- `.ai/sdd/PLAN.md`: `plan:approved` (Plan de Producto y Mapeo de Features de Juancito de Adrogué).

## Ideas

| ID | Name | Status | Path |
|----|------|--------|------|
| 001 | Arquitectura Feature Folders (Vertical Slices) | idea:captured | .ai/sdd/ideas/001-vertical-slice-folders.md |

## Feature Workspace

> Fuente de numeración: directorios reales en `.ai/sdd/specs/`, no este índice.

| Campo | Valor | Notas |
|-------|-------|-------|
| Próximo Feature ID | 003 | Recalcular desde el sistema de archivos antes de crear una nueva spec |
| Problemas de Numeración | Ninguno | Sin prefijos duplicados ni estados inválidos |

## Specs

| ID | Feature | Status | Requirements | Design | Tasks | Review |
|----|---------|--------|--------------|--------|-------|--------|
| 001 | Reestructuración a Feature Folders | implementation:done | .ai/sdd/specs/001-reestructuracion-feature-folders/requirements.md | .ai/sdd/specs/001-reestructuracion-feature-folders/design.md | .ai/sdd/specs/001-reestructuracion-feature-folders/tasks.md | - |
| 002 | Disparador de Audio Multisonido (`AudioSourceTrigger`) | implementation:done | .ai/sdd/specs/002-audio-trigger-multisonido/requirements.md | .ai/sdd/specs/002-audio-trigger-multisonido/design.md | .ai/sdd/specs/002-audio-trigger-multisonido/tasks.md | - |

## Handoff

- `.ai/sdd/handoff/sdd-brief.md`: Actualizado tras completar tareas de implementación.

## Próximas Acciones

- [x] Ejecutar la reestructuración completa a Feature Folders y Shared Core (`/sdd-exec`).
- [ ] Ejecutar revisión formal de la implementación con `/sdd-review`.
