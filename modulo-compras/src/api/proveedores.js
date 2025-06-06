const BASE = '/api';

export async function fetchProveedores() {
    const res = await fetch('/api/proveedores');
    if (!res.ok) throw new Error('Error cargando proveedores');
    return res.json();
}

export async function fetchProveedor(id) {
    const res = await fetch(`/api/proveedores/${id}`);
    if (!res.ok) throw new Error('Error cargando proveedor');
    return res.json();
}

export async function createProveedor(data) {
    const res = await fetch('/api/proveedores', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            ruc: data.ruc,
            nombre: data.nombre,
            nombreContacto: data.nombreContacto,
            telefono: data.telefono,
            correo: data.correo,
            categoriaIds: data.categoriaIds
        })
    });
    if (!res.ok) {
        const err = await res.text();
        throw new Error(`Error creando proveedor: ${err}`);
    }
    return res.json();
}

export async function updateProveedor(id, data) {
    const res = await fetch(`/api/proveedores/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            idProveedor: parseInt(id),
            ruc: data.ruc,
            nombre: data.nombre,
            nombreContacto: data.nombreContacto,
            telefono: data.telefono,
            correo: data.correo,
            categoriaIds: data.categoriaIds
        })
    });
    if (!res.ok) {
        const err = await res.text();
        throw new Error(`Error actualizando proveedor: ${err}`);
    }
}

export async function deleteProveedor(id) {
    const res = await fetch(`/api/proveedores/${id}`, { method: 'DELETE' });
    if (!res.ok) {
        const err = await res.text();
        throw new Error(`Error eliminando proveedor: ${err}`);
    }
}
