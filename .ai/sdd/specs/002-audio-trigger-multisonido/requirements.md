# Feature: Disparador de Audio Multisonido en `AudioSourceTrigger`

> Status: Approved
> Source: @.ai/sdd/PLAN.md (F05 — Disparadores de Audio)

## Overview

Extender el componente `AudioSourceTrigger` para permitir que un mismo GameObject (específicamente `Juancito`, que ya posee su `AudioSource` configurado) dispare múltiples contenedores o recursos de sonido distintos (por ejemplo, contenedor de golpes y contenedor de pasos) a partir del evento de animación `PlaySound`, identificando cada sonido mediante una clave/nombre configurable en el Inspector.

La solución preserva la compatibilidad hacia atrás con las animaciones actuales que invocan `PlaySound` sin parámetros y garantiza que los sonidos disparados puedan superponerse sin cortarse entre sí.

## Business Context

- **Problema:** Hoy `AudioSourceTrigger` solo reproduce el único recurso asignado directamente al `AudioSource` del GameObject (`Juancito` lo usa para disparar golpes desde su contenedor de golpes). Al incorporar sonidos de locomoción (contenedor de pasos), no es posible distinguir qué contenedor reproducir desde distintos eventos `PlaySound` ni evitar que un paso corte el audio de un golpe en curso.
- **Valor Jugable (Game Feel — `P-003`):** Dotar a `Juancito` (`P-001`) de retroalimentación sonora tanto en locomoción (pasos) como en combate (golpes) sin cortes abruptos entre efectos, manteniendo un único componente modular y fácil de configurar desde el Inspector (`P-004`).
- **Restricciones del Proyecto:**
  - Trabajar sobre la ubicación actual del script (`Assets/Scripts/Audio/AudioSourceTrigger.cs`) conservando intacto su `.meta`/GUID para no romper referencias existentes en escenas como `Nivel_01`.
  - Reutilizar la configuración base del `AudioSource` ya presente en el GameObject `Juancito` (grupo de mezcla SFX, volumen, espacialización).
  - Evitar errores o excepciones en consola si una clave no existe o falta asignar un recurso (`P-005`).

## User Stories

### US-001: Configuración de Múltiples Contenedores por Nombre en el Inspector
**As a** diseñador/desarrollador en Unity Editor  
**I want to** registrar una lista de sonidos con nombre/clave (ej. `"golpes"`, `"pasos"`) en el Inspector de `AudioSourceTrigger` de `Juancito`  
**So that** pueda centralizar y cambiar los contenedores de audio del personaje desde un solo componente sin duplicar scripts.

**Acceptance Criteria:**
- [ ] El Inspector de `AudioSourceTrigger` permite agregar múltiples entradas compuestas al menos por un identificador de texto (clave) y un recurso/contenedor de audio de Unity 6 (soportando contenedores aleatorios y clips individuales).
- [ ] Es posible definir o mantener un sonido por defecto para llamadas sin clave.

### US-002: Disparo Selectivo desde Eventos `PlaySound` y Retrocompatibilidad
**As a** animador/integrador en Unity Editor  
**I want to** invocar `PlaySound` desde Animation Events indicando el nombre del sonido (ej. `"pasos"` o `"golpes"`), mientras los eventos `PlaySound` existentes sin parámetro siguen funcionando  
**So that** pueda sumar pasos en las animaciones de caminar/correr sin romper las animaciones de golpe ya configuradas (`Player_Golpe_01`, `Player_Golpe_02`).

**Acceptance Criteria:**
- [ ] Un Animation Event `PlaySound` con parámetro de texto (ej. `"pasos"`) reproduce el contenedor asociado a esa clave.
- [ ] Un Animation Event `PlaySound` con parámetro vacío reproduce el sonido por defecto configurado o el recurso base asignado en el `AudioSource` de `Juancito`.
- [ ] La comparación de claves tolera espacios accidentales al inicio/final o diferencias de mayúsculas/minúsculas (ej. `"Pasos"` y `"pasos"` coinciden).

### US-003: Reproducción Superpuesta sin Cortes entre Efectos
**As a** jugador de *Juancito de Adrogué*  
**I want to** escuchar el impacto completo de los golpes de `Juancito` aunque coincidan con sonidos de pasos al moverse o atacar  
**So that** el combate mantenga toda su contundencia sonora sin efectos truncados.

