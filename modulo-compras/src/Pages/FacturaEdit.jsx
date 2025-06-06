import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

export default function FacturaEdit() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [form, setForm] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    axios.get(`/api/facturas/${id}`)
      .then(res => setForm(res.data))
      .catch(() => setError('Error al cargar la factura'))
      .finally(() => setLoading(false));
  }, [id]);

  const handleChange = (field, value) => {
        setForm(prev => ({ ...prev, [field]: value }));
    };

    const handleItemChange = (index, field, value) => {
    const items = [...form.facturaDetalles];
    const item = items[index];
    item[field] = isNaN(value) ? 0 : value;

    const subtotalItem = item.precio * item.cantidad;

    // Si el usuario no ha puesto ambos IVA en 0, calcular automáticamente
    const userDisabledIVA = item.iva5 === 0 && item.iva10 === 0;

    if (!userDisabledIVA) {
        // Recalcular según el que esté en uso (no sobreescribas si está en 0 explícitamente)
        if (item.iva5 > 0) {
        item.iva5 = subtotalItem * 0.05;
        item.iva10 = 0;
        } else if (item.iva10 > 0) {
        item.iva10 = subtotalItem * 0.10;
        item.iva5 = 0;
        }
    }

    recalcularTotales(items);
    };



  const handleSubmit = async (e) => {
    e.preventDefault();

    // Limpiar campos innecesarios antes de enviar
    const formLimpio = {
        ...form,
        facturaDetalles: form.facturaDetalles.map(d => ({
            idProducto: d.idProducto,
            cantidad: d.cantidad,
            precio: d.precio,
            iva5: d.iva5 ?? 0,
            iva10: d.iva10 ?? 0
        }))
    };


    try {
        console.log('Enviando a backend:', JSON.stringify(formLimpio, null, 2));
        await axios.put(`http://localhost:5000/api/facturas/${id}`, formLimpio);
        alert('Factura actualizada');
        navigate('/facturas');
    } catch (err) {
        console.error('Error al actualizar:', err.response?.data || err.message);
        alert('Error al actualizar');
    }

  };

  if (loading) return <p className="p-4">Cargando...</p>;
  if (error) return <p className="p-4 text-red-600">{error}</p>;

  const recalcularTotales = (detalles) => {
  let subtotal = 0;
  let iva5 = 0;
  let iva10 = 0;

  detalles.forEach(item => {
    const totalItem = item.cantidad * item.precio;
    subtotal += totalItem;
    iva5 += item.iva5 ?? 0;
    iva10 += item.iva10 ?? 0;
  });

  setForm(prev => ({
    ...prev,
    facturaDetalles: detalles,
    subtotal,
    iva5,
    iva10,
    montoTotal: subtotal + iva5 + iva10
  }));
};


  return (
    <div>
      <button
        type="button"
        onClick={() => navigate('/facturas')}
        className="mb-4 bg-gray-600 text-white px-4 py-2 rounded"
      >
        ← Volver
      </button>

      <form onSubmit={handleSubmit} className="p-4 max-w-3xl mx-auto bg-white rounded shadow">
        <h2 className="text-xl font-bold mb-4">Editar Factura #{id}</h2>

        <label className="block mb-2">Proveedor</label>
        <input type="text" className="w-full border p-2 mb-4" value={form.nombreProveedor} disabled />

        <label className="block mb-2">Fecha</label>
        <input
          type="date"
          className="w-full border p-2 mb-4"
          value={form.fecha}
          onChange={e => handleChange('fecha', e.target.value)}
        />

        <h3 className="font-semibold mb-2">Productos</h3>
        {form.facturaDetalles?.map((item, index) => (
            <div key={index} className="grid grid-cols-6 gap-2 mb-2 items-center">
                <div>ID: {item.idProducto}</div>

                <input
                type="number"
                className="border p-1"
                value={item.cantidad ?? 0}
                onChange={e => handleItemChange(index, 'cantidad', parseInt(e.target.value))}
                placeholder="Cantidad"
                />

                <input
                type="number"
                className="border p-1"
                value={item.precio ?? 0}
                onChange={e => handleItemChange(index, 'precio', parseFloat(e.target.value))}
                placeholder="Precio"
                />

                <input
                type="number"
                className="border p-1"
                value={item.iva5 ?? 0}
                onChange={e => handleItemChange(index, 'iva5', parseFloat(e.target.value))}
                placeholder="IVA 5%"
                />

                <input
                type="number"
                className="border p-1"
                value={item.iva10 ?? 0}
                onChange={e => handleItemChange(index, 'iva10', parseFloat(e.target.value))}
                placeholder="IVA 10%"
                />

                <div>Total: {item.cantidad * item.precio}</div>
            </div>
        ))}


        <button type="submit" className="mt-4 bg-green-600 text-white px-4 py-2 rounded">Guardar Cambios</button>
      </form>
    </div>
  );
}
