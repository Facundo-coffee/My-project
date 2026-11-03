# Tasks: Reestructuración a Feature Folders (Vertical Slices) y Shared Core

> Requirements: @requirements.md
> Design: @design.md
> Status: Approved

## Requirement Coverage

| Requisito | Tareas | Notas |
|-----------|--------|-------|
| **FR-001** (Jerarquía Raíz Estandarizada) | T1 | Creación de directorios base `_Core/`, `Features/` y subcarpetas |
| **FR-002** (Encapsulamiento de Player) | T3 | Agrupación completa de scripts, prefabs, anims y audio de Juancito |
| **FR-003** (Centralización de Core) | T2 | Traslado de Audio persistente, Cámara y Gestión de Escenas |
| **FR-004** (Modularización de Slices) | T4 | Reubicación de Enemies, Pickups, Dialogue y Entorno |
| **FR-005** (Preservación de Metadatos) | T2, T3, T4, T5 | Movimiento en pares `<file>` + `<file>.meta` y validación de GUIDs |
| **NFR-001** (Mantenibilidad y Desacople) | T2, T3, T4 | Organización física sin dependencias circulares entre features |
| **NFR-002** (Cero Errores de Compilación) | T5 | Verificación con `dotnet build Assembly-CSharp.csproj` |
| **NFR-003** (Compatibilidad de Arte) | T4 | Reubicación de escenarios comunes a `Assets/Environment/` |

## Implementation Readiness Check

| Check | Status | Notes |
|-------|--------|-------|
| Must Have requirements have tasks | Pass | FR-001 a FR-005 cubiertos exhaustivamente en T1-T5 |
| Requirements are covered by design | Pass | Mapeados en la sección 2 del diseño técnico |
| Critical Questions are answered | Pass | Q-001 resuelta con decisión D-003 (Migración Atómica) |
| Tasks have dependencies, acceptance criteria, files, and verification | Pass | Todas las tareas detalladas con checklist y validación |
| Verification commands are known or marked manual/N/A | Pass | `dotnet build` disponible y script PowerShell de auditoría de GUIDs |

## Implementation Slices

### S1: Preparación y Núcleo Compartido (Core Foundation)
- **Objetivo:** Establecer la infraestructura de directorios y consolidar los subsistemas transversales en `_Core/`.
- **Tareas:** T1, T2
- **Validación Independiente:** Directorios listados en árbol; audio y cámaras consolidados.

### S2: Empaquetamiento Vertical de Gameplay (Features Slices)
- **Objetivo:** Modularizar todas las features de juego (`Player`, `Enemies`, `Pickups`, `Dialogue`, `Environment`) con sus assets correspondientes.
- **Tareas:** T3, T4
- **Validación Independiente:** Ningún asset queda en las carpetas viejas (`Assets/Scripts`, `Assets/Juancito`, `Assets/Prefab`, etc.).

### S3: Integridad, Sincronización y Validación Final
- **Objetivo:** Auditar metadatos `.meta`, sincronizar `Assembly-CSharp.csproj` y comprobar compilación libre de errores.
- **Tareas:** T5
- **Validación Independiente:** Compilación 0 errores en `dotnet build` y cero `.meta` huérfanos.

---

## Task T1: Creación de la Estructura de Directorios Target

**Priority:** P0  
**Estimate:** 15m  
**Dependencies:** none  
**Covers:** FR-001  

### Work
- [x] Crear el árbol de directorios para `Assets/_Core/` (`Audio`, `Camera`, `SceneManagement`, `Common`).
- [x] Crear el árbol de directorios para `Assets/Features/Player/` (`Scripts`, `Prefabs`, `Animations`, `Sprites`, `Audio`).
- [x] Crear el árbol de directorios para `Assets/Features/Enemies/` (`Scripts`, `Sprites`, `Audio`).
- [x] Crear el árbol de directorios para `Assets/Features/Pickups/` (`Scripts`, `Sprites`).
- [x] Crear el árbol de directorios para `Assets/Features/Dialogue/` (`Scripts`).
- [x] Crear el árbol de directorios para `Assets/Environment/`.

