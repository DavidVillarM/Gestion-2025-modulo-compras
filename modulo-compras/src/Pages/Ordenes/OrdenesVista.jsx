import React from 'react';
import { useNavigate } from 'react-router-dom';

export const OrdenesVista = () => {
  const navigate = useNavigate();

  const productos = [
    { nombre: 'Producto A', numeroEnvio: '12345', estadoEnvio: 'En tránsito', cantidad: 10, subtotal: 1000, fechaEntrega: '2025-05-01' },
    { nombre: 'Producto B', numeroEnvio: '12346', estadoEnvio: 'Enviado', cantidad: 5, subtotal: 500, fechaEntrega: '2025-05-03' },
    { nombre: 'Producto C', numeroEnvio: '12347', estadoEnvio: 'Pendiente', cantidad: 8, subtotal: 1200, fechaEntrega: '2025-05-05' },
  ];

  const total = productos.reduce((acc, producto) => acc + producto.subtotal, 0);
  const fechaOrden = new Date().toLocaleDateString();

  return (
    <div className="p-6 bg-white min-h-screen">
      <h2 className="text-2xl font-bold mb-4">Orden Nro# 101</h2>

      <table className="min-w-full border">
        <thead>
          <tr className="bg-gray-100">
            <th className="border px-4 py-2">Producto</th>
            <th className="border px-4 py-2">Número de Envío</th>
            <th className="border px-4 py-2">Estado de Envío</th>
            <th className="border px-4 py-2">Cantidad</th>
            <th className="border px-4 py-2">Subtotal</th>
            <th className="border px-4 py-2">Fecha de Entrega Estimada</th>
          </tr>
        </thead>
        <tbody>
          {productos.map((producto, index) => (
            <tr key={index}>
              <td className="border px-4 py-2">{producto.nombre}</td>
              <td className="border px-4 py-2">{producto.numeroEnvio}</td>
              <td className="border px-4 py-2">{producto.estadoEnvio}</td>
              <td className="border px-4 py-2">{producto.cantidad}</td>
              <td className="border px-4 py-2">${producto.subtotal}</td>
              <td className="border px-4 py-2">{producto.fechaEntrega}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="mt-6">
        <p><strong>Total:</strong> ${total}</p>
        <p><strong>Fecha de Orden Realizada:</strong> {fechaOrden}</p>
      </div>

      <div className="mt-6 text-right">
        <button
          onClick={() => navigate('/Ordenes')}
          className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          Volver al Inicio
        </button>
      </div>
    </div>
  );
};
