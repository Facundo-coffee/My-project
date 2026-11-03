# Conventions Steering: Juancito de Adrogué

## Code Style

- **Nomenclatura en C#:**
  - Clases, Structs y Métodos públicos: `PascalCase` descriptivo en español o términos canónicos del proyecto (ej. `Movimiento2D`, `SaludJugador`, `RecibirDano`, `CurativoPickup`).
  - Variables privadas y campos locales: `camelCase` (ej. `vidaActual`, `tiempoSiguienteAtaque`).
  - Constantes: `UPPER_SNAKE_CASE` o `PascalCase` con palabra clave `const`.
- **Exposición en Inspector de Unity:**
  - Uso explícito de `[SerializeField]` y campos privados para variables que requieran asignación en el Inspector sin violar el encapsulamiento.
  - Organización visual mediante `[Header("...")]` y `[Tooltip("...")]`.
- **Manejo del Ciclo de Vida de Unity:**
  - Lectura de inputs en `Update()`.
  - Aplicación de traslaciones basadas en físicas (`MovePosition`, velocidades) en `FixedUpdate()`.
  - Limpieza de corrutinas y referencias en `OnDisable()` / `OnDestroy()` cuando aplique.

## Architecture Patterns

- **Vertical Slice Architecture / Feature Folders:**
  - El código y los assets del proyecto están organizados por características verticales autocontenidas bajo `Assets/Features/<Feature>/` (`Player`, `Enemies`, `Pickups`, `Dialogue`).
  - Cada carpeta de feature aloja sus scripts, prefabs, animaciones y audio local. Borrar una feature remueve todos sus archivos sin dejar artefactos huérfanos.
  - Los sistemas transversales e infraestructura global residen en `Assets/_Core/` (`Audio`, `Camera`, `SceneManagement`, `Common`).
  - Regla de dependencia: Las features pueden consultar `_Core/`, pero no deben depender directamente del código interno de otras features sin mediar interfaces o eventos en `_Core/`.
  - Los fondos y tilesets de escenarios compartidos residen en `Assets/Environment/`.
- **Composición sobre Herencia:** Entidades como el Jugador o los Enemigos están formadas por múltiples componentes independientes adosados al mismo GameObject (`Rigidbody2D`, `Collider2D`, scripts de control, scripts de salud).
- **Manejo de Salud y Daño:**
  - Interfaz común de interacción: Scripts de salud implementan `RecibirDano(float)` y emiten señales visuales (tambaleo, cambio temporal a color rojo, muerte con desvanecimiento alfa gradual).
  - Colisiones por capas (`LayerMask`) y etiquetas (`Tag`) oficiales:
    - Tag `Player` para Juancito.
    - Tag `Enemigo` para criaturas y rivales.
    - Tag `Curativo` / triggers para consumibles.
- **Transición de Escenas:** Uso del componente `CambiarEscena` mediante eventos de botones UI (`Button.onClick`) o activadores de zona (`Collider2D.OnTriggerEnter2D`).
- **Control de Audio:** Invocación a `AudioManager.Instance.PlayMusic(...)` y `AudioManager.Instance.PlaySFX(...)` desacoplada del renderizado.

## Testing & Verificación

- **Gizmos de Depuración:** Implementar `OnDrawGizmosSelected()` en scripts con áreas de efecto (ej. radios de ataque de `CombateJugador`) para visualización inmediata en la ventana de escena de Unity.
- **Robustez ante Destrucción de Objetos:** Antes de acceder a componentes externos, validar referencias nulas (`if (salud != null)`) para evitar `MissingReferenceException`.

## Accessibility / UX Rules

- **Feedback de Daño Inmediato:** Todo daño recibido debe ser percibido sin ambigüedad mediante cambio momentáneo de color del sprite (`Color.red`), micro-sacudida de posición y actualización en la barra de salud UI.
- **Accesibilidad en Diálogos:** Los diálogos con efecto máquina de escribir deben permitir acelerar la velocidad o completar la línea instantáneamente ante cualquier pulsación de tecla o clic del ratón.
- **Legibilidad de UI:** La barra de vida de Juancito debe mantenerse en una esquina despejada (HUD superior izquierdo) sin obstruir el plano de combate ni la visión de enemigos.

## Workflow & Git Rules

- **Archivos `.meta` de Unity:** Todo archivo o carpeta nuevo debe conservar su correspondiente archivo `.meta` con su GUID único sincronizado en Git.
- **Assets de Arte:** Los archivos maestros `.ase` (Aseprite) se almacenan en su respectiva feature (`Features/<Feature>/Sprites/`) o en `Assets/Environment/` para escenarios compartidos.
- **Commits Claros:** Separar cambios de código C# de modificaciones masivas a escenas o binarios.
