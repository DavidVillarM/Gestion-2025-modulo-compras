

// src/pages/ProveedoresParaOrden.jsx
//import React, { useEffect, useState } from "react";
//import { useNavigate, useParams } from "react-router-dom";
//import { fetchProveedoresParaOrden, marcarPendiente } from "../api/ordenes";
//import { createPresupuesto } from "../api/presupuestos";

//export default function ProveedoresParaOrden() {
//    const { ordenId } = useParams();
//    const navigate = useNavigate();

//    const [proveedores, setProveedores] = useState([]);
//    const [seleccionados, setSeleccionados] = useState(new Set());
//    const [loading, setLoading] = useState(true);

//    useEffect(() => {
//        setLoading(true);
//        fetchProveedoresParaOrden(ordenId)
//            .then((lista) => {
//                setProveedores(lista);
//            })
//            .catch((err) => {
//                console.error(err);
//                alert("Error cargando proveedores de la orden.");
//                navigate("/ordenes");
//            })
//            .finally(() => setLoading(false));
//    }, [ordenId, navigate]);

//    const toggleSeleccion = (idProveedor) => {
//        const copia = new Set(seleccionados);
//        if (copia.has(idProveedor)) copia.delete(idProveedor);
//        else copia.add(idProveedor);
//        setSeleccionados(copia);
//    };

//    const handleEnviar = async () => {
//        if (seleccionados.size === 0) {
//            alert("Debes elegir al menos un proveedor.");
//            return;
//        }

//        // 1) Marcamos la orden como “PENDIENTE” (solo si estaba INCOMPLETA)
//        try {
//            await marcarPendiente(ordenId);
//        } catch (err) {
//            console.error(err);
//            alert("No se pudo marcar la orden como PENDIENTE.");
//            return;
//        }

//        // 2) Creamos tantos presupuestos vacíos como proveedores seleccionados
//        const todosIds = Array.from(seleccionados);
//        try {
//            for (let idProv of todosIds) {
//                // construimos el DTO mínimo sin detalles:
//                const dtoCreacion = {
//                    idProveedor: idProv,
//                    fechaEntrega: new Date().toISOString(),
//                    subtotal: 0,
//                    iva5: 0,
//                    iva10: 0,
//                    total: 0,
//                    detalles: [] // sin filas inicialmente
//                };
//                await createPresupuesto(ordenId, dtoCreacion);
//            }
//            // 3) Una vez creados todos, redirigimos a la vista global de presupuestos:
//            alert("Solicitudes enviadas correctamente.");
//            navigate("/presupuestos");
//        } catch (err) {
//            console.error(err);
//            alert("Error creando presupuestos pendientes: " + err.message);
//        }
//    };

//    if (loading) return <p className="p-4">Cargando proveedores…</p>;
//    if (proveedores.length === 0)
//        return (
//            <p className="p-4">
//                No hay proveedores disponibles para esta orden.{" "}
//                <button
//                    onClick={() => navigate("/ordenes")}
//                    className="text-blue-600 hover:underline"
//                >
//                    Volver a órdenes
//                </button>
//            </p>
//        );

//    return (
//        <div className="max-w-lg mx-auto p-6 bg-white rounded-lg shadow mt-6">
//            <h2 className="text-2xl font-semibold mb-4">
//                Elegir Proveedores para Orden #{ordenId}
//            </h2>
//            <div className="space-y-2">
//                {proveedores.map((p) => (
//                    <label
//                        key={p.idProveedor}
//                        className="flex items-center justify-between border rounded px-4 py-3 hover:shadow"
//                    >
//                        <div>
//                            <p className="font-medium">{p.nombre}</p>
//                            <p className="text-sm text-gray-600">RUC: {p.ruc}</p>
//                        </div>
//                        <input
//                            type="checkbox"
//                            checked={seleccionados.has(p.idProveedor)}
//                            onChange={() => toggleSeleccion(p.idProveedor)}
//                            className="h-5 w-5"
//                        />
//                    </label>
//                ))}
//            </div>
//            <button
//                onClick={handleEnviar}
//                className="mt-6 w-full bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
//            >
//                Enviar Solicitud de Cotización
//            </button>
//        </div>
//    );
//}

