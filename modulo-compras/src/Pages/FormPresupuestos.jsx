//// src/pages/FormPresupuestos.jsx
//import React, { useEffect, useState } from "react";
//import { useNavigate, useParams, useLocation } from "react-router-dom";
//import DatePicker from "react-datepicker";
//import "react-datepicker/dist/react-datepicker.css";

//import { fetchOrden } from "../api/ordenes";
//import {
//    createPresupuesto,
//    updatePresupuesto,
//    fetchPresupuestosPorOrden,
//} from "../api/presupuestos";
//import { marcarPendiente } from "../api/ordenes"; // lo agregamos

//export default function FormPresupuesto() {
//    const { ordenId, provId, idPresupuesto } = useParams();
//    const navigate = useNavigate();
//    const location = useLocation();

//    // Si venimos desde "Elegir proveedores", location.state.todos tiene IDs de proveedores pendientes
//    // Solo usamos provsPendientes cuando entramos desde ProveedoresParaOrden, 
//    // pero si no existe, se queda vacío:
//    const provsPendientes = Array.isArray(location.state?.todos)
//           ? location.state.todos
//           : [];

//    const esEdicion = Boolean(idPresupuesto);

//    // ── Estado local ──
//    const [orden, setOrden] = useState(null);
//    const [loadingOrden, setLoadingOrden] = useState(true);
//    const [detalles, setDetalles] = useState([]); // { idProducto, nombreProducto, cantidad, precio, iva5, iva10 }
//    const [fechaEntrega, setFechaEntrega] = useState(new Date());
//    const [subtotal, setSubtotal] = useState(0);
//    const [iva5, setIva5] = useState(0);
//    const [iva10, setIva10] = useState(0);
//    const [total, setTotal] = useState(0);
//    const [loadingPresu, setLoadingPresu] = useState(esEdicion);

//    // 1) Cargar datos de la orden (productos y cantidades)
//    useEffect(() => {
//        setLoadingOrden(true);
//        fetchOrden(ordenId)
//            .then((o) => {
//                setOrden(o);
//                // Generamos estructura inicial de detalles
//                const arr = o.detalles.map((d) => ({
//                    idPresupuestoDetalle: d.idOrdenDetalle, // no se usa en creación
//                    idProducto: d.idProducto,
//                    nombreProducto: d.nombreProducto,
//                    cantidad: d.cantidad,
//                    precio: 0,
//                    iva5: 0,
//                    iva10: 0,
//                }));
//                setDetalles(arr);
//            })
//            .catch((err) => {
//                console.error(err);
//                alert("Error cargando datos de la orden.");
//                navigate("/ordenes");
//            })
//            .finally(() => setLoadingOrden(false));
//    }, [ordenId, navigate]);

//    // 2) Si es edición, cargar datos del presupuesto existente
//    useEffect(() => {
//        if (!esEdicion) return;

//        setLoadingPresu(true);
//        fetchPresupuestosPorOrden(ordenId)
//            .then((lista) => {
//                const presu = lista.find(
//                    (p) => p.idPresupuesto.toString() === idPresupuesto
//                );
//                if (!presu) {
//                    alert("No existe esa cotización.");
//                    navigate(-1);
//                    return;
//                }
//                setFechaEntrega(new Date(presu.fechaEntrega));
//                setSubtotal(presu.subtotal);
//                setIva5(presu.iva5);
//                setIva10(presu.iva10);
//                setTotal(presu.total);
//                // Mapear detalles del presupuesto
//                const dets = presu.detalles.map((d) => ({
//                    idPresupuestoDetalle: d.idPresupuestoDetalle,
//                    idProducto: d.idProducto,
//                    nombreProducto: d.nombreProducto,
//                    cantidad: d.cantidad,
//                    precio: d.precio,
//                    iva5: d.iva5,
//                    iva10: d.iva10,
//                }));
//                setDetalles(dets);
//            })
//            .catch((err) => {
//                console.error(err);
//                alert("Error cargando presupuesto existente.");
//                navigate(-1);
//            })
//            .finally(() => setLoadingPresu(false));
//    }, [esEdicion, ordenId, idPresupuesto, navigate]);

