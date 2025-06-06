//// src/Pages/FormPedidos.jsx
//import React, { useEffect, useState } from 'react'
//import DataTable from 'react-data-table-component'
//import { useNavigate, useParams } from 'react-router-dom'

//export const FormPedidos = () => {
//    const { id } = useParams()
//    const nav = useNavigate()

//    const [productos, setProductos] = useState([])
//    const [seleccionados, setSeleccionados] = useState([])
//    const [filtrarNombre, setFiltrarNombre] = useState('')
//    const [fechaEntrega, setFechaEntrega] = useState('')
//    const [estado, setEstado] = useState('GENERADA') // o pendiente si quieres

//    // Columnas para mostrar todos los productos
//    const colsProductos = [
//        { name: 'Id', selector: row => row.idProducto, sortable: true },
//        { name: 'Producto', selector: row => row.nombre, sortable: true },
//        { name: 'Marca', selector: row => row.marca, sortable: true },
//        { name: 'Cantidad Disponible', selector: row => row.cantidadTotal, sortable: true }
//    ]

//    // Columnas para los seleccionados
//    const colsSeleccionados = [
//        { name: 'Id', selector: row => row.idProducto, sortable: true },
//        { name: 'Producto', selector: row => row.nombre, sortable: true },
//        {
//            name: 'Cantidad Solicitada',
//            cell: row => (
//                <input
//                    type="number"
//                    min="1"
//                    defaultValue={row.cantidad || 1}
//                    onChange={e => {
//                        const qty = parseInt(e.target.value, 10) || 0
//                        setSeleccionados(sel =>
//                            sel.map(p => p.idProducto === row.idProducto
//                                ? { ...p, cantidad: qty }
//                                : p
//                            )
//                        )
//                    }}
//                    className="bg-gray-200 rounded-sm w-14 h-6"
//                />
//            )
//        }
//    ]

//    // Cargo productos y, si edito, cargo la solicitud (orden)
//    useEffect(() => {
//        fetch('/api/productos')
//            .then(r => r.json())
//            .then(setProductos)
//            .catch(console.error)

//        if (id) {
//            fetch(`/api/ordenes/${id}`)
//                .then(r => r.json())
//                .then(o => {
//                    setFechaEntrega(o.fechaEntrega?.split('T')[0] || '')
//                    setEstado(o.estado || '')
//                    setSeleccionados(
//                        o.ordenDetalles.map(d => ({
//                            idProducto: d.idProducto,
//                            nombre: d.producto.nombre,
//                            cantidad: d.cantidad
//                        }))
//                    )
//                })
//                .catch(console.error)
//        }
//    }, [id])

//    const prodFiltrados = productos.filter(p =>
//        p.nombre.toLowerCase().includes(filtrarNombre.toLowerCase())
//    )

//    const handleSave = () => {
//        const payload = {
//            estado,
//            ordenDetalles: seleccionados.map(p => ({
//                idProducto: p.idProducto,
//                cantidad: p.cantidad
//            }))
//        }
//        const url = id ? `/api/ordenes/${id}` : '/api/ordenes'
//        const method = id ? 'PUT' : 'POST'

//        fetch(url, {
//            method,
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify(payload)
//        })
//            .then(res => {
//                if (!res.ok) throw new Error('Error al guardar')
//                nav('/pedidos') // vuelve al listado de solicitudes
//            })
//            .catch(console.error)
//    }

//    const handleGenerarOrden = () => {
//        fetch(`/api/pedidos/${id}/generar-auto`, { method: 'POST' })
//            .then(res => {
//                if (!res.ok) throw new Error('Error al generar orden final')
//                alert('Órdenes de compra creadas')
//            })
//            .catch(console.error)
//    }

//    return (
//        <div className="p-4">
//            <h1 className="text-2xl font-bold mb-4">
//                {id ? 'Editar' : 'Nueva'} Solicitud de Compra
//            </h1>

//            <div className="flex items-center space-x-4 mb-4">
//                <input
//                    type="text"
//                    placeholder="Filtrar producto"
//                    value={filtrarNombre}
//                    onChange={e => setFiltrarNombre(e.target.value)}
//                    className="border rounded px-2 py-1 flex-1"
//                />
//                <input
//                    type="date"
//                    value={fechaEntrega}
//                    onChange={e => setFechaEntrega(e.target.value)}
//                    className="border rounded px-2 py-1"
//                />
//                {id && (
//                    <button
//                        onClick={handleGenerarOrden}
//                        className="bg-green-500 text-white px-4 py-1 rounded hover:bg-green-600"
//                    >
//                        Generar Órdenes
//                    </button>
//                )}
//            </div>

//            <DataTable
//                title="Todos los Productos"
//                columns={colsProductos}
//                data={prodFiltrados}
//                selectableRows
//                onSelectedRowsChange={({ selectedRows }) =>
//                    setSeleccionados(selectedRows.map(p => ({ ...p, cantidad: 1 })))
//                }
//                highlightOnHover
//                responsive
//            />

//            <div className="mt-6">
//                <h2 className="text-xl font-semibold mb-2">Productos Seleccionados</h2>
//                <DataTable
//                    columns={colsSeleccionados}
//                    data={seleccionados}
//                    highlightOnHover
//                    responsive
//                />
//            </div>

//            <div className="text-right mt-4">
//                <button
//                    onClick={handleSave}
//                    className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
//                >
//                    {id ? 'Actualizar Solicitud' : 'Crear Solicitud'}
//                </button>
//            </div>
//        </div>
//    )
//}
//export default FormPedidos;