### Acceptance Criteria
- [x] Toda la estructura de carpetas existe en el sistema de archivos de `Assets/`.

### Files
- `Assets/_Core/` — crear directorios
- `Assets/Features/` — crear directorios
- `Assets/Environment/` — crear directorio

### Verification
- [x] `Test-Path` confirma la existencia de cada carpeta objetivo.

---

## Task T2: Migración del Core Compartido (`Assets/_Core/`)

**Priority:** P0  
**Estimate:** 30m  
**Dependencies:** T1  
**Covers:** FR-003, FR-005, NFR-001  

### Work
- [x] Mover en pares `<archivo>` + `<archivo>.meta` a `Assets/_Core/Audio/`:
  - `Assets/Musica/AudioManager.cs` (+ `.meta`)
  - `Assets/Scripts/Audio/AudioSourceTrigger.cs` (+ `.meta`)
  - Subcarpetas `Assets/Musica/Mixers` (+ `.meta`) y `Assets/Musica/Ambiente` (+ `.meta`)
  - `Assets/Musica/Ambiente_Oscuro_01.mp3` (+ `.meta`)
- [x] Mover en pares a `Assets/_Core/Camera/`:
  - `Assets/Scripts/Camaras/DirectorCamaras.cs` (+ `.meta`)
  - `Assets/Scripts/Camaras/ZonaCombate.cs` (+ `.meta`)
  - `Assets/Scripts/Camaras/BloqueoZonaCombate.cs` (+ `.meta`)
- [x] Mover en pares a `Assets/_Core/SceneManagement/`:
  - `Assets/Scripts/Escenas/AdministradorEscenas.cs` (+ `.meta`)
  - `Assets/Scripts/Escenas/TransicionEscena.cs` (+ `.meta`)
  - `Assets/Scripts/Escenas/TransicionFade.cs` (+ `.meta`)

### Acceptance Criteria
- [x] Todos los archivos y carpetas de Core residen bajo `Assets/_Core/` junto a sus archivos `.meta` con GUIDs inalterados.

### Files
- `Assets/_Core/Audio/*` — reubicados
- `Assets/_Core/Camera/*` — reubicados
- `Assets/_Core/SceneManagement/*` — reubicados

### Verification
- [x] Verificación de pares `.meta` intactos para cada archivo en `_Core/`.

---

## Task T3: Migración del Slice de Jugador (`Assets/Features/Player/`)

**Priority:** P0  
**Estimate:** 30m  
**Dependencies:** T1  
**Covers:** FR-002, FR-005, US-001, US-002  

### Work
- [x] Mover scripts en pares a `Assets/Features/Player/Scripts/`:
  - `MOVIMIENTO.cs` (+ `.meta`)
  - `CombateJugador.cs` (+ `.meta`)
  - `SaludJugador.cs` (+ `.meta`)
  - `AnimacionJugador.cs` (+ `.meta`)
- [x] Mover prefab en pares a `Assets/Features/Player/Prefabs/`:
  - `Assets/Prefab/Juancito.prefab` (+ `.meta`)
- [x] Mover animaciones en pares a `Assets/Features/Player/Animations/`:
  - `Assets/Juancito/*.anim` (+ `.meta`)
- [x] Mover sprites y archivos Aseprite en pares a `Assets/Features/Player/Sprites/`:
  - `Assets/Juancito/*.ase`, `*.aseprite` (+ `.meta`)
  - `Assets/Arte/Juancito*.ase`, `*.png` (+ `.meta`)
- [x] Mover audio en pares a `Assets/Features/Player/Audio/`:
  - `Assets/Sonidos/muertejuancito.mp3` (+ `.meta`)
  - `Assets/Sonidos/pasos.mp3` (+ `.meta`)
  - `Assets/Sonidos/Sonido Golpe*.mp3` (+ `.meta`)

### Acceptance Criteria
- [x] Juancito queda 100% autocontenido en `Assets/Features/Player/`.
- [x] La carpeta antigua `Assets/Juancito` queda vacía y se elimina junto con su `.meta`.

