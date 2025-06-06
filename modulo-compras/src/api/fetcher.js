//// src/api/fetcher.js

//// URL base de tu API (ajusta el puerto/protocolo según tu configuración):
//export const BASE_URL = 'https://localhost:7151/api';

///**
// * Función auxiliar para hacer fetch a nuestra API.
// * endpoint: '/productos', '/ordenes/15/proveedores', etc.
// * options: { method, headers, body, ... }
// */
//async function request(endpoint, options = {}) {
//    const url = `${BASE_URL}${endpoint}`;
//    const config = {
//        headers: { 'Content-Type': 'application/json' },
//        ...options
//    };
//    const res = await fetch(url, config);
//    if (!res.ok) {
//        const texto = await res.text();
//        throw new Error(`Error ${res.status} en ${endpoint}: ${texto}`);
//    }
//    // Si la respuesta no tiene contenido, devolvemos null
//    if (res.status === 204) return null;
//    return res.json();
//}

//// ─── Productos ───────────────────────────────────────────────────

///**
// * Devuelve TODOS los productos
// * GET /api/productos
// */
//export async function fetchProductos() {
//    return request('/productos', { method: 'GET' });
//}

///**
// * Devuelve SOLO los productos cuyo stock (cantidadTotal) sea <= cantidadMinima.
// * Hace internamente fetchProductos() y filtra.
// */
//export async function fetchStockProductos() {
//    const todos = await fetchProductos();
//    // Suponemos que la entidad Producto tiene "cantidadTotal" y "cantidadMinima"
//    return todos.filter(p =>
//        typeof p.cantidadTotal === 'number' &&
//        typeof p.cantidadMinima === 'number' &&
//        p.cantidadTotal <= p.cantidadMinima
//    );
//}

///**
// * Devuelve un producto puntual
// * GET /api/productos/{id}
// */
//export async function fetchProducto(idProducto) {
//    return request(`/productos/${idProducto}`, { method: 'GET' });
//}

///**
// * Crea un producto (POST /api/productos)
// * dto debe tener: { nombre, marca, idCategoria, cantidadTotal, cantidadMinima }
// */
//export async function createProducto(dto) {
//    return request('/productos', {
//        method: 'POST',
//        body: JSON.stringify(dto),
//    });
//}

///**
// * Actualiza un producto (PUT /api/productos/{id})
// */
//export async function updateProducto(idProducto, dto) {
//    return request(`/productos/${idProducto}`, {
//        method: 'PUT',
//        body: JSON.stringify(dto),
//    });
//}

///**
// * Elimina un producto (DELETE /api/productos/{id})
// */
//export async function deleteProducto(idProducto) {
//    return request(`/productos/${idProducto}`, { method: 'DELETE' });
//}

//// ─── Órdenes (Solicitudes) ───────────────────────────────────────

//export async function fetchOrdenes() {
//    return request('/ordenes', { method: 'GET' });
//}

//export async function fetchOrden(idOrden) {
//    return request(`/ordenes/${idOrden}`, { method: 'GET' });
//}

//export async function createOrden(dto) {
//    return request('/ordenes', {
//        method: 'POST',
//        body: JSON.stringify(dto),
//    });
//}

//export async function updateOrden(idOrden, dto) {
//    return request(`/ordenes/${idOrden}`, {
//        method: 'PUT',
//        body: JSON.stringify(dto),
//    });
//}

//export async function deleteOrden(idOrden) {
//    return request(`/ordenes/${idOrden}`, { method: 'DELETE' });
//}

///**
// * Devuelve los proveedores que pueden cotizar la orden (GET /api/ordenes/{ordenId}/proveedores)
// */
//export async function fetchProveedoresParaOrden(idOrden) {
//    return request(`/ordenes/${idOrden}/proveedores`, { method: 'GET' });
//}

//// ─── Presupuestos (Cotizaciones) ────────────────────────────────

//export async function fetchPresupuestosGlobal() {
//    return request('/presupuestos', { method: 'GET' });
//}

///**
// * Crea una cotización para la orden (POST /api/ordenes/{ordenId}/presupuestos)
// * dto: { idProveedor, fechaEntrega, subtotal, iva5, iva10, total, detalles: [ ... ] }
// */
//export async function createPresupuesto(ordenId, dto) {
//    return request(`/ordenes/${ordenId}/presupuestos`, {
//        method: 'POST',
//        body: JSON.stringify(dto),
//    });
//}

//export async function fetchPresupuestoPorOrden(ordenId, presuId) {
//    return request(`/ordenes/${ordenId}/presupuestos/${presuId}`, { method: 'GET' });
//}

//// ─── Pedidos Finales ────────────────────────────────────────────

//export async function fetchPedidosFinales() {
//    return request('/pedidos', { method: 'GET' });
//}

//export async function fetchPedidoFinal(idPedido) {
//    return request(`/pedidos/${idPedido}`, { method: 'GET' });
//}

//export async function createPedidoFinal(dto) {
//    return request('/pedidos', {
//        method: 'POST',
//        body: JSON.stringify(dto),
//    });
//}

