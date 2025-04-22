import { useEffect, useState } from "react";
import axios from "axios";

export default function RecepcionProductos() {
  const [ordenes, setOrdenes] = useState([]);
  const [ordenSeleccionada, setOrdenSeleccionada] = useState(null);
  const [productosRecibidos, setProductosRecibidos] = useState([]);
  const [facturaInfo, setFacturaInfo] = useState({ numero: "", timbrado: "" });

  useEffect(() => {
    axios.get("/api/recepciones/ordenes-pendientes")
      .then(res => setOrdenes(res.data));
  }, []);

  const seleccionarOrden = (orden) => {
    setOrdenSeleccionada(orden);
    const productos = orden.detalles.map(p => ({
      productoId: p.producto.id,
      nombre: p.producto.nombre,
      cantidadSolicitada: p.cantidadSolicitada,
      cantidadRecibida: 0,
      motivoDevolucion: ""
    }));
    setProductosRecibidos(productos);
  };

  const handleCantidad = (index, value) => {
    const updated = [...productosRecibidos];
    updated[index].cantidadRecibida = Number(value);
    setProductosRecibidos(updated);
  };

  const handleDevolucion = (index, value) => {
    const updated = [...productosRecibidos];
    updated[index].motivoDevolucion = value;
    setProductosRecibidos(updated);
  };

  const registrarRecepcion = () => {
    axios.post("/api/recepciones/registrar", {
      ordenId: ordenSeleccionada.id,
      numeroFactura: facturaInfo.numero,
      timbrado: facturaInfo.timbrado,
      productos: productosRecibidos.map(p => ({
        productoId: p.productoId,
        cantidadRecibida: p.cantidadRecibida,
        motivoDevolucion: p.motivoDevolucion
      }))
    }).then(() => {
      alert("Recepción registrada con éxito");
      setOrdenSeleccionada(null);
      setProductosRecibidos([]);
    });
  };

  return (
    <div className="p-4">
      <h2 className="text-xl font-bold mb-4">Recepción de Productos</h2>
      
      {!ordenSeleccionada ? (
        <div>
          <h3 className="mb-2">Órdenes Pendientes</h3>
          <ul className="bg-white shadow rounded p-2">
            {ordenes.map((o) => (
              <li key={o.id} className="border-b p-2 cursor-pointer hover:bg-gray-100"
                onClick={() => seleccionarOrden(o)}>
                #{o.id} - {o.proveedor.nombre} - {new Date(o.fecha).toLocaleDateString()}
              </li>
            ))}
          </ul>
        </div>
      ) : (
        <div>
          <h3 className="mb-2">Productos de la Orden #{ordenSeleccionada.id}</h3>
          <table className="w-full text-left border">
            <thead>
              <tr>
                <th>Producto</th>
                <th>Solicitado</th>
                <th>Recibido</th>
                <th>Motivo devolución</th>
              </tr>
            </thead>
            <tbody>
              {productosRecibidos.map((p, i) => (
                <tr key={p.productoId}>
                  <td>{p.nombre}</td>
                  <td>{p.cantidadSolicitada}</td>
                  <td>
                    <input type="number" min="0"
                      value={p.cantidadRecibida}
                      onChange={(e) => handleCantidad(i, e.target.value)}
                      className="border rounded px-2 w-20"
                    />
                  </td>
                  <td>
                    <input type="text"
                      value={p.motivoDevolucion}
                      onChange={(e) => handleDevolucion(i, e.target.value)}
                      className="border rounded px-2"
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="mt-4">
            <input type="text" placeholder="Número de factura"
              className="border px-2 mr-2"
              value={facturaInfo.numero}
              onChange={(e) => setFacturaInfo({ ...facturaInfo, numero: e.target.value })} />
            <input type="text" placeholder="Timbrado"
              className="border px-2"
              value={facturaInfo.timbrado}
              onChange={(e) => setFacturaInfo({ ...facturaInfo, timbrado: e.target.value })} />
          </div>

          <button className="mt-4 px-4 py-2 bg-blue-600 text-white rounded"
            onClick={registrarRecepcion}>
            Confirmar Recepción
          </button>
        </div>
      )}
    </div>
  );
}
