import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

export const OrdenesPresupuestoFinal = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const {
    orden,
    productosBase,
    preciosProveedores,
    seleccionados,
    proveedorActivo,
  } = location.state || {};

  if (!orden || !productosBase || !preciosProveedores || !seleccionados) {
    return <div className="p-6">Faltan datos para mostrar el resumen.</div>;
  }

  const resumen = productosBase
    .map((producto, i) => {
      if (seleccionados[i]) {
        const precio = preciosProveedores[proveedorActivo].precios[i];
        const cantidad = producto.cantidades;
        const subtotal = precio * cantidad;
        return {
          nombre: producto.nombre,
          cantidad,
          subtotal,
          proveedor: `Proveedor ${proveedorActivo + 1}`,
          fechaEntrega: preciosProveedores[proveedorActivo].fechaEntrega,
        };
      }
      return null;
    })
    .filter(Boolean);

  const totalSubtotal = resumen.reduce((acc, item) => acc + item.subtotal, 0);
  const iva = totalSubtotal * 0.11;
  const total = totalSubtotal + iva;

  const realizarOrden = () => {
    // En un caso real: aquí harías un fetch o axios para actualizar el estado en backend
    // Simulación de cambio en localStorage o estado global
    alert('Orden realizada correctamente. Se actualizará el estado a Incompleto.');

    // Lógica de navegación, podrías pasar datos si necesitas mostrar algo actualizado
    navigate('/Ordenes', {
      state: {
        nroOrden: orden.nroOrden,
        nuevoEstado: 'Incompleta',
        total,
      },
    });
  };

  return (
    <div className="p-6 bg-white min-h-screen">
      <h2 className="text-2xl font-bold mb-4">Resumen de Pedido - Orden {orden.nroOrden}</h2>

      <table className="min-w-full border">
        <thead className="bg-gray-100">
          <tr>
            <th className="border px-4 py-2">Producto</th>
            <th className="border px-4 py-2">Cantidad</th>
            <th className="border px-4 py-2">Subtotal</th>
            <th className="border px-4 py-2">Proveedor</th>
            <th className="border px-4 py-2">Fecha de Entrega</th>
          </tr>
        </thead>
        <tbody>
          {resumen.map((item, index) => (
            <tr key={index}>
              <td className="border px-4 py-2">{item.nombre}</td>
              <td className="border px-4 py-2">{item.cantidad}</td>
              <td className="border px-4 py-2">${item.subtotal.toFixed(2)}</td>
              <td className="border px-4 py-2">{item.proveedor}</td>
              <td className="border px-4 py-2">{item.fechaEntrega}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="mt-6 text-right space-y-2">
        <p><strong>Subtotal:</strong> ${totalSubtotal.toFixed(2)}</p>
        <p><strong>IVA (21%):</strong> ${iva.toFixed(2)}</p>
        <p className="text-lg"><strong>Total:</strong> ${total.toFixed(2)}</p>

        <button
          onClick={realizarOrden}
          className="mt-4 px-6 py-2 bg-green-600 text-white rounded hover:bg-green-700"
        >
          Realizar Orden
        </button>
      </div>
    </div>
  );
};