// src/pages/ProveedoresParaOrden.jsx
// src/pages/ProveedoresParaOrden.jsx
//import React, { useEffect, useState } from 'react';
//import { useNavigate, useParams } from 'react-router-dom';
//import { fetchProveedoresParaOrden, marcarPendiente } from '../api/ordenes';
//import { createPresupuesto } from '../api/presupuestos';

//export default function ProveedoresParaOrden() {
//    const { ordenId } = useParams();
//    const navigate = useNavigate();

//    const [proveedores, setProveedores] = useState([]);
//    const [seleccionados, setSeleccionados] = useState(new Set());
//    const [loading, setLoading] = useState(true);

//    useEffect(() => {
//        setLoading(true);
//        fetchProveedoresParaOrden(ordenId)
//            .then(lista => {
//                setProveedores(lista);
//            })
//            .catch(err => {
//                console.error(err);
//                alert('Error cargando proveedores de la orden.');
//                navigate('/ordenes');
//            })
//            .finally(() => setLoading(false));
//    }, [ordenId, navigate]);

//    const toggleSeleccion = (idProveedor) => {
//        const copia = new Set(seleccionados);
//        if (copia.has(idProveedor)) copia.delete(idProveedor);
//        else copia.add(idProveedor);
//        setSeleccionados(copia);
//    };

//    const handleEnviar = async () => {
//        if (seleccionados.size === 0) {
//            alert('Debes elegir al menos un proveedor.');
//            return;
//        }

//        // 1) Marcamos la orden como PENDIENTE (solo si estaba INCOMPLETA).
//        try {
//            await marcarPendiente(ordenId);
//        } catch (e) {
//            console.error(e);
//            alert('No se pudo marcar la orden como PENDIENTE.');
//            return;
//        }

//        // 2) Creamos un placeholder para cada proveedor elegido:
//        const dtoBase = {
//            fechaEntrega: new Date().toISOString(),
//            subtotal: 0,
//            iva5: 0,
//            iva10: 0,
//            total: 0,
//            detalles: []
//        };

//        // Iteramos y esperamos a que termine cada POST
//        for (let provId of seleccionados) {
//            try {
//                // idProveedor + resto de campos
//                await createPresupuesto(ordenId, {
//                    ...dtoBase,
//                    idProveedor: parseInt(provId, 10)
//                });
//            } catch (err) {
//                console.error('Error creando placeholder de presupuesto:', err);
//                // Continuamos con el siguiente, pero avisamos al usuario:
//                alert(`No se pudo crear cotización para proveedor ${provId}: ${err.message}`);
//            }
//        }

//        // 3) Finalmente redirigimos a “Todas las Cotizaciones” (lista global).
//        navigate('/presupuestos');
//    };

//    if (loading) return <p className="p-4">Cargando proveedores…</p>;
//    if (proveedores.length === 0)
//        return (
//            <p className="p-4">
//                No hay proveedores disponibles para esta orden.{' '}
//                <button onClick={() => navigate('/ordenes')} className="text-blue-600">
//                    Volver a órdenes
//                </button>
//            </p>
//        );

//    return (
//        <div className="max-w-lg mx-auto p-6 bg-white rounded-lg shadow mt-6">
//            <h2 className="text-2xl font-semibold mb-4">
//                Elegir Proveedores para Orden #{ordenId}
//            </h2>
//            <div className="space-y-2">
//                {proveedores.map((p) => (
//                    <label
//                        key={p.idProveedor}
//                        className="flex items-center justify-between border rounded px-4 py-3 hover:shadow"
//                    >
//                        <div>
//                            <p className="font-medium">{p.nombre}</p>
//                            <p className="text-sm text-gray-600">RUC: {p.ruc}</p>
//                        </div>
//                        <input
//                            type="checkbox"
//                            checked={seleccionados.has(p.idProveedor)}
//                            onChange={() => toggleSeleccion(p.idProveedor)}
//                            className="h-5 w-5"
//                        />
//                    </label>
//                ))}
//            </div>
//            <button
//                onClick={handleEnviar}
//                className="mt-6 w-full bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
//            >
//                Enviar Solicitud de Cotización
//            </button>
//        </div>
//    );
//}
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { fetchProveedoresParaOrden, marcarPendiente } from '../api/ordenes';
import { createPresupuesto } from '../api/presupuestos';

