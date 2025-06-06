//// src/api/pedidos.js
//const BASE = '/api/pedidos';

//export async function fetchPedidos() {
//    const res = await fetch(BASE);
//    if (!res.ok) throw new Error('Error cargando pedidos finales');
//    return await res.json();
//}

//export async function fetchPedido(id) {
//    const res = await fetch(`${BASE}/${id}`);
//    if (!res.ok) throw new Error(`Error cargando pedido final ${id}`);
//    return await res.json();
//}

///**
// * createPedido(dto):
// * dto = {
// *   IdOrden,       // id de la orden original
// *   IdProveedor,
// *   MontoTotal,
// *   FechaPedido,
// *   FechaEntrega,
// *   Estado,
// *   Detalles: [ { IdProducto, Cantidad, Cotizacion, Iva }, … ]
// * }
// * POST /api/pedidos
// */
//export async function createPedido(dto) {
//    const res = await fetch(BASE, {
//        method: 'POST',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify(dto)
//    });
//    if (!res.ok) {
//        const text = await res.text();
//        throw new Error(`Error creando pedido final: ${text}`);
//    }
//    return await res.json();
//}

//export async function deletePedido(id) {
//    const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
//    if (!res.ok) {
//        const text = await res.text();
//        throw new Error(`Error eliminando pedido final ${id}: ${text}`);
//    }
//}

///**
// * generarAuto(idOrden):
// * POST /api/pedidos/{idOrden}/generar-auto
// * Devuelve la lista de pedidos finales creados a partir de esa orden.
// */
//export async function generarAuto(idOrden) {
//    const res = await fetch(`${BASE}/${idOrden}/generar-auto`, { method: 'POST' });
//    if (!res.ok) {
//        const text = await res.text();
//        throw new Error(`Error generando pedidos finales para orden ${idOrden}: ${text}`);
//    }
//    return await res.json();
//}

// src/api/pedidos.js
// src/api/pedidos.js
const BASE = '/api/pedidos';

// 1) Listar todos los pedidos finales
export async function fetchPedidosFinales() {
    const res = await fetch(BASE);
    if (!res.ok) throw new Error('Error cargando pedidos finales');
    return res.json();
}

// 2) Obtener un pedido final en particular
export async function fetchPedidoFinal(id) {
    const res = await fetch(`${BASE}/${id}`);
    if (!res.ok) throw new Error('Error cargando pedido final');
    return res.json();
}

// 3) Crear un pedido final (normalmente no se usa en flujo automático, pero lo dejamos)
//    DTO = { idOrden, idProveedor, montoTotal, fechaPedido, fechaEntrega, estado, detalles: [ { idProducto, cantidad, cotizacion, iva } ] }
export async function createPedidoFinal(dto) {
    const res = await fetch(BASE, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error creando pedido final: ${txt}`);
    }
    return res.json();
}

// 4) Eliminar un pedido final
export async function deletePedidoFinal(id) {
    const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error eliminando pedido final: ${txt}`);
    }
}
