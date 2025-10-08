# InventoryHub

Sistema modular para la gestión de inventarios construido sobre .NET y PostgreSQL, aplicando principios de Arquitectura Limpia / Onion, CQRS con MediatR, mapeo con AutoMapper y un enfoque explícito en mantenibilidad y extensibilidad.

---

## 🚀 Características Clave

- **Productos**: CRUD completo con validaciones básicas.
- **Inventarios**: Control del stock asociado a cada producto (1:1).
- **Movimientos de Inventario**: Registro histórico de entradas / salidas.
- **Tipos de Movimiento**: Catálogo parametrizable (ej. Entrada, Salida, Ajuste).
- **CQRS + MediatR**: Separación clara de operaciones de lectura y escritura.
- **Auditoría**: Campos de tracking (`Created`, `CreatedBy`, `LastModified`, `LastModifiedBy`).
- **Persistencia**: Entity Framework Core + scripts SQL (funciones y stored procedures requeridos tras migraciones).
- **Versionado de API** (extensión configurada).
- **Swagger / OpenAPI**: Exploración y prueba de endpoints.
- **Health Check**: Endpoint `/health` para monitoreo básico.
- **CORS restringido**: Origen permitido `http://localhost:5173` (frontend).
- **Sesión distribuida en memoria**: Cookie `MiSesion` con políticas seguras.

---

## 🧱 Arquitectura / Capas

```
Interface.WebApi (Presentación / Endpoints / Versionado / Swagger)
    ↑
Core.Application (CQRS, DTOs, Validaciones, Mapeos, Contratos)
    ↑
Core.Domain (Entidades, lógica de negocio pura)
    ↑
Infrastructure.Persistence (EF Core, Migrations, Repositorios, Scripts SQL)
```

### Principios aplicados

- Separación de responsabilidades
- Inversión de dependencias mediante registros en `ServiceRegistration.cs`
- DTOs para aislar el dominio del transporte
- AutoMapper para reducir repetición
- MediatR para orquestar comandos y queries

---

## 🗂️ Entidades del Dominio

| Entidad             | Descripción            | Notas                      |
| ------------------- | ---------------------- | -------------------------- |
| Product             | Catálogo de productos  | Posee Inventario (1:1)     |
| Inventory           | Stock actual           | Relación única con Product |
| InventoryMovement   | Registro de movimiento | Guarda cantidad y tipo     |
| MovementType        | Catálogo de tipos      | Ej: Entrada, Salida        |
| AuditableBaseEntity | Base audit             | Heredada por todas         |

---

## ⚙️ Requisitos Previos

- .NET 8 SDK (verifica con `dotnet --version`)
- PostgreSQL >= 14
- (Opcional) Docker + Docker Compose

---

## 🛫 Puesta en Marcha (Local)

1. Clonar repositorio

   ```bash
   git clone https://github.com/DaviddelaRosa15/InventoryHub.git
   cd InventoryHub
   ```

2. Variables de entorno / Configuración

   - En `appsettings.json` la cadena de conexión usa placeholder: `"PostgreSQL": "%POSTGRESQL%"`.
   - Crea un archivo `.env` (si no existe) y define:
     ```env
     POSTGRESQL=Host=localhost;Port=5432;Database=inventoryhub;Username=postgres;Password=TU_PASSWORD;
     ```
   - El cargado se realiza con `DotNetEnv.Env.Load()` en `Program.cs`.

3. Crear base de datos (si no existe)

   ```sql
   CREATE DATABASE inventoryhub;
   ```

4. Aplicar migraciones EF Core

   ```bash
   dotnet ef database update --project InventoryHub.Infrastructure.Persistence
   ```

5. Ejecutar scripts SQL (OBLIGATORIO después de migraciones)

   Estos scripts crean funciones y stored procedures usados por la capa de aplicación.

   Usando psql (ajusta ruta si usas otra shell / SO):

   ```bash
   psql "Host=localhost;Port=5432;Database=inventoryhub;Username=postgres;Password=TU_PASSWORD" -f Scripts/fn_producto_get_all.sql
   psql "Host=localhost;Port=5432;Database=inventoryhub;Username=postgres;Password=TU_PASSWORD" -f Scripts/fn_producto_get_by_id.sql
   psql "Host=localhost;Port=5432;Database=inventoryhub;Username=postgres;Password=TU_PASSWORD" -f Scripts/sp_producto_create.sql
   psql "Host=localhost;Port=5432;Database=inventoryhub;Username=postgres;Password=TU_PASSWORD" -f Scripts/sp_producto_update.sql
   psql "Host=localhost;Port=5432;Database=inventoryhub;Username=postgres;Password=TU_PASSWORD" -f Scripts/sp_producto_delete.sql
   ```

   Con Docker (contenedor llamado `pg`):

   ```bash
   docker exec -i pg psql -U postgres -d inventoryhub < Scripts/fn_producto_get_all.sql
   docker exec -i pg psql -U postgres -d inventoryhub < Scripts/fn_producto_get_by_id.sql
   docker exec -i pg psql -U postgres -d inventoryhub < Scripts/sp_producto_create.sql
   docker exec -i pg psql -U postgres -d inventoryhub < Scripts/sp_producto_update.sql
   docker exec -i pg psql -U postgres -d inventoryhub < Scripts/sp_producto_delete.sql
   ```

