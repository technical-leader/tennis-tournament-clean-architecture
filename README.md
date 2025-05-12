# tennis-tournament-clean-architecture

## **Nota importante :** _este repositorio esta en desarrollo, se estima ser liberado el lunes 19 de mayo del 2025_

---

Este repositorio contiene la implementación de un **sistema de simulación de torneos de tenis por eliminación directa** siguiendo los principios de **Clean Architecture**, aplicando buenas prácticas de programación orientada a objetos y estructuración modular.

## **📌 Índice**
1. Descripción del Proyecto  
2. Requerimientos  
3. Estructura del Proyecto  
4. Instalación y Ejecución  
5. Documentación  
6. Contribuciones y Licencia  

---

## **1️⃣ Descripción del Proyecto**
Este sistema permite modelar y simular torneos de tenis siguiendo un formato de **eliminación directa**, asegurando una estructura robusta y escalable.  
Cuenta con una separación clara de capas, donde cada módulo cumple una responsabilidad específica dentro de la arquitectura.

## **2️⃣ Requerimientos**
- Modelado basado en programación orientada a objetos.
- Aplicación de patrones de diseño como **Strategy, Factory, Repository, CQRS**.
- Implementación de una API REST documentada con **Swagger**.
- Uso de **SQL Server 2022** como base de datos.
- Despliegue opcional con **Docker/Kubernetes**.

### **Extras abordados**:
✅ **API Rest (Swagger)**  
✅ **Testing Integration** 
✅ **Testing Unitario** 
✅ **Persistencia en SQL Server (EF Core, Migrations)**  
✅ **Despliegue en contenedores (Docker Compose, Podman)**  

---

## **3️⃣ Estructura del Proyecto**
El proyecto se organiza de la siguiente manera siguiendo **Clean Architecture**:

```
📁 TennisTournament/
   ├── 📁 src/                      # Código fuente
   │    ├── 📁 Domain/              # Modelado de entidades y lógica de negocio
   │    ├── 📁 Application/         # Servicios y reglas de aplicación
   │    ├── 📁 Infrastructure/      # Persistencia y adaptadores de datos
   │    ├── 📁 API/                 # Exposición vía REST API
   │    ├── 📁 Tests.Unit/          # Pruebas unitarias
   │    ├── 📁 Tests.Integration/   # Pruebas de integración
   │    ├── 📄 TennisTournament.sln # Solución de .NET 9 C#
   │
   ├── 📁 Documentation/            # Documentación técnica y diagramas
   ├── 📁 Tools/                    # Scripts de automatización y validación
   ├── 📄 README.md                 # Documentación principal
   ├── 📄 .gitignore                # Archivos ignorados por Git
   ├── 📄 LICENSE                   # Licencia del repositorio
   ├── 📄 CONTRIBUTING              # Documentación marco para contribuir
```
✅ **Nuevo directorio `Documentation/`** para diagramas UML y análisis.  
✅ **Nuevo directorio `Tools/`** para scripts de soporte.  

---

## **4️⃣ Instalación y Ejecución**
### **1. Prerrequisitos**
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) o [Podman](https://podman.io/)
- [SQL Server 2022 - Developer (instancia local o en contenedor)](https://mcr.microsoft.com/artifact/mar/mssql/server/about)
- [Visual Studio Code](https://code.visualstudio.com/Download)
- _Deseable:_ [NGINX - SLIM](https://hub.docker.com/_/nginx)
  
### **2. Clonar el repositorio**
```sh
git clone https://github.com/Technical-Leader/tennis-tournament-clean-architecture.git
cd tennis-tournament-clean-architecture
```

### **3. Configuración y ejecución**
**📌 Levantar la API en entorno local**  
```sh
cd TennisTournament.API
dotnet run
```

**Otras opciones**

**📌 Ejecutar la API con Docker Compose**  
```sh
docker-compose up --build
```
**📌 Ejecutar tests unitarios**  
```sh
dotnet test TennisTournament.Tests.Unit/
```

---

## **5️⃣ Documentación**
La documentación técnica se encuentra en `Documentation/`, donde puedes consultar:
- **Diagrama de Clases** `#a01-Diagrama de Clases.png`
- **Diagrama Entidad-Relación** `#a03-Diagrama de Datos.png`
- **Explicación detallada de la arquitectura**  

📌 **Para pruebas de API**: Se da soporta vía `Swagger`.

---

## **6️⃣ Contribuciones y Licencia**
✅ Este proyecto sigue la **Licencia MIT**, permitiendo su uso y modificación con atribución.  
✅ Para contribuir, sigue la estructura definida en `CONTRIBUTING.md`.  
✅ Reportes de errores y mejoras pueden enviarse vía **Issues** en GitHub.  

---
Cambio temporal para validar PR
