# Product Steering: Juancito de Adrogué

## Product Vision

*Juancito de Adrogué* es un videojuego de acción Beat 'em up y plataformas 2D desarrollado en Unity con estética Pixel Art HD (sprites 64x64 px). Está ambientado en una versión fantástica y sobrenatural de Adrogué (partido de Almirante Brown, Buenos Aires, Argentina). El jugador acompaña a Juancito en su regreso nocturno a casa tras el cumpleaños de un amigo, recorriendo calles y sitios emblemáticos de la ciudad mientras enfrenta criaturas y manifestaciones del folclore argentino vinculadas a una antigua maldición en el Castillo Castelforte.

## Target Users / Personas

- **Jugador de Acción / Beat 'em Up 2D (PC):** Jugadores jóvenes y adultos que disfrutan del combate cuerpo a cuerpo dinámico, el ritmo clásico de avance lateral y desafíos progresivos con controles accesibles pero reactivos.
- **Entusiasta de la Cultura Local y Folclore:** Personas atraídas por la narrativa de fantasía urbana, el rescate del folclore nacional (Luz Mala, Alma Mula) y la reinterpretación de espacios urbanos reales dentro de un videojuego.
- **Comunidad de Videojuegos Independientes:** Jugadores que valoran propuestas indie con identidad propia, humor cotidiano y dirección artística cuidada en Pixel Art.

## Value Proposition

- **Identidad Territorial Real como Escenario Activo:** Adrogué no es un mero fondo estático; sus puntos neurálgicos (el ferrocarril Roca, la Casa Municipal de la Cultura, la Universidad Nacional Guillermo Brown - UNaB, la Plaza Almirante Brown y el Castillo Castelforte) estructuran la progresión de la aventura.
- **Folclore Argentino Integrado a las Mecánicas:** Las amenazas no son monstruos genéricos, sino figuras míticas argentinas adaptadas a la experiencia jugable (la tensión fugaz de la Luz Mala, la ferocidad de la Alma Mula y la maldición del gaucho Segundo).
- **Cercanía, Identidad y Humor Cotidiano:** Elementos culturales reconocibles (recuperación de salud con mates y medialunas, noticias diegéticas de medios locales de Brown) que dotan de autenticidad y encanto a la experiencia.

## Product Boundaries

### In Scope (MVP y Versión Base)

- **Protagonista Único (Juancito):** Enfoque concentrado en Juancito como único personaje jugable con combate cuerpo a cuerpo contundente (alcance acotado tras revisión de diseño).
- **Mecánicas Centrales:** Desplazamiento lateral y en profundidad (belt-scroller/top-down 2D), salto, ataque melee, sistema de salud con feedback visual (daño, tambaleo, hit-stop), y recolección de curaciones (mate y medialunas).
- **Progresión Narrativa y Niveles:** Recorrido lineal desde el entorno urbano nocturno de Adrogué pasando por la Casa de la Cultura y Plaza Brown hasta culminar en el interior y túneles del Castillo Castelforte.
- **Enemigos Definidos:**
  - Enemigos básicos urbanos / sobrenaturales.
  - Luz Mala (evento breve de tensión).
  - Alma Mula (mini-jefe / enfrentamiento especial en Plaza Brown).
  - Segundo (jefe final en Castelforte, antiguo gaucho vinculado a la maldición).
- **Audio y Narrativa:** Música ambiental nocturna, efectos de sonido con AudioMixers, y sistema de diálogos tipográficos (efecto máquina de escribir).

### Out of Scope

- **Personajes Jugables Adicionales en el Lanzamiento:** Gachi y Pachi quedan descartados del alcance inicial para asegurar la calidad y pulido del personaje principal.
- **Modos Multijugador / Cooperativo:** El juego está diseñado exclusivamente como experiencia single-player para PC.
- **Sistemas Complejos de RPG / Inventario Extenso:** No habrá árboles de habilidades complejos, crafteo ni gestión de inventario pesado; se prioriza la inmediatez del Beat 'em up.
- **Mundo Abierto:** La progresión es por niveles y tramos lineales guiados.

## Success Metrics

- **Sensación de Impacto (Game Feel):** El combate debe sentirse contundente mediante feedback inmediato (hit-stop, sacudida de pantalla, destellos de daño y efectos sonoros claros).
- **Fluidez del Ciclo Principal:** El ciclo `Explorar -> Enfrentar desafíos -> Combatir -> Superar obstáculos -> Avanzar` debe mantener un ritmo sostenido sin tiempos muertos excesivos.
- **Completitud del Recorrido:** Un jugador promedio debe poder iniciar en el menú principal, superar las secciones urbanas y completar el enfrentamiento final en Castelforte sin errores bloqueantes (`bugs` críticos o excepciones en Unity).

## Domain Glossary

- **Juancito:** Protagonista del juego, un pibe común que utiliza combate directo cuerpo a cuerpo para defenderse de lo paranormal.
- **Segundo:** Antagonista principal y jefe final; antiguo gaucho vinculado a la maldición de Castelforte.
- **Alma Mula:** Criatura sobrenatural del folclore argentino, antagonista de gran poder que se enfrenta en la Plaza Brown.
- **Luz Mala:** Manifestación luminosa misteriosa que anticipa la presencia sobrenatural frente a la Casa de la Cultura.
- **Castillo Castelforte:** Edificación histórica icónica de Adrogué, núcleo narrativo del juego y escenario del desenlace.
- **Curativo / Mate y Medialuna:** Objetos interactivos diegéticos que restauran la barra de salud del jugador.
- **Hit-stop:** Congelamiento momentáneo de fotogramas al impactar un golpe para potenciar la sensación física del impacto.