// src/Pages/FormPedidos.jsx
// src/Pages/FormPedidos.jsx
// src/Pages/FormPedidos.jsx
import React, { useEffect, useState } from 'react';
import DataTable from 'react-data-table-component';
import { useNavigate, useParams, useLocation } from 'react-router-dom';

import {
    fetchOrden,
    createOrden,
    updateOrden
} from '../api/ordenes';

export default function FormPedidos() {
    const { id } = useParams();            // undefined = crear, definido = editar
    const location = useLocation();
    const navigate = useNavigate();

    const [productos, setProductos] = useState([]);
    const [seleccionados, setSeleccionados] = useState([]);
    const [filtrarNombre, setFiltrarNombre] = useState('');
    const [estado, setEstado] = useState('INCOMPLETA');

    useEffect(() => {
        // 1) Cargar todos los productos para “buscar y agregar manualmente”
        fetch('/api/productos')
            .then(res => res.json())
            .then(setProductos)
            .catch(console.error);

        if (id) {
            // 2) Si estoy editando, cargar la orden existente
            fetchOrden(id)
                .then(o => {
                    setEstado(o.estado);
                    setSeleccionados(
                        o.detalles.map(d => ({
                            idProducto: d.idProducto,
                            nombre: d.nombreProducto,
                            cantidad: d.cantidad
                        }))
                    );
                })
                .catch(console.error);
        } else {
            // 3) Si vengo de “Poco stock”, pueden llegar productos preseleccionados
            const pre = location.state?.preselected || [];
            if (pre.length > 0) {
                setSeleccionados(pre);
            }
        }
    }, [id, location.state]);

    // Filtrado local de productos
    const filtrados = productos.filter(p =>
        p.nombre.toLowerCase().includes(filtrarNombre.toLowerCase())
    );

    // Columnas “Todos los productos”
    const colsProductos = [
        { name: 'ID', selector: row => row.idProducto, sortable: true, width: '80px' },
        { name: 'Producto', selector: row => row.nombre, sortable: true },
        { name: 'Marca', selector: row => row.marca, sortable: true },
        { name: 'Stock', selector: row => row.cantidadTotal, sortable: true }
    ];

    // Columnas “Productos Seleccionados”
    const colsSeleccionados = [
        { name: 'ID', selector: row => row.idProducto, sortable: true, width: '80px' },
        { name: 'Producto', selector: row => row.nombre, sortable: true },
        {
            name: 'Cantidad',
            cell: row => (
                <input
                    type="number"
                    min="1"
                    value={row.cantidad}
                    onChange={e => {
                        const qty = Math.max(1, parseInt(e.target.value, 10) || 1);
                        setSeleccionados(prev =>
                            prev.map(p =>
                                p.idProducto === row.idProducto ? { ...p, cantidad: qty } : p
                            )
                        );
                    }}
                    className="bg-gray-200 rounded-sm w-16 text-center px-1"
                />
            ),
            width: '120px'
        }
    ];

    // Guardar (crear o actualizar)
    const handleSave = () => {
        if (!seleccionados.length) {
            alert('Debes elegir al menos un producto.');
            return;
        }
        const dto = {
            Estado: estado,
            OrdenDetalles: seleccionados.map(p => ({
                IdProducto: p.idProducto,
                Cantidad: p.cantidad
            }))
        };

        if (!id) {
            createOrden(dto)
                .then(() => navigate('/pedidos'))
                .catch(err => {
                    console.error(err);
                    alert('Error al crear la solicitud.');
                });
        } else {
            updateOrden(id, dto)
                .then(() => navigate('/pedidos'))
                .catch(err => {
                    console.error(err);
                    alert('Error al actualizar la solicitud.');
                });
        }
    };

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <h1 className="text-2xl font-bold mb-4">
                {id ? 'Editar' : 'Nueva'} Solicitud de Compra
            </h1>

            <div className="flex gap-4 mb-4 items-center">
                <div className="flex flex-col">
                    <label className="text-sm">Filtrar producto</label>
                    <input
                        type="text"
                        placeholder="Buscar por nombre…"
                        value={filtrarNombre}
                        onChange={e => setFiltrarNombre(e.target.value)}
                        className="border rounded px-2 py-1"
                    />
                </div>

                {id && (
                    <div className="flex flex-col">
                        <label className="text-sm">Estado</label>
                        <select
                            value={estado}
                            onChange={e => setEstado(e.target.value)}
                            className="border rounded px-2 py-1"
                        >
                            <option value="INCOMPLETA">INCOMPLETA</option>
                            <option value="PENDIENTE">PENDIENTE</option>
                            <option value="COMPLETA">COMPLETA</option>
                        </select>
                    </div>
                )}

                <div className="ml-auto">
                    <button
                        onClick={handleSave}
                        className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
                    >
                        {id ? 'Actualizar Solicitud' : 'Crear Solicitud'}
                    </button>
                </div>
            </div>

            <DataTable
                title="Todos los Productos"
                columns={colsProductos}
                data={filtrados}
                selectableRows
                onSelectedRowsChange={({ selectedRows }) =>
                    setSeleccionados(
                        selectedRows.map(p => ({
                            idProducto: p.idProducto,
                            nombre: p.nombre,
                            cantidad: 1
                        }))
                    )
                }
                pagination
                highlightOnHover
            />

            <div className="mt-6">
                <h2 className="text-xl font-semibold mb-2">Productos Seleccionados</h2>
                <DataTable
                    columns={colsSeleccionados}
                    data={seleccionados}
                    highlightOnHover
                    noHeader
                />
            </div>
        </div>
    );
}
