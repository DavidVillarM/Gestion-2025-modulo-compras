import React, { useState } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';

export const OrdenesPresupuesto = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const orden = location.state?.orden;

  const proveedores = ['Proveedor 1', 'Proveedor 2', 'Proveedor 3'];
  const [proveedorActivo, setProveedorActivo] = useState(0);

  const productosBase = [
    { nombre: 'Producto A', cantidades: 10 },
    { nombre: 'Producto B', cantidades: 5 },
    { nombre: 'Producto C', cantidades: 8 },
  ];

  const preciosProveedores = [
    {
      fechaEntrega: '2025-05-01',
      precios: [100, 200, 150],
    },
    {
      fechaEntrega: '2025-05-03',
      precios: [110, 190, 160],
    },
    {
      fechaEntrega: '2025-05-05',
      precios: [105, 195, 155],
    },
  ];

  // Estado para almacenar qué productos están seleccionados por proveedor
  const [seleccionesPorProveedor, setSeleccionesPorProveedor] = useState(
    Array.from({ length: proveedores.length }, () => Array(productosBase.length).fill(false))
  );

  if (!orden) return <div className="p-6">No se encontró la orden.</div>;

  const toggleSeleccion = (index) => {
    // Desmarcar el producto en todos los proveedores
    const copia = [...seleccionesPorProveedor];

    // Desmarcar en los otros proveedores antes de marcar en el proveedor activo
    copia.forEach((selecciones, proveedorIndex) => {
      if (proveedorIndex !== proveedorActivo) {
        selecciones[index] = false;
      }
    });

    // Marcar el producto en el proveedor activo
    copia[proveedorActivo][index] = !copia[proveedorActivo][index];

    setSeleccionesPorProveedor(copia);
  };

  const calcularSubtotal = (precio, cantidad) => precio * cantidad;

  const calcularTotales = () => {
    let subtotal = 0;
    seleccionesPorProveedor[proveedorActivo].forEach((sel, i) => {
      if (sel) {
        const precio = preciosProveedores[proveedorActivo].precios[i];
        const cantidad = productosBase[i].cantidades;
        subtotal += calcularSubtotal(precio, cantidad);
      }
    });
    const iva = subtotal * 0.11;
    const total = subtotal + iva;
    return { subtotal, iva, total };
  };

  const { subtotal, iva, total } = calcularTotales();

  return (
    <div className="p-6 bg-white min-h-screen">
      <h2 className="text-2xl font-bold mb-4">Orden de Presupuesto: {orden.nroOrden}</h2>

      <div className="flex gap-4 mb-4">
        {proveedores.map((prov, index) => {
          return (
            <button
              key={index}
              onClick={() => setProveedorActivo(index)}
              className={`px-4 py-2 rounded ${proveedorActivo === index ? 'bg-blue-600 text-white' : 'bg-gray-200'
                }`}
            >
              {prov}
            </button>
          );
        })}
      </div>

      <table className="min-w-full border">
        <thead>
          <tr className="bg-gray-100">
            <th className="border px-4 py-2">Producto</th>
            <th className="border px-4 py-2">Precio Unitario</th>
            <th className="border px-4 py-2">Cantidad</th>
            <th className="border px-4 py-2">Pedir</th>
            <th className="border px-4 py-2">IVA</th>
            <th className="border px-4 py-2">Subtotal</th>
          </tr>
        </thead>
        <tbody>
          {productosBase.map((producto, index) => {
            const precio = preciosProveedores[proveedorActivo].precios[index];
            const cantidad = producto.cantidades;
            const estaSeleccionado = seleccionesPorProveedor[proveedorActivo][index];
            const sub = calcularSubtotal(precio, cantidad);
            return (
              <tr key={index}>
                <td className="border px-4 py-2">{producto.nombre}</td>
                <td className="border px-4 py-2">${precio}</td>
                <td className="border px-4 py-2">{cantidad}</td>
                <td className="border px-4 py-2 text-center">
                  <input
                    type="checkbox"
                    checked={estaSeleccionado}
                    onChange={() => toggleSeleccion(index)}
                  />
                </td>
                <td className="border px-4 py-2">${(sub * 0.21).toFixed(2)}</td>
                <td className="border px-4 py-2">${sub.toFixed(2)}</td>
              </tr>
            );
          })}
        </tbody>
      </table>

      <div className="mt-6">
        <p><strong>Fecha de Entrega:</strong> {preciosProveedores[proveedorActivo].fechaEntrega}</p>
        <p><strong>IVA:</strong> ${iva.toFixed(2)}</p>
        <p><strong>Total:</strong> ${total.toFixed(2)}</p>
      </div>

      <div className="mt-6 text-right">
        <button
          onClick={() =>
            navigate(`/Ordenes_Presupuesto_Final/${id}`, {
              state: {
                orden,
                productosBase,
                preciosProveedores,
                seleccionados: seleccionesPorProveedor[proveedorActivo],
                proveedorActivo,
              },
            })
          }
          className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          Siguiente ➜
        </button>
      </div>
    </div>
  );
};
