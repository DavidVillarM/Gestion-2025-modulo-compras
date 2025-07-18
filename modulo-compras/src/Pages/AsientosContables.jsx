import React, { useState, useEffect } from 'react';
import axios from 'axios';
import DataTable from 'react-data-table-component';
import { FaEye } from 'react-icons/fa';

export const AsientosContables = () => {
  const [asientos, setAsientos] = useState([]);
  const [asientosFiltrados, setAsientosFiltrados] = useState([]);
  const [search, setSearch] = useState('');
  const [asientoExpandido, setAsientoExpandido] = useState(null);

  useEffect(() => {
    const fetchAsientos = async () => {
      try {
        const token = localStorage.getItem('token');
        const { data } = await axios.get('/api/Asientos', {
          headers: { Authorization: `Bearer ${token}` }
        });

        const formateados = data.map(a => ({
          id: a.idAsiento,
          fecha: a.fecha,
          proveedor: a.proveedor,
          idFactura: a.id_Factura,
          notaId: a.idNota,
          montoTotal: a.montoTotal,
          descripcion: a.idNota
            ? `Nota de credito #${a.idNota} a ${a.proveedor}`
            : `Factura #${a.idFactura} a ${a.proveedor}`,
          detalles: a.detalles.map(d => ({
            cuenta: d.cuentaContable,
            debe: d.debe,
            haber: d.haber
          }))
        }));

        setAsientos(formateados);
        setAsientosFiltrados(formateados);
      } catch (error) {
        console.error('Error al obtener asientos:', error);
      }
    };

    fetchAsientos();
  }, []);

  useEffect(() => {
    const resultado = asientos.filter(a =>
      a.descripcion.toLowerCase().includes(search.toLowerCase())
    );
    setAsientosFiltrados(resultado);
  }, [search, asientos]);

  const columns = [
    { name: 'Fecha', selector: row => row.fecha, sortable: true },
    { name: 'Descripcion', selector: row => row.descripcion, sortable: true },
    { name: 'Total', selector: row => row.montoTotal, sortable: true },
    {
      name: 'Acciones',
      cell: row => (
        <button
          onClick={() =>
            setAsientoExpandido(prev => (prev === row.id ? null : row.id))
          }
          className="text-blue-600 text-xl hover:text-blue-800"
        >
          <FaEye />
        </button>
      ),
      ignoreRowClick: true,
      allowOverflow: true,
      button: true
    }
  ];

  const ExpandibleDetalle = ({ data }) => (
    <div className="p-4 bg-gray-100 rounded-md border mt-2">
      <h4 className="font-semibold mb-2">Detalle del asiento</h4>
      <table className="w-full text-sm border">
        <thead className="bg-gray-200">
          <tr>
            <th className="p-2 border">Cuenta contable</th>
            <th className="p-2 border text-right">Debe</th>
            <th className="p-2 border text-right">Haber</th>
          </tr>
        </thead>
        <tbody>
          {data.detalles.map((detalle, idx) => (
            <tr key={idx}>
              <td className="p-2 border">{detalle.cuenta}</td>
              <td className="p-2 border text-right">
                {detalle.debe?.toFixed(2) ?? '0.00'}
              </td>
              <td className="p-2 border text-right">
                {detalle.haber?.toFixed(2) ?? '0.00'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );

  return (
    <div className="p-6 bg-gray-100 min-h-screen">
      <h2 className="text-2xl font-semibold mb-4">Asientos contables</h2>

      <div className="flex flex-wrap gap-4 mb-4">
        <input
          type="text"
          placeholder="Buscar por descripcion"
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="px-4 py-2 border rounded shadow-sm"
        />
      </div>

      <DataTable
        columns={columns}
        data={asientosFiltrados}
        pagination
        highlightOnHover
        striped
        responsive
        noDataComponent="No se encontraron asientos"
        expandableRows
        expandableRowExpanded={row => row.id === asientoExpandido}
        expandableRowsComponent={({ data }) => (
          <ExpandibleDetalle data={data} />
        )}
      />
    </div>
  );
};