//    // 3) Cada vez que cambie precio/iva en detalles, recalcular totales
//    useEffect(() => {
//        let st = 0,
//            i5 = 0,
//            i10 = 0;
//        detalles.forEach((d) => {
//            st += d.precio * d.cantidad;
//            i5 += parseFloat(d.iva5 || 0);
//            i10 += parseFloat(d.iva10 || 0);
//        });
//        setSubtotal(st);
//        setIva5(i5);
//        setIva10(i10);
//        setTotal(st + i5 + i10);
//    }, [detalles]);

//    const handleDetalleChange = (idx, field, value) => {
//        const copia = [...detalles];
//        if (field === "precio") copia[idx].precio = parseFloat(value) || 0;
//        if (field === "iva5") copia[idx].iva5 = parseFloat(value) || 0;
//        if (field === "iva10") copia[idx].iva10 = parseFloat(value) || 0;
//        setDetalles(copia);
//    };

//    const handleSubmit = async (e) => {
//        e.preventDefault();
//        // 1) Primero marcamos la orden como "PENDIENTE" (solo si no era edición)
//        if (!esEdicion && orden.estado === "INCOMPLETA") {
//            try {
//                await marcarPendiente(ordenId);
//            } catch (err) {
//                console.error(err);
//                alert("No se pudo marcar la orden como PENDIENTE.");
//                return;
//            }
//        }

//        // 2) Armamos DTO de presupuesto
//        const dto = {
//            idProveedor: parseInt(provId, 10),
//            fechaEntrega: fechaEntrega.toISOString(),
//            subtotal,
//            iva5,
//            iva10,
//            total,
//            detalles: detalles.map((d) => ({
//                idProducto: d.idProducto,
//                cantidad: d.cantidad,
//                precio: d.precio,
//                iva5: d.iva5,
//                iva10: d.iva10,
//            })),
//        };

//        // 3) Enviar al backend (crear o actualizar)
//        try {
//            if (esEdicion) {
//                await updatePresupuesto(ordenId, idPresupuesto, dto);
//                alert("Cotización actualizada correctamente.");
//            } else {
//                await createPresupuesto(ordenId, dto);
//                alert("Cotización guardada correctamente.");
//            }
//        } catch (err) {
//            console.error(err);
//            alert("Error guardando cotización.");
//            return;
//        }

//        // 4) Redirigir: si hay más proveedores pendientes, vamos al próximo; si no, a la lista
//        if (!esEdicion && provsPendientes.length > 0) {
//            const siguientes = provsPendientes.filter(
//                (id) => id.toString() !== provId
//            );
//            if (siguientes.length > 0) {
//                navigate(`/ordenes/${ordenId}/presupuestos/${siguientes[0]}`, {
//                    state: { todos: siguientes },
//                });
//                return;
//            }
//        }

//        navigate(`/ordenes/${ordenId}/presupuestos`);
//    };

//    if (loadingOrden || (esEdicion && loadingPresu)) {
//        return <p className="p-4">Cargando datos…</p>;
//    }

//    return (
//        <div className="max-w-2xl mx-auto p-6 bg-white rounded-lg shadow-md mt-6">
//            <h2 className="text-2xl font-semibold mb-4">
//                {esEdicion
//                    ? `Editar Cotización para Orden #${ordenId}`
//                    : `Nueva Cotización (Proveedor ${provId})`}
//            </h2>

//            <form onSubmit={handleSubmit} className="space-y-4">
//                <div>
//                    <label className="block mb-1 font-medium">Fecha de Entrega:</label>
//                    <DatePicker
//                        selected={fechaEntrega}
//                        onChange={(date) => setFechaEntrega(date)}
//                        dateFormat="yyyy-MM-dd"
//                        className="w-full border px-2 py-1 rounded"
//                        required
//                    />
//                </div>

