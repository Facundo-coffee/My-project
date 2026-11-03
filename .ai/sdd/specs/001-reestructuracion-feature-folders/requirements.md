# Feature: Reestructuración a Feature Folders (Vertical Slices) y Shared Core

> Status: Approved
> Source: @.ai/sdd/ideas/001-vertical-slice-folders.md, @.ai/sdd/PLAN.md

## Overview

Reorganizar la estructura de carpetas de Unity desde el modelo tradicional horizontal ("Package by Layer": `Scripts/`, `Prefab/`, `Arte/`, `Musica/`) hacia una arquitectura basada en características verticales ("Vertical Slices / Feature Folders") complementada con un núcleo compartido (`Assets/_Core/`).

Esta estructura permite que cada mecánica (Juancito, Enemigos, Diálogos, Curativos) contenga sus scripts, prefabs y assets directos en una única carpeta aislada, haciendo trivial su modificación o eliminación sin riesgo de dejar archivos huérfanos ni generar cascadas de errores.

## Business Context

- **Problema:** En el desarrollo indie/rápido de *Juancito de Adrogué*, experimentar con mecánicas o descartar enemigos provisionales hoy requiere buscar y borrar manualmente archivos en 5 carpetas distintas (`Scripts`, `Prefab`, `Juancito`, `Arte`, `Sonidos`), arriesgando referencias rotas.
- **Valor:** Máxima velocidad de iteración, claridad mental en la estructura del proyecto y desacoplamiento limpio de cara a la producción del MVP.
- **Restricción crítica:** Mantener al 100% la integridad de los GUIDs de Unity (`.meta`) para que ninguna escena existente (`MainMenu`, `Nivel_01`, `Exterior`, `Interiior de CastleForte`) ni prefab (`Juancito.prefab`) sufra de `Missing Script` o enlaces desvinculados.

## User Stories

### US-001: Aislamiento y Eliminación Segura de Features
**As a** desarrollador de gameplay  
**I want to** tener todo lo relacionado a una feature (ej. `Player`, `Enemies/Murcielago`, `Dialogue`) encapsulado en su propia carpeta  
**So that** pueda modificar, versionar o eliminar la feature completa simplemente gestionando esa carpeta, sin romper el resto del juego.

**Acceptance Criteria:**
- [ ] La carpeta `Assets/Features/Player/` contiene de forma autosuficiente los scripts, prefabs y animaciones de Juancito.
- [ ] La eliminación hipotética de una carpeta de feature experimental no genera errores de compilación en `_Core/` ni en otras features no acopladas.

### US-002: Localización Rápida de Assets
**As a** diseñador/programador del equipo  
**I want to** ver la jerarquía del proyecto organizada por capacidad jugable  
**So that** no tenga que saltar entre carpetas raíz técnicas para editar un componente y su prefab asociado.

**Acceptance Criteria:**
- [ ] Al seleccionar la feature en la ventana Project de Unity, todos sus elementos dependientes están contenidos en esa rama de directorio.

### US-003: Preservación Absoluta de Referencias en Unity
**As a** mantenedor del proyecto  
**I want to** que la migración preserve intactos los GUIDs de metadatos (`.meta`)  
**So that** todas las escenas existentes carguen en Unity PlayMode sin advertencias de `Missing Component` o referencias nulas.

**Acceptance Criteria:**
- [ ] La escena `MainMenu` y `Nivel_01` abren y ejecutan sin excepciones en la consola de Unity.
- [ ] El prefab `Juancito.prefab` conserva todas sus asignaciones serializadas (velocidad, referencias a AudioSource, colliders, hitboxes).

## Functional Requirements

### FR-001: Creación de Jerarquía Raíz Estandarizada — Must Have
THE SYSTEM SHALL estructurar `Assets/` bajo dos pilares claros:
1. `Assets/_Core/`: Sistemas transversales indispensables compartidos por todo el juego.
2. `Assets/Features/`: Módulos verticales autocontenidos por funcionalidad.

### FR-002: Encapsulamiento del Slice de Jugador (`Features/Player`) — Must Have
WHEN se complete la migración  
THE SYSTEM SHALL agrupar en `Assets/Features/Player/`:
- Scripts del jugador (`MOVIMIENTO.cs`, `CombateJugador.cs`, `SaludJugador.cs`, `AnimacionJugador.cs`).
- Prefabs (`Juancito.prefab`).
- Animaciones y archivos `.ase` / `.aseprite` exclusivos de Juancito (actualmente en `Assets/Juancito/`).
SO THAT toda la entidad del jugador resida en un único paquete vertical.

### FR-003: Centralización de Sistemas Core (`Assets/_Core/`) — Must Have
THE SYSTEM SHALL reubicar en `Assets/_Core/` los componentes comunes:
- `_Core/Audio/`: `AudioManager.cs`, mixers y disparadores (`AudioSourceTrigger.cs`).
- `_Core/Camera/`: Scripts de Cinemachine y zonas de combate (`DirectorCamaras.cs`, `ZonaCombate.cs`, `BloqueoZonaCombate.cs`).
- `_Core/SceneManagement/`: Administradores y transiciones de escenas (`AdministradorEscenas.cs`, `TransicionEscena.cs`, `TransicionFade.cs`).

