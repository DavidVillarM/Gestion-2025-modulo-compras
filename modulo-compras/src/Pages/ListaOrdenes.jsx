//// src/pages/ListaOrdenes.jsx
//import React, { useEffect, useState } from 'react';
//import DataTable from 'react-data-table-component';
//import { FaRegEdit, FaTrash, FaEye, FaCartPlus } from 'react-icons/fa';
//import { Link, useNavigate } from 'react-router-dom';

//import {
//    fetchOrdenes,
//    deleteOrden,
//    fetchProveedoresParaOrden
//} from '../api/ordenes';

//export default function ListaOrdenes() {
//    const [ordenes, setOrdenes] = useState([]);
//    const [loading, setLoading] = useState(true);
//    const navigate = useNavigate();

//    useEffect(() => {
//        load();
//    }, []);

//    async function load() {
//        try {
//            setLoading(true);
//            const list = await fetchOrdenes();
//            setOrdenes(list);
//        } catch (e) {
//            console.error(e);
//            alert('Error cargando �rdenes: ' + e.message);
//        } finally {
//            setLoading(false);
//        }
//    }

//    async function handleDelete(idOrden) {
//        if (!window.confirm('�Eliminar esta orden?')) return;
//        try {
//            await deleteOrden(idOrden);
//            load();
//        } catch (e) {
//            console.error(e);
//            alert('Error eliminando orden: ' + e.message);
//        }
//    }

//    const handleSolicitar = (idOrden) => {
//        if (!window.confirm('�Seguro que quieres solicitar cotizaci�n para esta orden?'))
//            return;

//        fetchProveedoresParaOrden(idOrden)
//            .then(proveedores => {
//                const ids = proveedores.map(p => p.idProveedor);
//                navigate(`/ordenes/${idOrden}/proveedores`, {
//                    state: { todos: ids }
//                });
//            })
//            .catch(err => {
//                console.error(err);
//                alert('Error al obtener proveedores para cotizaci�n.');
//            });
//    };

//    const columns = [
//        { name: 'ID', selector: row => row.idOrden, sortable: true },
//        {
//            name: 'Fecha',
//            selector: row => new Date(row.fecha).toLocaleString(),
//            sortable: true
//        },
//        { name: 'Estado', selector: row => row.estado, sortable: true },
//        {
//            name: '# Detalles',
//            selector: row => row.detalles.length,
//            sortable: true
//        },
//        {
//            name: 'Acciones',
//            cell: row => (
//                <div className="flex gap-4">
//                    <button
//                        onClick={() => {
//                            const lista = row.detalles
//                                .map(d => `� ${d.nombreProducto} x ${d.cantidad}`)
//                                .join('\n');
//                            alert(`Detalles Orden #${row.idOrden}:\n` + lista);
//                        }}
//                        title="Ver Detalles"
//                        className="text-green-600 hover:text-green-800"
//                    >
//                        <FaEye size={18} />
//                    </button>

//                    <button
//                        onClick={() => navigate(`/ordenes/${row.idOrden}`)}
//                        title="Editar Orden"
//                        className="text-sky-600 hover:text-sky-800"
//                    >
//                        <FaRegEdit size={18} />
//                    </button>

//                    {row.estado === 'INCOMPLETA' && (
//                        <button
//                            onClick={() => handleSolicitar(row.idOrden)}
//                            title="Solicitar Cotizaci�n"
//                            className="text-blue-600 hover:text-blue-800"
//                        >
//                            <FaCartPlus size={18} />
//                        </button>
//                    )}

//                    {row.estado === 'PENDIENTE' && (
//                        <button
//                            onClick={() => navigate(`/ordenes/${row.idOrden}/presupuestos`)}
//                            title="Ver Cotizaciones"
//                            className="text-yellow-600 hover:text-yellow-800"
//                        >
//                            <FaEye size={18} />
//                        </button>
//                    )}

//                    <button
//                        onClick={() => handleDelete(row.idOrden)}
//                        title="Eliminar Orden"
//                        className="text-red-600 hover:text-red-800"
//                    >
//                        <FaTrash size={18} />
//                    </button>
//                </div>
//            ),
//            ignoreRowClick: true,
//            allowOverflow: true,
//            button: true
//        }
//    ];

