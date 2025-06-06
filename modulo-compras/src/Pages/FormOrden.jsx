// FormOrden.jsx
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { fetchProductos } from '../api/productos';
import { createOrden } from '../api/ordenes';

export default function FormOrden() {
    const [productos, setProductos] = useState([]);
    const [loadingProductos, setLoadingProductos] = useState(true);
    // Cada detalle: { idProducto, cantidad }
    const [detalles, setDetalles] = useState([{ idProducto: '', cantidad: 1 }]);
    const [guardando, setGuardando] = useState(false);
    const nav = useNavigate();

    // 1) Cargar lista de productos y preseleccionar el primero
    useEffect(() => {
        setLoadingProductos(true);
        fetchProductos()
            .then(data => {
                setProductos(data);
                if (data.length > 0) {
                    // Preseleccionamos primer producto automáticamente
                    setDetalles([{ idProducto: data[0].idProducto, cantidad: 1 }]);
                }
            })
            .catch(err => {
                console.error(err);
                alert('Error cargando lista de productos.');
            })
            .finally(() => setLoadingProductos(false));
    }, []);

    // Actualizar un detalle (producto o cantidad)
    const handleDetalleChange = (idx, field, value) => {
        const copia = [...detalles];
        if (field === 'idProducto') {
            copia[idx].idProducto = parseInt(value, 10);
        } else if (field === 'cantidad') {
            copia[idx].cantidad = parseInt(value, 10);
        }
        setDetalles(copia);
    };

    // Añadir/Eliminar filas
    const agregarLinea = () => {
        const primerId = productos.length > 0 ? productos[0].idProducto : '';
        setDetalles(prev => [...prev, { idProducto: primerId, cantidad: 1 }]);
    };
    const eliminarLinea = idx => {
        setDetalles(prev => prev.filter((_, i) => i !== idx));
    };

    // Enviar al back
    const handleSubmit = async e => {
        e.preventDefault();
        // Filtrar solo filas con idProducto y cantidad válidos
        const detallesValidos = detalles.filter(d =>
            Number.isInteger(d.idProducto) && d.idProducto > 0 &&
            Number.isInteger(d.cantidad) && d.cantidad > 0
        );
        if (detallesValidos.length === 0) {
            alert('Debes agregar al menos un producto con cantidad válida.');
            return;
        }

        // Aquí la clave: el controlador espera “detalles”, no “ordenDetalles”
        const dto = {
            estado: 'INCOMPLETA',
            detalles: detallesValidos.map(d => ({
                idProducto: d.idProducto,
                cantidad: d.cantidad
            }))
        };

        setGuardando(true);
        try {
            const nuevaOrden = await createOrden(dto);
            alert(`Orden creada con ID ${nuevaOrden.idOrden}`);
            nav('/ordenes');
        } catch (err) {
            console.error(err);
            alert('Error al crear la orden: ' + (err.message || err));
        } finally {
            setGuardando(false);
        }
    };

    return (
        <div className="max-w-lg mx-auto mt-8">
            <h2 className="text-2xl font-semibold mb-4">Crear Nueva Orden</h2>

            {loadingProductos ? (
                <p>Cargando productos para el formulario…</p>
            ) : (
                <form onSubmit={handleSubmit} className="space-y-6">
                    {detalles.map((d, idx) => (
                        <div key={idx} className="flex items-end space-x-2">
                            {/* SELECT de productos */}
                            <div className="flex-1">
                                <label className="block mb-1 font-medium">Producto:</label>
                                <select
                                    value={d.idProducto}
                                    onChange={e => handleDetalleChange(idx, 'idProducto', e.target.value)}
                                    className="w-full border px-2 py-1 rounded"
                                    required
                                >
                                    <option value="">-- Selecciona un producto --</option>
                                    {productos.map(p => (
                                        <option key={p.idProducto} value={p.idProducto}>
                                            {p.idProducto} – {p.nombre} ({p.nombreCategoria})
                                        </option>
                                    ))}
                                </select>
                            </div>

                            {/* INPUT de cantidad */}
                            <div className="w-32">
                                <label className="block mb-1 font-medium">Cantidad:</label>
                                <input
                                    type="number"
                                    min="1"
                                    value={d.cantidad}
                                    onChange={e => handleDetalleChange(idx, 'cantidad', e.target.value)}
                                    className="w-full border px-2 py-1 rounded"
                                    required
                                />
                            </div>

                            {/* BOTÓN eliminar */}
                            <button
                                type="button"
                                onClick={() => eliminarLinea(idx)}
                                className="mb-1 px-3 py-1 bg-red-600 text-white rounded hover:bg-red-700"
                            >
                                Eliminar
                            </button>
                        </div>
                    ))}

                    <button
                        type="button"
                        onClick={agregarLinea}
                        className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
                    >
                        + Agregar Producto
                    </button>

                    <div>
                        <button
                            type="submit"
                            disabled={guardando}
                            className="mt-4 px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700 disabled:opacity-50"
                        >
                            {guardando ? 'Guardando…' : 'Crear Orden'}
                        </button>
                    </div>
                </form>
            )}
        </div>
    );
}
