
//// src/pages/ListaPresupuestos.jsx
//import React, { useEffect, useState } from "react";
//import { useNavigate, Link } from "react-router-dom";
//import DataTable from "react-data-table-component";
//import { FaEye, FaRegEdit, FaTrash, FaUpload } from "react-icons/fa";

//import {
//    fetchPresupuestosGlobal,
//    deletePresupuestoGlobal,
//} from "../api/presupuestos";

//export default function ListaPresupuestos() {
//    const navigate = useNavigate();
//    const [presupuestos, setPresupuestos] = useState([]);
//    const [loading, setLoading] = useState(true);

//    // 1) Traer TODOS los presupuestos (global)
//    useEffect(() => {
//        setLoading(true);
//        fetchPresupuestosGlobal()
//            .then((list) => {
//                setPresupuestos(list);
//            })
//            .catch((err) => {
//                console.error(err);
//                alert("Error cargando la lista de presupuestos: " + err.message);
//            })
//            .finally(() => setLoading(false));
//    }, []);

//    // 2) Eliminar un presupuesto (global)
//    const handleDelete = async (row) => {
//        if (!window.confirm("¿Eliminar esta cotización?")) return;
//        try {
//            // row.idPresupuesto y row.idOrden vienen en cada fila
//            await deletePresupuestoGlobal(row.idPresupuesto, row.idOrden);
//            // recargar la lista completa
//            const list = await fetchPresupuestosGlobal();
//            setPresupuestos(list);
//        } catch (err) {
//            console.error(err);
//            alert("Error eliminando el presupuesto: " + err.message);
//        }
//    };

//    // 3) Columnas para DataTable
//    const columns = [
//        {
//            name: "ID Presupuesto",
//            selector: (row) => row.idPresupuesto,
//            sortable: true,
//            width: "120px",
//        },
//        {
//            name: "ID Orden",
//            selector: (row) => row.idOrden,
//            sortable: true,
//            width: "100px",
//            cell: (row) => (
//                <Link
//                    to={`/ordenes/${row.idOrden}`}
//                    className="text-blue-600 hover:underline"
//                >
//                    {row.idOrden}
//                </Link>
//            ),
//        },
//        {
//            name: "Proveedor",
//            selector: (row) => row.nombreProveedor,
//            sortable: true,
//        },
//        {
//            name: "Fecha Entrega",
//            selector: (row) =>
//                row.fechaEntrega ? new Date(row.fechaEntrega).toLocaleDateString() : "--",
//            sortable: true,
//            width: "130px",
//        },
//        {
//            name: "Subtotal",
//            selector: (row) =>
//                row.detalles && row.detalles.length > 0
//                    ? row.subtotal.toFixed(2)
//                    : "--",
//            sortable: true,
//            right: true,
//            width: "100px",
//        },
//        {
//            name: "IVA5",
//            selector: (row) =>
//                row.detalles && row.detalles.length > 0 ? row.iva5.toFixed(2) : "--",
//            sortable: true,
//            right: true,
//            width: "100px",
//        },
//        {
//            name: "IVA10",
//            selector: (row) =>
//                row.detalles && row.detalles.length > 0 ? row.iva10.toFixed(2) : "--",
//            sortable: true,
//            right: true,
//            width: "100px",
//        },
//        {
//            name: "Total",
//            selector: (row) =>
//                row.detalles && row.detalles.length > 0 ? row.total.toFixed(2) : "--",
//            sortable: true,
//            right: true,
//            width: "100px",
//        },
//        {
//            name: "Estado",
//            selector: (row) =>
//                row.detalles && row.detalles.length > 0 ? "Cargado" : "Pendiente",
//            sortable: true,
//            width: "120px",
//            cell: (row) => (
//                <span
//                    className={
//                        row.detalles && row.detalles.length > 0
//                            ? "text-green-600 font-medium"
//                            : "text-red-600 font-medium"
//                    }
//                >
//                    {row.detalles && row.detalles.length > 0 ? "Cargado" : "Pendiente"}
//                </span>
//            ),
//        },
//        {
//            name: "Acciones",
//            cell: (row) => (
//                <div className="flex gap-3">
//                    {row.detalles && row.detalles.length > 0 ? (
//                        <>
//                            {/* Ver detalles */}
//                            <button
//                                onClick={() => {
//                                    const detallesTexto = row.detalles
//                                        .map(
//                                            (d) =>
//                                                `• ${d.nombreProducto} x ${d.cantidad} → Precio: ${d.precio.toFixed(
//                                                    2
//                                                )}, IVA5: ${d.iva5.toFixed(2)}, IVA10: ${d.iva10.toFixed(2)}`
//                                        )
//                                        .join("\n");
//                                    alert(
//                                        `Cotización #${row.idPresupuesto} (Orden #${row.idOrden})\n\n` +
//                                        detallesTexto +
//                                        `\n\nSubtotal: ${row.subtotal.toFixed(
//                                            2
//                                        )}\nIVA5: ${row.iva5.toFixed(
//                                            2
//                                        )}\nIVA10: ${row.iva10.toFixed(
//                                            2
//                                        )}\nTotal: ${row.total.toFixed(2)}`
//                                    );
//                                }}
//                                title="Ver Detalles"
//                                className="text-green-600 hover:text-green-800"
//                            >
//                                <FaEye size={18} />
//                            </button>

