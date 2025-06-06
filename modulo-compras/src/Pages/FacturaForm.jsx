import { useState, useEffect } from 'react';
import axios from 'axios';

export default function FacturaForm() {
  const [pedidos, setPedidos] = useState([]);
  const [productos, setProductos] = useState([]);
  const [form, setForm] = useState({
  ordenId: '',
  proveedorId: '',
  nombreProveedor: '',
  fechaEmision: '',
  items: []
});

  useEffect(() => {
    axios.get('http://localhost:5000/api/pedidos')
      .then(res => setPedidos(res.data))
      .catch(console.error);
  }, []);

  useEffect(() => {
  if (form.ordenId) {
    const pedido = pedidos.find(p => p.idPedido === parseInt(form.ordenId));
    console.log("Pedido seleccionado:", pedido);
    if (pedido) {
      setProductos(pedido.detalles || []);
      setForm(prev => ({
        ...prev,
        proveedorId: pedido.idProveedor,
        nombreProveedor: pedido.nombreProveedor || pedido.proveedor?.nombre || '',
        items: (pedido.detalles || []).map(p => ({
          ProductoId: p.idProducto,
          cantidad: 1,
          precioUnitario: p.precioUnitario ?? 0
        }))
      }));
    }
  }
}, [form.ordenId, pedidos]);


  const handleChangeItem = (index, field, value) => {
    const updatedItems = [...form.items];
    updatedItems[index][field] = field === 'precioUnitario' || field === 'cantidad'
      ? parseFloat(value)
      : value;
    setForm({ ...form, items: updatedItems });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validación mínima antes de enviar
    if (!form.proveedorId || !form.fechaEmision || form.items.length === 0) {
      alert("Por favor, complete todos los campos obligatorios.");
      return;
    }

    console.log("Factura enviada:", JSON.stringify(form, null, 2));
    try {
      await axios.post('/api/facturas', form);
      alert('Factura registrada exitosamente');
    } catch (err) {
      alert('Error al registrar factura');
      console.error(err.response?.data || err.message);
    }
  };

  return (
  <form onSubmit={handleSubmit} className="p-4 max-w-3xl mx-auto bg-white rounded shadow">
    <h2 className="text-xl font-bold mb-4">Registrar Factura</h2>

    <label className="block mb-2">Pedido de Compra</label>
    <select
      value={form.ordenId}
      onChange={e => setForm({ ...form, ordenId: e.target.value })}
      className="w-full border p-2 mb-4"
      required
    >
      <option value="">Seleccione un pedido</option>
      {pedidos.map((p) => (
        <option key={p.idPedido} value={p.idPedido}>Pedido #{p.idPedido}</option>
      ))}
    </select>

    {/* 👇 Sección de proveedor visible */}
    {form.nombreProveedor && (
      <>
        <label className="block mb-2">Proveedor</label>
        <input
          type="text"
          className="w-full border p-2 mb-4 bg-gray-100"
          value={form.nombreProveedor}
          disabled
        />
      </>
    )}

    {productos.length > 0 && (
      <>
        <label className="block mb-2">Fecha de Emisión</label>
        <input
          type="date"
          value={form.fechaEmision}
          onChange={e => setForm({ ...form, fechaEmision: e.target.value })}
          className="w-full border p-2 mb-4"
          required
        />

        <h3 className="font-semibold mb-2">Productos</h3>
        {productos.map((prod, index) => (
          <div key={prod.idProducto} className="grid grid-cols-3 gap-4 mb-2">
            <div>
              <p>{prod.nombreProducto}</p>
              <p className="text-sm text-gray-500">Precio sugerido: Gs. {prod.precioUnitario?.toLocaleString()}</p>
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
        {form.items.length > 0 && (
      <div className="mt-4 text-right font-semibold">
        Total estimado: Gs.{" "}
        {form.items
          .reduce((sum, item) => sum + item.cantidad * item.precioUnitario, 0)
          .toLocaleString()}
      </div>
    )}


    <button type="submit" className="mt-4 bg-blue-600 text-white px-4 py-2 rounded">Registrar Factura</button>
  </form>
);

}
