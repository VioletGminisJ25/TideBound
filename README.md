# 🏴‍☠️ Tidebound

Tidebound es un metroidvania en desarrollo con mecánicas de exploración, combate dinámico y plataformas. En este juego, el protagonista se embarca en una aventura en busca del tesoro perdido del legendario pirata Ronan "El Desatado" a través de un archipiélago lleno de secretos, rivales y trampas.

## 🎮 Características

- 🌊 Mundo interconectado con múltiples zonas por explorar.
- ⚔️ Combate fluido con animaciones detalladas.
- 🏃‍♂️ Mecánicas de plataformas avanzadas con coyote time, jump buffer y un sistema de salto mejorado.
- 🔄 Viaje rápido entre checkpoints con un costo en monedas.
- 🏝️ Habitaciones ocultas y misiones secundarias para incentivar la exploración.

## 📜 Registro de Cambios

### 🗓️ 31 de marzo de 2025

- **Mejoras en el movimiento del personaje**:
  - Implementación de *coyote time* para permitir saltar justo después de abandonar una plataforma.
  - Implementación de *jump buffer* para mejorar la respuesta del salto.
  - Mejora del sistema de salto usando `Lerp` para una sensación más fluida y controlada.
  - Ajustes en la gravedad y el multiplicador de caída para una mejor sensación de peso del personaje.
  - Se implementó una máquina de estados para gestionar mejor las animaciones del personaje.
  
### 🗓️ 1 de abril de 2025

- **Intento de gancho**:
  
  Actualmente se está experimentando con la mecánica de gancho. Se intentó implementarlo usando:
  - **RayCast2D** para detectar colisiones con superficies enganchables.
  - **DampedSpringJoint2D** para generar la atracción del jugador hacia el punto de anclaje.
  - **Problema encontrado:** `CharacterBody2D` no se ve afectado por las físicas externas, lo que limita el movimiento natural del gancho.
  - **Próximo paso:** Evaluar si convertir al personaje en `RigidBody2D` o encontrar una solución alternativa con `CharacterBody2D`.

### 🗓️ 6 de abril de 2025

- **Implementación de la mecánica de gancho(basica)**:
  - Al final se recurrió al uso de `CharacterBody2D` para implementar la mecánica de gancho en vez de `RigidBody2D` a pesar de que no se ve afectado por las físicas externas, lo que limita el movimiento natural del gancho y aumenta el trabajo de implementación al tener que simular estas físicas adicionales.
  - Se implementó la mecánica de gancho básica usando `RayCast2D` y `Line2D`. Cuando el raycast detecta una colisión con una superficie enganchable, se activa el gancho y se mueve el personaje hacia el punto de anclaje, mediante una velocidad lineal.
- **Cambios a hacer**:
  - Modificar la velocidad del gancho para que sea más fluida y controlada. Se puede implementar una función de interpolación para mejorar la sensación de fluidez. Además de simular esta atracción como un resorte siguiendo la formula de la Ley de Hooke (F = k * x) modificada añadiendole una fuerza de amortiguamiento (F = k * x * t) para evitar que el resorte oscile indefinidamente, (F = (k * x) - (v * springDamping)).


## 🚀 Instalación y Ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/VioletGminisJ25/PirateGame.git
   ```
2. Abrir el proyecto en *Godot* (versión 4.x).
3. Ejecutar el juego desde el editor.

## 🏗️ Contribuciones

Si deseas contribuir con ideas, código o reportes de errores, siéntete libre de abrir un *issue* o enviar un *pull request* en [el repositorio](https://github.com/VioletGminisJ25/PirateGame).

## 📜 Licencia

Este proyecto está bajo la licencia MIT. Puedes usarlo y modificarlo libremente, siempre que des crédito a los autores originales.

¡Gracias por seguir el desarrollo de *Tidebound*! 🏴‍☠️

