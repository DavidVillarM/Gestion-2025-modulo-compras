// File: OrdenesPresupuesto.jsx
import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Tab, TabList, TabPanel, Tabs } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';

// Datos de ejemplo: proveedores y productos
const proveedores = [
  {
    id: 1,
    nombre: 'Proveedor A',
    fechaEnvio: '2025-06-05',
    productos: [
      { id: 1, nombre: 'Producto A', cantidad: 20, precio: 100 },
      { id: 2, nombre: 'Producto B', cantidad: 25, precio: 200 },
      { id: 3, nombre: 'Producto C', cantidad: 30, precio: 150 },
    ],
  },
  {
    id: 2,
    nombre: 'Proveedor B',
    fechaEnvio: '2025-06-07',
    productos: [
      { id: 1, nombre: 'Producto A', cantidad: 20, precio: 110 },
      { id: 2, nombre: 'Producto B', cantidad: 25, precio: 195 },
      { id: 3, nombre: 'Producto C', cantidad: 30, precio: 160 },
    ],
  },
  {
    id: 3,
    nombre: 'Proveedor C',
    fechaEnvio: '2025-06-10',
    productos: [
      { id: 1, nombre: 'Producto A', cantidad: 20, precio: 105 },
      { id: 2, nombre: 'Producto B', cantidad: 25, precio: 210 },
      { id: 3, nombre: 'Producto C', cantidad: 30, precio: 155 },
    ],
  },
];

const IVA = 0.12;

export const OrdenesPresupuesto = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const idFromState = location.state?.ordenId;
  const idStored = localStorage.getItem('ordenId');
  const ordenId = idFromState || idStored;

  useEffect(() => {
    if (idFromState) localStorage.setItem('ordenId', idFromState);
  }, [idFromState]);

  const initialSelections = proveedores.reduce((acc, prov) => {
    acc[prov.id] = {};
    prov.productos.forEach(p => {
      acc[prov.id][p.id] = false;
    });
    return acc;
  }, {});

  const [selections, setSelections] = useState(initialSelections);
  const [activeTab, setActiveTab] = useState(0);
  const [error, setError] = useState('');

  const isDisabled = (provId, prodId) =>
    proveedores.some(p => p.id !== provId && selections[p.id][prodId]);

  const toggleSelection = (provId, prodId) => {
    setSelections(prev => ({
      ...prev,
      [provId]: { ...prev[provId], [prodId]: !prev[provId][prodId] }
    }));
  };

  const calcTotals = prov => {
    let subtotal = 0;
    prov.productos.forEach(p => {
      if (selections[prov.id][p.id]) subtotal += p.precio * p.cantidad;
    });
    const iva = subtotal * IVA;
    return { subtotal, iva, total: subtotal + iva };
  };

  // Al presionar Siguiente, enviamos los detalles seleccionados a la siguiente pantalla
  const handleSiguiente = () => {
    const detalles = [];
    proveedores.forEach(prov => {
      prov.productos.forEach(prod => {
        if (selections[prov.id][prod.id]) {
          const subtotal = prod.precio * prod.cantidad;
          detalles.push({
            idProducto: prod.id,
            idProveedor: prov.id,
            cotizacion: prod.precio,
            cantidad: prod.cantidad,
            iva: parseFloat((subtotal * IVA).toFixed(2))
          });
        }
      });
    });

    if (detalles.length > 0) {
      setError('');
      // Navegamos a la pantalla de resumen final pasando toda la información necesaria
      navigate('/ordenes-presupuesto-final', { state: { ordenId, detalles } });
    } else {
      setError('Debes seleccionar al menos un producto.');
    }
  };

  return (
    <div className="p-8 bg-white min-h-screen">
      <h2 className="text-3xl font-bold mb-6">Presupuesto Orden #{ordenId}</h2>
      {error && <p className="text-red-600 mb-4">{error}</p>}

      <Tabs selectedIndex={activeTab} onSelect={setActiveTab}>
        <TabList className="flex space-x-4 border-b mb-4">
          {proveedores.map(p => (
            <Tab
              key={p.id}
              selectedClassName="border-b-2 border-blue-600 font-semibold"
            >
              {p.nombre}
            </Tab>
          ))}
        </TabList>

        {proveedores.map(prov => {
          const { subtotal, iva, total } = calcTotals(prov);
          return (
            <TabPanel key={prov.id}>
              <table className="w-full mb-4 table-auto border border-gray-200">
                <thead>
                  <tr className="bg-gray-100">
                    <th className="p-2 border">Seleccionar</th>
                    <th className="p-2 border">Producto</th>
                    <th className="p-2 border">Cantidad</th>
                    <th className="p-2 border">Precio Unitario</th>
                    <th className="p-2 border">Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {prov.productos.map(prod => (
                    <tr key={prod.id}>
                      <td className="p-2 border text-center">
                        <input
                          type="checkbox"
                          checked={selections[prov.id][prod.id]}
                          disabled={isDisabled(prov.id, prod.id)}
                          onChange={() => toggleSelection(prov.id, prod.id)}
                        />
                      </td>
                      <td className="p-2 border">{prod.nombre}</td>
                      <td className="p-2 border">{prod.cantidad}</td>
                      <td className="p-2 border">{prod.precio}</td>
                      <td className="p-2 border">
                        {(prod.precio * prod.cantidad).toFixed(2)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              <div className="flex justify-between items-center mb-6">
                <p><strong>Subtotal:</strong> {subtotal.toFixed(2)}</p>
                <p><strong>IVA:</strong> {iva.toFixed(2)}</p>
                <p className="font-semibold"><strong>Total:</strong> {total.toFixed(2)}</p>
              </div>
            </TabPanel>
          );
        })}
      </Tabs>

      <button
        onClick={handleSiguiente}
        className="mt-4 px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
      >
        Siguiente: Resumen Final
      </button>
    </div>
  );
};