//                            {/* Editar cotización */}
//                            <button
//                                onClick={() =>
//                                    navigate(`/ordenes/${row.idOrden}/presupuestos/${row.idProveedor}`)
//                                }
//                                title="Editar Cotización"
//                                className="text-sky-600 hover:text-sky-800"
//                            >
//                                <FaRegEdit size={18} />
//                            </button>
//                        </>
//                    ) : (
//                        <>
//                            {/* Cargar cotización (pendiente) */}
//                            <button
//                                onClick={() =>
//                                    navigate(`/ordenes/${row.idOrden}/presupuestos/${row.idProveedor}`)
//                                }
//                                title="Cargar Cotización"
//                                className="text-blue-600 hover:text-blue-800"
//                            >
//                                <FaUpload size={18} />
//                            </button>
//                        </>
//                    )}

//                    {/* Eliminar cotización */}
//                    <button
//                        onClick={() => handleDelete(row)}
//                        title="Eliminar Cotización"
//                        className="text-red-600 hover:text-red-800"
//                    >
//                        <FaTrash size={18} />
//                    </button>
//                </div>
//            ),
//            ignoreRowClick: true,
//            allowOverflow: true,
//            button: true,
//            width: "150px",
//        },
//    ];

//    if (loading) return <p className="p-4">Cargando cotizaciones…</p>;

//    return (
//        <div className="p-6 bg-gray-100 min-h-screen">
//            <div className="flex justify-between items-center mb-4">
//                <h2 className="text-xl font-bold text-sky-600">Todas las Cotizaciones</h2>
//                <Link to="/ordenes" className="text-blue-600 hover:underline">
//                    ← Volver a Órdenes
//                </Link>
//            </div>

