// src/Pages/FormProductos.jsx
import React, { useEffect, useState } from 'react';
import {
    fetchProducto,
    createProducto,
    updateProducto
} from '../api/productos';
import { fetchCategorias } from '../api/categorias'; // suponiendo que lo tienes
import { useNavigate, useParams } from 'react-router-dom';

export default function FormProductos() {
    const { id } = useParams();
    const isNew = !id;
    const navigate = useNavigate();

    const [form, setForm] = useState({
        nombre: '',
        marca: '',
        idCategoria: '',
        cantidadTotal: '',
        cantidadMinima: ''
    });
    const [allCats, setAllCats] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        // cargar categorías
        fetchCategorias().then(setAllCats).catch(console.error);

        if (!isNew) {
            fetchProducto(id)
                .then(p => {
                    setForm({
                        nombre: p.nombre,
                        marca: p.marca,
                        idCategoria: p.idCategoria || '',
                        cantidadTotal: p.cantidadTotal || '',
                        cantidadMinima: p.cantidadMinima || ''
                    });
                })
                .catch(console.error);
        }
    }, [id, isNew]);

    const handleChange = e => {
        const { name, value } = e.target;
        setForm(f => ({ ...f, [name]: value }));
    };

    const handleSubmit = async e => {
        e.preventDefault();
        setError(null);
        const payload = {
            nombre: form.nombre,
            marca: form.marca,
            idCategoria: form.idCategoria ? parseInt(form.idCategoria) : null,
            cantidadTotal: parseInt(form.cantidadTotal) || 0,
            cantidadMinima: parseInt(form.cantidadMinima) || 0
        };
        try {
            if (isNew) {
                await createProducto(payload);
            } else {
                await updateProducto(id, payload);
            }
            navigate('/productos');
        } catch (err) {
            console.error(err);
            setError(err.message);
        }
    };

    return (
        <div className="p-6 bg-gray-50 min-h-screen">
            <h2 className="text-2xl font-bold text-sky-600 mb-6">
                {isNew ? 'Agregar Producto' : 'Editar Producto'}
            </h2>

            {error && (
                <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">{error}</div>
            )}

            <form onSubmit={handleSubmit} className="grid grid-cols-2 gap-6 max-w-2xl">
                {/* Nombre */}
                <div className="col-span-2">
                    <label className="block mb-1 font-medium">Nombre</label>
                    <input
                        name="nombre"
                        value={form.nombre}
                        onChange={handleChange}
                        type="text"
                        required
                        className="w-full border rounded p-2"
                    />
                </div>

                {/* Marca */}
                <div>
                    <label className="block mb-1 font-medium">Marca</label>
                    <input
                        name="marca"
                        value={form.marca}
                        onChange={handleChange}
                        type="text"
                        className="w-full border rounded p-2"
                    />
                </div>

                {/* Categoría */}
                <div>
                    <label className="block mb-1 font-medium">Categoría</label>
                    <select
                        name="idCategoria"
                        value={form.idCategoria}
                        onChange={handleChange}
                        className="w-full border rounded p-2"
                    >
                        <option value="">— Seleccione —</option>
                        {allCats.map(c => (
                            <option key={c.idCategoria} value={c.idCategoria}>
                                {c.nombre}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Cantidad total */}
                <div>
                    <label className="block mb-1 font-medium">Stock actual</label>
                    <input
                        name="cantidadTotal"
                        value={form.cantidadTotal}
                        onChange={handleChange}
                        type="number"
                        className="w-full border rounded p-2"
                    />
                </div>

                {/* Cantidad mínima */}
                <div>
                    <label className="block mb-1 font-medium">Stock mínimo</label>
                    <input
                        name="cantidadMinima"
                        value={form.cantidadMinima}
                        onChange={handleChange}
                        type="number"
                        className="w-full border rounded p-2"
                    />
                </div>

                {/* Botones */}
                <div className="col-span-2 flex justify-end gap-4 mt-4">
                    <button
                        type="button"
                        onClick={() => navigate(-1)}
                        className="px-4 py-2 bg-gray-400 text-white rounded hover:bg-gray-500"
                    >
                        Cancelar
                    </button>
                    <button
                        type="submit"
                        className="px-4 py-2 bg-sky-500 text-white rounded hover:bg-sky-600"
                    >
                        Guardar
                    </button>
                </div>
            </form>
        </div>
    );
}
