import React from 'react';

const categories = [
  { id: 1, nombre: 'Electrónica', descripcion: 'Dispositivos y gadgets' },
  { id: 2, nombre: 'Ropa', descripcion: 'Vestimenta y moda' },
  { id: 3, nombre: 'Alimentos', descripcion: 'Comestibles y bebidas' },
];

export function Categorias() {
  return (
    <div className="p-6 w-full">
      <h1 className="text-2xl font-bold mb-4">Gestión de Categorías</h1>

      <div className="overflow-x-auto">
        <table className="min-w-full text-left border rounded-lg shadow-md bg-white">
          <thead className="bg-gray-100 border-b">
            <tr>
              <th className="px-4 py-2">ID</th>
              <th className="px-4 py-2">Nombre</th>
              <th className="px-4 py-2">Descripción</th>
              <th className="px-4 py-2 text-center">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {categories.map((cat) => (
              <tr key={cat.id} className="border-b hover:bg-gray-50">
                <td className="px-4 py-2">{cat.id}</td>
                <td className="px-4 py-2">{cat.nombre}</td>
                <td className="px-4 py-2">{cat.descripcion}</td>
                <td className="px-4 py-2 flex justify-center gap-2">
                  <button className="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600">Editar</button>
                  <button className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600">Eliminar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="mt-6">
        <button className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600">
          Nueva Categoría
        </button>
      </div>
    </div>
  );
}
