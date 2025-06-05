// src/api/presupuestos.js
// src/api/presupuestos.js
const BASE = '/api/ordenes';

// 1) Listar todos los presupuestos para una orden 
//    GET /api/ordenes/{ordenId}/presupuestos
export async function fetchPresupuestosPorOrden(ordenId) {
    const res = await fetch(`${BASE}/${ordenId}/presupuestos`);
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error cargando cotizaciones: ${txt}`);
    }
    return res.json();
}
// 2) Obtener un presupuesto en particular 
//    GET /api/ordenes/{ordenId}/presupuestos/{idPresupuesto}
export async function fetchPresupuesto(ordenId, idPresupuesto) {
    const res = await fetch(`${BASE}/${ordenId}/presupuestos/${idPresupuesto}`);
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error cargando cotización: ${txt}`);
    }
    return res.json();
}

// 2) Crear presupuesto para una orden (DTO = { idProveedor, fechaEntrega, subtotal, iva5, iva10, total, detalles: [ { idProducto, cantidad, precio, iva5, iva10 } ] } )
//    POST /api/ordenes/{ordenId}/presupuestos
export async function createPresupuesto(ordenId, dto) {
    const res = await fetch(`${BASE}/${ordenId}/presupuestos`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error creando cotización: ${txt}`);
    }
    // el back retorna `{ presuId: <nuevoId> }`, o similar
    return res.json();
}
// 3) Actualizar presupuesto
//    PUT /api/ordenes/{ordenId}/presupuestos/{idPresupuesto}
export async function updatePresupuesto(ordenId, idPresupuesto, dto) {
    const res = await fetch(
        `${BASE}/${ordenId}/presupuestos/${idPresupuesto}`,
        {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto),
        }
    );
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error actualizando cotización: ${txt}`);
    }
}

// 4) Eliminar presupuesto
//    DELETE /api/ordenes/{ordenId}/presupuestos/{idPresupuesto}
export async function deletePresupuesto(ordenId, idPresupuesto) {
    const res = await fetch(
        `${BASE}/${ordenId}/presupuestos/${idPresupuesto}`,
        { method: 'DELETE' }
    );
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error eliminando cotización: ${txt}`);
    }
}

// 1) Obtener todos los presupuestos
export async function fetchPresupuestosGlobal() {
    const res = await fetch("/api/presupuestos");
    if (!res.ok) {
        const err = await res.json();
        throw new Error(err.Message || "Error cargando presupuestos");
    }
    return await res.json();
}

// 2) Eliminar un presupuesto globalmente
export async function deletePresupuestoGlobal(idPresu, idOrden) {
    // Construimos la URL exacta de borrado:
    const url = `${BASE}/${idOrden}/presupuestos/${idPresu}`;
    const res = await fetch(url, {
        method: "DELETE",
    });
    if (!res.ok) {
        // Intentamos leer el JSON de error enviado por el back
        let errBody = null;
        try {
            errBody = await res.json();
        } catch {
            /* no es JSON, no pasa nada */
        }
        const msg = (errBody && (errBody.Message || errBody.message)) || "Error eliminando presupuesto";
        throw new Error(msg);
    }
}