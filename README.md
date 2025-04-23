# ✈️ Gestión de Reservas - Sector Turismo

Sistema de reservas para el sector turismo que permite a los usuarios buscar destinos, seleccionar ofertas personalizadas (vuelos, hoteles, tours) y realizar reservas con opción de pago parcial o total. El sistema coordina con múltiples proveedores y emite vouchers digitales al confirmarse la reserva.

---

## 🚀 Características principales

- Búsqueda y selección de destinos turísticos.
- Ofertas personalizadas: vuelos, hoteles y tours combinados.
- Reservas con opción de pago parcial o total.
- Integración con proveedores externos (aerolíneas, hoteles, operadores de tours).
- Emisión automática de vouchers digitales.
- Arquitectura basada en DDD, SOLID y patrones de diseño.

---

## 🏗️ Arquitectura y organización

El proyecto está organizado siguiendo buenas prácticas de **Domain-Driven Design (DDD)**:
GestionReserva/
│
├── API/ # Controladores y capa de presentación
├── Application/ # Comandos, handlers, DTOs y lógica de aplicación
├── Core/ # Dominio: agregados, entidades, objetos de valor, interfaces, servicios
│ ├── Aggregates/
│ ├── Entities/
│ ├── ValueObjects/
│ ├── Interfaces/
│ └── Services/
├── Infrastructure/ # Adaptadores, persistencia, servicios externos
│ ├── Adapters/
│ ├── Persistence/
│ └── Repositories/
└── Migrations/ # (Opcional) Migraciones de base de datos

---

## 🧩 Modelo de dominio

- **Reserva:** Agregado principal, representa la reserva realizada por el usuario.
- **OfertaPersonalizada:** Combinación de servicios turísticos seleccionados.
- **DetalleServicio:** Información de cada servicio (vuelo, hotel, tour).
- **Destino:** Lugar turístico seleccionado.
- **FechasViaje:** Fechas de inicio y fin del viaje.
- **Pago:** Registro de pagos asociados a la reserva.
- **Voucher:** Comprobante digital emitido tras la confirmación.

---

## ⚙️ Instalación y ejecución

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/tu-usuario/gestion-reserva-turismo.git
   cd gestion-reserva-turismo

2. **Restaura los paquetes y compila:**
    ```bash
    dotnet restore
    dotnet build

3. **Ejecuta la API:**
    ```bash
    dotnet run --project API

4. **Accede a Swagger:**
 - http://localhost:5153/swagger

 ## 📝 Ejemplo de uso (crear una reserva)
    ```json
    {
  "oferta": {
    "vuelo": {
      "proveedor": "Avianca",
      "idExterno": "AV123",
      "monto": { "valor": 200000, "moneda": "COP" }
    },
    "hotel": {
      "proveedor": "Hilton",
      "idExterno": "HIL456",
      "monto": { "valor": 300000, "moneda": "COP" }
    },
    "tour": null,
    "destino": {
      "nombre": "Cartagena",
      "descripcion": "Ciudad amurallada y playas"
    },
    "fechas": {
      "fechaInicio": "2025-06-01T00:00:00",
      "fechaFin": "2025-06-07T00:00:00"
    }
  },
  "montoTotal": 500000,
  "pagoCompleto": true
}