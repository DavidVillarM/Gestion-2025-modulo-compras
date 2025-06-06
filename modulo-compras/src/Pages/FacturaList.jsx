import { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import FacturaDetalleModal from '../Components/FacturaDetalleModal';

export default function FacturaList() {
  const [facturas, setFacturas] = useState([]);
  const [filtro, setFiltro] = useState({ proveedor: '', fecha: '', monto: '' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [facturaSeleccionada, setFacturaSeleccionada] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    cargarFacturas();
  }, []);

  const cargarFacturas = async () => {
    setLoading(true);
    try {
      const res = await axios.get('/api/facturas');
      setFacturas(res.data);
      setError('');
    } catch (err) {
      setError('Error al cargar las facturas');
    } finally {
      setLoading(false);
    }
  };

  const eliminarFactura = async (id) => {
    if (!confirm('¿Seguro que deseas eliminar esta factura?')) return;
    try {
      await axios.delete(`/api/facturas/${id}`);
      setFacturas(prev => prev.filter(f => f.idFactura !== id));
    } catch {
      alert('Error al eliminar');
    }
  };

  const facturasFiltradas = facturas.filter(f => {
    return (
      (!filtro.proveedor || f.nombreProveedor?.toLowerCase().includes(filtro.proveedor.toLowerCase())) &&
      (!filtro.fecha || f.fecha === filtro.fecha) &&
      (!filtro.monto || f.montoTotal >= parseFloat(filtro.monto))
    );
  });

  return (
    <div className="p-6">
      <h2 className="text-2xl font-bold mb-4">Facturas</h2>

      <div className="mb-4 flex gap-4">
        <input type="text" placeholder="Proveedor" className="border p-2"
          onChange={e => setFiltro({ ...filtro, proveedor: e.target.value })} />
        <input type="date" className="border p-2"
          onChange={e => setFiltro({ ...filtro, fecha: e.target.value })} />
        <input type="number" className="border p-2" placeholder="Monto mínimo"
          onChange={e => setFiltro({ ...filtro, monto: e.target.value })} />
        <button className="ml-auto bg-blue-600 text-white px-4 py-2 rounded"
          onClick={() => navigate('/facturas/nueva')}>+ Nueva Factura</button>
      </div>

      {loading && <p className="text-blue-600">Cargando...</p>}
      {error && <p className="text-red-600">{error}</p>}

      <table className="w-full border">
        <thead>
          <tr className="bg-gray-100">
            <th className="border p-2">#</th>
            <th className="border p-2">Proveedor</th>
            <th className="border p-2">Fecha</th>
            <th className="border p-2">Total</th>
            <th className="border p-2">Acciones</th>
          </tr>
        </thead>
        <tbody>
          {facturasFiltradas.map((f, i) => (
            <tr key={f.idFactura}>
              <td className="border p-2 text-center">{i + 1}</td>
              <td className="border p-2">{f.nombreProveedor}</td>
              <td className="border p-2 text-center">{f.fecha}</td>
              <td className="border p-2 text-right">Gs. {f.montoTotal?.toLocaleString()}</td>
              <td className="border p-2 text-center">
                <button onClick={() => setFacturaSeleccionada(f)} className="mx-1 text-blue-600">👁️</button>
                <button onClick={() => navigate(`/facturas/editar/${f.idFactura}`)} className="mx-1 text-yellow-600">✏️</button>
                <button onClick={() => eliminarFactura(f.idFactura)} className="mx-1 text-red-600">🗑️</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {facturaSeleccionada && (
        <FacturaDetalleModal factura={facturaSeleccionada} onClose={() => setFacturaSeleccionada(null)} />
      )}
    </div>
  );
}
