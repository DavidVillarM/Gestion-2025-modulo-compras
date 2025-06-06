// File: OrdenesPresupuesto.jsx
import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Tab, TabList, TabPanel, Tabs } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';

const IVA = 0.21; // si usas un IVA fijo general

export const OrdenesPresupuesto = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const idFromState = location.state?.ordenId;
  const idStored = localStorage.getItem('ordenId');
  const ordenId = idFromState || idStored;

  useEffect(() => {
    if (idFromState) localStorage.setItem('ordenId', idFromState);
  }, [idFromState]);

  const [presupuestos, setPresupuestos] = useState([]);
  const [selections, setSelections] = useState({});
  const [activeTab, setActiveTab] = useState(0);
  const [error, setError] = useState('');

  useEffect(() => {
    fetch(`http://localhost:5000/api/Presupuestoo/Orden/${ordenId}`)
      .then((res) => res.json())
      .then((data) => {
        setPresupuestos(data);
        const initial = {};
        data.forEach((presupuesto) => {
          initial[presupuesto.idProveedor] = {};
          presupuesto.detalles.forEach((detalle) => {
            initial[presupuesto.idProveedor][detalle.idProducto] = false;
          });
        });
        setSelections(initial);
      })
      .catch((err) => {
        console.error('Error al cargar los presupuestos:', err);
        setError('Error al cargar los presupuestos.');
      });
  }, [ordenId]);

  const isDisabled = (provId, prodId) =>
    presupuestos.some(
      (p) => p.idProveedor !== provId && selections[p.idProveedor]?.[prodId]
    );

  const toggleSelection = (provId, prodId) => {
    setSelections((prev) => ({
      ...prev,
      [provId]: {
        ...prev[provId],
        [prodId]: !prev[provId][prodId],
      },
    }));
  };

  const calcTotals = (prov) => {
    let subtotal = 0;
    let iva5 = 0;
    let iva10 = 0;

    prov.detalles.forEach((p) => {
      if (selections[prov.idProveedor]?.[p.idProducto]) {
        const itemSubtotal = p.precio * p.cantidad;
        subtotal += itemSubtotal;
        iva5 += p.iva5 || 0;
        iva10 += p.iva10 || 0;
      }
    });

    return {
      subtotal,
      iva5,
      iva10,
      total: subtotal + iva5 + iva10,
    };
  };

  const handleSiguiente = () => {
    const detalles = [];

    presupuestos.forEach((presupuesto) => {
      const { idProveedor, fechaEntrega } = presupuesto;
      presupuesto.detalles.forEach((detalle) => {
        const { idProducto, precio, cantidad, iva5, iva10 } = detalle;
        if (selections[idProveedor]?.[idProducto]) {
          const iva = parseFloat((iva5 + iva10).toFixed(2));
          detalles.push({
            idProducto,
            idProveedor,
            cotizacion: precio,
            cantidad,
            iva,
            fechaEntrega,
          });
        }
      });
    });

    if (detalles.length > 0) {
      setError('');
      navigate('/ordenes-presupuesto-final', {
        state: { ordenId, detalles },
      });
    } else {
      setError('Debes seleccionar al menos un producto.');
    }
  };

  return (
    <div className="p-8 bg-white min-h-screen">
      <h2 className="text-3xl font-bold mb-6">
        Presupuesto Orden #{ordenId}
      </h2>
      {error && <p className="text-red-600 mb-4">{error}</p>}

      <Tabs selectedIndex={activeTab} onSelect={setActiveTab}>
        <TabList className="flex space-x-4 border-b mb-4 ">
          {presupuestos.map((p) => (
            <Tab
              key={p.idProveedor}
              selectedClassName="border-b-2 border-blue-600 font-semibold"
            >
              {p.proveedor}
            </Tab>
          ))}
        </TabList>

        {presupuestos.map((presupuesto) => {
          const { idProveedor, detalles, fechaEntrega } = presupuesto;
          const { subtotal, iva5, iva10, total } = calcTotals(presupuesto);

          return (
            <TabPanel key={idProveedor}>
              <table className="w-full mb-4 table-auto border border-gray-200">
                <thead>
                  <tr className="bg-gray-100">
                    <th className="p-2 border">Seleccionar</th>
                    <th className="p-2 border">Producto</th>
                    <th className="p-2 border">Cantidad</th>
                    <th className="p-2 border">Precio Unitario</th>
                    <th className="p-2 border">IVA 5%</th>
                    <th className="p-2 border">IVA 10%</th>
                    <th className="p-2 border">Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {detalles.map((prod) => (
                    <tr key={prod.idProducto}>
                      <td className="p-2 border text-center">
                        <input
                          type="checkbox"
                          checked={
                            selections[idProveedor]?.[prod.idProducto] || false
                          }
                          disabled={isDisabled(idProveedor, prod.idProducto)}
                          onChange={() =>
                            toggleSelection(idProveedor, prod.idProducto)
                          }
                        />
                      </td>
                      <td className="p-2 border">{prod.producto}</td>
                      <td className="p-2 border">{prod.cantidad}</td>
                      <td className="p-2 border">{prod.precio}</td>
                      <td className="p-2 border">{prod.iva5.toFixed(2)}</td>
                      <td className="p-2 border">{prod.iva10.toFixed(2)}</td>
                      <td className="p-2 border">
                        {(prod.precio * prod.cantidad).toFixed(2)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              <div className="flex flex-col gap-1 text-right mb-6">
                <p>
                  <strong>Subtotal:</strong> {subtotal.toFixed(2)}
                </p>
                <p>
                  <strong>IVA 5%:</strong> {iva5.toFixed(2)}
                </p>
                <p>
                  <strong>IVA 10%:</strong> {iva10.toFixed(2)}
                </p>
                <p className="font-semibold">
                  <strong>Total parcial:</strong> {total.toFixed(2)}
                </p>
                <p>
                  <strong>Fecha de entrega:</strong> {fechaEntrega}
                </p>
                  <button
        onClick={handleSiguiente}
        className="mt-4 px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
      >
        Siguiente: Resumen Final
      </button>
              </div>
            </TabPanel>
          );
        })}
      </Tabs>

    
    </div>
  );
};
