# Idea: Arquitectura por Feature Folders (Vertical Slices) y Shared Core

> Status: idea:captured
> Created: 2026-10-27

## Raw Idea

"Necesito estructurar las carpetas de este projecto en forma de layered por feature. Se que tiene un nombre nunca recuerdo cual es, es un formato de clean architecture el cual contiene cada feature en una carpeta, con todas sus depencias, asi borrar cada feature no es una catrastofe cada vez que es necesario hacerlo."

## Problem Space

- Actualmente el proyecto Unity organiza sus archivos de forma horizontal técnica ("Package by Layer"): `Assets/Scripts`, `Assets/Arte`, `Assets/Prefab`, `Assets/Musica`, `Assets/Sonidos`, `Assets/Scenes`, etc.
- Cuando una feature (como combate, movimiento, inventario o un enemigo particular) necesita modificarse, refactorizarse o eliminarse, sus archivos están dispersos por múltiples carpetas.
- Eliminar o desacoplar una feature se vuelve propenso a errores ("catástrofe"), dejando prefabs huérfanos, scripts sin usar o referencias rotas.
- El objetivo es adoptar **Vertical Slice Architecture** / **Package by Feature** / **Screaming Architecture**, agrupando en una sola carpeta todo lo relativo a una feature (código, prefabs, arte y configuraciones específicas) con límites claros respecto a dependencias comunes.

## Target Users

- Desarrolladores y diseñadores del proyecto ("Juancito de Adrogué").
- Mantenedores de la base de código que requieran iterar, agregar o remover mecánicas sin efectos colaterales descontrolados.

## Current Alternatives

- **Package by Layer (Estado actual):** Carpetas por tipo de asset (`Scripts/`, `Prefab/`, `Arte/`, `Musica/`). Muy común en proyectos pequeños de Unity, pero escala mal al crecer las mecánicas.
- **Monolito sin convención:** Archivos colocados ad-hoc en la raíz de `Assets/`.
- **Arquitectura de Ensamblados Estrictos (.asmdef en cada carpeta):** Aislamiento rígido a nivel de compilador, excelente para proyectos grandes, pero añade fricción inicial en prototipado si hay muchas dependencias circulares.

## Desired Outcome

- Estructura limpia donde cada funcionalidad viva en su propio directorio: `Assets/Features/<NombreFeature>/`.
- Contención de dependencias: cada carpeta de feature contiene sus Scripts, Prefabs y assets locales.
- Existencia de una capa común bien definida (`Assets/_Core/` o `Assets/Shared/`) para managers persistentes (`AudioManager`), interfaces base de daño/salud, configuraciones globales de URP y utilidades compartidas.
- Posibilidad de eliminar o archivar una feature borrando únicamente su carpeta sin desestabilizar el resto del juego.
- Migración segura en Unity sin perder GUIDs ni referencias en `.meta`.

## Possible Directions

### Direction A: Feature Folders + Shared Core (Seleccionada)

- **Description:** Reorganizar el proyecto en `Assets/Features/<Feature>/` (con subcarpetas locales `Scripts/`, `Prefabs/`, etc., según amerite) y un `Assets/_Core/` (o `Shared/`) para audio global, salud común, singletons y escenas base.
- **Pros:**
  - Fácil de entender y navegar; la estructura "grita" las capacidades del juego.
  - Eliminar una feature es directo: se remueve su carpeta.
  - Mantiene flexibilidad sin imponer la rigidez de `.asmdef` de inmediato.
- **Cons:**
  - Requiere disciplina para no crear dependencias invisibles entre features hermanas.
- **Risks:**
  - Al mover archivos existentes en Unity, se debe preservar cuidadosamente el par `.cs` + `.cs.meta` y `.prefab` + `.prefab.meta` para no romper referencias de escenas.

### Direction B: Full Modular con Assembly Definitions (.asmdef)

- **Description:** Igual que la Dirección A, pero agregando un archivo `.asmdef` en cada carpeta de feature.
- **Pros:**
  - El compilador de C# bloquea físicamente referencias indebidas entre features.
  - Tiempos de recompilación ultrarrápidos en Unity al modificar un solo script.
- **Cons:**
  - Requiere desacoplar eventos mediante ScriptableObjects o interfaces ubicadas en `Core`.
- **Risks:**
  - Mayor costo de configuración inicial y refactorización de scripts existentes que se llaman directamente.

### Direction C: Feature-First solo en Código (Scripts)

- **Description:** Reestructurar únicamente `Assets/Scripts/Features/<Feature>/` y dejar los assets visuales y de audio en sus carpetas globales (`Arte/`, `Audio/`).
- **Pros:**
  - Menor riesgo de romper prefabs y escenas durante la migración.
- **Cons:**
  - No resuelve el problema para arte, prefabs o sonidos específicos de esa feature; borrar una feature sigue requiriendo buscar en múltiples carpetas de assets.
- **Risks:**
  - Solución a medias que no cumple totalmente el objetivo de encapsulamiento total.

## Open Questions

- ¿Cuáles son las primeras features a delimitar? (Ejemplos iniciales: `Player`, `Enemies`, `Combat`, `Audio`, `Environment/CastleForte`).
- ¿Qué convenciones de nomenclatura se usarán para la carpeta base? (Ejemplo: `Assets/_Core/` vs `Assets/Shared/` y `Assets/Features/`).
- ¿Se moverán los assets desde el explorador de Windows o desde el editor de Unity abierto para que Unity gestione automáticamente los `.meta`?

## Constraints

- **Unity 6 (`6000.5.9f1`):** La integridad de los archivos `.meta` y GUIDs es crítica.
- **Convenciones del proyecto:** Respetar lo estipulado en `.ai/steering/conventions.md`.
- **Git:** Separar la migración de carpetas en commits claros de movimiento de archivos para trazabilidad en Git.

## Signals of Value

- Localizar cualquier script, prefab o comportamiento de una mecánica toma segundos en el Project window de Unity.
- Eliminar un prototipo o enemigo descartado se reduce a borrar su carpeta correspondiente en `Assets/Features/`.

## Recommendation

- [x] Create PLAN
- [ ] Create REQUIREMENTS directly
- [ ] Keep exploring
- [ ] Drop

**Reason:** Dado que este cambio impacta la estructura transversal de todo el repositorio de Unity y afecta las convenciones de trabajo del equipo, se recomienda formalizarlo en el plan del proyecto (`.ai/sdd/PLAN.md`) o actualizar `.ai/steering/conventions.md` antes de ejecutar los traslados de carpetas.

