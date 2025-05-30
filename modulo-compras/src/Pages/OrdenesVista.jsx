import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

export const OrdenesVista = () => {
  const { id } = useParams();              // obtenemos el id por URL
  const navigate = useNavigate();

  const [detalles, setDetalles]     = useState([]);
  const [infoOrden, setInfoOrden]   = useState(null);
  const [loading, setLoading]       = useState(true);
  const [error, setError]           = useState('');

  // 1) Traer datos de la orden (fecha, estado, montoTotal)
  useEffect(() => {
    if (!id) {
      setError('ID de orden inválido');
      setLoading(false);
      return;
    }
    setLoading(true);
    axios.get(`http://localhost:5000/api/OrdenesPago/${id}`)
      .then(resp => {
        setInfoOrden(resp.data);
        setError('');
      })
      .catch(err => {
        console.error(err);
        setError('No se pudo cargar la orden.');
      })
      .finally(() => setLoading(false));
  }, [id]);

  // 2) Traer líneas de detalle
  useEffect(() => {
    if (!id) return;
    axios.get(`http://localhost:5000/api/OrdenesPago/${id}/detalles`)
      .then(resp => {
        setDetalles(resp.data);
        setError('');
      })
      .catch(err => {
        console.error(err);
        setError('No se pudieron cargar los detalles.');
        setDetalles([]);
      });
  }, [id]);

  if (loading) return <p>Cargando...</p>;
  if (error)   return <p className="text-red-600">{error}</p>;
  if (!infoOrden) return <p>Orden no encontrada.</p>;

  const total = detalles.reduce(
    (sum, d) => sum + (d.cantidad * d.cotizacion + d.iva),
    0
  );

  return (
    <div className="p-6 bg-white min-h-screen">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">
          Orden Nro# {infoOrden.idOrden}
        </h2>
        <button
          onClick={() => navigate(-1)}
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
        >
          Volver
        </button>
      </div>

      <div className="mb-4">
        <p><strong>Fecha de Pedido:</strong> {infoOrden.fechaPedido || '—'}</p>
        <p><strong>Estado:</strong> {infoOrden.estado}</p>
        <p><strong>Monto Total:</strong> ${infoOrden.montoTotal?.toFixed(2) ?? '0.00'}</p>
      </div>

      <table className="min-w-full border">
        <thead>
          <tr className="bg-gray-100">
            <th className="border px-4 py-2">ID Detalle</th>
            <th className="border px-4 py-2">Producto</th>
            <th className="border px-4 py-2">Proveedor</th>
            <th className="border px-4 py-2">Cotización</th>
            <th className="border px-4 py-2">Cantidad</th>
            <th className="border px-4 py-2">IVA</th>
            <th className="border px-4 py-2">Subtotal</th>
          </tr>
        </thead>
        <tbody>
          {detalles.map(d => (
          <tr key={d.idPedidoDetalle}>
              <td className="border px-4 py-2">{d.idPedidoDetalle}</td>
               <td className="border px-4 py-2">{d.idProductoNavigation?.nombre || '—'}</td>
              <td className="border px-4 py-2">{d.idProveedorNavigation?.nombre || '—'}</td>

              <td className="border px-4 py-2">${d.cotizacion.toFixed(2)}</td>
              <td className="border px-4 py-2">{d.cantidad}</td>
              <td className="border px-4 py-2">${d.iva.toFixed(2)}</td>
              <td className="border px-4 py-2">
                ${(d.cantidad * d.cotizacion + d.iva).toFixed(2)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="mt-6">
        <p><strong>Total calculado:</strong> ${total.toFixed(2)}</p>
      </div>
    </div>
  );
};
