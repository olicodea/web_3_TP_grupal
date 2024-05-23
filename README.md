# Generador de Exámenes con Gemini AI

## Descripción

El objetivo de este trabajo es generar exámenes, con preguntas y respuestas, a través de la IA, específicamente con la ayuda de Gemini AI. La aplicación permite generar parciales sobre un tema determinado a partir de un texto proporcionado.

Nuestro enfoque se centra en la inteligencia artificial generativa, un tipo de sistema de IA capaz de generar texto. Los modelos de IA generativa aprenden los patrones y la estructura de sus datos de entrenamiento de entrada y luego generan nuevos datos con características similares.

## Características

- Generación automática de preguntas y respuestas basadas en un texto de entrada.
- Integración con Gemini AI para el procesamiento de lenguaje natural.
- Interfaz de usuario intuitiva y fácil de usar.
- Capacidad para generar exámenes personalizados.

## Requisitos

- .NET 6.0 o superior
- Visual Studio 2022 o superior
- Cuenta y credenciales de API de Gemini AI

## Instalación

1. Clona el repositorio desde GitHub.
2. Abre el proyecto en Visual Studio.
3. Configura las credenciales de Gemini AI:
    - Crea un archivo `appsettings.json` en la raíz del proyecto (si no existe).
    - Agrega tus credenciales de Gemini AI en el archivo `appsettings.json`:
      ```json
      {
        "GeminiAI": {
          "ApiKey": "TU_API_KEY"
        }
      }
      ```
4. Restaura los paquetes de NuGet.
5. Ejecuta la aplicación.

## Uso

1. **Ingresar el texto de entrada:**
    - Navega a la página de generación de exámenes.
    - Ingresa el texto sobre el cual deseas generar preguntas y respuestas.

2. **Generar preguntas:**
    - Haz clic en el botón "Generar Examen".
    - La aplicación enviará el texto a Gemini AI y recibirá las preguntas generadas.

3. **Revisar y exportar el examen:**
    - Revisa las preguntas y respuestas generadas.
    - Opcionalmente, puedes exportar el examen a un formato de tu elección (por ejemplo, PDF).

## Colaboradores

Rocio Belen Crespo

## Licencia

Este proyecto está licenciado bajo la [MIT License](LICENSE).

---
