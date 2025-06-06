// src/pages/ListaProductos.jsx
import React, { useEffect, useState } from 'react';
import DataTable from 'react-data-table-component';
import { FaRegEdit, FaTrash, FaEye } from 'react-icons/fa';
import { Link, useNavigate } from 'react-router-dom';
import { fetchProductos } from '../api/productos';

export default function ListaProductos() {
    const [productos, setProductos] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        load();
    }, []);

    async function load() {
        try {
            setLoading(true);
            const list = await fetchProductos();
            setProductos(list);
        } catch (e) {
            console.error(e);
            alert('Error cargando productos');
        } finally {
            setLoading(false);
        }
    }

    async function handleDelete(idProducto) {
        if (!window.confirm('¿Eliminar este producto?')) return;
        // Aquí podrías llamar a deleteProducto(idProducto) y luego load().
        alert('Función eliminar no implementada en este ejemplo');
    }

    const columns = [
        { name: 'ID', selector: row => row.idProducto, sortable: true },
        { name: 'Nombre', selector: row => row.nombre, sortable: true },
        { name: 'Categoría', selector: row => row.nombreCategoria, sortable: true },
        { name: 'Stock Actual', selector: row => row.cantidadTotal, sortable: true },
        { name: 'Stock Mínimo', selector: row => row.cantidadMinima, sortable: true },
        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-2">
                    <button
                        onClick={() => navigate(`/productos/${row.idProducto}/detalles`)}
                        title="Ver Detalles"
                        className="text-green-600 hover:text-green-800"
                    >
                        <FaEye size={18} />
                    </button>
                    <button
                        onClick={() => navigate(`/productos/${row.idProducto}/editar`)}
                        title="Editar"
                        className="text-sky-600 hover:text-sky-800"
                    >
                        <FaRegEdit size={18} />
                    </button>
                    <button
                        onClick={() => handleDelete(row.idProducto)}
                        title="Eliminar"
                        className="text-red-600 hover:text-red-800"
                    >
                        <FaTrash size={18} />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true
        }
    ];

    if (loading) return <p className="p-4">Cargando productos…</p>;
    if (productos.length === 0) return <p className="p-4">No hay productos registrados.</p>;

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold text-sky-600">Productos</h2>
                <Link to="/productos/nuevo">
                    <button className="bg-sky-500 text-white px-4 py-2 rounded hover:bg-sky-600">
                        + Nuevo Producto
                    </button>
                </Link>
            </div>
            <DataTable columns={columns} data={productos} pagination highlightOnHover />
        </div>
    );
}
