import React, { useEffect, useState } from 'react';
import axios from 'axios';

const RecepcionProductos = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [ordenSeleccionada, setOrdenSeleccionada] = useState(null);
  const [facturaInfo, setFacturaInfo] = useState({ numero: '', timbrado: '' });
  const [motivoRechazo, setMotivoRechazo] = useState('');


  useEffect(() => {
    cargarOrdenes();
  }, []);

  /*const cargarOrdenes = async () => {
    try {
      const res = await axios.get('http://localhost:5001/api/recepciones/ordenes-pendientes');
      setOrdenes(res.data);
    } catch (err) {
      console.error('Error cargando órdenes pendientes', err);
    }
  };*/
  const cargarOrdenes = async () => {
    // Datos estáticos de prueba
    const datosFalsos = Array.from({ length: 20 }, (_, i) => ({
      id: i + 1,
      proveedorNombre: `Proveedor ${i + 1}`,
      fecha: new Date(2025, 3, i + 1).toISOString(),
      productos: [
        {
          id: i * 3 + 1,
          nombre: `Producto A${i + 1}`,
          cantidadSolicitada: Math.floor(Math.random() * 20) + 1,
        },
        {
          id: i * 3 + 2,
          nombre: `Producto B${i + 1}`,
          cantidadSolicitada: Math.floor(Math.random() * 20) + 1,
        },
        {
          id: i * 3 + 3,
          nombre: `Producto C${i + 1}`,
          cantidadSolicitada: Math.floor(Math.random() * 20) + 1,
        }
      ]
    }));
  
    setOrdenes(datosFalsos);
  };

  const [paginaActual, setPaginaActual] = useState(1);
  const ordenesPorPagina = 10;
  const indexUltimaOrden = paginaActual * ordenesPorPagina;
  const indexPrimeraOrden = indexUltimaOrden - ordenesPorPagina;
  const ordenesActuales = ordenes.slice(indexPrimeraOrden, indexUltimaOrden);
  const totalPaginas = Math.ceil(ordenes.length / ordenesPorPagina);

  

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
  const rechazarRecepcion = async () => {
    if (!motivoRechazo.trim()) {
      alert('Por favor, escriba el motivo del rechazo.');
      return;
    }
  
    const payload = {
      ordenId: ordenSeleccionada.id,
      motivo: motivoRechazo
    };
  
    try {
      await axios.post('http://localhost:5000/api/recepciones/rechazar', payload);
      alert('Orden rechazada correctamente');
      setOrdenSeleccionada(null);
      setMotivoRechazo('');
      cargarOrdenes();
    } catch (err) {
      console.error('Error al rechazar recepción', err);
      alert('Ocurrió un error al rechazar la orden');
    }
  };
  

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Recepción de Productos</h1>

      {!ordenSeleccionada ? (
        <div>
          <h2 className="text-xl mb-2">Órdenes de Compra Pendientes</h2>
          <ul className="divide-y">
            {ordenesActuales.map((orden) => (
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
          <div className="mt-6">
          <label className="block font-semibold mb-1">Motivo de Rechazo</label>
          <textarea
            className="border rounded w-full p-2"
            rows="3"
            placeholder="Describa el motivo del rechazo"
            value={motivoRechazo}
            onChange={(e) => setMotivoRechazo(e.target.value)}
          ></textarea>

          <button
            className="mt-2 bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
            onClick={rechazarRecepcion}
          >
            Rechazar Recepción
          </button>
        </div>

        </div>
      )}
      <div className="flex justify-center mt-4 gap-2">
        {Array.from({ length: totalPaginas }, (_, i) => (
          <button
            key={i}
            className={`px-3 py-1 rounded ${paginaActual === i + 1 ? 'bg-blue-700 text-white' : 'bg-gray-200 text-black hover:bg-gray-300'}`}
            onClick={() => setPaginaActual(i + 1)}
          >
            {i + 1}
          </button>
        ))}
      </div>
    </div>
  );
};

export default RecepcionProductos;
