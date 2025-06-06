/* OrdenesPago.jsx */
import React, { useState, useEffect } from 'react';
import DataTable from 'react-data-table-component';
import { FiTrash2 } from 'react-icons/fi';
import { FaEye } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

export const OrdenesPago = () => {
  const navigate = useNavigate();
  const [dataOriginal, setDataOriginal] = useState([]);
  const [data, setData] = useState([]);
  const [search, setSearch] = useState('');
  const [estadoFiltro, setEstadoFiltro] = useState('');

  const fetchOrdenes = async () => {
  try {
    const token = localStorage.getItem('token');
    const response = await axios.get('http://localhost:5000/api/OrdenesPago', {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    setDataOriginal(response.data);
    setData(response.data);
  } catch (error) {
    console.error('Error al obtener órdenes:', error);
  }
};
  
  

  useEffect(() => {
    fetchOrdenes();
  }, []);

  useEffect(() => {
    let resultado = [...dataOriginal];

    if (search.trim()) {
      // Buscar sobre el campo idOrden convertido a string
      resultado = resultado.filter(item =>
        item.idOrden.toString().includes(search.trim())
      );
    }

    if (estadoFiltro) {
      resultado = resultado.filter(item => item.estado === estadoFiltro);
    }

    setData(resultado);
  }, [search, estadoFiltro, dataOriginal]);

  const handleEliminar = async (idOrden) => {
    if (!window.confirm('¿Confirma la eliminacion de esta orden?')) return;
    try {
      await axios.delete(`http://localhost:5000/api/OrdenesPago/${idOrden}`);
      setDataOriginal(prev => prev.filter(item => item.idOrden !== idOrden));
      setData(prev => prev.filter(item => item.idOrden !== idOrden));
    } catch (error) {
      console.error('Error al eliminar la orden:', error);
    }
  };

  const columns = [
    { name: 'Nro Orden', selector: row => row.idOrden, sortable: true },
    { name: 'Fecha', selector: row => row.fechaPedido, sortable: true },
    { name: 'Total', selector: row => row.montoTotal, sortable: true },
    {
      name: 'Estado',
      cell: row => (
        <span
          className={`cursor-pointer ${row.estado === 'Incompleta' ? 'text-red-600 underline' : ''}`}
          onClick={() => {
            if (row.estado === 'Incompleta') {
              // Si necesitas mantener la orden para otra pantalla:
              localStorage.setItem('ordenId', row.idOrden);
              navigate(`/ordenes-presupuesto`, { state: { orden: row } });
            }
          }}
        >
          {row.estado}
        </span>
      ),
      sortable: true,
      ignoreRowClick: true,
      allowOverflow: true,
      button: true,
    },
    {
      name: 'Acciones',
      cell: row => (
        <div className="flex gap-2">
          {/* Aquí cambiamos row.id por row.idOrden */}
          <button
            onClick={() => navigate(`/ordenes-vista/${row.idOrden}`)}
            className="text-blue-600 text-xl hover:text-blue-800"
          >
            <FaEye />
          </button>
          <button
            onClick={() => handleEliminar(row.idOrden)}
            className="text-red-600 text-xl hover:text-red-800"
          >
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
      <h2 className="text-2xl font-semibold mb-4">Ordenes</h2>

      <div className="flex flex-wrap gap-4 mb-4">
        <input
          type="text"
          placeholder="Buscar por Nro de Orden"
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="px-4 py-2 border rounded shadow-sm"
        />

        <select
          value={estadoFiltro}
          onChange={e => setEstadoFiltro(e.target.value)}
          className="px-4 py-2 border rounded shadow-sm"
        >
          <option value="">Todos los estados</option>
          <option value="Completa">Completa</option>
          <option value="Incompleta">Incompleta</option>
          <option value="Pendiente">Pendiente</option>
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
