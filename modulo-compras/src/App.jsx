import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import {SideBar} from './Components/SideBar.jsx'
import {Products} from './Pages/Products.jsx'
import { Categorias } from './Pages/Categorias.jsx'
import RecepcionProductos from './Pages/RecepcionProductos.jsx'
import Dashboard from './Pages/Dashboard.jsx';
import { OrdenesPago } from './Pages/OrdenesPago.jsx'
import { OrdenesPresupuesto } from './Pages/OrdenesPresupuesto.jsx'
import { OrdenesPresupuestoFinal } from './Pages/OrdenesPresupuestoFinal.jsx'
import { OrdenesVista } from './Pages/OrdenesVista.jsx'
import FacturaForm from './Pages/FacturaForm';
import FacturaEdit from './Pages/FacturaEdit';
import { useLocation } from "react-router-dom";
import FacturaList from './Pages/FacturaList';
import {Login} from './Pages/login.jsx';
import { ListaProveedores } from './Pages/ListaProveedores.jsx';
import Stock from "./pages/Stock.jsx";
import ListaProductos from "./pages/ListaProductos.jsx";
import ListaOrdenes from "./pages/ListaOrdenes.jsx";
import FormOrden from "./pages/FormOrden.jsx";
import ProveedoresParaOrden from "./pages/ProveedoresParaOrden.jsx";
import ListaPresupuestosPorOrden from "./pages/ListaPresupuestosPorOrden.jsx";
import FormPresupuesto from "./pages/FormPresupuestos.jsx";
import FormProductos from "./pages/FormProductos.jsx";
import ListaPedidosFinales from "./pages/ListaPedidosFinales.jsx";
import ListaPresupuestos from "./pages/ListaPresupuestos";
import { FormProveedores } from './Pages/FormProveedores.jsx';
import { DetallesProveedores } from './Pages/DetallesProveedores.jsx';
import CargarNota from './Pages/CargarNota.jsx'
import ListaNotas from './Pages/ListaNotas.jsx'


function App() {
  const location = useLocation();
  const isLoginPage = location.pathname === '/login';

  return (
    <div className="flex h-screen overflow-hidden">
      {!isLoginPage && <SideBar />}
      <div className="flex-1 overflow-y-auto bg-gray-100">
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/categorias" element={<Categorias />} />
          <Route path="/recepcion" element={<RecepcionProductos />} />
          <Route path="/ordenes-pago" element={<OrdenesPago />} />
          <Route path="/ordenes-presupuesto/" element={<OrdenesPresupuesto />} />
          <Route path="/ordenes-vista/:id" element={<OrdenesVista />} />
          <Route path="/ordenes-presupuesto-final" element={<OrdenesPresupuestoFinal />} />
          <Route path="/login" element={<Login />} />
          <Route path="/proveedores" element={<ListaProveedores />} />
         
                    {/* STOCK */}
                    <Route path="/stock" element={<Stock />} />

                    {/* PRODUCTOS */}
                    <Route path="/productos" element={<ListaProductos />} />
                    <Route path="/productos/nuevo" element={<FormProductos />} />
                    <Route path="/productos/:id/editar" element={<FormProductos />} />
                    <Route path="/productos/:id/detalles" element={<FormProductos />} />

                    {/* ÓRDENES (solicitudes) */}
                    <Route path="/ordenes" element={<ListaOrdenes />} />
                    <Route path="/ordenes/nueva" element={<FormOrden />} />
                    <Route
                        path="/ordenes/:ordenId/proveedores"
                        element={<ProveedoresParaOrden />}
                    />
                    <Route
                        path="/ordenes/:ordenId/presupuestos"
                        element={<ListaPresupuestosPorOrden />}
                    />

                    {/* FACTURAS */}
                      <Route path="/facturas" element={<FacturaList />} />
                      <Route path="/facturas/nueva" element={<FacturaForm />} />
                      <Route path="/facturas/editar/:id" element={<FacturaEdit />} />


                    {/* Formular cotización (edición) */}
                    <Route
                        path="/ordenes/:ordenId/presupuestos/:idPresupuesto"
                        element={<FormPresupuesto />}
                    />

                    {/* PRESUPUESTOS GLOBALES */}
                    <Route path="/presupuestos" element={<ListaPresupuestos />} />

                    {/* PROVEEDORES */}
                    <Route path="/proveedores" element={<ListaProveedores />} />
                    <Route path="/proveedores/nuevo" element={<FormProveedores />} />
                    <Route
                        path="/proveedores/:id/detalles"
                        element={<DetallesProveedores />}
                    />
                    <Route path="/proveedores/:id/editar" element={<FormProveedores />} />

                    {/* PEDIDOS FINALES */}
                    <Route path="/pedidos" element={<ListaPedidosFinales />} />

                    {/* FACTURAS/CATEGORÍAS */}
                    {/*<Route path="/facturas" element={<Facturas />} />*/}
                    <Route path="/categorias" element={<Categorias />} />

                    <Route path="/notas-credito" element={<ListaNotas/>} />
                    <Route path="/notas-credito/cargar" element={<CargarNota/>} />


                    
        </Routes>
      </div>
    </div>
  );
}

export default App;