**Acceptance Criteria:**
- [ ] Si se dispara un sonido mientras otro sonido previo de `Juancito` aún se está reproduciendo (ej. paso seguido inmediatamente de golpe, o golpe seguido de paso), ambos sonidos se escuchan completos sin cortarse mutuamente.
- [ ] Todos los sonidos reproducidos respetan la configuración de salida del `AudioSource` principal de `Juancito` (como su `OutputAudioMixerGroup`, volumen y propiedades 2D/3D).

## Functional Requirements

### FR-001: Catálogo de Sonidos Configurable en Inspector — Must Have
THE SYSTEM SHALL exponer en el Inspector de `AudioSourceTrigger` una colección configurable de entradas de sonido, donde cada entrada asocia un identificador de texto (clave) con un recurso de audio compatible con contenedores de Unity 6 (`AudioRandomContainer` / `AudioClip`).

### FR-002: Selección de Sonido por Clave en `PlaySound` — Must Have
WHEN se invoque el evento `PlaySound` con un identificador de texto no vacío  
IF existe una entrada en `AudioSourceTrigger` cuya clave coincida (ignorando mayúsculas/minúsculas y espacios extremos) y tenga un recurso de audio asignado  
THE SYSTEM SHALL reproducir dicho recurso de audio  
SO THAT cada animación pueda disparar su contenedor correspondiente (ej. golpes vs. pasos).

### FR-003: Retrocompatibilidad para Eventos `PlaySound` sin Clave — Must Have
WHEN se invoque `PlaySound` sin parámetro o con texto vacío  
IF existe un recurso por defecto configurado en `AudioSourceTrigger` o asignado en el `AudioSource` del GameObject  
THE SYSTEM SHALL reproducir ese recurso por defecto  
SO THAT las animaciones actuales de golpe sigan sonando sin requerir edición obligatoria.

### FR-004: Superposición Sonora Heredando Ajustes del `AudioSource` Base — Must Have
WHEN se dispare un sonido mediante `PlaySound` mientras otro sonido disparado en el mismo GameObject continúa reproduciéndose  
THE SYSTEM SHALL reproducir el nuevo sonido en paralelo sin detener ni truncar el sonido en curso, respetando el grupo de mezcla (`AudioMixerGroup`) y ajustes acústicos del `AudioSource` de `Juancito`  
SO THAT los pasos y los golpes nunca se corten entre sí durante el juego.

### FR-005: Manejo Seguro de Claves Inexistentes o Recursos Nulos — Should Have
WHEN se invoque `PlaySound` con una clave no registrada o cuyo recurso de audio no esté asignado  
THE SYSTEM SHALL omitir la reproducción sin lanzar excepciones (`NullReferenceException`) y emitir una advertencia clara en consola en modo desarrollo/editor  
SO THAT sea sencillo detectar errores de tipeo en Animation Events sin bloquear la partida (`P-005`).

## Non-Functional Requirements

### NFR-001: Usabilidad en el Inspector de Unity
- Los campos expuestos en el Inspector deben contar con encabezados (`[Header]`) y descripciones (`[Tooltip]`) claros en español acorde a las convenciones del proyecto (`conventions.md`).

### NFR-002: Rendimiento y Control de Instancias en Tiempo de Ejecución
- La reproducción superpuesta no debe generar acumulación indefinida de objetos o componentes huérfanos a lo largo de una sesión de juego prolongada.

### NFR-003: Integridad de Referencias de Unity
- El archivo `Assets/Scripts/Audio/AudioSourceTrigger.cs` debe conservar su nombre de clase y archivo `.meta` actual para mantener intactos los enlaces existentes en escenas y prefabs.

## Out of Scope

- **Configuración manual de assets y animaciones de pasos:** Extraer archivos `.zip` de sonidos, crear el asset contenedor de pasos en Unity y agregar los Animation Events en los clips de caminata/corrida lo realizará el usuario directamente en el Editor de Unity.
- **Migración de carpetas (`F00`):** Este cambio se aplica sobre `Assets/Scripts/Audio/AudioSourceTrigger.cs` en su ruta actual; su eventual mudanza a `Assets/_Core/Audio/` corresponde a la spec `001-reestructuracion-feature-folders`.
- **Modificación del `AudioManager` global:** No se altera la música ni el singleton `AudioManager`, ya que este requerimiento opera sobre el componente local `AudioSourceTrigger` del GameObject `Juancito`.
- **Detección automática de superficies (pasto, piedra, madera):** Todos los pasos se dispararán desde el contenedor que se asocie a la clave correspondiente; cambios dinámicos de contenedor por tipo de suelo quedan fuera de esta iteración.

