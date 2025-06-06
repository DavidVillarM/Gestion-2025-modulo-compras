// src/pages/ListaPresupuestosPorOrden.jsx
import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { fetchPresupuestosPorOrden } from '../api/presupuestos';

export default function ListaPresupuestosPorOrden() {
    const { ordenId } = useParams();
    const [presupuestos, setPresupuestos] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        setLoading(true);
        fetchPresupuestosPorOrden(ordenId)
            .then(data => setPresupuestos(data))
            .catch(err => {
                console.error(err);
                alert('Error cargando presupuestos.');
            })
            .finally(() => setLoading(false));
    }, [ordenId]);

    if (loading) return <p className="p-4">Cargando cotizaciones…</p>;
    if (presupuestos.length === 0) {
        return (
            <div className="p-4">
                <p>No hay cotizaciones para la orden {ordenId}.</p>
                <Link to="/ordenes" className="text-blue-600 hover:underline">← Volver a Órdenes</Link>
            </div>
        );
    }

    return (
        <div className="p-6">
            <h2 className="text-2xl font-semibold mb-4">Cotizaciones para Orden #{ordenId}</h2>

            {presupuestos.map(p => (
                <div key={p.idPresupuesto} className="border rounded mb-4 p-4 bg-white">
                    <h3 className="text-xl font-medium">Proveedor: {p.nombreProveedor} (ID {p.idProveedor})</h3>
                    <p>Fecha Entrega: {new Date(p.fechaEntrega).toLocaleDateString()}</p>
                    <p>
                        Subtotal: {p.subtotal.toFixed(2)} | IVA5: {p.iva5.toFixed(2)} | IVA10: {p.iva10.toFixed(2)} | Total: {p.total.toFixed(2)}
                    </p>

                    <table className="min-w-full bg-white mt-2">
                        <thead>
                            <tr className="bg-gray-200 text-left">
                                <th className="p-2">Producto</th>
                                <th className="p-2">Cantidad</th>
                                <th className="p-2">Precio</th>
                                <th className="p-2">IVA5</th>
                                <th className="p-2">IVA10</th>
                            </tr>
                        </thead>
                        <tbody>
                            {p.detalles.map(d => (
                                <tr key={d.idPresupuestoDetalle} className="border-b hover:bg-gray-50">
                                    <td className="p-2">{d.nombreProducto}</td>
                                    <td className="p-2">{d.cantidad}</td>
                                    <td className="p-2">{d.precio.toFixed(2)}</td>
                                    <td className="p-2">{d.iva5.toFixed(2)}</td>
                                    <td className="p-2">{d.iva10.toFixed(2)}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            ))}

            <Link to="/ordenes" className="mt-4 inline-block text-blue-600 hover:underline">
                ← Volver a Órdenes
            </Link>
        </div>
    );
}
 