// src/pages/PocoStock.jsx
import React, { useEffect, useState } from 'react';
import DataTable from 'react-data-table-component';
import { useNavigate } from 'react-router-dom';
import { fetchStockProductos } from '../api/productos';
import { createOrden } from '../api/ordenes';

export default function PocoStock() {
    const [bajos, setBajos] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selected, setSelected] = useState(new Map());
    const nav = useNavigate();

    useEffect(() => {
        setLoading(true);
        fetchStockProductos()
            .then(data => setBajos(data))
            .catch(err => {
                console.error(err);
                alert('Error cargando productos de poco stock.');
            })
            .finally(() => setLoading(false));
    }, []);

    const toggleSeleccion = prod => {
        const copia = new Map(selected);
        if (copia.has(prod.idProducto)) {
            copia.delete(prod.idProducto);
        } else {
            // calculamos cantidad = m�nima - actual (al menos 1)
            const cant = Math.max(prod.cantidadMinima - prod.cantidadTotal, 1);
            copia.set(prod.idProducto, { idProducto: prod.idProducto, cantidad: cant });
        }
        setSelected(copia);
    };

    const handleGenerarOrden = () => {
        if (selected.size === 0) {
            alert('Debes seleccionar al menos un producto de poco stock.');
            return;
        }
        const detalles = Array.from(selected.values());
        const dto = {
            estado: 'INCOMPLETA',
            detalles: detalles
        };
        createOrden(dto)
            .then(nuevaOrden => {
                alert(`Orden creada (ID ${nuevaOrden.idOrden}).`);
                nav('/ordenes');
            })
            .catch(err => {
                console.error(err);
                alert('Error creando la orden.');
            });
    };

    if (loading) return <p className="p-4">Cargando productos de poco stock</p>;
    if (bajos.length === 0) return <p className="p-4">No hay productos con stock bajo.</p>;

    const columns = [
        {
            name: 'Seleccionar',
            cell: row => (
                <input
                    type="checkbox"
                    checked={selected.has(row.idProducto)}
                    onChange={() => toggleSeleccion(row)}
                />
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            width: '100px'
        },
        { name: 'Producto', selector: row => row.nombre, sortable: true },
        { name: 'Categoriaa', selector: row => row.nombreCategoria, sortable: true },
        { name: 'Stock Actual', selector: row => row.cantidadTotal, sortable: true },
        { name: 'Stock Minimo', selector: row => row.cantidadMinima, sortable: true }
    ];

    return (
        <div className="p-6 bg-gray-100 min-h-screen">
            <h2 className="text-xl font-bold text-sky-600 mb-4">Productos con Stock Bajo</h2>
            <DataTable columns={columns} data={bajos} pagination highlightOnHover />
            <div className="mt-4">
                <button
                    onClick={handleGenerarOrden}
                    className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                >
                    Generar Orden desde Poco Stock
                </button>
            </div>
        </div>
    );
}
