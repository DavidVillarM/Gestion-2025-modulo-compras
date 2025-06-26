// File: OrdenesPresupuesto.jsx
import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Tab, TabList, TabPanel, Tabs } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';

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
  const [productosAgrupados, setProductosAgrupados] = useState([]);
  const [selections, setSelections] = useState({});
  const [activeTab, setActiveTab] = useState(0);
  const [error, setError] = useState('');

  useEffect(() => {
    fetch(`http://localhost:5000/api/Presupuestoo/Orden/${ordenId}`)
      .then((res) => res.json())
      .then((data) => {
        setPresupuestos(data);

        // Inicializar selección
        const initialSelections = {};
        data.forEach((presupuesto) => {
          initialSelections[presupuesto.idProveedor] = {};
          presupuesto.detalles.forEach((detalle) => {
            initialSelections[presupuesto.idProveedor][detalle.idProducto] = false;
          });
        });
        setSelections(initialSelections);

        // Agrupar productos
        const productosMap = {};

        data.forEach((presupuesto) => {
          presupuesto.detalles.forEach((detalle) => {
            if (!productosMap[detalle.idProducto]) {
              productosMap[detalle.idProducto] = {
                idProducto: detalle.idProducto,
                nombre: detalle.producto,
                proveedores: [],
              };
            }
            productosMap[detalle.idProducto].proveedores.push({
              proveedor: presupuesto.proveedor,
              idProveedor: presupuesto.idProveedor,
              precio: detalle.precio,
              cantidad: detalle.cantidad,
              iva5: detalle.iva5,
              iva10: detalle.iva10,
              fechaEntrega: presupuesto.fechaEntrega,
            });
          });
        });

        // Convertir a array y ordenar cada lista de proveedores por precio
        const agrupados = Object.values(productosMap).map((producto) => ({
          ...producto,
          proveedores: producto.proveedores.sort((a, b) => a.precio - b.precio),
        }));

        setProductosAgrupados(agrupados);
      })
      .catch((err) => {
        console.error('Error al cargar los presupuestos:', err);
        setError('Error al cargar los presupuestos.');
      });
  }, [ordenId]);

  const toggleSelection = (provId, prodId) => {
    setSelections((prev) => ({
      ...prev,
      [provId]: {
        ...prev[provId],
        [prodId]: !prev[provId][prodId],
      },
    }));
  };

  const isDisabled = (provId, prodId) =>
    presupuestos.some(
      (p) => p.idProveedor !== provId && selections[p.idProveedor]?.[prodId]
    );

  const handleSiguiente = () => {
    const detalles = [];

    presupuestos.forEach((presupuesto) => {
      const { idProveedor, fechaEntrega } = presupuesto;
      presupuesto.detalles.forEach((detalle) => {
        const { idProducto, precio, cantidad, iva5, iva10 } = detalle;
        if (selections[idProveedor]?.[detalle.idProducto]) {
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
        Comparativa de productos - Orden #{ordenId}
      </h2>
      {error && <p className="text-red-600 mb-4">{error}</p>}

      <Tabs selectedIndex={activeTab} onSelect={setActiveTab}>
        <TabList className="flex space-x-4 border-b mb-4 overflow-x-auto">
          {productosAgrupados.map((p) => (
            <Tab
              key={p.idProducto}
              selectedClassName="border-b-2 border-blue-600 font-semibold"
            >
              {p.nombre}
            </Tab>
          ))}
        </TabList>

        {productosAgrupados.map((producto) => (
          <TabPanel key={producto.idProducto}>
            <table className="w-full mb-4 table-auto border border-gray-200">
              <thead>
                <tr className="bg-gray-100">
                  <th className="p-2 border">Seleccionar</th>
                  <th className="p-2 border">Proveedor</th>
                  <th className="p-2 border">Cantidad</th>
                  <th className="p-2 border">Precio Unitario</th>
                  <th className="p-2 border">IVA 5%</th>
                  <th className="p-2 border">IVA 10%</th>
                  <th className="p-2 border">Subtotal</th>
                  <th className="p-2 border">Entrega</th>
                </tr>
              </thead>
              <tbody>
                {producto.proveedores.map((prov) => (
                  <tr key={prov.idProveedor}>
                    <td className="p-2 border text-center">
                      <input
                        type="checkbox"
                        checked={
                          selections[prov.idProveedor]?.[producto.idProducto] || false
                        }
                        disabled={isDisabled(prov.idProveedor, producto.idProducto)}
                        onChange={() =>
                          toggleSelection(prov.idProveedor, producto.idProducto)
                        }
                      />
                    </td>
                    <td className="p-2 border">{prov.proveedor}</td>
                    <td className="p-2 border">{prov.cantidad}</td>
                    <td className="p-2 border">{prov.precio}</td>
                    <td className="p-2 border">{prov.iva5.toFixed(2)}</td>
                    <td className="p-2 border">{prov.iva10.toFixed(2)}</td>
                    <td className="p-2 border">
                      {(prov.precio * prov.cantidad).toFixed(2)}
                    </td>
                    <td className="p-2 border">{prov.fechaEntrega}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </TabPanel>
        ))}
      </Tabs>

      <div className="text-right">
        <button
          onClick={handleSiguiente}
          className="mt-4 px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        >
          Siguiente: Resumen Final
        </button>
      </div>
    </div>
  );
};
