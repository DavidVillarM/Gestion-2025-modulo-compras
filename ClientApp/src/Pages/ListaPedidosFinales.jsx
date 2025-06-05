// src/pages/ListaPedidosFinales.jsx
import React, { useEffect, useState } from 'react';
import DataTable from 'react-data-table-component';
import { FaRegEdit, FaTrash, FaEye } from 'react-icons/fa';
import { fetchPedidosFinales, deletePedidoFinal } from '../api/pedidos';

export default function ListaPedidosFinales() {
    const [pedidos, setPedidos] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        load();
    }, []);

    async function load() {
        setLoading(true);
        try {
            const list = await fetchPedidosFinales();
            setPedidos(list);
        } catch (e) {
            console.error(e);
            alert('Error cargando pedidos finales');
        } finally {
            setLoading(false);
        }
    }

    async function handleDelete(idPedido) {
        if (!window.confirm('¿Eliminar este pedido final?')) return;
        try {
            await deletePedidoFinal(idPedido);
            load();
        } catch (e) {
            console.error(e);
            alert('Error eliminando pedido final');
        }
    }

    const columns = [
        { name: 'ID', selector: row => row.idPedido, sortable: true },
        {
            name: 'Proveedor',
            selector: row => row.nombreProveedor,
            sortable: true,
        },
        { name: 'Monto Total', selector: row => row.montoTotal.toFixed(2), sortable: true },
        {
            name: 'Fecha Pedido',
            selector: row => new Date(row.fechaPedido).toLocaleDateString(),
            sortable: true,
        },
        {
            name: 'Fecha Entrega',
            selector: row => new Date(row.fechaEntrega).toLocaleDateString(),
            sortable: true,
        },
        {
            name: 'Estado',
            selector: row => row.estado,
            sortable: true,
        },
        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-4">
                    <button
                        onClick={() =>
                            alert(
                                'Detalles:\n' +
                                row.detalles
                                    .map(
                                        d =>
                                            `• ${d.nombreProducto} x ${d.cantidad} — Cotización: ${d.cotizacion.toFixed(
                                                2
                                            )}, IVA: ${d.iva}`
                                    )
                                    .join('\n')
                            )
                        }
                        title="Ver Detalles"
                        className="text-green-600 hover:text-green-800"
                    >
                        <FaEye size={18} />
                    </button>
                    {/* Si deseas editar un pedido final, implementa ruta adicional /pedidos/:id */}
                    <button
                        onClick={() => alert('Función “Editar” no implementada aquí')}
                        title="Editar Pedido Final"
                        className="text-sky-600 hover:text-sky-800"
                    >
                        <FaRegEdit size={18} />
                    </button>
                    <button
                        onClick={() => handleDelete(row.idPedido)}
                        title="Eliminar Pedido Final"
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

    if (loading) return <p className="p-4">Cargando pedidos finales…</p>;
    if (pedidos.length === 0) return <p className="p-4">No hay pedidos finales.</p>;

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <h2 className="text-xl font-bold text-sky-600 mb-4">Pedidos Finales</h2>
            <DataTable columns={columns} data={pedidos} pagination highlightOnHover />
        </div>
    );
}
