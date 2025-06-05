    //// src/api/productos.js
//const BASE = '/api/Productos';

//export async function fetchProductos() {
//    const res = await fetch(BASE);
//    if (!res.ok) throw new Error('Error cargando productos');
//    return res.json();
//}

//export async function fetchProducto(id) {
//    const res = await fetch(`${BASE}/${id}`);
//    if (!res.ok) throw new Error('Error cargando producto');
//    return res.json();
//}

//export async function createProducto(prod) {
//    const res = await fetch(BASE, {
//        method: 'POST',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify(prod),
//    });
//    if (!res.ok) throw new Error('Error creando producto');
//    return res.json();
//}

//export async function updateProducto(id, prod) {
//    const res = await fetch(`${BASE}/${id}`, {
//        method: 'PUT',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify({ ...prod, idProducto: id }),
//    });
//    if (!res.ok) throw new Error('Error actualizando producto');
//}

//export async function deleteProducto(id) {
//    const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
//    if (!res.ok) throw new Error('Error eliminando producto');
//}

// src/api/productos.js
const BASE = '/api/productos';

export async function fetchProductos() {
    const res = await fetch(`${BASE}`);
    if (!res.ok) throw new Error(`Error ${res.status} en /productos`);
    return await res.json(); // regresará array de ProductoReadDto
}

// Endpoint para “stock-bajo” (asegúrate de que en tu backend exista /api/productos/stock-bajo):
export async function fetchStockProductos() {
    const res = await fetch(`${BASE}/stock-bajo`);
    if (!res.ok) throw new Error(`Error ${res.status} en /productos/stock-bajo`);
    return await res.json(); // regresará array de ProductoReadDto con NombreCategoria
}

// Devuelve un solo producto por su id
export async function fetchProducto(id) {
    const res = await fetch(`${BASE}/${id}`);
    if (!res.ok) throw new Error('Error cargando producto');
    return res.json();
}

// Crea un producto nuevo (espera un DTO con { nombre, marca, idCategoria, cantidadTotal, cantidadMinima })
export async function createProducto(dto) {
    const res = await fetch(BASE, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error creando producto: ${txt}`);
    }
    return res.json();
}

// Actualiza un producto (se espera el DTO igual que en create, y en URL el id)
export async function updateProducto(id, dto) {
    const res = await fetch(`${BASE}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto),
    });
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error actualizando producto: ${txt}`);
    }
}

// Elimina un producto
export async function deleteProducto(id) {
    const res = await fetch(`${BASE}/${id}`, { method: 'DELETE' });
    if (!res.ok) {
        const txt = await res.text();
        throw new Error(`Error eliminando producto: ${txt}`);
    }
}
