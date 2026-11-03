# Tech Stack Steering: Juancito de Adrogué

## Runtime / Platform

- **Motor de Desarrollo:** Unity 6 (`6000.5.9f1`).
- **Lenguaje de Programación:** C# (.NET Standard / Unity Mono Runtime).
- **Plataforma Objetivo:** PC (Windows Standalone x64).
- **Control de Versiones:** Git con repositorio alojado en GitHub y configuración `.gitignore` específica para Unity (`Library/`, `Temp/`, `Logs/`, `UserSettings/` excluidas).

## Packages & Librerías Clave

- **Render Pipeline:** Universal Render Pipeline (URP `17.6.0`).
- **Sistema de Entrada:** Unity Input System (`com.unity.inputsystem` `1.20.0`) con soporte para teclado y mouse.
- **Cámara & Composición:** Cinemachine (`com.unity.cinemachine` `3.1.7`) para seguimiento lateral 2D.
- **2D & Sprites:**
  - 2D Animation (`com.unity.2d.animation` `15.1.0`).
  - 2D Aseprite Importer (`com.unity.2d.aseprite` `5.0.3`) para importación directa de archivos `.ase`.
  - 2D Tilemap & Extras (`com.unity.2d.tilemap` `1.0.0`, `com.unity.2d.tilemap.extras` `8.0.3`).
  - 2D SpriteShape (`com.unity.2d.spriteshape` `15.0.3`).
- **UI & Tipografía:** TextMesh Pro (`com.unity.ugui` `2.5.0` y `TextMesh Pro`) para textos tipográficos y UI en pantalla.
- **Audio:** Unity AudioMixer con buses enrutados (`Master`, `Musica`, `SFX`) y persistencia vía `AudioManager`.

## Testing / Verificación

- **Compilación en Editor:** Unity Editor Console sin errores de compilación (`CS0xxx`) ni advertencias críticas de referencias nulas.
- **Pruebas en PlayMode:** Verificación interactiva de escenas (`MainMenu`, `Nivel_01`, `Exterior`, `Interiior de CastleForte`).
- **Física y Colisiones:** Gizmos visuales de depuración en editor (`OnDrawGizmosSelected` para hitboxes circulares y radios de detección).

## Constraints

- **Estándar Gráfico Pixel Art:** Texturas y sprites importados en modo Point (no filter), compresión desactivada (`None`), y resolución consistente (base 64x64 px).
- **Físicas 2D:** Uso de `Rigidbody2D` con detección cinemática/dinámica según corresponda, sin uso innecesario de físicas 3D.
- **Dependencias Externas:** Mantener dependencias mínimas basadas exclusivamente en paquetes oficiales de Unity Package Manager.

## Architectural Decisions

- **Separación de Responsabilidades en Componentes:**
  - El jugador no es un monolito: `Movimiento2D` gestiona la traslación y orientación; `CombateJugador` gestiona ataques y cooldowns; `SaludJugador` gestiona daño, curación y Game Over.
- **Lógica de Daño Reactiva:** Quien recibe el impacto gestiona el daño (`SaludJugador` y `SaludEnemigo`), exponiendo métodos públicos como `RecibirDano(float cantidad)` y `Curar(float cantidad)`.
- **Persistencia de Audio:** `AudioManager` opera como Singleton persistente con `DontDestroyOnLoad` para evitar interrupciones de música entre transiciones de escenas.
- **Diálogos Modulares:** `Dialogo.cs` utiliza eventos desacoplados (`UnityEvent dialogoFinalizado`) para desencadenar el avance de nivel o eventos jugables al concluir el texto.