## Decisions

### D-001: Especificación Puntual Independiente (`002-audio-trigger-multisonido`)
**Decision:** Gestionar esta mejora como una spec puntual en `.ai/sdd/specs/002-audio-trigger-multisonido/` sobre la ruta actual `Assets/Scripts/Audio/AudioSourceTrigger.cs`.  
**Reason:** Es un cambio acotado a un único componente existente y no amerita reabrir el roadmap general ni bloquearse esperando la migración de carpetas `F00`.  
**Source:** Consulta inicial de encauce SDD con el usuario.  
**Impacts:** Overview, Business Context, NFR-003, Out of Scope.

### D-002: Identificación de Sonidos por Lista con Clave/Nombre + Fallback por Defecto
**Decision:** Configurar los contenedores en una lista con nombre/clave en el Inspector de `Juancito` (ej. `"golpes"`, `"pasos"`) y mantener un sonido por defecto cuando el evento `PlaySound` llega vacío.  
**Reason:** Centraliza la asignación de contenedores en el Inspector de `Juancito` y evita romper los eventos `PlaySound` vacíos ya presentes en `Player_Golpe_01.anim` y `Player_Golpe_02.anim`.  
**Source:** Q-001.  
**Impacts:** US-001, US-002, FR-001, FR-002, FR-003.

### D-003: Reproducción Superpuesta sin Cortes Heredando el `AudioSource` de `Juancito`
**Decision:** Los sonidos disparados deben superponerse sin cortarse mutuamente, tomando los ajustes de salida/mezcla del `AudioSource` ya presente en `Juancito`.  
**Reason:** En un Beat 'em up (`P-003`), un paso nunca debe truncar el sonido de impacto de un golpe ni viceversa.  
**Source:** Q-002.  
**Impacts:** US-003, FR-004, NFR-002.

### D-004: Alcance Acotado al Componente `AudioSourceTrigger`
**Decision:** Entregar el script `AudioSourceTrigger` listo para configurar en el Editor; la asignación del contenedor de pasos y de los eventos en las animaciones la realiza el usuario en Unity.  
**Reason:** Mantiene el cambio limpio en código C# sin modificar escenas o binarios de animación innecesariamente (`conventions.md`).  
**Source:** Q-003.  
**Impacts:** US-001, Out of Scope.

## Questions

### Q-001: Selección del contenedor desde eventos `PlaySound` en Unity Editor
**Status:** answered  
**Why it matters:** Define el flujo de trabajo en el Inspector y en la ventana Animation, además de la compatibilidad con los clips existentes.  
**Answer:** Lista con nombre/clave en el Inspector de `Juancito` + sonido por defecto si el evento va vacío (`D-002`).

### Q-002: Comportamiento audible ante sonidos simultáneos o consecutivos cercanos
**Status:** answered  
**Why it matters:** Determina si un paso puede cortar la cola sonora de un golpe en el `AudioSource` de `Juancito`, impactando directamente en el Game Feel.  
**Answer:** Reproducción superpuesta sin cortarse entre sí, heredando el mixer y ajustes del `AudioSource` de `Juancito` (`D-003`).

### Q-003: Límite de alcance sobre assets y animaciones de pasos
**Status:** answered  
**Why it matters:** Delimita los criterios de aceptación entre código del componente y edición manual en el Editor de Unity.  
**Answer:** Alcance enfocado exclusivamente en dejar el componente `AudioSourceTrigger` listo para configurar en el Editor (`D-004`).

## Glossary

- **Contenedor de Audio (`AudioRandomContainer` / `AudioResource`):** Recurso nativo de Unity 6 que agrupa múltiples clips de audio (ej. variaciones de golpes o de pasos) con reglas de selección aleatoria, volumen y tono, reproducible a través de un `AudioSource`.
- **Animation Event (`PlaySound`):** Marcador temporal dentro de un clip de animación de Unity que invoca un método público por nombre en los componentes del mismo GameObject donde reside el `Animator`.