//            {presupuestos.length === 0 ? (
//                <p className="p-4">No hay cotizaciones registradas todavía.</p>
//            ) : (
//                <DataTable columns={columns} data={presupuestos} pagination highlightOnHover />
//            )}
//        </div>
//    );
//}
// src/pages/ListaPresupuestos.jsx
// src/pages/ListaPresupuestos.jsx
import React, { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import DataTable from "react-data-table-component";
import { FaEye, FaRegEdit, FaTrash, FaUpload } from "react-icons/fa";

import {
    fetchPresupuestosGlobal,
    deletePresupuestoGlobal,  // ahora espera (idPresu, idOrden)
} from "../api/presupuestos";

export default function ListaPresupuestos() {
    const navigate = useNavigate();
    const [presupuestos, setPresupuestos] = useState([]);
    const [loading, setLoading] = useState(true);

    // 1) Traer TODOS los presupuestos (global)
    useEffect(() => {
        setLoading(true);
        fetchPresupuestosGlobal()
            .then((list) => {
                setPresupuestos(list);
            })
            .catch((err) => {
                console.error(err);
                alert("Error cargando la lista de presupuestos: " + err.message);
            })
            .finally(() => setLoading(false));
    }, []);

    // 2) Eliminar un presupuesto (recibe *fila* entera para extraer idPresupuesto e idOrden)
    const handleDelete = async (row) => {
        if (!window.confirm("¿Eliminar esta cotización?")) return;
        try {
            // llamamos con ambos parámetros: idPresu y idOrden
            await deletePresupuestoGlobal(row.idPresupuesto, row.idOrden);
            // recargamos la lista
            const list = await fetchPresupuestosGlobal();
            setPresupuestos(list);
        } catch (err) {
            console.error(err);
            alert("Error eliminando el presupuesto: " + err.message);
        }
    };

    // 3) Columnas para DataTable
    const columns = [
        {
            name: "ID Presupuesto",
            selector: (row) => row.idPresupuesto,
            sortable: true,
            width: "120px",
        },
        {
            name: "ID Orden",
            selector: (row) => row.idOrden,
            sortable: true,
            width: "100px",
            cell: (row) => (
                <Link
                    to={`/ordenes/${row.idOrden}`}
                    className="text-blue-600 hover:underline"
                >
                    {row.idOrden}
                </Link>
            ),
        },
        {
            name: "Proveedor",
            selector: (row) => row.nombreProveedor,
            sortable: true,
        },
        {
            name: "Fecha Entrega",
            selector: (row) =>
                row.fechaEntrega ? new Date(row.fechaEntrega).toLocaleDateString() : "--",
            sortable: true,
            width: "130px",
        },
        {
            name: "Subtotal",
            selector: (row) =>
                row.detalles && row.detalles.length > 0
                    ? row.subtotal.toFixed(2)
                    : "--",
            sortable: true,
            right: true,
            width: "100px",
        },
        {
            name: "IVA5",
            selector: (row) =>
                row.detalles && row.detalles.length > 0 ? row.iva5.toFixed(2) : "--",
            sortable: true,
            right: true,
            width: "100px",
        },
        {
            name: "IVA10",
            selector: (row) =>
                row.detalles && row.detalles.length > 0 ? row.iva10.toFixed(2) : "--",
            sortable: true,
            right: true,
            width: "100px",
        },
        {
            name: "Total",
            selector: (row) =>
                row.detalles && row.detalles.length > 0 ? row.total.toFixed(2) : "--",
            sortable: true,
            right: true,
            width: "100px",
        },
        {
            name: "Estado",
            selector: (row) =>
                row.detalles && row.detalles.length > 0 ? "Cargado" : "Pendiente",
            sortable: true,
            width: "120px",
            cell: (row) => (
                <span
                    className={
                        row.detalles && row.detalles.length > 0
                            ? "text-green-600 font-medium"
                            : "text-red-600 font-medium"
                    }
                >
                    {row.detalles && row.detalles.length > 0 ? "Cargado" : "Pendiente"}
                </span>
            ),
        },
        {
            name: "Acciones",
            cell: (row) => (
                <div className="flex gap-3">
                    {row.detalles && row.detalles.length > 0 ? (
                        <>
                            {/* Ver detalles */}
                            <button
                                onClick={() => {
                                    const detallesTexto = row.detalles
                                        .map(
                                            (d) =>
                                                `• ${d.nombreProducto} x ${d.cantidad} → Precio: ${d.precio.toFixed(
                                                    2
                                                )}, IVA5: ${d.iva5.toFixed(2)}, IVA10: ${d.iva10.toFixed(
                                                    2
                                                )}`
                                        )
                                        .join("\n");
                                    alert(
                                        `Cotización #${row.idPresupuesto} (Orden #${row.idOrden})\n\n` +
                                        detallesTexto +
                                        `\n\nSubtotal: ${row.subtotal.toFixed(
                                            2
                                        )}\nIVA5: ${row.iva5.toFixed(
                                            2
                                        )}\nIVA10: ${row.iva10.toFixed(
                                            2
                                        )}\nTotal: ${row.total.toFixed(2)}`
                                    );
                                }}
                                title="Ver Detalles"
                                className="text-green-600 hover:text-green-800"
                            >
                                <FaEye size={18} />
                            </button>

                            {/* Editar cotización */}
                            <button
                                onClick={() =>
                                    navigate(`/ordenes/${row.idOrden}/presupuestos/${row.idPresupuesto}`)
                                }
                                title="Editar Cotización"
                                className="text-sky-600 hover:text-sky-800"
                            >
                                <FaRegEdit size={18} />
                            </button>
                        </>
                    ) : (
                        <>
                            {/* Cargar cotización (pendiente) */}
                            <button
                                onClick={() =>
                                    navigate(`/ordenes/${row.idOrden}/presupuestos/${row.idPresupuesto}`)
                                }
                                title="Cargar Cotización"
                                className="text-blue-600 hover:text-blue-800"
                            >
                                <FaUpload size={18} />
                            </button>
                        </>
                    )}

                    {/* Eliminar cotización */}
                    <button
                        onClick={() => handleDelete(row)}
                        title="Eliminar Cotización"
                        className="text-red-600 hover:text-red-800"
                    >
                        <FaTrash size={18} />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
            width: "150px",
        },
    ];

    if (loading) return <p className="p-4">Cargando cotizaciones…</p>;

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold text-sky-600">Todas las Cotizaciones</h2>
                <Link to="/ordenes" className="text-blue-600 hover:underline">
                    ← Volver a Órdenes
                </Link>
            </div>

            {presupuestos.length === 0 ? (
                <p className="p-4">No hay cotizaciones registradas todavía.</p>
            ) : (
                <DataTable columns={columns} data={presupuestos} pagination highlightOnHover />
            )}
        </div>
    );
}