### FR-004: Modularización de Enemigos, Curativos y Diálogos — Must Have
THE SYSTEM SHALL agrupar:
- `Assets/Features/Enemies/`: `EnemigoBasico.cs`, `SaludEnemigo.cs`, y sprites/prefabs asociados.
- `Assets/Features/Pickups/`: `CurativoPickup.cs` y prefabs de Mate/Medialuna.
- `Assets/Features/Dialogue/`: `Dialogo.cs` y prefabs/layouts de interfaz conversacional.

### FR-005: Preservación de Metadatos y Trazabilidad Git — Must Have
WHEN se mueva cualquier archivo `.cs`, `.prefab`, `.anim` o `.ase`  
THE SYSTEM SHALL mover de manera simultánea e idéntica su archivo `.meta` asociado  
SO THAT Unity no regenere GUIDs ni desvincule referencias en la base de datos de assets.

## Non-Functional Requirements

### NFR-001: Mantenibilidad y Desacople
- Las features en `Assets/Features/` pueden depender de `Assets/_Core/`, pero ninguna feature debe referenciar directamente código interno privado de otra feature sin pasar por abstracciones, eventos o interfaces compartidas en `_Core/`.

### NFR-002: Cero Errores en Tiempo de Compilación
- La solución `Assembly-CSharp` debe compilar limpiamente tras la migración sin un solo error de compilación (`CS0246`, `CS0103`, etc.).

### NFR-003: Compatibilidad de Assets de Arte Genéricos
- Los fondos, tilesets y arte común de escenarios se mantendrán ordenados en `Assets/Arte/` (o subcarpeta de entorno compartida) evitando duplicaciones innecesarias de texturas pesadas.

## Out of Scope

- **No introducción de `.asmdef` (Assembly Definitions):** Para no forzar una refactorización de código en C# en este primer paso, se mantendrá la compilación estándar en `Assembly-CSharp`.
- **No modificación de lógica o algoritmos:** Esta feature es estrictamente una reorganización arquitectónica de carpetas y metadatos; no debe alterar la lógica matemática de combate, daño ni física.
- **No eliminación de código funcional:** Ningún script en uso debe ser borrado durante este proceso (únicamente scripts vacíos no utilizados como `NewEmptyCSharpScript.cs` podrán ser depurados previa verificación).

## Decisions

### D-001: Adopción de Vertical Slices + Shared Core
**Decision:** Organizar en `Assets/Features/<Feature>/` y `Assets/_Core/`.  
**Reason:** Ofrece los beneficios de Clean Architecture y eliminación segura de módulos sin la sobrecarga inicial de Assembly Definitions.  
**Source:** Q-001 de exploración en Idea 001.  
**Impacts:** FR-001, FR-002, FR-003, FR-004.

### D-002: Conservación de Arte de Entorno en Directorio Compartido
**Decision:** Mantener los tilesets y fondos de escenarios en `Assets/Arte/` (o `Assets/Shared/Arte/`) en vez de fragmentar un mismo tileset en varias carpetas.  
**Reason:** Los tilesets son compartidos entre múltiples niveles y no pertenecen exclusivamente a una única feature.  
**Source:** Directiva de integridad de arte de pixel art en `conventions.md`.  
**Impacts:** NFR-003.

### D-003: Migración Completa Atómica
**Decision:** Ejecutar la reestructuración de todos los módulos del proyecto en una sola operación atómica coordinada.  
**Reason:** El tamaño actual del proyecto es compacto (15 scripts clave) y una ejecución atómica garantiza un árbol de archivos completamente limpio de inmediato sin estados híbridos intermedios.  
**Source:** Q-001.  
**Impacts:** FR-001, FR-002, FR-003, FR-004, FR-005.

## Questions

### Q-001: Estrategia de Ejecución de la Migración
**Status:** answered  
**Why it matters:** Determina si migramos todos los módulos en una única operación atómica limpia o si realizamos un pase gradual empezando por `Player` y `_Core`.  
**Recommended:** Opción A (Migración completa atómica).  
**Resolution:** Opción A seleccionada por el usuario (ver D-003).

| Option | Answer | Choose this if... | Impact |
|--------|--------|-------------------|--------|
| A | Migración completa atómica | Deseas tener toda la estructura limpia y ordenada de una sola vez | Se reorganizan `Player`, `Enemies`, `Pickups`, `Dialogue` y `_Core` en una sola ejecución verificada. |
| B | Migración gradual (Player + Core primero) | Prefieres probar la nueva estructura con la entidad principal antes de tocar enemigos o UI | Se divide la migración en dos pasos; temporalmente conviven carpetas viejas y nuevas. |
| Custom | Definida por el usuario | Deseas un orden específico | Se adapta el plan de ejecución |

## Glossary

- **Vertical Slice:** Paquete vertical que contiene la representación completa de una característica de juego (código, datos, presentación).
- **Package by Feature:** Patrón de organización física de archivos donde la carpeta principal agrupa por dominio/mecánica en vez de tipo técnico.
- **GUID (.meta):** Identificador único universal que Unity asigna a cada archivo en su correspondiente archivo `.meta` para rastrear referencias entre prefabs y escenas.