6. (Opcional) Semillas iniciales

   - Si se agrega lógica de seeding controlada por `InitialRun` en `appsettings.json`, establece `InitialRun: true` y vuelve a ejecutar.

7. Ejecutar la API

   ```bash
   dotnet run --project InventoryHub.Interface.WebApi
   ```

8. Navegar a Swagger
   - https://localhost:5001/swagger (o el puerto asignado)

---

## 🐳 Ejecución con Docker (Opcional)

Ejemplo simple (asumiendo que se agregará un Dockerfile):

```bash
docker build -t inventoryhub-api -f InventoryHub.Interface.WebApi/Dockerfile .
docker run -p 5001:8080 --env-file .env inventoryhub-api
```

> Si aún no existe Dockerfile, un paso siguiente recomendado es generarlo.

---

## 🧪 Comandos Útiles de EF Core

Crear nueva migración:

```bash
dotnet ef migrations add NombreMigracion --project InventoryHub.Infrastructure.Persistence
```

Actualizar base:

```bash
dotnet ef database update --project InventoryHub.Infrastructure.Persistence
```

Eliminar última migración (sin aplicar):

```bash
dotnet ef migrations remove --project InventoryHub.Infrastructure.Persistence
```

---

## 🔌 Endpoints Principales (Productos)

| Método | Ruta               | Descripción        |
| ------ | ------------------ | ------------------ |
| GET    | /api/products      | Lista productos    |
| GET    | /api/products/{id} | Obtiene por Id     |
| POST   | /api/products      | Crea producto      |
| PUT    | /api/products/{id} | Actualiza producto |
| DELETE | /api/products/{id} | Elimina producto   |

Endpoints análogos existen para inventarios y movimientos. Revisa Swagger para el contrato completo.

---

## 🛡️ CORS / Seguridad / Sesión

- Política CORS: Solo `http://localhost:5173` (modificar en `Program.cs` para otros orígenes).
- Sesiones: Cookie `MiSesion`, `HttpOnly`, `SameSite=None`, `Secure=Always`.
- Agregar autenticación real es un próximo paso (JWT o Identity).

---

## ❤️ Health Check

- Endpoint: `GET /health` devuelve estado 200 si la aplicación está viva.

---

## 🧩 Scripts SQL (Obligatorios)

Ubicados en `Scripts/` y DEBEN ejecutarse después de aplicar las migraciones para que la aplicación pueda usar las funciones / procedimientos.

| Archivo                     | Tipo      | Propósito               |
| --------------------------- | --------- | ----------------------- |
| `fn_producto_get_all.sql`   | Function  | Listar productos        |
| `fn_producto_get_by_id.sql` | Function  | Obtener producto por Id |
| `sp_producto_create.sql`    | Procedure | Insertar producto       |
| `sp_producto_update.sql`    | Procedure | Actualizar producto     |
| `sp_producto_delete.sql`    | Procedure | Eliminar producto       |

Ejemplo de uso (psql):

```sql
CALL sp_product_create('uuid', 'Teclado', 'Mecánico', 50.99, 'admin');
SELECT * FROM fn_product_get_all();
```

---

## 🧪 Testing (Pendiente)

Se recomienda agregar:

- Tests de unidad (xUnit / NUnit) para Application (Handlers CQRS)
- Tests de integración con una BD PostgreSQL temporal o contenedor Docker

---

## 🛣️ Roadmap Sugerido

- [ ] Agregar autenticación / autorización (JWT)
- [ ] Agregar capa de validación FluentValidation
- [ ] Implementar caching distribuido (Redis)
- [ ] Agregar pruebas unitarias e integración
- [ ] Docker Compose (API + PostgreSQL + pgAdmin)
- [ ] Métricas (Prometheus + Grafana)
- [ ] Observabilidad (Serilog + OpenTelemetry)
- [ ] Paginación / filtros avanzados en queries
- [ ] Soft delete / eventos de dominio

---

## 🤝 Contribuciones

1. Haz fork y crea tu rama: `feature/nueva-funcionalidad`
2. Asegura formato y convenciones
3. Incluye pruebas (si aplica)
4. Abre Pull Request describiendo cambios y motivación

Commits sugeridos (convencional):

```
feat: añade endpoint para XYZ
fix: corrige null reference en handler de productos
refactor: extrae lógica de validación
docs: actualiza sección de instalación
```

---

## 📄 Licencia

Proyecto privado (sin licencia declarada). No distribuir sin autorización del autor.

---

## 👤 Autor

- [DaviddelaRosa15](https://github.com/DaviddelaRosa15)

---

Si necesitas un Dockerfile, pipeline CI o integración de autenticación, abre un issue o solicita la mejora y lo añadimos.
