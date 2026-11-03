# SDD Workflow: Juancito de Adrogué

Guía práctica del flujo Spec-Driven Development (SDD) para el desarrollo asistido por IA en el proyecto *Juancito de Adrogué*.

## Pipeline Central

```text
IDEA -> PLAN -> REQUIREMENTS -> DESIGN -> TASKS -> IMPLEMENTATION -> REVIEW
```

1. **IDEA (`/sdd-idea`):** Exploración y lluvia de ideas sin compromiso técnico vinculante.
2. **PLAN (`/sdd-plan`):** Mapa de ruta (roadmap), definición de fases (MVP vs Essentials) y límites de las características.
3. **REQUIREMENTS (`/sdd-prd`):** Contrato funcional (QUÉ y POR QUÉ). Historias de usuario, criterios de aceptación, requerimientos funcionales EARS y no funcionales (NFRs).
4. **DESIGN (`/sdd-spec`):** Contrato técnico (CÓMO). Arquitectura, diagramas de flujo, componentes Unity, estado, física y decisiones técnicas (ADRs).
5. **TASKS (`/sdd-tasks`):** Desglose en tareas atómicas con estimaciones, criterios de aceptación, archivos involucrados y verificación.
6. **IMPLEMENTATION (`/sdd-exec`):** Escritura y modificación de código C#, assets, prefabs y escenas según las tareas aprobadas.
7. **REVIEW (`/sdd-review`):** Auditoría y verificación contra requerimientos, diseño y tareas.

## Reglas de Compuertas (Gate Rules)

- **No escribir código antes de que las tareas estén aprobadas (`tasks:approved`).**
- **No redactar diseño vinculante antes de que los requisitos estén aprobados (`requirements:approved`).**
- **No redactar tareas vinculantes antes de que el diseño esté aprobado (`design:approved`).**
- Cada directorio de feature en `.ai/sdd/specs/NNN-feature-name/` debe contener su archivo `.status`.

## Estados Válidos de `.status`

- `idea:exploring` / `idea:captured`
- `plan:draft` / `plan:approved`
- `requirements:draft` / `requirements:approved`
- `design:draft` / `design:approved`
- `tasks:draft` / `tasks:approved`
- `implementation:in-progress` / `implementation:done`
- `review:done`

## Estructura Canónica de Directorios

```text
.ai/
  steering/
    product.md       # Visión, público, límites y glosario
    tech-stack.md    # Unity 6, URP, Input System, C#
    conventions.md   # Estilo C#, arquitectura, tags, capas
    principles.md    # Reglas MUST/SHOULD de gobernanza
  sdd/
    INDEX.md         # Tablero general
    WORKFLOW.md      # Esta guía
    PLAN.md          # Plan y mapa de features del juego
    handoff/         # Handoffs consolidados para downstream
    specs/           # Especificaciones detalladas por feature
      001-feature-slug/
        .status
        requirements.md
        design.md
        tasks.md
        review.md
```

