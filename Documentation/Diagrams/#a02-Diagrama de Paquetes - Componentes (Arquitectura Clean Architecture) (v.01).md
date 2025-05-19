## **#a02 Diagrama de Paquetes - Componentes (Arquitectura Clean Architecture)**

### Este diagrama de paquetes/componentes está alineado con la arquitectura Clean Architecture y la solución implementada en el proyecto. Refleja correctamente la separación de responsabilidades y dependencias entre capas típicas de Clean Architecture en .NET:

- **API_Controllers** representa la capa de presentación (controladores REST).
- **Application_Services, Application_Handlers, Application_DTOs** representan la capa de aplicación (servicios de orquestación, casos de uso, DTOs).
- **Domain_Entities, Domain_Enums, Domain_Interfaces, Domain_Services** representan el núcleo de dominio (entidades, enums, interfaces, lógica de dominio).
- **Infrastructure_Repositories, Infrastructure_Data** representan la infraestructura (repositorios, acceso a datos/persistencia).

### Las relaciones dibujadas (flechas y dependencias) también son correctas:

- Los controladores usan servicios de aplicación.
- Los servicios de aplicación orquestan entidades de dominio, acceden a repositorios, usan DTOs, handlers y servicios de dominio.
- Los repositorios acceden a la capa de datos.
- Las entidades implementan interfaces y usan enums.
- Los servicios de dominio operan sobre entidades.
