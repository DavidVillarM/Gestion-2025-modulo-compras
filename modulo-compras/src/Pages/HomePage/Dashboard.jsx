import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer
} from 'recharts';

const Dashboard = () => {
  const [comprasMensuales, setComprasMensuales] = useState(0);
  const [ordenesPendientes, setOrdenesPendientes] = useState(0);
  const [pedidosEnCurso, setPedidosEnCurso] = useState(0);
  const [pagosPendientes, setPagosPendientes] = useState(0);
  const [productosMasPedidos, setProductosMasPedidos] = useState([]);
  const [productosMenosPedidos, setProductosMenosPedidos] = useState([]);
  const [productosPorMes, setProductosPorMes] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const endpoints = [
        ['compras-mensuales', setComprasMensuales],
        ['ordenes-pendientes', setOrdenesPendientes],
        ['pedidos-en-curso', setPedidosEnCurso],
        ['pagos-pendientes', setPagosPendientes],
        ['productos-mas-pedidos', setProductosMasPedidos],
        ['productos-menos-pedidos', setProductosMenosPedidos],
        ['productos-por-mes', setProductosPorMes]
      ];

      for (const [endpoint, setter] of endpoints) {
        try {
          const res = await axios.get(`http://localhost:5000/api/dashboard/${endpoint}`);
          setter(res.data);
        } catch (error) {
          console.error(`Error al cargar ${endpoint}`, error);
        }
      }
    };

    fetchData();
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-6">Panel de Control</h1>

      {/* Métricas */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
        <div className="bg-blue-100 p-4 rounded inset-shadow-sm w-full justify-items-center text-center">
            <h2 className="text-sm font-semibold">Compras del Mes</h2>
            <p className="text-[80px] font-bold leading-none">₲ {comprasMensuales.toLocaleString()}</p>
        </div>
        <div className="bg-yellow-100 p-4 rounded inset-shadow-sm w-full justify-items-center text-center">
            <h2 className="text-sm font-semibold">Órdenes Pendientes</h2>
            <p className="text-[80px] font-bold leading-none">{ordenesPendientes}</p>
        </div>
        <div className="bg-green-100 p-4 rounded inset-shadow-sm w-full justify-items-center text-center">
            <h2 className="text-sm font-semibold">Pedidos en Curso</h2>
            <p className="text-[80px] font-bold leading-none">{pedidosEnCurso}</p>
        </div>
        <div className="bg-red-100 p-4 rounded inset-shadow-sm w-full justify-items-center text-center">
            <h2 className="text-sm font-semibold">Pagos Pendientes</h2>
            <p className="text-[80px] font-bold leading-none">{pagosPendientes}</p>
        </div>
      </div>

      {/* Tablas */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
        <div>
            <h3 className="text-lg font-semibold mb-2">Productos más pedidos</h3>
            <table className="w-full border text-sm">
                <thead className="bg-gray-100">
                <tr>
                    <th className="border px-2 py-1">Producto</th>
                    <th className="border px-2 py-1">Precio Unitario</th>
                    <th className="border px-2 py-1">Cantidad</th>
                </tr>
                </thead>
                <tbody>
                {productosMasPedidos.map((prod, idx) => (
                    <tr key={idx}>
                    <td className="border px-2 py-1">{prod.nombre}</td>
                    <td className="border px-2 py-1 text-center">Gs. {prod.precioUnitario.toLocaleString()}</td>
                    <td className="border px-2 py-1 text-center">{prod.total}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>


        <div>
            <h3 className="text-lg font-semibold mb-2">Productos menos pedidos</h3>
            <table className="w-full border text-sm">
                <thead className="bg-gray-100">
                <tr>
                    <th className="border px-2 py-1">Producto</th>
                    <th className="border px-2 py-1">Precio Unitario</th>
                    <th className="border px-2 py-1">Cantidad</th>
                </tr>
                </thead>
                <tbody>
                {productosMenosPedidos.map((prod, idx) => (
                    <tr key={idx}>
                    <td className="border px-2 py-1">{prod.nombre}</td>
                    <td className="border px-2 py-1 text-center">Gs. {prod.precioUnitario.toLocaleString()}</td>
                    <td className="border px-2 py-1 text-center">{prod.total}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>

      </div>

      {/* Gráfico */}
      <div>
        <h3 className="text-lg font-semibold mb-2">Cantidad de productos pedidos por mes</h3>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={productosPorMes}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="mes" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="cantidad" fill="#3b82f6" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default Dashboard;