//                <table className="w-full bg-white border mb-4">
//                    <thead>
//                        <tr className="bg-gray-200">
//                            <th className="p-2">Producto</th>
//                            <th className="p-2">Cantidad</th>
//                            <th className="p-2">Precio</th>
//                            <th className="p-2">IVA5</th>
//                            <th className="p-2">IVA10</th>
//                        </tr>
//                    </thead>
//                    <tbody>
//                        {detalles.map((d, idx) => (
//                            <tr key={idx} className="border-b">
//                                <td className="p-2">{d.nombreProducto}</td>
//                                <td className="p-2">{d.cantidad}</td>
//                                <td className="p-2">
//                                    <input
//                                        type="number"
//                                        min="0"
//                                        step="0.01"
//                                        value={d.precio}
//                                        onChange={(e) =>
//                                            handleDetalleChange(idx, "precio", e.target.value)
//                                        }
//                                        className="w-full border px-1 py-1 rounded"
//                                        required
//                                    />
//                                </td>
//                                <td className="p-2">
//                                    <input
//                                        type="number"
//                                        min="0"
//                                        step="0.01"
//                                        value={d.iva5}
//                                        onChange={(e) =>
//                                            handleDetalleChange(idx, "iva5", e.target.value)
//                                        }
//                                        className="w-full border px-1 py-1 rounded"
//                                    />
//                                </td>
//                                <td className="p-2">
//                                    <input
//                                        type="number"
//                                        min="0"
//                                        step="0.01"
//                                        value={d.iva10}
//                                        onChange={(e) =>
//                                            handleDetalleChange(idx, "iva10", e.target.value)
//                                        }
//                                        className="w-full border px-1 py-1 rounded"
//                                    />
//                                </td>
//                            </tr>
//                        ))}
//                    </tbody>
//                </table>

//                <div className="flex justify-end space-x-4">
//                    <div>
//                        <p>
//                            <span className="font-medium">Subtotal:</span>{" "}
//                            {subtotal.toFixed(2)}
//                        </p>
//                        <p>
//                            <span className="font-medium">IVA5:</span> {iva5.toFixed(2)}
//                        </p>
//                        <p>
//                            <span className="font-medium">IVA10:</span> {iva10.toFixed(2)}
//                        </p>
//                        <p>
//                            <span className="font-medium">Total:</span> {total.toFixed(2)}
//                        </p>
//                    </div>
//                </div>

//                <div>
//                    <button
//                        type="submit"
//                        className="mt-4 px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
//                    >
//                        {esEdicion ? "Actualizar Cotización" : "Guardar Cotización"}
//                    </button>
//                </div>
//            </form>
//        </div>
//    );
//}


import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

import { fetchOrden } from "../api/ordenes";
import {
    fetchPresupuesto,
    updatePresupuesto,
} from "../api/presupuestos";

