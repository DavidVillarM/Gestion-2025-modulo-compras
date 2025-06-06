import { useState, useEffect } from 'react';
import axios from 'axios';

export default function FacturaForm() {
  const [pedidos, setPedidos] = useState([]);
  const [productos, setProductos] = useState([]);
  const [form, setForm] = useState({
    ordenId: '',
    proveedorId: '',
    fechaEmision: '',
    items: []
  });

  useEffect(() => {
    axios.get('/api/pedidos')
      .then(res => setPedidos(res.data))
      .catch(console.error);
  }, []);

  useEffect(() => {
    if (form.ordenId) {
      const pedido = pedidos.find(p => p.id === parseInt(form.ordenId));
      if (pedido) {
        setForm(prev => ({ ...prev, proveedorId: pedido.idProveedor }));
        setProductos(pedido.detalles);
        setForm(prev => ({
          ...prev,
          items: pedido.detalles.map(p => ({
            productoId: p.productoId,
            cantidad: 0,
            precioUnitario: 0
          }))
        }));
      }
    }
  }, [form.ordenId]);

  const handleChangeItem = (index, field, value) => {
    const updatedItems = [...form.items];
    updatedItems[index][field] = field === 'precioUnitario' || field === 'cantidad' ? parseFloat(value) : value;
    setForm({ ...form, items: updatedItems });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('/api/facturas', form);
      alert('Factura registrada exitosamente');
    } catch (err) {
      alert('Error al registrar factura');
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="p-4 max-w-3xl mx-auto bg-white rounded shadow">
      <h2 className="text-xl font-bold mb-4">Registrar Factura</h2>

      <label className="block mb-2">Pedido de Compra</label>
      <select value={form.ordenId} onChange={e => setForm({ ...form, ordenId: e.target.value })} className="w-full border p-2 mb-4">
        <option value="">Seleccione un pedido</option>
        {pedidos.map(p => (
          <option key={p.id} value={p.id}>Pedido #{p.id}</option>
        ))}
      </select>

      {productos.length > 0 && (
        <>
          <label className="block mb-2">Fecha de Emisión</label>
          <input type="date" value={form.fechaEmision} onChange={e => setForm({ ...form, fechaEmision: e.target.value })} className="w-full border p-2 mb-4" required />

          <h3 className="font-semibold mb-2">Productos</h3>
          {productos.map((prod, index) => (
            <div key={prod.productoId} className="grid grid-cols-3 gap-4 mb-2">
              <div>
                <p>{prod.nombreProducto}</p>
              </div>
              <input
                type="number"
                min="0"
                placeholder="Cantidad"
                className="border p-1"
                value={form.items[index]?.cantidad || ''}
                onChange={e => handleChangeItem(index, 'cantidad', e.target.value)}
              />
              <input
                type="number"
                min="0"
                step="0.01"
                placeholder="Precio Unitario"
                className="border p-1"
                value={form.items[index]?.precioUnitario || ''}
                onChange={e => handleChangeItem(index, 'precioUnitario', e.target.value)}
              />
            </div>
          ))}
        </>
      )}

      <button type="submit" className="mt-4 bg-blue-600 text-white px-4 py-2 rounded">Registrar Factura</button>
    </form>
  );
}