//export async function generarPedidosAuto(idOrden) {
//    return request(`/pedidos/${idOrden}/generar-auto`, {
//        method: 'POST',
//    });
//}

//export async function deletePedidoFinal(idPedido) {
//    return request(`/pedidos/${idPedido}`, { method: 'DELETE' });
//}

//// ─── Proveedores ─────────────────────────────────────────────────

//export async function fetchProveedores() {
//    return request('/proveedores', { method: 'GET' });
//}

//export async function fetchProveedor(idProveedor) {
//    return request(`/proveedores/${idProveedor}`, { method: 'GET' });
//}

//export async function createProveedor(dto) {
//    return request('/proveedores', {
//        method: 'POST',
//        body: JSON.stringify(dto),
//    });
//}

//export async function updateProveedor(idProveedor, dto) {
//    return request(`/proveedores/${idProveedor}`, {
//        method: 'PUT',
//        body: JSON.stringify(dto),
//    });
//}

//export async function deleteProveedor(idProveedor) {
//    return request(`/proveedores/${idProveedor}`, { method: 'DELETE' });
//}

//// ─── Categorías ──────────────────────────────────────────────────

//export async function fetchCategorias() {
//    return request('/categorias', { method: 'GET' });
//}

//export async function createCategoria(dto) {
//    return request('/categorias', {
//        method: 'POST',
//        body: JSON.stringify(dto),
//    });
//}

//export async function updateCategoria(idCategoria, dto) {
//    return request(`/categorias/${idCategoria}`, {
//        method: 'PUT',
//        body: JSON.stringify(dto),
//    });
//}

//export async function deleteCategoria(idCategoria) {
//    return request(`/categorias/${idCategoria}`, { method: 'DELETE' });
//}
// src/api/fetcher.js

// BASE_URL: ajusta protocolo y puerto si tu API corre en HTTP o en otro puerto.
// src/api/fetcher.js
// ───────────────────────────────────────────────────────────────────────────────
// Todas las llamadas HTTP que el Front requiere para comunicarse con el back.
// Ajusta BASE_URL si tu API no está en "/api".
const BASE_URL = "/api";

// ─────────── ÓRDENES (Solicitudes de Compra) ───────────

// 1) GET /api/ordenes  → Lista todas las órdenes (solicitudes) con sus detalles
export async function fetchOrdenes() {
    const res = await fetch(`${BASE_URL}/ordenes`);
    if (!res.ok) throw new Error("Error cargando órdenes");
    return res.json();
}

// 2) GET /api/ordenes/{id} → Devuelve una orden específica (con detalles)
export async function fetchOrden(id) {
    const res = await fetch(`${BASE_URL}/ordenes/${id}`);
    if (!res.ok) throw new Error(`Error cargando orden ${id}`);
    return res.json();
}

// 3) POST /api/ordenes  → Crea nueva orden
//    Body esperado: { estado: string, ordenDetalles: [ { idProducto, cantidad }, ... ] }
export async function createOrden(dto) {
    const res = await fetch(`${BASE_URL}/ordenes`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const errorBody = await res.json();
        throw new Error(`Error ${res.status} en /ordenes: ${JSON.stringify(errorBody)}`);
    }
    return res.json();
}