export default function FormPresupuesto() {
    const { ordenId, idPresupuesto } = useParams();
    const navigate = useNavigate();

    const [loading, setLoading] = useState(true);
    const [, setOrden] = useState(null);
    const [presupuesto, setPresupuesto] = useState(null);

    const [fechaEntrega, setFechaEntrega] = useState(new Date());
    const [detalles, setDetalles] = useState([]);
    const [subtotal, setSubtotal] = useState(0);
    const [iva10, setIva10] = useState(0);
    const [total, setTotal] = useState(0);

    useEffect(() => {
        async function cargarTodo() {
            try {
                setLoading(true);
                const o = await fetchOrden(ordenId);
                setOrden(o);

                const presu = await fetchPresupuesto(ordenId, idPresupuesto);
                setPresupuesto(presu);

                setFechaEntrega(new Date(presu.fechaEntrega || new Date().toISOString()));
                setSubtotal(presu.subtotal);
                setIva10(presu.iva10);
                setTotal(presu.total);

                if (presu.detalles && presu.detalles.length > 0) {
                    setDetalles(
                        presu.detalles.map((d) => ({
                            idPresupuestoDetalle: d.idPresupuestoDetalle,
                            idProducto: d.idProducto,
                            nombreProducto: d.nombreProducto,
                            cantidad: d.cantidad,
                            precio: d.precio,
                            iva10: +(d.precio * d.cantidad * 0.1).toFixed(2),
                        }))
                    );
                } else {
                    const arr = o.detalles.map((d) => ({
                        idPresupuestoDetalle: null,
                        idProducto: d.idProducto,
                        nombreProducto: d.nombreProducto,
                        cantidad: d.cantidad,
                        precio: 0,
                        iva10: 0,
                    }));
                    setDetalles(arr);
                }
            } catch (err) {
                console.error(err);
                alert("Error cargando datos de la orden o presupuesto.");
                navigate("/presupuestos");
            } finally {
                setLoading(false);
            }
        }

        cargarTodo();
    }, [ordenId, idPresupuesto, navigate]);

    useEffect(() => {
        let st = 0, i10 = 0;
        detalles.forEach((d) => {
            st += (parseFloat(d.precio) || 0) * d.cantidad;
            i10 += parseFloat(d.iva10 || 0);
        });
        setSubtotal(st);
        setIva10(i10);
        setTotal(st + i10);
    }, [detalles]);

    const handleDetalleChange = (idx, field, value) => {
        const copia = [...detalles];
        const detalle = copia[idx];

        if (field === "precio") {
            detalle.precio = parseFloat(value) || 0;
            detalle.iva10 = +(detalle.precio * detalle.cantidad * 0.1).toFixed(2);
        }

        setDetalles(copia);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const dto = {
                idProveedor: presupuesto.idProveedor,
                fechaEntrega: fechaEntrega.toISOString(),
                subtotal,
                iva5: 0,
                iva10,
                total,
                detalles: detalles.map((d) => ({
                    idProducto: d.idProducto,
                    cantidad: d.cantidad,
                    precio: d.precio,
                    iva5: 0,
                    iva10: d.iva10,
                })),
            };

            await updatePresupuesto(ordenId, idPresupuesto, dto);
            alert("Cotización actualizada correctamente.");
            navigate("/presupuestos");
        } catch (err) {
            console.error(err);
            alert("Error guardando cotización: " + err.message);
        }
    };

    if (loading) {
        return <p className="p-4">Cargando datos…</p>;
    }

    return (
        <div className="max-w-2xl mx-auto p-6 bg-white rounded-lg shadow-md mt-6">
            <h2 className="text-2xl font-semibold mb-4">
                {`Editar Cotización (ID ${idPresupuesto}) para Orden #${ordenId}`}
                {presupuesto.nombreProveedor && (
                    <> — Proveedor: {presupuesto.nombreProveedor}</>
                )}
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label className="block mb-1 font-medium">Fecha de Entrega:</label>
                    <DatePicker
                        selected={fechaEntrega}
                        onChange={(date) => setFechaEntrega(date)}
                        dateFormat="yyyy-MM-dd"
                        className="w-full border px-2 py-1 rounded"
                        required
                    />
                </div>

            

                <table className="w-full bg-white border mb-4">
                    <thead>
                        <tr className="bg-gray-200">
                            <th className="p-2">Producto</th>
                            <th className="p-2">Cantidad</th>
                            <th className="p-2">Precio</th>
                            <th className="p-2">IVA 10%</th>
                        </tr>
                    </thead>
                    <tbody>
                        {detalles.map((d, idx) => (
                            <tr key={idx} className="border-b">
                                <td className="p-2">{d.nombreProducto}</td>
                                <td className="p-2">{d.cantidad}</td>
                                <td className="p-2">
                                    <input
                                        type="number"
                                        min="0"
                                        step="0.01"
                                        value={d.precio}
                                        onChange={(e) =>
                                            handleDetalleChange(idx, "precio", e.target.value)
                                        }
                                        className="w-full border px-1 py-1 rounded"
                                        required
                                    />
                                </td>
                                <td className="p-2">
                                    <input
                                        type="number"
                                        value={d.iva10}
                                        readOnly
                                        className="w-full border px-1 py-1 rounded bg-gray-100 text-gray-800 font-medium"
                                    />
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>

                <div className="flex justify-end space-x-4">
                    <div>
                        <p>
                            <span className="font-medium">Subtotal:</span> {subtotal.toFixed(2)}
                        </p>
                        <p>
                            <span className="font-medium">IVA 10%:</span> {iva10.toFixed(2)}
                        </p>
                        <p>
                            <span className="font-medium">Total:</span> {total.toFixed(2)}
                        </p>
                    </div>
                </div>

                <div>
                    <button
                        type="submit"
                        className="mt-4 px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                    >
                        Guardar Cotización
                    </button>
                </div>
            </form>
        </div>
    );
}
