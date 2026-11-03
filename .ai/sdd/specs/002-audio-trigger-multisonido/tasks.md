# Tasks: Disparador de Audio Multisonido en `AudioSourceTrigger`

> Requirements: @requirements.md
> Design: @design.md
> Status: Approved

## Requirement Coverage

| Requirement | Tasks | Notes |
|-------------|-------|-------|
| **FR-001** (Catálogo de sonidos en Inspector) | T1 | Clase serializable `EntradaSonido` (`id`, `AudioResource`, `volumen = 1f`) y lista `sonidos` |
| **FR-002** (Selección por clave en `PlaySound`) | T1 | Índice `Dictionary<string, EntradaSonido>` (`OrdinalIgnoreCase` + `Trim`) y `PlaySound(string)` |
| **FR-003** (Retrocompatibilidad `PlaySound` vacío) | T1 | `recursoPorDefecto` opcional + captura `recursoInicialFuente` en `Awake()` + `PlayDefaultSound()` |
| **FR-004** (Superposición heredando `AudioSource` base) | T1 | Pool acotado `poolFuentes` (`maxFuentesSimultaneas = 4`) clonando ajustes acústicos de `Juancito` |
| **FR-005** (Manejo seguro de claves/nulos) | T1 | Validaciones nulas (`P-005`) y `Debug.LogWarning` contextual en Editor/Development Build |
| **NFR-001** (Usabilidad en Inspector) | T1 | `[Header]`, `[Tooltip]` y `[Range]` en español acorde a `conventions.md` |
| **NFR-002** (Rendimiento y tope de instancias) | T1 | Límite `maxFuentesSimultaneas` con reciclaje circular y `HideFlags.DontSave` |
| **NFR-003** (Integridad de `.meta` y clase) | T1 | Edición in-place de `AudioSourceTrigger.cs` preservando GUID `221b63f03b983bf48b39758f3672b7c6` |

## Implementation Readiness Check

| Check | Status | Notes |
|-------|--------|-------|
| Must Have requirements have tasks | Pass | `FR-001` a `FR-004` cubiertos íntegramente en `T1` |
| Requirements are covered by design | Pass | Mapeados en Sección 2 de `design.md` (`TD-001`, `TD-002`, `TD-003`) |
| Critical Questions are answered | Pass | `Q-001` a `Q-003` respondidas y consolidadas en `D-001` a `D-004` |
| Tasks have dependencies, acceptance criteria, files, and verification | Pass | `T1` detalla subtareas, criterios observables, archivo objetivo y validaciones |
| Verification commands are known or marked manual/N/A | Pass | `dotnet build Assembly-CSharp.csproj -nologo` + chequeo de `.meta` + guía manual PlayMode |

## Implementation Slices

### MVP Slice — Componente `AudioSourceTrigger` Multisonido Completo
- **Goal:** Habilitar en `AudioSourceTrigger` el disparo por clave y superpuesto de múltiples contenedores (`AudioResource`) conservando retrocompatibilidad total con las animaciones actuales de `Juancito`.
- **User Stories:** `US-001`, `US-002`, `US-003`
- **Tasks:** `T1`
- **Independent validation:** Compilación limpia de `Assembly-CSharp.csproj`, preservación del GUID en `AudioSourceTrigger.cs.meta` y prueba en Unity PlayMode con eventos `PlaySound` vacíos y con clave.

---

## Task T1: Implementar Catálogo por Clave y Pool Superpuesto en `AudioSourceTrigger`

**Priority:** P0  
**Estimate:** 45m  
**Dependencies:** none  
**Covers:** FR-001, FR-002, FR-003, FR-004, FR-005, NFR-001, NFR-002, NFR-003

### Work
- [x] Definir la clase `[Serializable] public class EntradaSonido` con campos `id` (`string`), `recurso` (`UnityEngine.Audio.AudioResource`) y `volumen` (`float = 1f` con `[Range(0f, 1.5f)]`).
- [x] Exponer en el Inspector de `AudioSourceTrigger` (con `[Header]` y `[Tooltip]` en español): `recursoPorDefecto`, lista `sonidos` y `maxFuentesSimultaneas` (`[Range(1, 8)]`, default `4`).
- [x] Inicializar en `Awake()` la referencia `fuenteBase`, capturar `recursoInicialFuente = fuenteBase.resource`, inicializar `poolFuentes` y construir `mapaSonidos` normalizando claves (`Trim` + `StringComparer.OrdinalIgnoreCase`), sincronizando también cambios en Editor mediante `OnValidate()`.
- [x] Implementar `public void PlaySound(string nombreSonido = "")` y `public void PlayDefaultSound()` resolviendo recurso por clave o fallback por defecto y emitiendo advertencias seguras (`#if UNITY_EDITOR || DEVELOPMENT_BUILD`) ante claves inexistentes, duplicadas o recursos nulos (`P-005`).
- [x] Implementar la selección/creación acotada de `AudioSource` en `poolFuentes` hasta `maxFuentesSimultaneas` (con `HideFlags.DontSave` en fuentes auxiliares y reciclaje circular al alcanzar el tope), sincronizando `outputAudioMixerGroup`, volumen base * multiplicador de entrada, `pitch`, `spatialBlend` y curvas desde `fuenteBase` antes de ejecutar `Play()`.
- [x] Verificar compilación limpia e integridad del archivo `.meta`.

### Acceptance Criteria
- [x] `AudioSourceTrigger` expone la lista de sonidos con `volumen = 1f` por defecto, `recursoPorDefecto` y `maxFuentesSimultaneas` en el Inspector.
- [x] `PlaySound("")` o `PlayDefaultSound()` reproduce `recursoPorDefecto` o el `AudioResource` inicial del `AudioSource` de `Juancito` sin perder la referencia tras reproducir otros contenedores.
- [x] `PlaySound("pasos")` / `PlaySound("golpes")` resuelve la entrada sin sensibilidad a mayúsculas ni espacios extremos y reproduce los sonidos superpuestos sin truncar efectos en curso, heredando el `OutputAudioMixerGroup` y ajustes de `Juancito`.
- [x] Claves desconocidas o recursos sin asignar no arrojan excepciones en tiempo de ejecución.
- [x] `Assembly-CSharp.csproj` compila con `0` errores y `AudioSourceTrigger.cs.meta` conserva el GUID `221b63f03b983bf48b39758f3672b7c6`.

### Files
- `Assets/Scripts/Audio/AudioSourceTrigger.cs` — modify *(o `Assets/_Core/Audio/AudioSourceTrigger.cs` si la migración `001` se ejecutó previamente)*

### Verification
- [x] Compilación C# pasa sin errores (`dotnet build Assembly-CSharp.csproj -nologo`).
- [x] Verificación de `.meta` confirma GUID `221b63f03b983bf48b39758f3672b7c6` intacto y sin archivos huérfanos.
- [x] Comportamiento manual en Unity Editor / PlayMode documentado para validación final en `Juancito`.
