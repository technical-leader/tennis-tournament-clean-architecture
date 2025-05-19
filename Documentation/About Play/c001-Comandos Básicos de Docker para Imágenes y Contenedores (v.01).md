## Comandos Básicos de Docker para Imágenes y Contenedores

## Índice

- [Consideraciones Previas](#consideraciones-previas)
- [Construir una Imagen](#construir-una-imagen-a-partir-de-un-dockerfile)
- [Listar Imágenes Locales](#listar-imágenes-locales)
- [Listar Contenedores en Ejecución](#listar-contenedores-en-ejecución)
- [Listar Todos los Contenedores](#listar-todos-los-contenedores-incluyendo-detenidos)
- [Ejecutar un Contenedor](#ejecutar-un-contenedor-a-partir-de-una-imagen)
- [Ver Logs de un Contenedor](#ver-los-logs-de-un-contenedor)
- [Detener un Contenedor](#detener-un-contenedor)
- [Eliminar un Contenedor](#eliminar-un-contenedor-debe-estar-detenido)
- [Eliminar una Imagen](#eliminar-una-imagen-debe-estar-libre-sin-contenedores-usándola)

---

Aquí se listan los comandos básicos utilizados para gestionar imágenes y contenedores Docker

### Consideraciones previas

- _En mi caso particular generé la imagen: `challenge` y el nombre del contenedor: `tennis-app-prod`_
- _Considerar que trabajé en el directorio raíz a la solución (donde se ubica el sln). En este directorio se encuentra el Dockerfile_

### Construir una imagen a partir de un Dockerfile:

```bash
docker build -t challenge:latest -t challenge:v1 .
```

(El `.` indica que el contexto de compilación es el directorio actual donde se encuentra el _Dockerfile_).
_A modo de observación, menciono que se declararon dos tag (etiquetas), uno para referenciar al `latest` y otro a `v1`_

- Sobre el etiquetado de la imagen que se está construyendo, su desglose sería:

  - challenge: Este es el nombre que le estoy dando a la imagen. Es el identificador principal de la imagen.
  - : (dos puntos): Es el separador entre el nombre de la imagen y la etiqueta (tag).
  - latest: Esta es una etiqueta (tag). Por convención, latest se usa a menudo para referirse a la versión más reciente y estable de una imagen. Es como decir "esta es la última versión buena que tengo".
  - v1: Esta es otra etiqueta (tag) que le estoy aplicando a la misma imagen. En este caso, estoy usando v1 para indicar una versión específica de la "Tennis Tournament API". Podría ser la versión 1, o la primera versión mayor de la API.

- **Listar imágenes locales:**

  ```bash
  docker images
  # o
  docker image ls
  ```

- **Listar contenedores en ejecución:**

  ```bash
  docker ps
  ```

- **Listar todos los contenedores (incluyendo detenidos):**

  ```bash
  docker ps -a
  ```

- **Ejecutar un contenedor a partir de una imagen:**

  ```bash
  docker run [OPTIONS] IMAGE [COMMAND] [ARG...]
  ```

  Ejemplo con opciones comunes:

  ```bash
  docker run -d -p 5000:5000 [-e `varialbes como ser de conexión`] --name tennis-app-prod challenge
  ```

  (`-d` para ejecutar en segundo plano, `-p` para mapear puertos, `--name` para asignar un nombre).

- **Ver los logs de un contenedor:**

  ```bash
  docker logs tennis-app-prod
  # Para seguir los logs en tiempo real:
  docker logs -f tennis-app-prod
  ```

- **Detener un contenedor:**

  ```bash
  docker stop tennis-app-prod
  ```

- **Eliminar un contenedor (debe estar detenido):**

  ```bash
  docker rm tennis-app-prod
  ```

- **Eliminar una imagen (debe estar libre, sin contenedores usándola):**
  ```bash
  docker rmi challenge
  ```

---

_Documento generado como resumen de la configuración de producción y comandos Docker._
