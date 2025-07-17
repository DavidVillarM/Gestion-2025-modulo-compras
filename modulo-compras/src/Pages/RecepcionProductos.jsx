import React, { useEffect, useState } from 'react';
import axios from 'axios';
import jsPDF from 'jspdf';
import Modal from 'react-modal';
import { useNavigate } from 'react-router-dom';

const RecepcionProductos = () => {
  const navigate = useNavigate();
  const [recepciones, setRecepciones] = useState([]);
  const [detalleRecepcion, setDetalleRecepcion] = useState(null);
  const [filtros, setFiltros] = useState({ proveedor: '', estado: '', fechaDesde: '', fechaHasta: '' });
  const [modalAbierto, setModalAbierto] = useState(false);
  const [modalRechazoAbierto, setModalRechazoAbierto] = useState(false);
  const [productosRechazo, setProductosRechazo] = useState([]);
  const [motivoGeneral, setMotivoGeneral] = useState('');


  const irANuevaRecepcion = () => {
    navigate('/recepciones/nueva');
  };

  const cargarRecepciones = async () => {
    try {
      const params = new URLSearchParams(filtros);
      const res = await axios.get(`http://localhost:5000/api/recepciones/buscar?${params.toString()}`);
      setRecepciones(res.data);
    } catch (err) {
      console.error('Error al cargar recepciones', err);
    }
  };

  const abrirModal = async (id) => {
    try {
      const res = await axios.get(`http://localhost:5000/api/recepciones/${id}`);
      const data = res.data;
      data.productos = data.productos.map(p => ({ ...p, cantidadFinal: p.cantidadRecibida }));
      setDetalleRecepcion(data);
      setModalAbierto(true);
    } catch (err) {
      console.error('Error al cargar detalle', err);
    }
  };

  const confirmarRecepcion = async () => {
    const confirmar = confirm("¿Estás seguro de confirmar esta recepción?");
    if (!confirmar) return;
    try {
      await axios.put(`http://localhost:5000/api/recepciones/${detalleRecepcion.id}/confirmar`, {
        productos: detalleRecepcion.productos.map(p => ({ productoId: p.idProducto, cantidadFinal: p.cantidadFinal }))
      });
      alert('Recepcion confirmada');
      cerrarModal();
      cargarRecepciones();
    } catch (err) {
      console.error('Error al confirmar', err);
    }
  };

  const abrirModalRechazo = () => {
  const productosIniciales = detalleRecepcion.productos.map(p => ({
    productoId: p.idProducto,
    nombre: p.nombre,
    cantidad: 0,
    motivo: ''
  }));
  setProductosRechazo(productosIniciales);
  setModalRechazoAbierto(true);
};


  const rechazarRecepcion = async () => {
    const motivo = prompt("Motivo de rechazo:");
    if (!motivo) return;
    try {
      await axios.post(`http://localhost:5000/api/recepciones/${detalleRecepcion.id}/rechazar`, { motivo });
      alert('Recepcion rechazada');
      cerrarModal();
      cargarRecepciones();
    } catch (err) {
      console.error('Error al rechazar', err);
    }
  };

  const confirmarRechazo = async () => {
    const confirmar = confirm("¿Estás seguro de rechazar esta recepción?");
    if (!confirmar) return;
    if (detalleRecepcion?.productos?.length === 0) {
      console.log("Rechazo sin productos, motivo:", motivoGeneral); // DEBUG

      if (!motivoGeneral.trim()) return alert("Debes ingresar un motivo de rechazo");

      try {
        await axios.post(`http://localhost:5000/api/recepciones/${detalleRecepcion.id}/rechazar`, {
          motivo: motivoGeneral
        });
        alert("Recepción rechazada correctamente");
        setModalRechazoAbierto(false);
        cerrarModal();
        cargarRecepciones();
      } catch (err) {
        console.error("Error al rechazar recepción vacía", err);
        alert("Error al rechazar recepción vacía");
      }

    } else {
      const productos = productosRechazo.filter(p => p.cantidad > 0 && p.motivo.trim() !== '');
      if (productos.length === 0) {
        alert("Debes ingresar al menos un producto con cantidad y motivo");
        return;
      }

      try {
        await axios.post(`http://localhost:5000/api/recepciones/${detalleRecepcion.id}/rechazar`, { productos });
        alert("Recepción rechazada correctamente");
        setModalRechazoAbierto(false);
        cerrarModal();
        cargarRecepciones();
      } catch (err) {
        console.error("Error al rechazar recepción", err);
        alert("Error al rechazar recepción");
      }
    }
  };

  
  const exportarPDF = () => {
    const doc = new jsPDF();
    doc.setFontSize(12);
    doc.text(`Recepcion #${detalleRecepcion.id}`, 10, 10);
    doc.text(`Proveedor: ${detalleRecepcion.proveedor.nombre} (${detalleRecepcion.proveedor.ruc})`, 10, 20);
    doc.text(`Factura: ${detalleRecepcion.numeroFactura}`, 10, 30);
    doc.text(`Timbrado: ${detalleRecepcion.timbrado}`, 10, 40);
    doc.text(`Fecha: ${detalleRecepcion.fecha}`, 10, 50);
    doc.text(`Estado: ${detalleRecepcion.estado}`, 10, 60);
    doc.text(`Productos:`, 10, 70);
    let y = 80;
    detalleRecepcion.productos.forEach(p => {
      doc.text(`- ${p.nombre}: ${p.cantidadFinal}`, 12, y);
      y += 8;
    });
    doc.save(`recepcion-${detalleRecepcion.id}.pdf`);
  };

  const cerrarModal = () => {
    setModalAbierto(false);
    setDetalleRecepcion(null);
  };

  useEffect(() => { cargarRecepciones(); }, []);

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Recepciones</h1>
        <button className="bg-green-600 text-white px-4 py-2 rounded" onClick={irANuevaRecepcion}>+ Nueva Recepción</button>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-2 mb-4">
        <input type="text" placeholder="Proveedor o RUC" className="border px-2 py-1 rounded" value={filtros.proveedor} onChange={(e) => setFiltros({ ...filtros, proveedor: e.target.value })} />
        <select className="border px-2 py-1 rounded" value={filtros.estado} onChange={(e) => setFiltros({ ...filtros, estado: e.target.value })}>
          <option value="">Todos los estados</option>
          <option value="PENDIENTE">Pendiente</option>
          <option value="COMPLETA">Completa</option>
          <option value="INCOMPLETA">Incompleta</option>
          <option value="RECHAZADA">Rechazada</option>
        </select>
        <input type="date" className="border px-2 py-1 rounded" value={filtros.fechaDesde} onChange={(e) => setFiltros({ ...filtros, fechaDesde: e.target.value })} />
        <input type="date" className="border px-2 py-1 rounded" value={filtros.fechaHasta} onChange={(e) => setFiltros({ ...filtros, fechaHasta: e.target.value })} />
      </div>
      <button className="mb-4 bg-blue-600 text-white px-4 py-2 rounded" onClick={cargarRecepciones}>Buscar</button>

      <table className="w-full text-sm border rounded shadow mt-4">
        <thead className="bg-gray-50 text-gray-700">
          <tr>
            <th className="px-4 py-2 text-left">Proveedor</th>
            <th className="px-4 py-2 text-left">Factura</th>
            <th className="px-4 py-2 text-left">Fecha</th>
            <th className="px-4 py-2 text-left">Estado</th>
            <th className="px-4 py-2 text-left">Acción</th>
          </tr>
        </thead>
        <tbody>
          {recepciones.map((r) => (
            <tr key={r.id} className="border-t hover:bg-gray-50">
              <td className="px-4 py-2 font-medium">{r.proveedor?.nombre || '-'}</td>
              <td className="px-4 py-2">{r.numeroFactura}</td>
              <td className="px-4 py-2">{r.fecha}</td>
              <td className="px-4 py-2">
                <span className={`text-xs font-semibold px-2 py-1 rounded-full 
                  ${r.estado === 'COMPLETA' ? 'bg-green-100 text-green-700' : ''}
                  ${r.estado === 'PENDIENTE' ? 'bg-yellow-100 text-yellow-700' : ''}
                  ${r.estado === 'INCOMPLETA' ? 'bg-orange-100 text-orange-700' : ''}
                  ${r.estado === 'RECHAZADA' ? 'bg-red-100 text-red-700' : ''}`}>
                  {r.estado}
                </span>
              </td>
              <td className="px-4 py-2">
                <button
                  className="text-sm text-blue-600 hover:underline font-medium"
                  onClick={() => abrirModal(r.id)}
                >
                  Ver Detalles
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>


      {/* Modal de Detalles */}
      <Modal isOpen={modalAbierto} onRequestClose={cerrarModal} className="p-6 bg-white rounded shadow-lg max-w-2xl mx-auto mt-12">
        {detalleRecepcion && (
          <div>
            <h2 className="text-xl font-bold mb-4">Recepción #{detalleRecepcion.id}</h2>
            <p><strong>Proveedor:</strong> {detalleRecepcion.proveedor.nombre} ({detalleRecepcion.proveedor.ruc})</p>
            <p><strong>Factura:</strong> {detalleRecepcion.numeroFactura}</p>
            <p><strong>Timbrado:</strong> {detalleRecepcion.timbrado}</p>
            <p><strong>Fecha:</strong> {detalleRecepcion.fecha}</p>
            <p><strong>Estado:</strong> {detalleRecepcion.estado}</p>

            <table className="w-full mt-4 border text-sm">
              <thead className="bg-gray-100">
                <tr>
                  <th className="border px-2 py-1">Producto</th>
                  <th className="border px-2 py-1">Cantidad Recibida</th>
                  <th className="border px-2 py-1">Cantidad Final</th>
                </tr>
              </thead>
              <tbody>
                {detalleRecepcion.productos.map((p, idx) => (
                  <tr key={p.idProducto}>
                    <td className="border px-2 py-1">{p.nombre}</td>
                    <td className="border px-2 py-1 text-center">{p.cantidadRecibida}</td>
                    <td className="border px-2 py-1 text-center">
                      <input
                        type="number"
                        value={p.cantidadFinal}
                        min={0}
                        max={p.cantidadRecibida}
                        className="w-16 border px-1 rounded text-center"
                        disabled={['COMPLETA', 'RECHAZADA'].includes(detalleRecepcion.estado)}
                        onChange={(e) => {
                          if (['COMPLETA', 'RECHAZADA'].includes(detalleRecepcion.estado)) return;
                          const valor = parseInt(e.target.value) || 0;
                          if (valor > p.cantidadRecibida) return;
                          const nuevos = [...detalleRecepcion.productos];
                          nuevos[idx].cantidadFinal = valor;
                          setDetalleRecepcion({ ...detalleRecepcion, productos: nuevos });
                        }}
                      />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="mt-4 flex gap-4">
              {!['COMPLETA', 'RECHAZADA'].includes(detalleRecepcion.estado) && (
                <>
                  <button className="bg-green-600 text-white px-4 py-2 rounded" onClick={confirmarRecepcion}>Aceptar</button>
                  <button className="bg-red-600 text-white px-4 py-2 rounded" onClick={abrirModalRechazo}>Rechazar Productos</button>
                </>
              )}
              <button className="bg-gray-600 text-white px-4 py-2 rounded" onClick={exportarPDF}>Exportar PDF</button>
              <button className="ml-auto text-blue-600 underline" onClick={cerrarModal}>Cerrar</button>
            </div>
          </div>
        )}
      </Modal>

      {/* Modal de Rechazo */}
      <Modal isOpen={modalRechazoAbierto} onRequestClose={() => setModalRechazoAbierto(false)} className="p-6 bg-white rounded shadow-lg max-w-2xl mx-auto mt-12">
        <h2 className="text-xl font-bold mb-4">Rechazo de Recepción</h2>

        {detalleRecepcion?.productos?.length > 0 ? (
          <table className="w-full border text-sm">
            <thead className="bg-gray-100">
              <tr>
                <th className="border px-2 py-1">Producto</th>
                <th className="border px-2 py-1">Cantidad Rechazada</th>
                <th className="border px-2 py-1">Motivo</th>
              </tr>
            </thead>
            <tbody>
              {productosRechazo.map((p, idx) => (
                <tr key={p.productoId}>
                  <td className="border px-2 py-1">{p.nombre}</td>
                  <td className="border px-2 py-1 text-center">
                    <input type="number" min={0} value={p.cantidad} onChange={(e) => {
                      const copia = [...productosRechazo];
                      copia[idx].cantidad = parseInt(e.target.value) || 0;
                      setProductosRechazo(copia);
                    }} className="w-20 text-center border rounded" />
                  </td>
                  <td className="border px-2 py-1 text-center">
                    <input type="text" value={p.motivo} onChange={(e) => {
                      const copia = [...productosRechazo];
                      copia[idx].motivo = e.target.value;
                      setProductosRechazo(copia);
                    }} className="w-full border rounded px-2 py-1" />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <div className="mt-4">
            <label className="block mb-2 font-medium">Motivo de rechazo</label>
            <textarea
              className="w-full border rounded px-2 py-1"
              rows={3}
              value={motivoGeneral}
              onChange={(e) => setMotivoGeneral(e.target.value)}
            />
          </div>
        )}

        <div className="flex justify-end mt-4 gap-3">
          <button className="bg-red-600 text-white px-4 py-2 rounded" onClick={confirmarRechazo}>Confirmar Rechazo</button>
          <button className="text-blue-600 underline" onClick={() => setModalRechazoAbierto(false)}>Cancelar</button>
        </div>
      </Modal>
    </div>
  );

};

export default RecepcionProductos;
