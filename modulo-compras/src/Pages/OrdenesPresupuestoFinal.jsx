// OrdenesPresupuestoFinal.jsx
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

  if (loading) return <p className="text-center text-lg">Cargando resumen...</p>;
  if (error) return <p className="text-center text-red-600 text-lg">{error}</p>;

  const totalGeneral = lineas.reduce(
    (sum, d) => sum + (d.cotizacion * d.cantidad + d.iva),
    0
  );

  return (
    <div className="max-w-screen-xl p-6 bg-white min-h-screen">
      <div className="space-y-10 pl-4">
        <h2 className="text-3xl font-bold text-left">Resumen Final Orden #{ordenId}</h2>

        <div className="overflow-x-auto max-w-4xl">
          <table className="w-full border text-base">
            <thead>
              <tr className="bg-gray-100 text-left">
                <th className="p-4 border">Producto</th>
                <th className="p-4 border">Proveedor</th>
                <th className="p-4 border">Cotización</th>
                <th className="p-4 border">Cantidad</th>
                <th className="p-4 border">IVA</th>
                <th className="p-4 border">Subtotal</th>
              </tr>
            </thead>
            <tbody>
              {lineas.map((d, idx) => (
                <tr key={idx} className="hover:bg-gray-50">
                  <td className="p-4 border">{productNames[d.idProducto] || 'Cargando...'}</td>
                  <td className="p-4 border">{d.idProveedor}</td>
                  <td className="p-4 border">${d.cotizacion.toFixed(2)}</td>
                  <td className="p-4 border">{d.cantidad}</td>
                  <td className="p-4 border">${d.iva.toFixed(2)}</td>
                  <td className="p-4 border">${(d.cotizacion * d.cantidad + d.iva).toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="flex flex-col items-start gap-6">
          <div className="text-lg leading-relaxed space-y-2">
            <p className="font-semibold text-xl">Total General: ${totalGeneral.toFixed(2)}</p>
            <p className="text-lg">Fecha estimada de entrega: {estimatedDate()}</p>
          </div>
          <div className="flex gap-4">
            <button
              onClick={() => navigate(-1)}
              className="px-6 py-3 bg-gray-200 rounded text-lg font-medium hover:bg-gray-300"
            >
              Volver
            </button>
            <button
              onClick={confirmDialog}
              className="px-6 py-3 bg-blue-600 text-white rounded text-lg font-medium hover:bg-blue-700"
            >
              Hacer Pedido
            </button>
          </div>
        </div>

        {showDialog && (
          <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
            <div className="bg-white p-6 rounded-lg w-full max-w-sm">
              <p className="mb-4 text-center text-lg font-medium">¿Confirmar pedido?</p>
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
                  Confirmar
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
