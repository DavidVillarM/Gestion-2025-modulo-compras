//// src/api/ordenes.js
//const BASE = '/api/ordenes';

//export async function fetchOrdenes() {
//    const res = await fetch(BASE);
//    if (!res.ok) throw new Error('Error cargando órdenes');
//    return await res.json();
//}

//export async function fetchOrden(id) {
//    const res = await fetch(`${BASE}/${id}`);
//    if (!res.ok) throw new Error(`Error cargando la orden ${id}`);
//    return await res.json();
//}

///**
// * dto = {
// *   Estado,                             // "INCOMPLETA" | "PENDIENTE" | "COMPLETA"
// *   OrdenDetalles: [ { IdProducto, Cantidad }, … ]
// * }
// */
//export async function createOrden(dto) {
//    const res = await fetch(BASE, {
//        method: 'POST',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify(dto)
//    });
//    if (!res.ok) {
//        const text = await res.text();
//        throw new Error(`Error creando orden: ${text}`);
//    }
//    return await res.json();
//}

//export async function updateOrden(id, dto) {
//    const res = await fetch(`${BASE}/${id}`, {
//        method: 'PUT',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify(dto)
//    });
//    if (!res.ok) {
//        const text = await res.text();
//        throw new Error(`Error actualizando orden ${id}: ${text}`);
//    }
//}

//export async function deleteOrden(id) {
//    const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
//    if (!res.ok) {
//        const text = await res.text();
//        throw new Error(`Error eliminando orden ${id}: ${text}`);
//    }
//}

///**
// * Obtiene la lista de proveedores que pueden cotizar
// * ESTA RUTA QUEDA IGUAL: /api/ordenes/{id}/proveedores
// */
//export async function fetchProveedoresParaOrden(ordenId) {
//    const res = await fetch(`${BASE}/${ordenId}/proveedores`);
//    if (!res.ok) throw new Error(`Error cargando proveedores para orden ${ordenId}`);
//    return await res.json();
//}

// src/api/ordenes.js
// src/api/ordenes.js
// src/api/ordenes.js
const BASE = '/api/ordenes';

async function request(path, opts = {}) {
    const res = await fetch(BASE + path, {
        ...opts,
        headers: {
            'Content-Type': 'application/json',
            ...(opts.headers || {})
        }
    });
    if (!res.ok) {
        // Intentamos leer un JSON con “message” o “Message”
        let errBody = null;
        try {
            errBody = await res.json();
        } catch {
            /* no es JSON */
        }
        const msg = (errBody && (errBody.message || errBody.Message)) || res.statusText;
        throw new Error(`Error ${res.status} en /ordenes${path}: ${msg}`);
    }
    // Si no hay contenido, res.json() lanzará error. En ese caso retornamos null.
    try {
        return await res.json();
    } catch {
        return null;
    }
}

export async function fetchOrdenes() {
    return request('');
}

export async function fetchOrden(idOrden) {
    const res = await fetch(`/api/ordenes/${idOrden}`);
    if (!res.ok) throw new Error("No se pudo cargar la orden");
    return await res.json();
}

export async function createOrden(dto) {
    return request('', {
        method: 'POST',
        body: JSON.stringify(dto)
    });
}

export async function updateOrden(id, dto) {
    return request(`/${id}`, {
        method: 'PUT',
        body: JSON.stringify(dto)
    });
}

export async function deleteOrden(id) {
    return request(`/${id}`, {
        method: 'DELETE'
    });
}

// -------------------------------------------------------------
// 6.1 GET /api/ordenes/{id}/proveedores  (solo lista, no muta)
export async function fetchProveedoresParaOrden(id) {
    return request(`/${id}/proveedores`);
}


export async function marcarPendiente(idOrden) {
    const res = await fetch(`/api/ordenes/${idOrden}/marcar-pendiente`, {
        method: "PUT",
    });
    if (!res.ok) {
        const error = await res.json();
        throw new Error(error.Message || "Error marcando la orden");
    }
}

// 7) Generar pedidos finales (ya existía)
//    Nota: Asegúrate de que en el back la ruta sea POST /api/pedidos/{id}/generar-auto
export async function generarPedidosAuto(id) {
    return request(`/pedidos/${id}/generar-auto`, {
        method: 'POST'
    });
}
