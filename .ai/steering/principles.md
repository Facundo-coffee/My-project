# Project Principles: Juancito de Adrogué

> Status: Active
> Last Updated: 2026-10-07

## Purpose

Este documento establece los principios de diseño y desarrollo que gobiernan la toma de decisiones para *Juancito de Adrogué*. Guía las etapas de especificación (PRD, DESIGN), tareas (TASKS), ejecución técnica y revisiones (REVIEW).

## How to Use These Principles

- `MUST`: Regla innegociable obligatoria; cualquier excepción requiere aprobación explícita.
- `SHOULD`: Comportamiento predeterminado recomendado; excepciones deben estar técnicamente justificadas.
- `MAY`: Opción o sugerencia permitida, no obligatoria.
- Si un principio entra en conflicto con un artefacto aprobado, se debe detener la ejecución y consultar al usuario.

## Principles

### P-001: Foco en Juancito y Alcance Acotado (Single Character MVP)

**Level:** MUST  
**Rule:** Juancito es el único personaje jugable del MVP. Se prohíbe dispersar esfuerzos en personajes secundarios jugables (Gachi o Pachi) hasta que el personaje principal esté completamente pulido, jugable y probado de inicio a fin.  
**Reason:** Decisión consensuada en el diseño del proyecto para evitar retrasos y asegurar que el movimiento, animaciones y combate tengan la máxima calidad posible.  
**Applies to:** PRD, SPEC, TASKS, EXEC, REVIEW

### P-002: Identidad Territorial y Cultural Auténtica

**Level:** MUST  
**Rule:** Los entornos jugables, la narrativa y los elementos interactivos deben respetar y representar fielmente los lugares de Adrogué (Casa de la Cultura, UNaB, Plaza Brown, Castillo Castelforte) y el folclore argentino (Luz Mala, Alma Mula, gaucho Segundo, mate/medialuna) sin caer en clichés descontextualizados.  
**Reason:** Es la propuesta de valor única (USP) del videojuego y lo que lo distingue de otros beat 'em ups comerciales.  
**Applies to:** PRD, SPEC, TASKS, REVIEW

### P-003: Contundencia y Feedback Jugable ("Game Feel" & Juice)

**Level:** SHOULD  
**Rule:** Toda interacción de combate (dar o recibir golpes, esquivar, derrotar enemigos) debe proporcionar retroalimentación audiovisual inmediata: destellos de color, sacudida de cámara/sprite (screen shake), pausa microscópica al impacto (hit-stop), y efectos de sonido enérgicos.  
**Reason:** El género Beat 'em up depende directamente de la sensación física y el impacto satisfactorio de los ataques.  
**Applies to:** SPEC, TASKS, EXEC, REVIEW

### P-004: Modularidad y Responsabilidad Única en C#

**Level:** SHOULD  
**Rule:** Los scripts de Unity deben mantener una responsabilidad única (separando movimiento, combate, salud, audio e interfaz). No se deben crear scripts monolíticos tipo "PlayerManager" o "GameManager" universales que contengan toda la lógica.  
**Reason:** Facilita la depuración, evita dependencias circulares y permite reutilizar lógica (por ejemplo, el sistema de salud en jugador y enemigos).  
**Applies to:** SPEC, TASKS, EXEC, REVIEW

### P-005: Control de Errores y Nulidad en Ciclo de Escenas

**Level:** MUST  
**Rule:** Toda búsqueda o manipulación de referencias entre GameObjects debe contar con validaciones de existencia (`if (obj != null)`) para evitar excepciones (`NullReferenceException`) al transicionar entre escenas o al destruir entidades.  
**Reason:** Garantiza estabilidad durante la ejecución continua del juego en PC.  
**Applies to:** EXEC, REVIEW

## Decision Rules

1. La estabilidad y jugabilidad del combate tienen prioridad sobre el añadido de contenido cosmético.
2. Los requisitos aprobados de producto tienen prioridad sobre preferencias técnicas personales.
3. Si existe una duda sobre el alcance de un enemigo o escenario, elegir la opción más compacta y pulida.
4. Ante cualquier discrepancia no resuelta en diseño, consultar explícitamente al equipo/usuario.

## Review Expectations

- [ ] Los requisitos respetan que Juancito es el único protagonista del MVP (`P-001`).
- [ ] Los escenarios y amenazas mantienen coherencia con Adrogué y el folclore (`P-002`).
- [ ] Las tareas de combate incluyen tareas de feedback visual y sonoro (`P-003`).
- [ ] El código mantiene componentes modulares e independientes (`P-004`).
- [ ] Las pruebas en editor y PlayMode no arrojan errores de consola (`P-005`).

