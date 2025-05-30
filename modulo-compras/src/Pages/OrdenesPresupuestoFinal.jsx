// File: OrdenesPresupuestoFinal.jsx
import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';

export const OrdenesPresupuestoFinal = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const ordenId = location.state?.ordenId || localStorage.getItem('ordenId');

  const initialDetalles = location.state?.detalles || [];
  const [lineas, setLineas] = useState(initialDetalles);
  const [productNames, setProductNames] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showDialog, setShowDialog] = useState(false);

  useEffect(() => {
    if (initialDetalles.length > 0) {
      localStorage.setItem('detalles', JSON.stringify(initialDetalles));
      setLineas(initialDetalles);
    } else {
      const saved = localStorage.getItem('detalles');
      if (saved) setLineas(JSON.parse(saved));
      else setError('No hay detalles disponibles. Regresa y selecciona al menos un producto.');
    }
    setLoading(false);
  }, [initialDetalles]);

  useEffect(() => {
    const ids = [...new Set(lineas.map(d => d.idProducto))];
    ids.forEach(async id => {
      try {
        const resp = await axios.get(`http://localhost:5000/api/Productos/${id}`);
        setProductNames(prev => ({ ...prev, [id]: resp.data.nombre }));
      } catch (e) {
        console.error('Error al cargar nombre de producto', e);
      }
    });
  }, [lineas]);

  const estimatedDate = () => {
    const date = new Date();
    date.setDate(date.getDate() + 7);
    return date.toLocaleDateString('es-PE');
  };

  const confirmDialog = () => setShowDialog(true);

  const handleConfirm = async () => {
    setShowDialog(false);
    setError('');
    try {
      await axios.post('http://localhost:5000/api/OrdenesPago/presupuesto', {
        ordenId: parseInt(ordenId),
        detalles: lineas
      });
      localStorage.removeItem('detalles');
      navigate('/ordenes-pago');
    } catch (e) {
      console.error(e);
      setError('Error al confirmar pedido.');
    }
  };

  if (loading) return <p>Cargando resumen...</p>;
  if (error) return <p className="text-red-600">{error}</p>;

  const totalGeneral = lineas.reduce(
    (sum, d) => sum + (d.cotizacion * d.cantidad + d.iva),
    0
  );

  return (
    <div className="p-6 bg-white min-h-screen flex flex-col">
      <h2 className="text-2xl font-semibold mb-4">Resumen Final Orden #{ordenId}</h2>

      <div className="flex-1 overflow-auto">
        <table className="w-full border">
          <thead>
            <tr className="bg-gray-100">
              <th className="p-2 border">Producto</th>
              <th className="p-2 border">Proveedor</th>
              <th className="p-2 border">Cotizacion</th>
              <th className="p-2 border">Cantidad</th>
              <th className="p-2 border">IVA</th>
              <th className="p-2 border">Subtotal</th>
            </tr>
          </thead>
          <tbody>
            {lineas.map((d, idx) => (
              <tr key={idx} className="hover:bg-gray-50">
                <td className="p-2 border">{productNames[d.idProducto] || 'Cargando...'}</td>
                <td className="p-2 border">{d.idProveedor}</td>
                <td className="p-2 border">${d.cotizacion.toFixed(2)}</td>
                <td className="p-2 border">{d.cantidad}</td>
                <td className="p-2 border">${d.iva.toFixed(2)}</td>
                <td className="p-2 border">${(d.cotizacion * d.cantidad + d.iva).toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

<div className="mt-2 flex justify-between items-start gap-4">
  <div className="text-sm leading-tight">
    <p className="text-lg font-medium">Total General: ${totalGeneral.toFixed(2)}</p>
    <p>Fecha estimada de entrega: {estimatedDate()}</p>
  </div>
  <div className="flex gap-2">
    <button
      onClick={() => navigate(-1)}
      className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300 text-sm"
    >
      Volver
    </button>
    <button
      onClick={confirmDialog}
      className="px-3 py-1 bg-blue-600 text-white rounded hover:bg-blue-700 text-sm"
    >
      Hacer Pedido
    </button>
  </div>
</div>



      {showDialog && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg w-96">
            <p className="mb-4 text-center">Confirmar pedido?</p>
            <div className="flex justify-end space-x-4">
              <button
                onClick={() => setShowDialog(false)}
                className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
              >
                Cancelar
              </button>
              <button
                onClick={handleConfirm}
                className="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
              >
                Confirmar Pedido
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};