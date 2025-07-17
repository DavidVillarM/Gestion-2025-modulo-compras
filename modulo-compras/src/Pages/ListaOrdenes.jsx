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
import { BiCommentDetail } from "react-icons/bi";

import ModalDetalles from "./ModalDetalles";


export default function ListaOrdenes() {
    const [ordenes, setOrdenes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [filtrarId, setFiltrarId] = useState(""); // Estado para el filtro
    const navigate = useNavigate();
    const [mostrar, setMostrar] = useState(false);
    const [detalles, setDetalles] = useState([]);
    const [estado, setEstado] = useState([]);
    const [ordenN, setOrdenN] = useState([]);

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
            alert("Error cargando Ordenes");
        } finally {
            setLoading(false);
        }
    }

    async function handleDelete(idOrden) {
        if (!window.confirm("¿Eliminar esta orden?")) return;
        try {
            await deleteOrden(idOrden);
            load();
        } catch (e) {
            console.error(e);
            alert("Error eliminando orden");
        }
    }

    const filteredOrdenes = ordenes.filter((orden) =>
        orden.idOrden.toString().includes(filtrarId.trim())
    );

    const customStyles = {
        rows: {
            style: {
                display: 'flex',
                justifyContent: 'flex-end',
            },
        },
        headCells: {
            style: {
                width: '200px',
            },
        },
        cells: {
            style: {

                minWidth: '200px',
                display: 'flex',
                justifyContent: 'flex-start',
            },
        },
    };
    const columns = [
        {
            name: "ID", selector: (row) => row.idOrden,
            sortable: true,

        },
        {
            name: "Fecha",
            selector: (row) => new Date(row.fecha).toLocaleString(),
            sortable: true,



        },
        {
            name: "Estado",
            selector: (row) => row.estado,
            sortable: true,

        },
        {
            name: "# Detalles",
            selector: (row) => row.detalles.length,
            sortable: true,

        },
        {
            name: "Acciones",
            width: "200px",

            cell: (row) => (
                <div className="flex gap-4">
                    <button
                        onClick={() => {
                            const lista = row.detalles.map((d) => `${d.nombreProducto} x ${d.cantidad}`);
                            setDetalles(lista); 
                            setEstado(row.estado);
                            setOrdenN(row.idOrden);
                            setMostrar(true);



                        }}
                        title="Ver Detalles"
                        className="text-gray-600 hover:text-green-500"
                    >
                        <FaEye size={20} />
                    </button>

                    <button
                        onClick={() => navigate(`/ordenes/${row.idOrden}`)}
                        title="Editar Orden"
                        className="text-gray-600 hover:text-sky-400"
                    >
                        <FaRegEdit size={20} />
                    </button>

                    {
                        row.estado === "INCOMPLETA" && (
                            <button
                                onClick={() =>
                                    navigate(`/ordenes/${row.idOrden}/proveedores`)
                                }
                                title="Solicitar Cotización"
                                className="text-blue-600 hover:text-blue-800"
                            >
                                <FaCartPlus size={20} />
                            </button>
                        )
                    }

                    {
                        row.estado === "PENDIENTE" && (
                            <button
                                onClick={() =>
                                    navigate(`/ordenes/${row.idOrden}/presupuestos`)
                                }
                                title="Ver Cotizaciones"
                                className="text-gray-600 hover:text-yellow-800"
                            >
                                <BiCommentDetail size={20} />
                            </button>
                        )
                    }

                    < button
                        onClick={() => handleDelete(row.idOrden)
                        }
                        title="Eliminar Orden"
                        className="text-gray-600 hover:text-red-600"
                    >
                        <FaTrash size={17} />
                    </button >
                </div >
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
        },
    ];

    if (loading) return <p className="p-4">Cargando Órdenes...</p>;
    if (ordenes.length === 0) return <p className="p-4">No hay Órdenes creadas.</p>;

    return (

        <div className="p-6 bg-gray-100 min-h-screen">
            <ModalDetalles  
                show={mostrar}
                datos={detalles}
                orden={ordenN}
                estado={estado}
                onClose={() => setMostrar(false)}

            />
            <div className="flex justify-between items-center mb-4 gap-4">
                <h2 className="text-xl font-bold text-sky-600">
                    Órdenes de Compra (Solicitudes)
                </h2>
                <input
                    placeholder="Buscar por ID"
                    type="text"
                    value={filtrarId}
                    onChange={(e) => setFiltrarId(e.target.value)}
                    className="border border-gray-400 bg-white rounded-sm h-8 px-2"
                />
                <Link to="/ordenes/nueva">
                    <button className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
                        + Nueva Orden
                    </button>
                </Link>
            </div>
            <DataTable
                columns={columns}
                data={filteredOrdenes}
                customStyles={customStyles}
                pagination
                highlightOnHover
                persistTableHead
            />
        </div>
    );
}