// src/components/ListaProveedores.jsx
import React, { useEffect, useState } from 'react';
import DataTable from 'react-data-table-component';
import { FaRegEdit, FaTrash, FaEye } from 'react-icons/fa';
import { Link, useNavigate } from 'react-router-dom';
import {
    fetchProveedores,
    deleteProveedor
} from '../api/proveedores';

export function ListaProveedores() {
    const [data, setData] = useState([]);
    const [filter, setFilter] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        load();
    }, []);

    async function load() {
        try {
            const list = await fetchProveedores();
            setData(list);
        } catch (e) {
            console.error(e);
        }
    }

    async function handleDelete(id) {
        if (!window.confirm('¿Eliminar este proveedor?')) return;
        try {
            await deleteProveedor(id);
            load();
        } catch (e) {
            console.error(e);
        }
    }

    const columns = [
        { name: 'Nombre', selector: row => row.nombre, sortable: true },
        {
            name: 'Categorías',
            selector: row =>
                // Ahora usamos `row.categorias` que viene del DTO
                (row.categorias || [])
                    .map(c => c.nombre)
                    .join(', '),
            sortable: true
        },
        { name: 'Contacto', selector: row => row.telefono, sortable: true },
        { name: 'RUC', selector: row => row.ruc, sortable: true },
        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-4">
                    <button
                        onClick={() => navigate(`/proveedores/${row.idProveedor}/detalles`)}
                        title="Ver detalles"
                        className="text-green-600 hover:text-green-800"
                    >
                        <FaEye size={18} />
                    </button>
                    <button
                        onClick={() => navigate(`/proveedores/${row.idProveedor}/editar`)}
                        title="Editar"
                        className="text-sky-600 hover:text-sky-800"
                    >
                        <FaRegEdit size={18} />
                    </button>
                    <button
                        onClick={() => handleDelete(row.idProveedor)}
                        title="Eliminar"
                        className="text-red-600 hover:text-red-800"
                    >
                        <FaTrash size={18} />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
        }
    ];

    const filtered = data.filter(p =>
        p.nombre.toLowerCase().includes(filter.toLowerCase())
    );

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold text-sky-600">Proveedores</h2>
                <Link to="/proveedores/nuevo">
                    <button className="bg-sky-500 text-white px-4 py-2 rounded hover:bg-sky-600">
                        + Nuevo Proveedor
                    </button>
                </Link>
            </div>

            <div className="mb-4">
                <input
                    type="text"
                    placeholder="Buscar por nombre…"
                    value={filter}
                    onChange={e => setFilter(e.target.value)}
                    className="w-full border rounded p-2"
                />
            </div>

            <DataTable
                columns={columns}
                data={filtered}
                pagination
                highlightOnHover
            />
        </div>
    );
}

export default ListaProveedores;
