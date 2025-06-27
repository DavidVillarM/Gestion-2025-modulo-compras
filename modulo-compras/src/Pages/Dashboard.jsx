import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer
} from 'recharts';
import { FaMoneyBillWave, FaUserFriends, FaUserPlus, FaChartBar } from 'react-icons/fa';
import { FaBox } from 'react-icons/fa';

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
      <h1 className="text-2xl font-bold mb-6">Dashboard</h1>

      {/* Métricas principales */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
        <Card icon={<FaMoneyBillWave size={24} />} title="Compras del Mes" value={`₲ ${comprasMensuales.toLocaleString()}`} change="+55%" trend="positive" />
        <Card icon={<FaUserFriends size={24} />} title="Órdenes Pendientes" value={ordenesPendientes} change="+3%" trend="positive" />
        <Card icon={<FaUserPlus size={24} />} title="Pedidos en Curso" value={pedidosEnCurso} change="-2%" trend="negative" />
        <Card icon={<FaChartBar size={24} />} title="Pagos Pendientes" value={pagosPendientes} change="+5%" trend="positive" />
      </div>

      {/* Gráfico */}
      <div className="bg-white p-4 rounded shadow mb-6">
        <h3 className="text-lg font-semibold mb-2">Cantidad de productos pedidos por mes</h3>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={productosPorMes}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="mes" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="cantidad" fill="#4ade80" />
          </BarChart>
        </ResponsiveContainer>
      </div>

      {/* Tablas */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <ProductTable title="Productos más pedidos" data={productosMasPedidos} />
        <ProductTable title="Productos menos pedidos" data={productosMenosPedidos} />
      </div>
    </div>
  );
};

// Componente Card
const Card = ({ icon, title, value, change, trend }) => {
  const trendColor = trend === 'positive' ? 'text-green-500' : 'text-red-500';

  return (
    <div className="bg-white shadow p-4 rounded flex flex-col gap-2">
      <div className="flex items-center gap-2 text-gray-600">
        {icon}
        <span className="font-semibold text-sm">{title}</span>
      </div>
      <div className="text-2xl font-bold">{value}</div>
      <div className={`text-sm ${trendColor}`}>{change} que el periodo anterior</div>
    </div>
  );
};

// Componente tabla reutilizable
const ProductTable = ({ title, data }) => {
  const maxCantidad = Math.max(...data.map((p) => p.total || 1), 1);

  return (
    <div className="bg-white p-4 rounded shadow">
      <h3 className="text-lg font-semibold mb-4">{title}</h3>
      <div className="flex flex-col gap-4">
        {data.map((prod, idx) => {
          const porcentaje = Math.round((prod.total / maxCantidad) * 100);

          return (
            <div key={idx} className="flex items-center justify-between">
              {/* Producto + nombre */}
              <div className="flex items-center gap-3 w-1/3">
                <div className="bg-gray-200 p-2 rounded">
                  <FaBox className="text-gray-600" />
                </div>
                <div>
                  <p className="font-semibold">{prod.nombre}</p>
                  <p className="text-sm text-gray-500">Gs. {prod.precioUnitario.toLocaleString()}</p>
                </div>
              </div>

              {/* Cantidad y barra */}
              <div className="w-1/2">
                <div className="text-sm text-right text-gray-700 mb-1">{prod.total} pedidos</div>
                <div className="w-full bg-gray-100 rounded h-2">
                  <div
                    className="bg-blue-500 h-2 rounded"
                    style={{ width: `${porcentaje}%` }}
                  ></div>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};


export default Dashboard;
