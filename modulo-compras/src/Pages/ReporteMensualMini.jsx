import React, { useEffect, useState } from 'react';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import axios from 'axios';
import dayjs from 'dayjs';
import 'dayjs/locale/es';

dayjs.locale('es');

const ReporteMensualMini = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [mesSeleccionado, setMesSeleccionado] = useState('');
  const [procesando, setProcesando] = useState(false);

  useEffect(() => {
    fetchOrdenes();
  }, []);

  const fetchOrdenes = async () => {
    try {
      const token = localStorage.getItem('token');
      const res = await axios.get('http://localhost:5000/api/OrdenesPago', {
        headers: { Authorization: `Bearer ${token}` },
      });
      setOrdenes(res.data);
    } catch (err) {
      console.error('Error al obtener órdenes', err);
    }
  };

  const generarReporte = async () => {
    setProcesando(true);
    const doc = new jsPDF();
    let y = 20;

    const ordenesFiltradas = ordenes.filter(o => {
      const fecha = dayjs(o.fechaPedido);
      return (
        o.estado.toUpperCase() === 'COMPLETA' &&
        fecha.format('YYYY-MM') === mesSeleccionado
      );
    });

    let totalMensual = 0;

    // Título del reporte
    const mesTexto = dayjs(mesSeleccionado).format('MMMM YYYY');
    doc.setFontSize(18);
    doc.text(`Reporte de Pedidos de ${mesTexto}`, 105, 10, { align: 'center' });

    for (const orden of ordenesFiltradas) {
      const pedidosRes = await axios.get(
        `http://localhost:5000/api/OrdenesPago/${orden.idOrden}/pedidos-con-detalles`
      );
      const pedidos = pedidosRes.data;

      doc.setFontSize(14);
      doc.text(`Orden N° ${orden.idOrden}`, 10, y);
      doc.setFontSize(10);
      doc.text(`Fecha: ${orden.fechaPedido} - Total: $${orden.montoTotal}`, 10, y + 6);

      totalMensual += orden.montoTotal;
      y += 12;

      for (const pedido of pedidos) {
        autoTable(doc, {
          startY: y,
          head: [[
            `Pedido N° ${pedido.idPedido} - Proveedor: ${pedido.proveedor}`,
            `Fecha Pedido: ${pedido.fechaPedido}`,
            `Entrega: ${pedido.fechaEntrega}`,
            `Total: $${pedido.montoTotal}`
          ]],
          body: [],
          theme: 'grid',
        });

        y = doc.lastAutoTable.finalY + 4;

        const detalles = pedido.detalles.map(det => ([
          det.producto,
          det.cantidad,
          `$${det.cotizacion}`,
          `${det.iva}%`,
          `$${det.subtotal}`,
        ]));

        autoTable(doc, {
          startY: y,
          head: [['Producto', 'Cantidad', 'Cotización', 'IVA', 'Subtotal']],
          body: detalles,
          theme: 'striped',
        });

        y = doc.lastAutoTable.finalY + 10;

        if (y > 260) {
          doc.addPage();
          y = 10;
        }
      }
    }

    doc.setFontSize(12);
    doc.text(`Total de órdenes completas en ${mesTexto}: $${totalMensual}`, 10, y);

    doc.save(`Reporte_Ordenes_${mesSeleccionado}.pdf`);
    setProcesando(false);
  };

  const mesesDisponibles = [...new Set(ordenes.map(o => dayjs(o.fechaPedido).format('YYYY-MM')))];

  return (
    <div className="flex gap-2 items-center">
      <select
        value={mesSeleccionado}
        onChange={e => setMesSeleccionado(e.target.value)}
        className="px-3 py-1 border rounded"
      >
        <option value="">Mes</option>
        {mesesDisponibles.map(m => (
          <option key={m} value={m}>
            {dayjs(m).format('MMMM YYYY')}
          </option>
        ))}
      </select>
      <button
        onClick={generarReporte}
        disabled={!mesSeleccionado || procesando}
        className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700 disabled:opacity-50"
      >
        {procesando ? 'Generando...' : '📄 PDF'}
      </button>
    </div>
  );
};

export default ReporteMensualMini;
