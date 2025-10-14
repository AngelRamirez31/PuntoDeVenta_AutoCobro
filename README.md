# Punto de Venta de Auto-Cobro (Self-Checkout)
Este proyecto es una aplicación web desarrollada en **ASP.NET Core MVC** que simula un sistema de punto de venta de auto-cobro para una tienda de autoservicio. La aplicación permite a los clientes escanear productos y finalizar la compra, mientras que ofrece un panel de administración para el personal de la tienda.

---

## Características Principales

### Flujo del Cliente
- **Pantalla de Bienvenida:** Un punto de inicio claro para comenzar el proceso de compra.
- **Carrito de Compras Dinámico:**
    - **Agregar Productos:** Simula el escaneo de productos a través de su código de barras.
    - **Eliminar Productos:** Permite quitar artículos del carrito de forma individual.
    - **Cancelar Compra:** Opción para vaciar el carrito y regresar a la bienvenida.
- **Proceso de Pago Simulado:**
    - **Selección de Método de Pago:** Soporta pagos simulados con "Tarjeta" y "Efectivo".
    - **Cálculo de Cambio:** Para pagos en efectivo, el sistema calcula y muestra el cambio a devolver.
- **Recibo de Compra:** Al finalizar, se genera un resumen detallado con la fecha, listado de artículos, total y detalles del pago.

### Flujo de Administración
- **Gestión de Productos:**
    - Listar, agregar y eliminar productos del catálogo.
- **Búsqueda de Tickets:** Permite al personal buscar y visualizar el detalle de cualquier compra por su ID.
- **Reportes de Ventas:**
    - Muestra reportes de ventas diarios y mensuales.
    - **Métricas Clave:** Ingresos totales, ingresos por método de pago, Top 10 productos y Top 3 categorías más vendidas.

---

## Tecnologías Utilizadas

- **Backend:** C# con ASP.NET Core 8 MVC
- **Base de Datos:** SQLite a través de Entity Framework Core
- **Frontend:** HTML, CSS, Bootstrap 5
- **Manejo de Datos CSV:** Librería `CsvHelper` para la carga inicial del catálogo
- **Arquitectura:** Modelo-Vista-Controlador (MVC)

---
