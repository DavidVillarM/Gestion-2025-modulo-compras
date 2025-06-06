const BASE = '/api';

export async function fetchCategorias() {
    const res = await fetch(`${BASE}/categorias`);
    return res.json();
}
