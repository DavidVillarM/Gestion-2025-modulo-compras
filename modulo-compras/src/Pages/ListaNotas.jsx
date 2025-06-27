import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const ListaNotas = () => {
    const navigate = useNavigate();
    const [notas, setNotas] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        fetchNotas();
    }, [currentPage]);

    const fetchNotas = async () => {
        try {
            setIsLoading(true);
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5000/api/NotaCredito/listado-simple', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (!response.ok) {
                throw new Error('Error al cargar las notas de crédito');
            }
            const data = await response.json();
            setNotas(data || []);
            setTotalPages(Math.ceil((data.length || 0) / 10));
        } catch (error) {
            console.error('Error:', error);
            alert('Error al cargar las notas de crédito');
            setNotas([]);
            setTotalPages(1);
        } finally {
            setIsLoading(false);
        }
    };

    const handlePageChange = (newPage) => {
        setCurrentPage(newPage);
    };

    const formatDate = (dateString) => {
        if (!dateString) return '';
        const date = new Date(dateString);
        return date.toLocaleDateString('es-ES');
    };

    const formatCurrency = (amount) => {
        if (!amount) return '₲ 0';
        return new Intl.NumberFormat('es-PY', {
            style: 'currency',
            currency: 'PYG'
        }).format(amount);
    };

    return (
        <div className="p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold">Notas de Crédito</h1>
                <button
                    onClick={() => navigate('/notas-credito/cargar')}
                    className="px-4 py-2 bg-sky-600 text-white rounded-md hover:bg-sky-700 focus:outline-none focus:ring-2 focus:ring-sky-500 focus:ring-offset-2"
                >
                    Nueva Nota
                </button>
            </div>

            {isLoading ? (
                <div className="flex justify-center items-center h-64">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-sky-600"></div>
                </div>
            ) : (
                <>
                    <div className="bg-white shadow-md rounded-lg overflow-hidden">
                        <table className="min-w-full divide-y divide-gray-200">
                            <thead className="bg-gray-50">
                                <tr>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID Factura</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre Proveedor</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Fecha</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">RUC</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Monto Total</th>
                                </tr>
                            </thead>
                            <tbody className="bg-white divide-y divide-gray-200">
                                {notas && notas.length > 0 ? (
                                    notas.map((nota, idx) => (
                                        <tr key={idx} className="hover:bg-gray-50">
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{nota.idFactura}</td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{nota.nombreProveedor}</td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{formatDate(nota.fecha)}</td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{nota.ruc}</td>
                                            <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{formatCurrency(nota.montoTotal)}</td>
                                        </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td colSpan="5" className="px-6 py-4 text-center text-sm text-gray-500">
                                            No hay notas de crédito disponibles
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>

                    {/* Pagination */}
                    {notas && notas.length > 0 && (
                        <div className="flex justify-between items-center mt-4">
                            <div className="text-sm text-gray-700">
                                Mostrando página {currentPage} de {totalPages}
                            </div>
                            <div className="flex space-x-2">
                                <button
                                    onClick={() => handlePageChange(currentPage - 1)}
                                    disabled={currentPage === 1}
                                    className="px-3 py-1 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                                >
                                    Anterior
                                </button>
                                <button
                                    onClick={() => handlePageChange(currentPage + 1)}
                                    disabled={currentPage === totalPages}
                                    className="px-3 py-1 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                                >
                                    Siguiente
                                </button>
                            </div>
                        </div>
                    )}
                </>
            )}
        </div>
    );
};

export default ListaNotas;