// src/Pages/Facturas.jsx
import React, { useEffect, useState } from 'react';

export function Facturas() {
    const [providers, setProviders] = useState([]);
    const [facturas, setFacturas] = useState([]);
    const [modalFactura, setModalFactura] = useState(null);

    const [form, setForm] = useState({
        Fecha: '',
        Id_Proveedor: '',
        Timbrado: '',
        Estado: 'Pendiente',
        detalles: [
            { Producto: '', Precio: '', Cantidad: '', Iva5: '', Iva10: '' }
        ]
    });

    // Carga inicial
    useEffect(() => {
        fetch('/api/proveedores')
            .then(r => r.json())
            .then(setProviders);

        fetch('/api/facturas')
            .then(r => r.json())
            .then(setFacturas);
    }, []);

    // Campos generales
    const handleChange = e => {
        const { name, value } = e.target;
        setForm(f => ({ ...f, [name]: value }));
    };

    // Campos de detalle
    const handleDetalleChange = (idx, e) => {
        const { name, value } = e.target;
        setForm(f => {
            const nuevos = [...f.detalles];
            nuevos[idx] = { ...nuevos[idx], [name]: value };
            return { ...f, detalles: nuevos };
        });
    };

    // Añadir/quitar filas
    const addRow = () =>
        setForm(f => ({
            ...f,
            detalles: [
                ...f.detalles,
                { Producto: '', Precio: '', Cantidad: '', Iva5: '', Iva10: '' }
            ]
        }));
    const removeRow = idx =>
        setForm(f => ({
            ...f,
            detalles: f.detalles.filter((_, i) => i !== idx)
        }));

    // Cálculos
    const subtotal = form.detalles.reduce((sum, d) => {
        const p = parseFloat(d.Precio) || 0;
        const q = parseFloat(d.Cantidad) || 0;
        return sum + p * q;
    }, 0);
    const totalIva5 = form.detalles.reduce((sum, d) => sum + (parseFloat(d.Iva5) || 0), 0);
    const totalIva10 = form.detalles.reduce((sum, d) => sum + (parseFloat(d.Iva10) || 0), 0);
    const total = subtotal + totalIva5 + totalIva10;

    // Envío al backend
    const handleSubmit = e => {
        e.preventDefault();
        const dto = {
            Fecha: new Date(form.Fecha),
            Ruc: providers.find(p => p.id_Proveedor == form.Id_Proveedor)?.ruc || '',
            Nombre_Proveedor: providers.find(p => p.id_Proveedor == form.Id_Proveedor)?.nombre || '',
            Timbrado: form.Timbrado,
            Subtotal: subtotal,
            Iva5: totalIva5,
            Iva10: totalIva10,
            Monto_Total: total,
            Estado: form.Estado,
            Detalles: form.detalles.map(d => ({
                Id_Producto: 0,
                Precio: parseFloat(d.Precio) || 0,
                Cantidad: parseInt(d.Cantidad) || 0,
                Iva5: parseFloat(d.Iva5) || 0,
                Iva10: parseFloat(d.Iva10) || 0
            }))
        };

        fetch('/api/facturas', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        })
            .then(res => {
                if (!res.ok) throw new Error('Error al crear factura');
                return fetch('/api/facturas').then(r => r.json());
            })
            .then(setFacturas)
            .then(() =>
                setForm({
                    Fecha: '',
                    Id_Proveedor: '',
                    Timbrado: '',
                    Estado: 'Pendiente',
                    detalles: [
                        { Producto: '', Precio: '', Cantidad: '', Iva5: '', Iva10: '' }
                    ]
                })
            )
            .catch(console.error);
    };

    return (
        <div className="p-6">
            <h1 className="text-2xl font-bold mb-6">Gestión de Facturas</h1>

            <form onSubmit={handleSubmit} className="space-y-6">
                {/* Campos generales */}
                <div className="grid grid-cols-2 gap-4">
                    <input
                        type="date"
                        name="Fecha"
                        value={form.Fecha}
                        onChange={handleChange}
                        className="border p-2 rounded"
                        required
                    />

                    <select
                        name="Id_Proveedor"
                        value={form.Id_Proveedor}
                        onChange={handleChange}
                        className="border p-2 rounded"
                        required
                    >
                        <option value="">Proveedor...</option>
                        {providers.map(p => (
                            <option key={p.id_Proveedor} value={p.id_Proveedor}>
                                {p.nombre}
                            </option>
                        ))}
                    </select>

                    <input
                        name="Timbrado"
                        placeholder="Timbrado"
                        value={form.Timbrado}
                        onChange={handleChange}
                        className="border p-2 rounded col-span-2"
                    />

                    <select
                        name="Estado"
                        value={form.Estado}
                        onChange={handleChange}
                        className="border p-2 rounded col-span-2"
                    >
                        {['Pendiente', 'Parcial', 'Completada', 'Cancelada'].map(es => (
                            <option key={es}>{es}</option>
                        ))}
                    </select>
                </div>

                {/* Resumen totales */}
                <div className="grid grid-cols-4 gap-4">
                    <div className="border p-2 rounded">
                        <div className="text-sm">Subtotal</div>
                        <div className="font-bold">{subtotal.toFixed(2)}</div>
                    </div>
                    <div className="border p-2 rounded">
                        <div className="text-sm">IVA 5%</div>
                        <div className="font-bold">{totalIva5.toFixed(2)}</div>
                    </div>
                    <div className="border p-2 rounded">
                        <div className="text-sm">IVA 10%</div>
                        <div className="font-bold">{totalIva10.toFixed(2)}</div>
                    </div>
                    <div className="col-span-2 border p-2 rounded bg-gray-100">
                        <div className="text-sm">Total</div>
                        <div className="font-bold text-lg">{total.toFixed(2)}</div>
                    </div>
                </div>

                {/* Tabla detalles */}
                <div>
                    <h2 className="font-semibold mb-2">Detalles de Productos</h2>
                    <table className="w-full border mb-2">
                        <thead className="bg-gray-100">
                            <tr>
                                {['Producto', 'Precio', 'Cantidad', 'Iva5', 'Iva10', 'Acción'].map(h => (
                                    <th key={h} className="px-2 py-1 border">{h}</th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {form.detalles.map((d, i) => (
                                <tr key={i} className="hover:bg-gray-50">
                                    <td className="border p-1">
                                        <input
                                            name="Producto"
                                            value={d.Producto}
                                            onChange={e => handleDetalleChange(i, e)}
                                            className="w-full p-1 border rounded"
                                        />
                                    </td>
                                    <td className="border p-1">
                                        <input
                                            name="Precio"
                                            type="number"
                                            value={d.Precio}
                                            onChange={e => handleDetalleChange(i, e)}
                                            className="w-full p-1 border rounded"
                                        />
                                    </td>
                                    <td className="border p-1">
                                        <input
                                            name="Cantidad"
                                            type="number"
                                            value={d.Cantidad}
                                            onChange={e => handleDetalleChange(i, e)}
                                            className="w-full p-1 border rounded"
                                        />
                                    </td>
                                    <td className="border p-1">
                                        <input
                                            name="Iva5"
                                            type="number"
                                            value={d.Iva5}
                                            onChange={e => handleDetalleChange(i, e)}
                                            className="w-full p-1 border rounded"
                                        />
                                    </td>
                                    <td className="border p-1">
                                        <input
                                            name="Iva10"
                                            type="number"
                                            value={d.Iva10}
                                            onChange={e => handleDetalleChange(i, e)}
                                            className="w-full p-1 border rounded"
                                        />
                                    </td>
                                    <td className="border p-1 text-center">
                                        <button
                                            type="button"
                                            disabled={form.detalles.length === 1}   // <--- deshabilita cuando solo quede 1
                                            onClick={() => removeRow(i)}
                                            className="text-red-600 disabled:opacity-50"
                                        >
                                            ✖
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <button
                        type="button"
                        onClick={addRow}
                        className="px-3 py-1 bg-blue-600 text-white rounded"
                    >
                        + Fila
                    </button>
                </div>

                <button
                    type="submit"
                    className="mt-4 px-6 py-2 bg-green-600 text-white rounded"
                >
                    Crear Factura
                </button>
            </form>

            {/* Tabla facturas */}
            <table className="min-w-full border mt-8">
                <thead className="bg-gray-100">
                    <tr>
                        {[
                            'ID',
                            'Fecha',
                            'Proveedor',
                            'Subtotal',
                            'IVA5',
                            'IVA10',
                            'Total',
                            'Estado',
                            'Acciones'
                        ].map(h => (
                            <th key={h} className="px-4 py-2 border">{h}</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {facturas.map(f => (
                        <tr key={f.id_Factura} className="hover:bg-gray-50">
                            <td className="px-4 py-2 border">{f.id_Factura}</td>
                            <td className="px-4 py-2 border">{new Date(f.fecha).toLocaleDateString()}</td>
                            <td className="px-4 py-2 border">{f.nombre_Proveedor}</td>
                            <td className="px-4 py-2 border">{f.subtotal.toFixed(2)}</td>
                            <td className="px-4 py-2 border">{f.iva5.toFixed(2)}</td>
                            <td className="px-4 py-2 border">{f.iva10.toFixed(2)}</td>
                            <td className="px-4 py-2 border">{f.monto_Total.toFixed(2)}</td>
                            <td className="px-4 py-2 border">{f.estado}</td>
                            <td className="px-4 py-2 border">
                                <button
                                    className="px-2 py-1 bg-blue-600 text-white rounded"
                                    onClick={() => setModalFactura(f)}
                                >
                                    Ver Detalles
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {/* Modal detalles */}
            {modalFactura && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
                    <div className="bg-white rounded shadow-lg max-w-xl w-full p-6">
                        <h2 className="text-xl font-bold mb-4">
                            Detalles de Factura #{modalFactura.id_Factura}
                        </h2>
                        <table className="w-full border mb-4">
                            <thead className="bg-gray-100">
                                <tr>
                                    {['Producto', 'Precio', 'Cantidad', 'Iva5', 'Iva10'].map(h => (
                                        <th key={h} className="px-2 py-1 border">{h}</th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {modalFactura.detalles.map((d, i) => (
                                    <tr key={i} className="hover:bg-gray-50">
                                        <td className="px-2 py-1 border">{d.producto || '—'}</td>
                                        <td className="px-2 py-1 border">{d.precio}</td>
                                        <td className="px-2 py-1 border">{d.cantidad}</td>
                                        <td className="px-2 py-1 border">{d.iva5}</td>
                                        <td className="px-2 py-1 border">{d.iva10}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        <button
                            className="mt-2 px-4 py-2 bg-gray-500 text-white rounded"
                            onClick={() => setModalFactura(null)}
                        >
                            Cerrar
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
}
