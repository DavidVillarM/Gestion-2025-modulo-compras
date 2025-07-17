// File: src/pages/OrdenesVista.jsx
import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";

export const OrdenesVista = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  // 1) Info general de la orden (fechaPedido, estado, montoTotal global)
  const [infoOrden, setInfoOrden] = useState(null);

  // 2) Lista de Pedidos con sus detalles para esta orden
  const [pedidosConDetalles, setPedidosConDetalles] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // 1) Traer datos de la orden (fechaPedido, estado, montoTotal)
  useEffect(() => {
    if (!id) {
      setError("ID de orden invalido");
      setLoading(false);
      return;
    }

    setLoading(true);
    // Asumimos que GET /api/OrdenesPago/{id} devuelve un objeto con { idOrden, fechaPedido, estado, montoTotal }
    axios
      .get(`http://localhost:5000/api/OrdenesPago/${id}`)
      .then((resp) => {
        setInfoOrden(resp.data);
        setError("");
      })
      .catch((err) => {
        console.error(err);
        setError("No se pudo cargar la orden");
      })
      .finally(() => setLoading(false));
  }, [id]);

  // 2) Traer Pedidos + Detalles
  useEffect(() => {
    if (!id) return;

    axios
      .get(`http://localhost:5000/api/OrdenesPago/${id}/pedidos-con-detalles`)
      .then((resp) => {
        setPedidosConDetalles(resp.data);
        setError("");
      })
      .catch((err) => {
        console.error(err);
        setError("No se pudieron cargar los pedidos");
        setPedidosConDetalles([]);
      });
  }, [id]);

  if (loading) return <p>Cargando...</p>;
  if (error) return <p className="text-red-600">{error}</p>;
  if (!infoOrden) return <p>Orden no encontrada</p>;

  return (
    <div className="p-6 bg-white min-h-screen space-y-8">
      {/* CABECERA CON INFO DE LA ORDEN */}
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Orden Nro# {infoOrden.idOrden}</h2>
        <button
          onClick={() => navigate(-1)}
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
        >
          Volver
        </button>
      </div>

      <div className="mb-4 space-y-1">
        <p>
          <strong>Fecha de Creacion de la orden:</strong>{" "}
          {infoOrden.fechaPedido || "—"}
        </p>
        <p>
          <strong>Estado:</strong> {infoOrden.estado}
        </p>
        <p>
          <strong>Monto Total de la orden:</strong>{" "}
          ${infoOrden.montoTotal?.toFixed(2) ?? "0.00"}
        </p>
      </div>

      {/* PARA CADA PEDIDO: SUBTITULO + TABLA DE DETALLES */}
      {pedidosConDetalles.map((pedido) => (
        <div key={pedido.idPedido} className="mb-8">
          {/* SUBTITULO DEL PEDIDO */}
          <div className="border-b pb-2 mb-4">
            <h3 className="text-xl font-semibold">Pedido Nro# {pedido.idPedido}</h3>
            <p>
              <strong>Proveedor:</strong> {pedido.proveedor}
            </p>
            <p>
              <strong>Fecha Pedido:</strong> {pedido.fechaPedido}
            </p>
            <p>
              <strong>Fecha Entrega:</strong>{" "}
              {pedido.fechaEntrega ?? "—"}
            </p>
          </div>

          {/* TABLA DE DETALLES PARA ESTE PEDIDO */}
          <table className="min-w-full border mb-4">
            <thead>
              <tr className="bg-gray-100">
                <th className="border px-4 py-2">ID Detalle</th>
                <th className="border px-4 py-2">Producto</th>
                <th className="border px-4 py-2">Cotizacion</th>
                <th className="border px-4 py-2">Cantidad</th>
                <th className="border px-4 py-2">IVA</th>
                <th className="border px-4 py-2">Subtotal</th>
              </tr>
            </thead>
            <tbody>
              {pedido.detalles.map((d) => (
                <tr key={d.idPedidoDetalle}>
                  <td className="border px-4 py-2">{d.idPedidoDetalle}</td>
                  <td className="border px-4 py-2">{d.producto}</td>
                  <td className="border px-4 py-2">
                    ${d.cotizacion.toFixed(2)}
                  </td>
                  <td className="border px-4 py-2">{d.cantidad}</td>
                  <td className="border px-4 py-2">
                    ${d.iva.toFixed(2)}
                  </td>
                  <td className="border px-4 py-2">
                    ${d.subtotal.toFixed(2)}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {/* TOTAL DE ESTE PEDIDO */}
          <div className="text-right pr-4">
            <p>
              <strong>Total Pedido:</strong> $
              {pedido.montoTotal.toFixed(2)}
            </p>
          </div>
        </div>
      ))}

      {/* RESUMEN FINAL: MONTO TOTAL DE LA ORDEN */}
      <div className="mt-6 border-t pt-4">
        <p className="text-lg font-bold">
          <strong>Monto Total (sumatoria de todos los pedidos):</strong>{" "}
          ${infoOrden.montoTotal?.toFixed(2) ?? "0.00"}
        </p>
      </div>
    </div>
  );
};
