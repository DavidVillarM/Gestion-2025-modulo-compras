import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const NuevaRecepcion = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [ordenSeleccionada, setOrdenSeleccionada] = useState(null);
  const [factura, setFactura] = useState({ numero: '', timbrado: '' });
  const [productos, setProductos] = useState([]);
  const [filtros, setFiltros] = useState({ proveedor: '', fechaDesde: '', fechaHasta: '' });
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  const cargarOrdenes = async () => {
    try {
      const params = new URLSearchParams();
      if (filtros.proveedor) params.append('proveedor', filtros.proveedor);
      if (filtros.fechaDesde) params.append('fechaDesde', filtros.fechaDesde);
      if (filtros.fechaHasta) params.append('fechaHasta', filtros.fechaHasta);

      const res = await axios.get(`http://localhost:5000/api/recepciones/ordenes-pendientes?${params.toString()}`);
      setOrdenes(res.data || []);
    } catch (err) {
      console.error('Error cargando órdenes', err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    cargarOrdenes();
  }, []);

  const seleccionarOrden = (orden) => {
    setOrdenSeleccionada(orden);
    const productosConCantidad = orden.productos?.map(p => ({
      ...p,
      idProducto: p.id, // aseguramos compatibilidad
      cantidadRecibida: 0
    })) || [];
    setProductos(productosConCantidad);
  };

  const manejarCambioCantidad = (idProducto, valor) => {
    const actualizados = productos.map(p =>
      p.idProducto === idProducto
        ? { ...p, cantidadRecibida: Math.min(parseInt(valor) || 0, p.cantidadSolicitada) }
        : p
    );
    setProductos(actualizados);
  };

  const registrarRecepcion = async () => {
    const exceso = productos.find(p => p.cantidadRecibida > p.cantidadSolicitada);
    if (exceso) {
      return alert(`La cantidad recibida del producto "${exceso.nombre}" supera lo solicitado`);
    }

    if (!factura.numero || !factura.timbrado) {
      return alert('Debe completar número de factura y timbrado');
    }

    const confirmar = window.confirm('¿Está seguro de registrar esta recepción?');
    if (!confirmar) return;

    const payload = {
      ordenId: ordenSeleccionada.idOrden,
      numeroFactura: factura.numero,
      timbrado: factura.timbrado,
      productos: productos.map(p => ({
        productoId: p.idProducto,
        cantidadRecibida: p.cantidadRecibida
      }))
    };

    try {
      await axios.post('http://localhost:5000/api/recepciones/registrar', payload);
      alert('Recepción registrada correctamente');
      navigate('/recepcion');
    } catch (err) {
      console.error('Error al registrar recepción', err);
      alert('Error al registrar recepción');
    }
  };

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Nueva Recepción</h1>

      {!ordenSeleccionada && (
        <div className="mb-4 grid grid-cols-1 sm:grid-cols-3 gap-2">
          <input
            type="text"
            className="border px-2 py-1 rounded"
            placeholder="Proveedor"
            value={filtros.proveedor}
            onChange={e => setFiltros({ ...filtros, proveedor: e.target.value })}
          />
          <input
            type="date"
            className="border px-2 py-1 rounded"
            value={filtros.fechaDesde}
            onChange={e => setFiltros({ ...filtros, fechaDesde: e.target.value })}
          />
          <input
            type="date"
            className="border px-2 py-1 rounded"
            value={filtros.fechaHasta}
            onChange={e => setFiltros({ ...filtros, fechaHasta: e.target.value })}
          />
          <button
            onClick={cargarOrdenes}
            className="col-span-1 sm:col-span-3 bg-blue-600 text-white py-1 rounded"
          >
            Buscar
          </button>
        </div>
      )}

      {isLoading ? (
        <p>Cargando órdenes...</p>
      ) : !ordenSeleccionada ? (
        <div>
          <h2 className="text-lg font-semibold mb-2">Seleccionar Orden Pendiente</h2>
          <ul className="divide-y">
            {ordenes.map(o => (
              <li key={o.idOrden} className="py-2 px-4 bg-white shadow rounded mb-2 flex justify-between">
                <div>
                  <p><strong>ID Orden:</strong> #{o.idOrden}</p>
                  <p><strong>Proveedor:</strong> {o.proveedor?.nombre || 'N/A'}</p>
                  <p><strong>Fecha:</strong> {o.fechaPedido || 'Sin fecha'}</p>
                </div>
                <button
                  className="bg-blue-600 text-white px-4 py-1 rounded"
                  onClick={() => seleccionarOrden(o)}
                >
                  Seleccionar
                </button>
              </li>
            ))}
          </ul>
        </div>
      ) : (
        <div>
          <h2 className="text-lg font-semibold mb-2">Datos de la Orden #{ordenSeleccionada.idOrden}</h2>
          <p><strong>Proveedor:</strong> {ordenSeleccionada.proveedor?.nombre || 'N/A'}</p>

          <div className="mt-4">
            <label className="block mb-1">Número de Factura</label>
            <input
              type="text"
              className="border px-2 py-1 rounded w-full"
              value={factura.numero}
              onChange={(e) => setFactura({ ...factura, numero: e.target.value })}
            />
            <label className="block mt-2 mb-1">Timbrado</label>
            <input
              type="text"
              className="border px-2 py-1 rounded w-full"
              value={factura.timbrado}
              onChange={(e) => setFactura({ ...factura, timbrado: e.target.value })}
            />
          </div>

          <h3 className="mt-4 font-semibold">Productos</h3>
          <table className="w-full mt-2 border text-sm">
            <thead className="bg-gray-100">
              <tr>
                <th className="border px-2 py-1">Producto</th>
                <th className="border px-2 py-1">Solicitado</th>
                <th className="border px-2 py-1">Recibido</th>
              </tr>
            </thead>
            <tbody>
              {productos.map(p => (
                <tr key={p.idProducto}>
                  <td className="border px-2 py-1">{p.nombre}</td>
                  <td className="border px-2 py-1 text-center">{p.cantidadSolicitada}</td>
                  <td className="border px-2 py-1 text-center">
                    <input
                      type="number"
                      className="w-20 border rounded text-center"
                      min={0}
                      max={p.cantidadSolicitada}
                      value={p.cantidadRecibida}
                      onChange={(e) => manejarCambioCantidad(p.idProducto, e.target.value)}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="mt-4 flex gap-4">
            <button
              className="bg-green-600 text-white px-4 py-2 rounded"
              onClick={registrarRecepcion}
            >
              Registrar Recepción
            </button>
            <button
              className="text-blue-600 underline"
              onClick={() => setOrdenSeleccionada(null)}
            >
              ← Cambiar Orden
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default NuevaRecepcion;
