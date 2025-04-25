import React, { useState, useEffect } from 'react';
import DataTable from 'react-data-table-component';
import { FiTrash2 } from 'react-icons/fi';
import { FaEye } from 'react-icons/fa';
import { Outlet, useNavigate } from 'react-router-dom';

export const OrdenesPago = () => {
  const navigate = useNavigate();

  const ordenesPagoOriginal = [
    { id: 101, nroOrden: 'OP-001', estado: 'Completa', fecha: '2025-03-15', total: 1500 },
    { id: 102, nroOrden: 'OP-002', estado: 'Incompleta', fecha: '2025-03-16', total: 2000 },
    { id: 103, nroOrden: 'OP-003', estado: 'Sin realizar', fecha: '2025-03-17', total: 0 },
    { id: 104, nroOrden: 'OP-004', estado: 'Completa', fecha: '2025-03-18', total: 900 },
    { id: 105, nroOrden: 'OP-005', estado: 'Incompleta', fecha: '2025-03-19', total: 2200 },
    { id: 106, nroOrden: 'OP-006', estado: 'Sin realizar', fecha: '2025-03-20', total: 0 },
    { id: 107, nroOrden: 'OP-007', estado: 'Completa', fecha: '2025-03-21', total: 1300 },
    { id: 108, nroOrden: 'OP-008', estado: 'Sin realizar', fecha: '2025-03-22', total: 0 },
    { id: 109, nroOrden: 'OP-009', estado: 'Incompleta', fecha: '2025-03-23', total: 950 },
    { id: 110, nroOrden: 'OP-010', estado: 'Completa', fecha: '2025-03-24', total: 3000 },
    { id: 111, nroOrden: 'OP-011', estado: 'Sin realizar', fecha: '2025-03-20', total: 0 },
    { id: 112, nroOrden: 'OP-012', estado: 'Completa', fecha: '2025-03-21', total: 1300 },
    { id: 113, nroOrden: 'OP-013', estado: 'Sin realizar', fecha: '2025-03-22', total: 0 },
    { id: 114, nroOrden: 'OP-014', estado: 'Incompleta', fecha: '2025-03-23', total: 950 },
    { id: 115, nroOrden: 'OP-015', estado: 'Completa', fecha: '2025-03-24', total: 3000 },
  ];

  const [data, setData] = useState(ordenesPagoOriginal);
  const [search, setSearch] = useState('');
  const [estadoFiltro, setEstadoFiltro] = useState('');

  // Efecto para filtrar automáticamente
  useEffect(() => {
    let resultado = ordenesPagoOriginal;

    if (search.trim()) {
      resultado = resultado.filter(item =>
        item.nroOrden.toLowerCase().includes(search.toLowerCase())
      );
    }

    if (estadoFiltro) {
      resultado = resultado.filter(item => item.estado === estadoFiltro);
    }

    setData(resultado);
  }, [search, estadoFiltro]);

  const handleEliminar = (id) => {
    setData(prev => prev.filter(item => item.id !== id));
  };

  const columns = [
    { name: 'Nro Orden', selector: row => row.nroOrden, sortable: true },
    { name: 'Fecha', selector: row => row.fecha, sortable: true },
    { name: 'Total', selector: row => row.total, sortable: true },
    {
      name: 'Estado',
      cell: row => (
        <span
          className={`cursor-pointer ${row.estado === 'Sin realizar' ? 'text-red-600 underline' : ''}`}
          onClick={() => {
            if (row.estado === 'Sin realizar') {
              navigate(`OrdenesPresupuesto/${row.id}`, { state: { orden: row } });
            }
          }}
        >
          {row.estado}
        </span>
      ),
      sortable: true
    },
    {
      name: 'Acciones',
      cell: row => (
        <div className="flex gap-2">
          <button onClick={() => navigate(`Vistas/`)} className="text-blue-600 text-xl hover:text-blue-800">
            <FaEye />
          </button>
          <button onClick={() => handleEliminar(row.id)} className="text-red-600 text-xl hover:text-red-800">
            <FiTrash2 />
          </button>
        </div>
      ),
      ignoreRowClick: true,
      allowOverflow: true,
      button: true,
    },
  ];

  return (
    <div className="p-6 bg-gray-100 min-h-screen">
      <h2 className="text-2xl font-semibold mb-4">Ordenes de Pago</h2>

      <div className="flex flex-wrap gap-4 mb-4">
        <input
          type="text"
          placeholder="Buscar por Nro de Orden"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="px-4 py-2 border rounded shadow-sm"
        />

        <select
          value={estadoFiltro}
          onChange={(e) => setEstadoFiltro(e.target.value)}
          className="px-4 py-2 border rounded shadow-sm"
        >
          <option value="">Todos los estados</option>
          <option value="Completa">Completa</option>
          <option value="Incompleta">Incompleta</option>
          <option value="Sin realizar">Sin realizar</option>
        </select>
      </div>

      <DataTable
        columns={columns}
        data={data}
        pagination
        highlightOnHover
        responsive
        striped
        noDataComponent="No se encontraron ordenes"
      />
      
    </div>
  );
};
