// src/utils/generarPDFOrden.js
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import axios from 'axios';
import dayjs from 'dayjs';
import 'dayjs/locale/es';

dayjs.locale('es');

export const generarPDFOrden = async (ordenId) => {
  if (!ordenId) return;

  try {
    const doc = new jsPDF();
    const fechaActual = dayjs().format('DD [de] MMMM [de] YYYY');

    const ordenRes = await axios.get(`http://localhost:5000/api/OrdenesPago/${ordenId}`);
    const infoOrden = ordenRes.data;

    const pedidosRes = await axios.get(
      `http://localhost:5000/api/OrdenesPago/${ordenId}/pedidos-con-detalles`
    );
    const pedidos = pedidosRes.data;

    for (let i = 0; i < pedidos.length; i++) {
      const pedido = pedidos[i];

      if (i > 0) doc.addPage();
      let y = 20;

      // Encabezado de la empresa / título
      doc.setFontSize(18);
      doc.setFont('helvetica', 'bold');
      doc.text('Orden de Compra', 105, y, { align: 'center' });

      y += 7;
      doc.setFontSize(11);
      doc.setFont('helvetica', 'normal');
      doc.text(`Emitida el: ${fechaActual}`, 105, y, { align: 'center' });

      y += 10;
      doc.setLineWidth(0.5);
      doc.line(14, y, 196, y); // línea horizontal

      y += 6;
      doc.setFontSize(12);
      doc.text(`Destinatario: ${pedido.proveedor}`, 14, y);
      doc.text(`Correo: ${pedido.correoProveedor || '—'}`, 120, y);

      y += 8;
      doc.setFontSize(11);
      doc.text(`Fecha de Pedido: ${pedido.fechaPedido}`, 14, y);
      doc.text(`Fecha de Entrega: ${pedido.fechaEntrega || '—'}`, 120, y);

      y += 10;
      doc.setFontSize(14);
      doc.setFont('helvetica', 'bold');
      doc.text(`Pedido N° ${pedido.idPedido} (Orden N° ${infoOrden.idOrden})`, 14, y);

      y += 6;
      doc.setFontSize(10);
      doc.setFont('helvetica', 'normal');
      doc.text('Detalle de Productos:', 14, y);

      y += 4;

      autoTable(doc, {
        startY: y,
        head: [['Producto', 'Cantidad', 'Cotización', 'IVA', 'Subtotal']],
        body: pedido.detalles.map((d) => [
          d.producto,
          d.cantidad,
          `$${d.cotizacion.toFixed(2)}`,
          `$${d.iva.toFixed(2)}`,
          `$${d.subtotal.toFixed(2)}`
        ]),
        theme: 'striped',
        styles: { fontSize: 10 },
        headStyles: { fillColor: [41, 128, 185] }
      });

      y = doc.lastAutoTable.finalY + 10;

      doc.setFontSize(12);
      doc.setFont('helvetica', 'bold');
      doc.text(`Total del Pedido: $${pedido.montoTotal.toFixed(2)}`, 14, y);

      y += 20;
      doc.setFontSize(10);
      doc.setFont('helvetica', 'italic');
      doc.text(
        'Este documento representa una solicitud de compra por parte de nuestra organización. Para cualquier consulta, por favor comuníquese con nosotros.',
        14,
        y,
        { maxWidth: 180 }
      );
    }

    doc.save(`Orden_${ordenId}_Pedidos.pdf`);
  } catch (err) {
    console.error('Error generando PDF:', err);
  }
};
