import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const CargarNota = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        idFactura: '',
        fecha: '',
        ruc: '',
        nombreProveedor: '',
        timbrado: '',
        montoTotal: '',
        iva5: '',
        iva10: '',
        estado: ''
    });

    const [facturasList, setFacturasList] = useState([]);
    const [productosList, setProductosList] = useState([]);
    const [isSaving, setIsSaving] = useState(false);

    // Calculate totals
    const calculateTotals = () => {
        const totals = productosList.reduce((acc, producto) => {
            const subtotal = producto.precio * producto.cantidadEditable;
            const iva5 = producto.iva5 * producto.cantidadEditable;
            const iva10 = producto.iva10 * producto.cantidadEditable;
            
            return {
                subtotal: acc.subtotal + subtotal,
                iva5: acc.iva5 + iva5,
                iva10: acc.iva10 + iva10,
                total: acc.total + subtotal
            };
        }, { subtotal: 0, iva5: 0, iva10: 0, total: 0 });

        return totals;
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('http://localhost:5000/api/NotaCredito/facturas-pendientes', {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });
                if (!response.ok) {
                    throw new Error('Error al cargar los datos');
                }
                const data = await response.json();
                setFacturasList(data);
            } catch (error) {
                console.error('Error:', error);
                alert('Error al cargar los datos de la nota de crédito');
            }
        };

        fetchData();
    }, []);

    const fetchProductosDetalle = async (idFactura) => {
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5000/api/NotaCredito/factura-detalles/${idFactura}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (!response.ok) {
                throw new Error('Error al cargar los detalles de la factura');
            }
            const data = await response.json();
            const productosConCantidadEditable = data.map(producto => ({
                ...producto,
                cantidadEditable: producto.cantidad
            }));
            setProductosList(productosConCantidadEditable);
        } catch (error) {
            console.error('Error:', error);
            alert('Error al cargar los detalles de la factura');
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSearch = () => {
        if (!formData.idFactura) {
            alert('Por favor ingrese un ID de factura');
            return;
        }
        
        const facturaEncontrada = facturasList.find(factura => 
            String(factura.idFactura) === String(formData.idFactura)
        );

        if (facturaEncontrada) {
            const updatedData = {
                idFactura: String(facturaEncontrada.idFactura || ''),
                fecha: facturaEncontrada.fecha || '',
                ruc: facturaEncontrada.ruc || '',
                nombreProveedor: facturaEncontrada.nombreProveedor || '',
                timbrado: facturaEncontrada.timbrado || '',
                montoTotal: String(facturaEncontrada.montoTotal || ''),
                iva5: String(facturaEncontrada.iva5 || ''),
                iva10: String(facturaEncontrada.iva10 || ''),
                estado: facturaEncontrada.estado || ''
            };
            setFormData(updatedData);
            fetchProductosDetalle(facturaEncontrada.idFactura);
        } else {
            alert('No se encontró la factura');
            setProductosList([]);
        }
    };

    const handleCantidadChange = (index, newValue) => {
        const maxCantidad = productosList[index].cantidad;
        const cantidad = Math.min(Math.max(0, parseInt(newValue) || 0), maxCantidad);
        
        setProductosList(prevList => {
            const newList = [...prevList];
            newList[index] = {
                ...newList[index],
                cantidadEditable: cantidad
            };
            return newList;
        });
    };

    const resetForm = () => {
        setFormData({
            idFactura: '',
            fecha: '',
            ruc: '',
            nombreProveedor: '',
            timbrado: '',
            montoTotal: '',
            iva5: '',
            iva10: '',
            estado: ''
        });
        setProductosList([]);
    };

    const handleSave = async () => {
        if (!formData.idFactura) {
            alert('Por favor seleccione una factura');
            return;
        }

        if (productosList.length === 0) {
            alert('No hay productos para guardar');
            return;
        }

        // Filter out items with quantity 0
        const productosConCantidad = productosList.filter(producto => producto.cantidadEditable > 0);

        if (productosConCantidad.length === 0) {
            alert('No hay productos con cantidad mayor a 0 para guardar');
            return;
        }

        setIsSaving(true);
        try {
            const token = localStorage.getItem('token');
            // First API call to create the header
            const headerResponse = await fetch(`http://localhost:5000/api/NotaCredito/crear-cabecera/${formData.idFactura}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!headerResponse.ok) {
                throw new Error('Error al crear la cabecera de la nota de crédito');
            }

            const { idNota } = await headerResponse.json();

            // Prepare the details data
            const detallesData = productosConCantidad.map(producto => ({
                idNota: idNota,
                idProducto: producto.idProducto,
                precio: producto.precio,
                cantidad: producto.cantidadEditable,
                iva5: producto.iva5,
                iva10: producto.iva10
            }));

            // Second API call to save the details
            const detailsResponse = await fetch(`http://localhost:5000/api/NotaCredito/${idNota}/detalles`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(detallesData)
            });

            if (!detailsResponse.ok) {
                const errorData = await detailsResponse.text();
                throw new Error(`Error al guardar los detalles de la nota de crédito: ${errorData}`);
            }

            alert('Nota de crédito guardada exitosamente');
            resetForm();
            navigate('/notas-credito');
        } catch (error) {
            console.error('Error:', error);
            alert(error.message);
        } finally {
            setIsSaving(false);
        }
    };

    const totals = calculateTotals();

    return (
        <div className="p-6">
            <h1 className="text-2xl font-bold mb-6">Cargar Nota de Crédito</h1>
            
            {/* Form Section */}
            <div className="mb-8">
                <h2 className="text-xl font-semibold mb-4">Datos de la Nota de Crédito</h2>
                <form className="space-y-4 max-w-2xl">
                    <div className="grid grid-cols-2 gap-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700">ID Factura</label>
                            <div className="flex gap-2">
                                <input
                                    type="text"
                                    name="idFactura"
                                    value={formData.idFactura}
                                    onChange={handleChange}
                                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                    required
                                />
                                <button
                                    type="button"
                                    onClick={handleSearch}
                                    className="mt-1 px-3 py-2 bg-sky-600 text-white rounded-md hover:bg-sky-700 focus:outline-none focus:ring-2 focus:ring-sky-500 focus:ring-offset-2"
                                >
                                    <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                        <path fillRule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clipRule="evenodd" />
                                    </svg>
                                </button>
                            </div>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Fecha</label>
                            <input
                                type="date"
                                name="fecha"
                                value={formData.fecha}
                                onChange={handleChange}
                                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">RUC</label>
                            <input
                                type="text"
                                name="ruc"
                                value={formData.ruc}
                                onChange={handleChange}
                                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                required
                            />
                        </div>
                        <div className="col-span-2">
                            <label className="block text-sm font-medium text-gray-700">Nombre Proveedor</label>
                            <input
                                type="text"
                                name="nombreProveedor"
                                value={formData.nombreProveedor}
                                onChange={handleChange}
                                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Timbrado</label>
                            <input
                                type="text"
                                name="timbrado"
                                value={formData.timbrado}
                                onChange={handleChange}
                                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700">Estado</label>
                            <select
                                name="estado"
                                value={formData.estado}
                                onChange={handleChange}
                                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                required
                            >
                                <option value="">Seleccione un estado</option>
                                <option value="Pendiente">Pendiente</option>
                                <option value="Completado">Completado</option>
                            </select>
                        </div>
                    </div>
                </form>
            </div>

            {/* Product List Section */}
            <div>
                <h2 className="text-xl font-semibold mb-4">Lista de Productos</h2>
                <div className="bg-white shadow-md rounded-lg overflow-hidden">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                            <tr>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Producto</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Precio</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Cantidad</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Subtotal</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">IVA 5%</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">IVA 10%</th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {productosList.map((producto, index) => {
                                const subtotal = producto.precio * producto.cantidadEditable;
                                const iva5 = producto.iva5 * producto.cantidadEditable;
                                const iva10 = producto.iva10 * producto.cantidadEditable;
                                
                                return (
                                    <tr key={index}>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{producto.nombreProducto}</td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{producto.precio}</td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                                            <input
                                                type="number"
                                                min="0"
                                                max={producto.cantidad}
                                                value={producto.cantidadEditable}
                                                onChange={(e) => handleCantidadChange(index, e.target.value)}
                                                className="w-20 rounded-md border-gray-300 shadow-sm focus:border-sky-500 focus:ring-sky-500"
                                            />
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{subtotal.toFixed(2)}</td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{iva5.toFixed(2)}</td>
                                        <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{iva10.toFixed(2)}</td>
                                    </tr>
                                );
                            })}
                        </tbody>
                        <tfoot className="bg-gray-50">
                            <tr>
                                <td colSpan="3" className="px-6 py-4 text-right font-medium text-gray-900">Totales:</td>
                                <td className="px-6 py-4 text-sm font-medium text-gray-900">{totals.subtotal.toFixed(2)}</td>
                                <td className="px-6 py-4 text-sm font-medium text-gray-900">{totals.iva5.toFixed(2)}</td>
                                <td className="px-6 py-4 text-sm font-medium text-gray-900">{totals.iva10.toFixed(2)}</td>
                            </tr>
                            <tr>
                                <td colSpan="5" className="px-6 py-4 text-right font-medium text-gray-900">Monto Total:</td>
                                <td className="px-6 py-4 text-sm font-medium text-gray-900">{totals.total.toFixed(2)}</td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>

            <div className="flex justify-end space-x-4 mt-6">
                <button
                    type="button"
                    onClick={() => navigate('/notas-credito/cargar')}
                    className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                >
                    Volver
                </button>
                <button
                    type="button"
                    onClick={handleSave}
                    disabled={isSaving}
                    className="px-4 py-2 bg-sky-600 text-white rounded-md hover:bg-sky-700 disabled:bg-sky-300"
                >
                    {isSaving ? 'Guardando...' : 'Guardar'}
                </button>
            </div>
        </div>
    );
};
