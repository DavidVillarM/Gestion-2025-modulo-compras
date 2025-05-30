import React, { useEffect, useState } from 'react';
import axios from 'axios';

const RecepcionProductos = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [ordenSeleccionada, setOrdenSeleccionada] = useState(null);
  const [facturaInfo, setFacturaInfo] = useState({ numero: '', timbrado: '' });
  //const [motivoRechazo, setMotivoRechazo] = useState('');
  const [isLoading, setIsLoading] = useState(false);


  useEffect(() => {
    cargarOrdenes();
  }, []);

  const cargarOrdenes = async () => {
    setIsLoading(true);
    try {
      const res = await axios.get('http://localhost:5000/api/recepciones/ordenes-pendientes');
      console.log("Respuesta:", res.data); 
      setOrdenes(res.data);
    } catch (err) {
      console.error('Error cargando órdenes pendientes', err);
    }
    setIsLoading(false);
  };
  
  /*const cargarOrdenes = async () => {
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
  };*/

  useEffect(() => {
    cargarOrdenes();
  }, []);

  const seleccionarOrden = (orden) => {
    setOrdenSeleccionada({
      ...orden,
      productos: orden.productos.map(det => ({
        id: det.id,
        nombre: det.nombre,
        cantidadSolicitada: det.cantidadSolicitada,
        cantidadRecibida: 0
      }))
    });
  };

  const manejarCambioCantidad = (productoId, valor) => {
    const productosActualizados = ordenSeleccionada.productos.map(prod =>
      prod.id === productoId ? { ...prod, cantidadRecibida: parseInt(valor) || 0 } : prod
    );
    setOrdenSeleccionada({ ...ordenSeleccionada, productos: productosActualizados });
  };


  const confirmarRecepcion = async () => {
    const exceso = ordenSeleccionada.productos.find(p => p.cantidadRecibida > p.cantidadSolicitada);
      if (exceso) {
        alert(`La cantidad recibida del producto "${exceso.nombre}" supera la cantidad solicitada.`);
        return;
      }
    const payload = {
      ordenId: ordenSeleccionada.idOrden,
      numeroFactura: facturaInfo.numero,
      timbrado: facturaInfo.timbrado,
      productos: ordenSeleccionada.productos.map(p => ({
        productoId: p.id,
        cantidadRecibida: p.cantidadRecibida,
      }))
    };

    try {
      await axios.post('http://localhost:5000/api/recepciones/registrar', payload);
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
    const payload = {
      ordenId: ordenSeleccionada.idOrden
    };
  
    try {
      await axios.post('http://localhost:5000/api/recepciones/rechazar', payload);
      alert('Orden rechazada correctamente');
      setOrdenSeleccionada(null);
      cargarOrdenes();
    } catch (err) {
      console.error('Error al rechazar recepción', err);
      alert('Ocurrió un error al rechazar la orden');
    }
  };
  
  const [paginaActual, setPaginaActual] = useState(1);
  const ordenesPorPagina = 10;
  const indexUltimaOrden = paginaActual * ordenesPorPagina;
  const indexPrimeraOrden = indexUltimaOrden - ordenesPorPagina;
  //console.log("ORDENES:", ordenes); 
  const ordenesFiltradas = ordenes.filter(o => o.estado === 'Pendiente' || o.estado === 'Incompleta');
  const ordenesActuales = ordenesFiltradas.slice(indexPrimeraOrden, indexUltimaOrden);
  const totalPaginas = ordenes?.length ? Math.ceil(ordenes.length / ordenesPorPagina) : 1;

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Recepción de Productos</h1>

      {isLoading ? (
        <div className="text-center py-10 text-gray-600">Cargando órdenes...</div>
      ) : !ordenSeleccionada ? (
        <div>
          <h2 className="text-xl mb-2">Ordenes de Compra Pendientes</h2>
          <ul className="divide-y">
          {ordenesActuales.map((orden) => {
            const detalles = orden.ordenDetalles;
            const estado = orden.estado;

            const estadoColor = {
              Completa: 'text-green-600',
              Incompleta: 'text-yellow-600',
              Rechazada: 'text-red-600',
              Pendiente: 'text-gray-600'
            };
              return (
                <li key={orden.idOrden} className="py-3 px-2 flex justify-between items-center bg-white shadow-sm rounded mb-2">
                  <div>
                    <div><strong>Proveedor:</strong> {orden.proveedorNombre || 'N/A'}</div>
                    <div><strong>Fecha:</strong> {new Date(orden.fecha || Date.now()).toLocaleDateString()}</div>
                    <div className={`font-semibold ${estadoColor[estado]}`}>Estado: {estado}</div>
                  </div>
                  <button
                    className="bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700"
                    onClick={() => seleccionarOrden(orden)}
                  >
                    Recibir
                  </button>
                </li>
              );
            })}
          </ul>
          <div className="flex justify-center mt-4 gap-2">
            {Array.from({ length: totalPaginas }, (_, i) => (
              <button
                key={i}
                className={`px-3 py-1 rounded ${paginaActual === i + 1 ? 'bg-blue-700 text-white' : 'bg-gray-200 hover:bg-gray-300'}`}
                onClick={() => setPaginaActual(i + 1)}
              >
                {i + 1}
              </button>
            ))}
          </div>
        </div>
      ) : (
        <div>
          <button
            className="mb-4 text-blue-600 underline"
            onClick={() => setOrdenSeleccionada(null)}
          >
            ← Volver a órdenes
          </button>

          <h2 className="text-xl mb-2">Recepcionando orden #{ordenSeleccionada.idOrden}</h2>
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

          <table className="w-full border mt-4 text-sm">
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

          <div className="flex gap-4 mt-4">
            <button
              className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
              onClick={confirmarRecepcion}
            >
              Confirmar Recepción
            </button>

            <div className="flex">
            <button
              className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
              onClick={rechazarRecepcion}
            >
              Rechazar Recepción
            </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default RecepcionProductos;