export default function ProveedoresParaOrden() {
    const { ordenId } = useParams();
    const navigate = useNavigate();

    const [proveedores, setProveedores] = useState([]);
    const [seleccionados, setSeleccionados] = useState(new Set());
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        setLoading(true);
        fetchProveedoresParaOrden(ordenId)
            .then(lista => {
                setProveedores(lista);
            })
            .catch(err => {
                console.error(err);
                alert('Error cargando proveedores de la orden.');
                navigate('/ordenes');
            })
            .finally(() => setLoading(false));
    }, [ordenId, navigate]);

    const toggleSeleccion = (idProveedor) => {
        const copia = new Set(seleccionados);
        if (copia.has(idProveedor)) copia.delete(idProveedor);
        else copia.add(idProveedor);
        setSeleccionados(copia);
    };

    const handleEnviar = async () => {
        if (seleccionados.size === 0) {
            alert('Debes elegir al menos un proveedor.');
            return;
        }

        // 1) Marcamos la orden como PENDIENTE (solo si estaba INCOMPLETA).
        try {
            await marcarPendiente(ordenId);
        } catch (e) {
            console.error(e);
            alert('No se pudo marcar la orden como PENDIENTE.');
            return;
        }

        // 2) Creamos un placeholder para cada proveedor elegido:
        const dtoBase = {
            fechaEntrega: new Date().toISOString(),
            subtotal: 0,
            iva5: 0,
            iva10: 0,
            total: 0,
            detalles: []
        };

        // Iteramos y esperamos a que termine cada POST
        for (let provId of seleccionados) {
            try {
                // idProveedor + resto de campos
                await createPresupuesto(ordenId, {
                    ...dtoBase,
                    idProveedor: parseInt(provId, 10)
                });
            } catch (err) {
                console.error('Error creando placeholder de presupuesto:', err);
                // Continuamos con el siguiente, pero avisamos al usuario:
                alert(`No se pudo crear cotización para proveedor ${provId}: ${err.message}`);
            }
        }

        // 3) Finalmente redirigimos a “Todas las Cotizaciones” (lista global).
        navigate('/presupuestos');
    };

    if (loading) return <p className="p-4">Cargando proveedores…</p>;
    if (proveedores.length === 0)
        return (
            <p className="p-4">
                No hay proveedores disponibles para esta orden.{' '}
                <button onClick={() => navigate('/ordenes')} className="text-blue-600">
                    Volver a órdenes
                </button>
            </p>
        );

    return (
        <div className="max-w-lg mx-auto p-6 bg-white rounded-lg shadow mt-6">
            <h2 className="text-2xl font-semibold mb-4">
                Elegir Proveedores para Orden #{ordenId}
            </h2>
            <div className="space-y-2">
                {proveedores.map((p) => (
                    <label
                        key={p.idProveedor}
                        className="flex items-center justify-between border rounded px-4 py-3 hover:shadow"
                    >
                        <div>
                            <p className="font-medium">{p.nombre}</p>
                            <p className="text-sm text-gray-600">RUC: {p.ruc}</p>
                        </div>
                        <input
                            type="checkbox"
                            checked={seleccionados.has(p.idProveedor)}
                            onChange={() => toggleSeleccion(p.idProveedor)}
                            className="h-5 w-5"
                        />
                    </label>
                ))}
            </div>
            <button
                onClick={handleEnviar}
                className="mt-6 w-full bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
                Enviar Solicitud de Cotización
            </button>
        </div>
    );
}
