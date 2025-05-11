# **CONTRIBUTING.md - Guía de Contribución**

¡Bienvenido! 🚀 Gracias por tu interés en contribuir al proyecto **Tennis Tournament Clean Architecture**. A continuación, te explicamos cómo puedes colaborar de manera efectiva.

## **📌 Índice**
1. Reglas generales  
2. Cómo reportar problemas o solicitar mejoras  
3. Cómo contribuir con código  
4. Estructura del código y buenas prácticas  
5. Proceso de revisión y aprobación  
6. Licencia  

---

## **1️⃣ Reglas Generales**
- Asegúrate de que tu contribución **siga la arquitectura y principios SOLID** del proyecto.  
- Toda nueva funcionalidad debe incluir **pruebas unitarias y de integración**.  
- Usa mensajes de `commit` descriptivos siguiendo [Convención de Git](https://www.conventionalcommits.org/).  
- Los cambios deben realizarse en **ramas separadas**, nunca en `main`.  

---

## **2️⃣ Reportar problemas o solicitar mejoras**
Si encuentras un **error** o quieres proponer una **mejora**, sigue estos pasos:
1. Ve a la pestaña **Issues** en GitHub.
2. Abre un nuevo Issue describiendo el problema o mejora:
   - Incluye ejemplos de código si aplica.
   - Explica el impacto esperado de tu propuesta.
3. Etiqueta tu Issue con la categoría correspondiente (`bug`, `enhancement`, `question`).  

---

## **3️⃣ Contribuir con código**
### **1. Fork & Clone**
Haz un **fork** del repositorio y clónalo a tu entorno local:
```sh
git clone https://github.com/Technical-Leader/tennis-tournament-clean-architecture.git
cd tennis-tournament-clean-architecture
```
### **2. Crear una rama de trabajo**
```sh
git checkout -b feature/nueva-funcionalidad
```
### **3. Realizar cambios y probar**
Implementa tu código y asegúrate de ejecutar pruebas:
```sh
dotnet test TennisTournament.Tests.Unit/
dotnet test TennisTournament.Tests.Integration/
```
### **4. Commit y push**
```sh
git add .
git commit -m "feat: Implementación de nueva funcionalidad"
git push origin feature/nueva-funcionalidad
```
### **5. Crear un Pull Request**
- Ve al repositorio en GitHub y abre un **Pull Request** hacia `main`.  
- Explica claramente los cambios realizados.  
- Espera la revisión de los mantenedores antes de que tu código sea aprobado.  

---

## **4️⃣ Estructura del código y buenas prácticas**
✅ Usa **Clean Architecture**, separando `Domain`, `Application`, `Infrastructure`, `API`.  
✅ Sigue **nomenclatura PascalCase para clases y camelCase para variables**.  
✅ Implementa patrones como **Strategy, Repository, CQRS y Mediator**.  
✅ Agrega comentarios claros para mejorar la legibilidad del código.  

---

## **5️⃣ Proceso de Revisión y Aprobación**
- Cada Pull Request será revisado por los mantenedores del proyecto.  
- Se requiere aprobación de al menos **1 revisor** antes de fusionar cambios a `main`.  
- Se pueden solicitar modificaciones antes de aceptar la contribución.  

---

## **6️⃣ Licencia**
Este proyecto está bajo la **Licencia MIT**, lo que significa que cualquier contribución se realizará bajo los mismos términos.  
Consulta [`LICENSE`](./LICENSE) para más detalles.  

---

### 🎾 **¡Gracias por contribuir!**
Tu apoyo ayuda a mejorar este proyecto y hacerlo más robusto.  
Si tienes dudas, pregunta en los **Issues** o envía un mensaje a los mantenedores. 🚀  

---
