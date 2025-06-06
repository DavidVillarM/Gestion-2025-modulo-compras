// src/Pages/ListaPedidos.jsx
import React, { useState, useEffect } from 'react';
import DataTable from 'react-data-table-component';
import { MdUploadFile, MdEdit, MdAttachMoney } from 'react-icons/md';
import { FiTrash2 } from 'react-icons/fi';
import { useNavigate } from 'react-router-dom';

export function ListaPedidos() {
    const [ordenes, setOrdenes] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        fetch('/api/ordenes')
            .then(res => res.json())
            .then(data => setOrdenes(data))
            .catch(console.error);
    }, []);

    // Abrir detalle/PDF de la solicitud
    const handleView = id => {
        navigate(`/pedidos/${id}`);
    };

    // Editar la solicitud
    const handleEdit = id => {
        navigate(`/pedidos/${id}`);
    };

    // Cambiar estado a "PENDIENTE" y dirigir a cotizaciones
    const handleSolicitarCotizacion = id => {
        // Actualizamos el estado en el back a "PENDIENTE"
        // Para eso, necesitaremos traer la orden actual para reenviársela con nuevo estado
        fetch(`/api/ordenes/${id}`)
            .then(res => res.json())
            .then(o => {
                // o = { idOrden, fecha, estado, detalles: [...] }
                return fetch(`/api/ordenes/${id}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        fecha: o.fecha?.split('T')[0] || '',
                        estado: 'PENDIENTE',
                        ordenDetalles: o.detalles.map(d => ({
                            idProducto: d.idProducto,
                            cantidad: d.cantidad
                        }))
                    })
                });
            })
            .then(res => {
                if (!res.ok) throw new Error('Error al cambiar a PENDIENTE');
                // Navegamos a la pantalla de cotizaciones para esta orden:
                navigate(`/presupuestos/nuevo/${id}`);
            })
            .catch(err => {
                console.error(err);
                alert('No fue posible solicitar la cotización. Revisa la consola.');
            });
    };

    // Eliminar la solicitud
    const handleDelete = id => {
        if (!window.confirm('¿Eliminar esta solicitud?')) return;
        fetch(`/api/ordenes/${id}`, { method: 'DELETE' })
            .then(res => {
                if (!res.ok) throw new Error('Error eliminando');
                setOrdenes(prev => prev.filter(o => o.idOrden !== id));
            })
            .catch(console.error);
    };

    const columns = [
        { name: 'ID', selector: row => row.idOrden, sortable: true },
        { name: 'Fecha', selector: row => row.fecha?.split('T')[0], sortable: true },
        { name: 'Estado', selector: row => row.estado, sortable: true },
        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-3">
                    <button
                        onClick={() => handleView(row.idOrden)}
                        title="Ver/Editar"
                        className="text-sky-600 hover:text-sky-800"
                    >
                        <MdUploadFile />
                    </button>
                    <button
                        onClick={() => handleEdit(row.idOrden)}
                        title="Editar"
                        className="text-blue-600 hover:text-blue-800"
                    >
                        <MdEdit />
                    </button>
                    {/* Solo permitimos “Solicitar Cotización” si la orden NO esté ya en estado PENDIENTE, 
              COMPLTA o similar. Ajusta según tu lógica de estados. */}
                    {row.estado === 'INCOMPLETA' && (
                        <button
                            onClick={() => handleSolicitarCotizacion(row.idOrden)}
                            title="Solicitar Cotización"
                            className="text-green-600 hover:text-green-800"
                        >
                            <MdAttachMoney />
                        </button>
                    )}
                    <button
                        onClick={() => handleDelete(row.idOrden)}
                        title="Eliminar"
                        className="text-red-600 hover:text-red-800"
                    >
                        <FiTrash2 />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true
        }
    ];

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <div className="flex justify-between items-center mb-4">
                <h2 className="text-2xl font-bold text-sky-600">Listado de Órdenes</h2>
                <button
                    onClick={() => navigate('/pedidos/nuevo')}
                    className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                >
                    + Nueva Solicitud
                </button>
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
export default ListaPedidos;