// 4) PUT /api/ordenes/{id} → Actualiza estado y detalles de la orden
export async function updateOrden(id, dto) {
    const res = await fetch(`${BASE_URL}/ordenes/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const errorBody = await res.json();
        throw new Error(`Error ${res.status} en /ordenes/${id}: ${JSON.stringify(errorBody)}`);
    }
    return;
}

// 5) DELETE /api/ordenes/{id} → Elimina orden
export async function deleteOrden(id) {
    const res = await fetch(`${BASE_URL}/ordenes/${id}`, {
        method: "DELETE",
    });
    if (!res.ok) {
        const errorTxt = await res.text();
        throw new Error(`Error eliminando orden ${id}: ${errorTxt}`);
    }
    return;
}

// 6) PUT /api/ordenes/{id}/solicitar-cotizacion
//    Cambia el estado de la orden a “PENDIENTE” y devuelve lista de proveedores
export async function solicitarCotizacionOrden(idOrden) {
    const res = await fetch(`${BASE_URL}/ordenes/${idOrden}/solicitar-cotizacion`, {
        method: "PUT",
    });
    if (!res.ok) {
        const errorBody = await res.json();
        throw new Error(
            `Error ${res.status} en /ordenes/${idOrden}/solicitar-cotizacion: ${JSON.stringify(errorBody)}`
        );
    }
    return res.json(); // → [ { idProveedor, nombre, ruc }, ... ]
}

// 7) POST /api/ordenes/{id}/generar-auto
//    Genera automáticamente los pedidos finales (agrupados por proveedor)
export async function generarPedidosAuto(idOrden) {
    const res = await fetch(`${BASE_URL}/ordenes/${idOrden}/generar-auto`, {
        method: "POST",
    });
    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(`Error ${res.status} en /ordenes/${idOrden}/generar-auto: ${errorBody}`);
    }
    return res.json(); // → Devuelve lista de pedidos finales creados
}

// 8) GET /api/ordenes/{idOrden}/proveedores
//    Devuelve la lista de proveedores a los que enviar la solicitud de cotización
export async function fetchProveedoresParaOrden(idOrden) {
    const res = await fetch(`${BASE_URL}/ordenes/${idOrden}/proveedores`);
    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(`Error ${res.status} en /ordenes/${idOrden}/proveedores: ${errorBody}`);
    }
    return res.json(); // → [ { idProveedor, nombre, ruc }, ... ]
}

// ─────────── PRODUCTOS ───────────

// 9) GET /api/productos  → Lista todos los productos (para “Productos” y “Stock”)
export async function fetchProductos() {
    const res = await fetch(`${BASE_URL}/productos`);
    if (!res.ok) throw new Error("Error cargando productos");
    return res.json(); // Cada producto debe tener: { idProducto, nombre, cantidadTotal, cantidadMinima, categoria: { idCategoria, nombre } }
}

// 10) GET /api/stock (o /api/productos/stock) → Solo productos de poco stock
//     Asegúrate de exponer en tu API un endpoint que devuelva únicamente aquellos con cantidadTotal < cantidadMinima.
//     En este ejemplo asumimos que existe GET /api/stock para ello.
export async function fetchStockProductos() {
    const res = await fetch(`${BASE_URL}/stock`);
    if (!res.ok) throw new Error("Error cargando productos de poco stock");
    return res.json();
}

// ─────────── PRESUPUESTOS (Cotizaciones) ───────────

// 11) GET /api/ordenes/{ordenId}/presupuestos
//     Lista todas las cotizaciones creadas para esa orden
export async function fetchPresupuestosPorOrden(ordenId) {
    const res = await fetch(`${BASE_URL}/ordenes/${ordenId}/presupuestos`);
    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(`Error ${res.status} en /ordenes/${ordenId}/presupuestos: ${errorBody}`);
    }
    return res.json();
}

// 12) POST /api/ordenes/{ordenId}/presupuestos
//     Crea nueva cotización para esa orden
//     Body esperado: { idProveedor, fechaEntrega, subtotal, iva5, iva10, total, detalles: [ { idProducto, cantidad, precio, iva5, iva10 }, … ] }
export async function createPresupuesto(ordenId, dto) {
    const res = await fetch(`${BASE_URL}/ordenes/${ordenId}/presupuestos`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const errorBody = await res.json();
        throw new Error(
            `Error ${res.status} en /ordenes/${ordenId}/presupuestos: ${JSON.stringify(errorBody)}`
        );
    }
    return res.json();
}

// 13) PUT /api/ordenes/{ordenId}/presupuestos/{idPresu}  → Actualiza cotización
export async function updatePresupuesto(ordenId, idPresu, dto) {
    const res = await fetch(`${BASE_URL}/ordenes/${ordenId}/presupuestos/${idPresu}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const errorBody = await res.json();
        throw new Error(
            `Error ${res.status} en /ordenes/${ordenId}/presupuestos/${idPresu}: ${JSON.stringify(errorBody)}`
        );
    }
    return;
}

// 14) DELETE /api/ordenes/{ordenId}/presupuestos/{idPresu}  → Borra cotización
export async function deletePresupuesto(ordenId, idPresu) {
    const res = await fetch(`${BASE_URL}/ordenes/${ordenId}/presupuestos/${idPresu}`, {
        method: "DELETE",
    });
    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(`Error ${res.status} en /ordenes/${ordenId}/presupuestos/${idPresu}: ${errorBody}`);
    }
    return;
}

// ─────────── PEDIDOS FINALES (Órdenes de Compra) ───────────

// 15) GET /api/pedidos  → Lista todos los pedidos finales
export async function fetchPedidosFinales() {
    const res = await fetch(`${BASE_URL}/pedidos`);
    if (!res.ok) throw new Error("Error cargando pedidos finales");
    return res.json();
}

// 16) GET /api/pedidos/{id} → Un pedido final específico
export async function fetchPedidoFinal(id) {
    const res = await fetch(`${BASE_URL}/pedidos/${id}`);
    if (!res.ok) throw new Error(`Error cargando pedido final ${id}`);
    return res.json();
}

// 17) DELETE /api/pedidos/{id} → Borra un pedido final
export async function deletePedidoFinal(id) {
    const res = await fetch(`${BASE_URL}/pedidos/${id}`, {
        method: "DELETE",
    });
    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(`Error eliminando pedido final ${id}: ${errorBody}`);
    }
    return;
}

// 18) (Opcional) Si quieres crear manualmente un pedido final: POST /api/pedidos
//     Body: { idOrden, idProveedor, montoTotal, fechaPedido, fechaEntrega, estado, detalles: [ { idProducto, cantidad, cotizacion, iva } ] }
export async function createPedidoFinal(dto) {
    const res = await fetch(`${BASE_URL}/pedidos`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const errorBody = await res.json();
        throw new Error(`Error ${res.status} en /pedidos: ${JSON.stringify(errorBody)}`);
    }
    return res.json();
}