### Files
- `Assets/Features/Player/Scripts/*` — reubicados
- `Assets/Features/Player/Prefabs/*` — reubicados
- `Assets/Features/Player/Animations/*` — reubicados
- `Assets/Features/Player/Sprites/*` — reubicados
- `Assets/Features/Player/Audio/*` — reubicados

### Verification
- [x] Inspeccionar que los GUIDs referenciados en `Juancito.prefab` coinciden con los de `Assets/Features/Player/Scripts/*.cs.meta`.

---

## Task T4: Migración de Slices de Enemigos, Pickups, Diálogos y Entorno

**Priority:** P0  
**Estimate:** 40m  
**Dependencies:** T1  
**Covers:** FR-004, FR-005, NFR-003  

### Work
- [x] Mover a `Assets/Features/Enemies/`:
  - Scripts: `EnemigoBasico.cs`, `SaludEnemigo.cs` (+ `.meta`) a `Scripts/`
  - Sprites: `Assets/Arte/Enemigos/*` (+ `.meta`) y `Assets/Arte/Mula.png` (+ `.meta`) a `Sprites/`
  - Audio: `Assets/Sonidos/Gruñidos.mp3` (+ `.meta`) a `Audio/`
- [x] Mover a `Assets/Features/Pickups/`:
  - Scripts: `CurativoPickup.cs` (+ `.meta`) a `Scripts/`
  - Sprites: `Assets/Arte/Mate.ase`, `Media luna.ase` (+ `.meta`) a `Sprites/`
- [x] Mover a `Assets/Features/Dialogue/`:
  - Scripts: `Assets/Scripts/Textos/Dialogo.cs` (+ `.meta`) a `Scripts/`
- [x] Mover a `Assets/Environment/`:
  - `Assets/Assets/*` (archivos de estación `.ase` + `.meta`)
  - `Assets/Arte/EstacionTren*.ase`, `Entrada castillo.ase`, `adrogué.ase`, etc. (+ `.meta`)
- [x] Depurar archivo plantilla vacío:
  - Eliminar `Assets/Scripts/NewEmptyCSharpScript.cs` (+ `.meta`)
- [x] Limpiar directorios obsoletos vacíos y sus correspondientes `.meta`:
  - `Assets/Scripts/`, `Assets/Juancito/`, `Assets/Prefab/`, `Assets/Sonidos/`, `Assets/Assets/`, `Assets/Musica/`.

### Acceptance Criteria
- [x] Todas las mecánicas y assets de entorno quedan reubicados limpiamente.
- [x] Ninguna carpeta obsoleta vacía permanece en `Assets/`.

### Files
- `Assets/Features/Enemies/*` — reubicados
- `Assets/Features/Pickups/*` — reubicados
- `Assets/Features/Dialogue/*` — reubicados
- `Assets/Environment/*` — reubicados

### Verification
- [x] Validar que no queden archivos sin clasificar en las carpetas de origen.

---

## Task T5: Verificación de Integridad de Metadatos, GUIDs y Compilación

**Priority:** P0  
**Estimate:** 20m  
**Dependencies:** T2, T3, T4  
**Covers:** FR-005, NFR-002, US-003  

### Work
- [x] Actualizar las rutas de `<Compile Include="..." />` en `Assembly-CSharp.csproj` para que apunten a sus nuevas ubicaciones bajo `Features/` y `_Core/`.
- [x] Ejecutar auditoría automatizada de metadatos en PowerShell:
  - Confirmar que para todo archivo `.cs`, `.prefab`, `.anim`, `.ase`, `.mp3` exista su correspondiente `.meta`.
  - Confirmar que no exista ningún archivo `.meta` huérfano.
- [x] Ejecutar compilación C# con `dotnet build Assembly-CSharp.csproj`.
- [x] Validar que `git status` reporte los traslados como operaciones de renombrado ordenadas.

### Acceptance Criteria
- [x] `dotnet build Assembly-CSharp.csproj` finaliza con 0 errores (`Build succeeded`).
- [x] Cero archivos `.meta` huérfanos.
- [x] Sincronización completa de GUIDs verificada.

### Files
- `Assembly-CSharp.csproj` — actualizar rutas de compilación

### Verification
- [x] Comando de compilación: `dotnet build Assembly-CSharp.csproj` exit code 0.
