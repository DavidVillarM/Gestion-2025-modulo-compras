import React from 'react';

export default function FacturaDetalleModal({ factura, onClose }) {
  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex justify-center items-center">
      <div className="bg-white p-6 rounded shadow max-w-2xl w-full">
        <h2 className="text-xl font-bold mb-4">Detalles de Factura #{factura.idFactura}</h2>
        <p><strong>Proveedor:</strong> {factura.nombreProveedor}</p>
        <p><strong>Fecha:</strong> {factura.fecha}</p>
        <p><strong>Total:</strong> Gs. {factura.montoTotal?.toLocaleString()}</p>
        <p className="mt-4 font-semibold">Productos:</p>
        <ul className="list-disc pl-6">
          {factura.facturaDetalles?.map((item, i) => (
            <li key={i}>
              Producto ID: {item.idProducto} - Cantidad: {item.cantidad} - Precio: Gs. {item.precio} - IVA5: {item.iva5} - IVA10: {item.iva10}
            </li>
          ))}
        </ul>
        <button onClick={onClose} className="mt-4 bg-gray-600 text-white px-4 py-2 rounded">Cerrar</button>
      </div>
    </div>
  );
}
