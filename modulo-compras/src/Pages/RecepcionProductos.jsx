import React, { useEffect, useState } from 'react';
import axios from 'axios';

const RecepcionProductos = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [ordenSeleccionada, setOrdenSeleccionada] = useState(null);
  const [facturaInfo, setFacturaInfo] = useState({ numero: '', timbrado: '' });

  useEffect(() => {
    cargarOrdenes();
  }, []);

  const cargarOrdenes = async () => {
    try {
      const res = await axios.get('http://localhost:5000/api/recepciones/ordenes-pendientes');
      setOrdenes(res.data);
    } catch (err) {
      console.error('Error cargando órdenes pendientes', err);
    }
  };

  const seleccionarOrden = (orden) => {
    setOrdenSeleccionada({
      ...orden,
      productos: orden.productos.map(prod => ({ ...prod, cantidadRecibida: 0 }))
    });
  };

  const manejarCambioCantidad = (productoId, valor) => {
    const productosActualizados = ordenSeleccionada.productos.map(prod =>
      prod.id === productoId ? { ...prod, cantidadRecibida: parseInt(valor) || 0 } : prod
    );
    setOrdenSeleccionada({ ...ordenSeleccionada, productos: productosActualizados });
  };

  const confirmarRecepcion = async () => {
    const payload = {
      ordenId: ordenSeleccionada.id,
      productos: ordenSeleccionada.productos.map(p => ({
        productoId: p.id,
        cantidadRecibida: p.cantidadRecibida
      })),
      factura: {
        numero: facturaInfo.numero,
        timbrado: facturaInfo.timbrado
      }
    };

    try {
      await axios.post('http://localhost:5000/api/recepciones/confirmar', payload);
      alert('Recepción registrada correctamente');
      setOrdenSeleccionada(null);
      setFacturaInfo({ numero: '', timbrado: '' });
      cargarOrdenes();
    } catch (err) {
      console.error('Error al confirmar recepción', err);
      alert('Ocurrió un error al confirmar la recepción');
    }
  };

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Recepción de Productos</h1>

      {!ordenSeleccionada ? (
        <div>
          <h2 className="text-xl mb-2">Órdenes de Compra Pendientes</h2>
          <ul className="divide-y">
            {ordenes.map((orden) => (
              <li key={orden.id} className="py-2 flex justify-between items-center">
                <div>
                  <strong>Proveedor:</strong> {orden.proveedorNombre} <br />
                  <strong>Fecha:</strong> {new Date(orden.fecha).toLocaleDateString()}
                </div>
                <button
                  className="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700"
                  onClick={() => seleccionarOrden(orden)}
                >
                  Recibir
                </button>
              </li>
            ))}
          </ul>
        </div>
      ) : (
        <div>
          <button
            className="mb-4 text-blue-600 underline"
            onClick={() => setOrdenSeleccionada(null)}
          >
            ← Volver a órdenes
          </button>

          <h2 className="text-xl mb-2">Recepcionando orden #{ordenSeleccionada.id}</h2>
          <div className="mb-4">
            <label className="block">N° Factura</label>
            <input
              type="text"
              className="border px-2 py-1 rounded w-full"
              value={facturaInfo.numero}
              onChange={(e) => setFacturaInfo({ ...facturaInfo, numero: e.target.value })}
            />
            <label className="block mt-2">Timbrado</label>
            <input
              type="text"
              className="border px-2 py-1 rounded w-full"
              value={facturaInfo.timbrado}
              onChange={(e) => setFacturaInfo({ ...facturaInfo, timbrado: e.target.value })}
            />
          </div>

          <table className="w-full border mt-4">
            <thead>
              <tr className="bg-gray-100">
                <th className="border px-2 py-1">Producto</th>
                <th className="border px-2 py-1">Cantidad Solicitada</th>
                <th className="border px-2 py-1">Cantidad Recibida</th>
              </tr>
            </thead>
            <tbody>
              {ordenSeleccionada.productos.map((prod) => (
                <tr key={prod.id}>
                  <td className="border px-2 py-1">{prod.nombre}</td>
                  <td className="border px-2 py-1 text-center">{prod.cantidadSolicitada}</td>
                  <td className="border px-2 py-1 text-center">
                    <input
                      type="number"
                      min="0"
                      max={prod.cantidadSolicitada}
                      className="w-20 text-center border rounded"
                      value={prod.cantidadRecibida}
                      onChange={(e) => manejarCambioCantidad(prod.id, e.target.value)}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <button
            className="mt-4 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
            onClick={confirmarRecepcion}
          >
            Confirmar Recepción
          </button>
        </div>
      )}
    </div>
  );
};

export default RecepcionProductos;
