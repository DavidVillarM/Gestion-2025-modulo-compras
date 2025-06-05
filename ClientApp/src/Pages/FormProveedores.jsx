import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { fetchProveedor, createProveedor, updateProveedor } from '../api/proveedores';
import { fetchCategorias } from '../api/categorias';

export function FormProveedores() {
    const { id } = useParams();
    const isEdit = Boolean(id);
    const navigate = useNavigate();

    const [form, setForm] = useState({
        ruc: '',
        nombre: '',
        nombreContacto: '',
        telefono: '',
        correo: '',
        categoriaIds: [],
    });
    const [allCats, setAllCats] = useState([]);
    const [error, setError] = useState(null);

    // Carga inicial de categorías y datos si es edición
    useEffect(() => {
        fetchCategorias()
            .then(data => setAllCats(data))
            .catch(err => console.error(err));

        if (isEdit) {
            fetchProveedor(id)
                .then(p => setForm({
                    ruc: p.ruc || '',
                    nombre: p.nombre || '',
                    nombreContacto: p.nombreContacto || '',
                    telefono: p.telefono || '',
                    correo: p.correo || '',
                    categoriaIds: p.categorias?.map(c => c.idCategoria) || []
                }))
                .catch(err => {
                    console.error(err);
                    alert('Error cargando proveedor');
                    navigate('/proveedores');
                });
        }
    }, [id, isEdit, navigate]);

    const handleChange = e => {
        const { name, value, type, checked } = e.target;
        if (type === 'checkbox') {
            setForm(prev => {
                const ids = prev.categoriaIds;
                return checked
                    ? { ...prev, categoriaIds: [...ids, parseInt(value)] }
                    : { ...prev, categoriaIds: ids.filter(i => i !== parseInt(value)) };
            });
        } else {
            setForm(prev => ({ ...prev, [name]: value }));
        }
    };

    const handleSubmit = async e => {
        e.preventDefault();
        setError(null);
        try {
            if (isEdit) {
                await updateProveedor(id, form);
            } else {
                await createProveedor(form);
            }
            navigate('/proveedores');
        } catch (err) {
            console.error(err);
            setError(err.message);
        }
    };

    return (
        <div className="p-6 bg-gray-50 min-h-screen">
            <h2 className="text-2xl font-bold text-sky-600 mb-6">
                {isEdit ? 'Editar Proveedor' : 'Agregar Proveedor'}
            </h2>
            {error && <div className="mb-4 p-3 bg-red-100 text-red-700 rounded">{error}</div>}
            <form onSubmit={handleSubmit} className="grid grid-cols-2 gap-6 max-w-2xl">
                {/* RUC */}
                <div className="col-span-2">
                    <label className="block mb-1 font-medium">RUC</label>
                    <input
                        name="ruc"
                        type="text"
                        required
                        value={form.ruc}
                        onChange={handleChange}
                        className="w-full border rounded p-2"
                    />
                </div>
                {/* Nombre */}
                <div>
                    <label className="block mb-1 font-medium">Nombre</label>
                    <input
                        name="nombre"
                        type="text"
                        required
                        value={form.nombre}
                        onChange={handleChange}
                        className="w-full border rounded p-2"
                    />
                </div>
                {/* Nombre de contacto */}
                <div>
                    <label className="block mb-1 font-medium">Nombre Contacto</label>
                    <input
                        name="nombreContacto"
                        type="text"
                        value={form.nombreContacto}
                        onChange={handleChange}
                        className="w-full border rounded p-2"
                    />
                </div>
                {/* Teléfono */}
                <div>
                    <label className="block mb-1 font-medium">Teléfono</label>
                    <input
                        name="telefono"
                        type="text"
                        value={form.telefono}
                        onChange={handleChange}
                        className="w-full border rounded p-2"
                    />
                </div>
                {/* Correo */}
                <div>
                    <label className="block mb-1 font-medium">Correo electrónico</label>
                    <input
                        name="correo"
                        type="email"
                        value={form.correo}
                        onChange={handleChange}
                        className="w-full border rounded p-2"
                    />
                </div>
                {/* Categorías */}
                <fieldset className="col-span-2 mb-6">
                    <legend className="font-medium mb-2">Categorías</legend>
                    <div className="grid grid-cols-2 gap-2">
                        {allCats.map(c => (
                            <label key={c.idCategoria} className="flex items-center gap-2">
                                <input
                                    type="checkbox"
                                    name="categoriaIds"
                                    value={c.idCategoria}
                                    checked={form.categoriaIds.includes(c.idCategoria)}
                                    onChange={handleChange}
                                    className="h-4 w-4"
                                />
                                <span>{c.nombre}</span>
                            </label>
                        ))}
                    </div>
                </fieldset>
                {/* Botones */}
                <div className="col-span-2 flex justify-end gap-4 mt-4">
                    <button
                        type="button"
                        onClick={() => navigate('/proveedores')}
                        className="px-4 py-2 bg-gray-400 text-white rounded hover:bg-gray-500"
                    >Cancelar</button>
                    <button
                        type="submit"
                        className="px-4 py-2 bg-sky-500 text-white rounded hover:bg-sky-600"
                    >Aceptar</button>
                </div>
            </form>
        </div>
    );
}