//    if (loading) return <p className="p-4">Cargando �rdenes�</p>;
//    if (ordenes.length === 0) return <p className="p-4">No hay �rdenes creadas.</p>;

//    return (
//        <div className="p-6 bg-gray-100 min-h-screen">
//            <div className="flex justify-between items-center mb-4">
//                <h2 className="text-xl font-bold text-sky-600">
//                    �rdenes de Compra (Solicitudes)
//                </h2>
//                <Link to="/ordenes/nueva">
//                    <button className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
//                        + Nueva Orden
//                    </button>
//                </Link>
//            </div>
//            <DataTable
//                columns={columns}
//                data={ordenes}
//                pagination
//                highlightOnHover
//            />
//        </div>
//    );
//}

// src/pages/ListaOrdenes.jsx
import React, { useEffect, useState } from "react";
import DataTable from "react-data-table-component";
import {
    FaRegEdit,
    FaTrash,
    FaEye,
    FaCartPlus,
} from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import { fetchOrdenes, deleteOrden } from "../api/ordenes";

export default function ListaOrdenes() {
    const [ordenes, setOrdenes] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        load();
    }, []);

    async function load() {
        setLoading(true);
        try {
            const list = await fetchOrdenes();
            setOrdenes(list);
        } catch (e) {
            console.error(e);
            alert("Error cargando ordenes");
        } finally {
            setLoading(false);
        }
    }

    async function handleDelete(idOrden) {
        if (!window.confirm("Eliminar esta orden?")) return;
        try {
            await deleteOrden(idOrden);
            load();
        } catch (e) {
            console.error(e);
            alert("Error eliminando orden");
        }
    }

    const columns = [
        { name: "ID", selector: (row) => row.idOrden, sortable: true },
        {
            name: "Fecha",
            selector: (row) => new Date(row.fecha).toLocaleString(),
            sortable: true,
        },
        { name: "Estado", selector: (row) => row.estado, sortable: true },
        {
            name: "# Detalles",
            selector: (row) => row.detalles.length,
            sortable: true,
        },
        {
            name: "Acciones",
            cell: (row) => (
                <div className="flex gap-4">
                    <button
                        onClick={() => {
                            const lista = row.detalles
                                .map((d) => `� ${d.nombreProducto} x ${d.cantidad}`)
                                .join("\n");
                            alert(`Detalles Orden #${row.idOrden}:\n` + lista);
                        }}
                        title="Ver Detalles"
                        className="text-green-600 hover:text-green-800"
                    >
                        <FaEye size={18} />
                    </button>

                    <button
                        onClick={() => navigate(`/ordenes/${row.idOrden}`)}
                        title="Editar Orden"
                        className="text-sky-600 hover:text-sky-800"
                    >
                        <FaRegEdit size={18} />
                    </button>

                    {row.estado === "INCOMPLETA" && (
                        <button
                            onClick={() =>
                                navigate(`/ordenes/${row.idOrden}/proveedores`)
                            }
                            title="Solicitar Cotizacion"
                            className="text-blue-600 hover:text-blue-800"
                        >
                            <FaCartPlus size={18} />
                        </button>
                    )}

                    {row.estado === "PENDIENTE" && (
                        <button
                            onClick={() =>
                                navigate(`/ordenes/${row.idOrden}/presupuestos`)
                            }
                            title="Ver Cotizaciones"
                            className="text-yellow-600 hover:text-yellow-800"
                        >
                            <FaEye size={18} />
                        </button>
                    )}

                    <button
                        onClick={() => handleDelete(row.idOrden)}
                        title="Eliminar Orden"
                        className="text-red-600 hover:text-red-800"
                    >
                        <FaTrash size={18} />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
        },
    ];

    if (loading) return <p className="p-4">Cargando ordenes</p>;
    if (ordenes.length === 0) return <p className="p-4">No hay ordenes creadas.</p>;

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold text-sky-600">
                    ordenes de Compra (Solicitudes)
                </h2>
                <Link to="/ordenes/nueva">
                    <button className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
                        + Nueva Orden
                    </button>
                </Link>
            </div>
            <DataTable
                columns={columns}
                data={ordenes}
                pagination
                highlightOnHover
            />
        </div>
    );
}
