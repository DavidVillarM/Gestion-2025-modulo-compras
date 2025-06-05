////import React from 'react';
////import { Routes, Route } from 'react-router-dom';

////import { SideBar } from './Components/SideBar.jsx';

////import { ListaProveedores } from './Pages/ListaProveedores.jsx';
////import { FormProveedores } from './Pages/FormProveedores.jsx';
////import { DetallesProveedores } from './Pages/DetallesProveedores.jsx';
////import { Categorias } from './Pages/Categorias.jsx';

////import  ListaProductos from './Pages/ListaProductos.jsx';
////import  FormProductos from './Pages/FormProductos.jsx';

////import  ListaPedidos from './Pages/ListaPedidos.jsx';
////import  FormPedidos  from './Pages/FormPedidos.jsx';

////import { Facturas } from './Pages/Facturas.jsx';
////import Stock from './Pages/Stock.jsx';

////import ListaPresupuestos from './Pages/ListaPresupuestos.jsx'
////import FormPresupuestos from './Pages/FormPresupuestos.jsx'



////import './App.css';

////export default function App() {
////    return (
////        <div className="flex h-screen overflow-hidden">
////            <SideBar />
////            <div className="flex-1 overflow-auto bg-gray-50">
////                <Routes>
////                    {/* PESTAÑA STOCK */}
////                    <Route path="/stock" element={<Stock />} />
////                    <Route path="/stock" element={<Stock />}>
////                        <Route path="nuevo" element={<FormPedidos />} />
////                        <Route path=":id" element={<FormPedidos />} />
////                    </Route>


////                    {/* PROVEEDORES */}
////                    <Route path="/proveedores" element={<ListaProveedores />} />
////                    <Route path="/proveedores/nuevo" element={<FormProveedores />} />
////                    <Route path="/proveedores/:id/detalles" element={<DetallesProveedores />} />
////                    <Route path="/proveedores/:id/editar" element={<FormProveedores />} />

////                    {/* PRODUCTOS */}
////                    <Route path="/productos" element={<ListaProductos />} />
////                    <Route path="/productos/nuevo" element={<FormProductos />} />
////                    <Route path="/productos/:id/editar" element={<FormProductos />} />
////                    <Route path="/productos/:id/detalles" element={<FormProductos />} />

////                    {/* PEDIDOS */}
////                    <Route path="/pedidos" element={<ListaPedidos />} />
////                    <Route path="/pedidos/nuevo" element={<FormPedidos />} />
////                    <Route path="/pedidos/:id" element={<FormPedidos />} />

////                    {/* FACTURAS Y CATEGORÍAS */}
////                    <Route path="/facturas" element={<Facturas />} />
////                    <Route path="/categorias" element={<Categorias />} />

////                    {/* HOME */}
////                    <Route path="/" element={<Categorias />} />

////                    {/* Crear nueva cotización para una orden */}
////                    <Route
////                        path="/presupuestos/nuevo/:ordenId"
////                        element={<FormPresupuestos />}
////                    />

////                    {/* Cargar cotizaciones existentes de un proveedor para esa orden */}
////                    <Route
////                        path="/presupuestos/cargar/:ordenId/:proveedorId"
////                        element={<FormPresupuestos />}
////                    />

////                    {/* Listar todas las cotizaciones de una orden */}
////                    <Route
////                        path="/cotizaciones/:ordenId"
////                        element={<ListaPresupuestos />}
////                    />




////                </Routes>
////            </div>
////        </div>
////    );
////}

//// src/App.jsx
//import React from 'react';
//import { Routes, Route } from 'react-router-dom';

//import { SideBar } from './Components/SideBar.jsx';

//import { ListaProveedores } from './Pages/ListaProveedores.jsx';
//import { FormProveedores } from './Pages/FormProveedores.jsx';
//import { DetallesProveedores } from './Pages/DetallesProveedores.jsx';
//import { Categorias } from './Pages/Categorias.jsx';

//import ListaProductos from './Pages/ListaProductos.jsx';
//import FormProductos from './Pages/FormProductos.jsx';

//import ListaPedidos from './Pages/ListaPedidos.jsx';
//import FormPedidos from './Pages/FormPedidos.jsx';

//import { Facturas } from './Pages/Facturas.jsx';
//import Stock from './Pages/Stock.jsx';

//import ListaPresupuestos from './Pages/ListaPresupuestos.jsx';
//import FormPresupuestos from './Pages/FormPresupuestos.jsx';

//import './App.css';

//export default function App() {
//    return (
//        <div className="flex h-screen overflow-hidden">
//            <SideBar />
//            <div className="flex-1 overflow-auto bg-gray-50">
//                <Routes>
//                    {/* PESTAÑA STOCK */}
//                    <Route path="/stock" element={<Stock />} />
//                    {/* Como FormPedidos maneja crear (sin id) y editar (con id) */}
//                    <Route path="/pedidos/nuevo" element={<FormPedidos />} />
//                    <Route path="/pedidos/:id" element={<FormPedidos />} />

//                    {/* PROVEEDORES */}
//                    <Route path="/proveedores" element={<ListaProveedores />} />
//                    <Route path="/proveedores/nuevo" element={<FormProveedores />} />
//                    <Route path="/proveedores/:id/detalles" element={<DetallesProveedores />} />
//                    <Route path="/proveedores/:id/editar" element={<FormProveedores />} />

//                    {/* PRODUCTOS */}
//                    <Route path="/productos" element={<ListaProductos />} />
//                    <Route path="/productos/nuevo" element={<FormProductos />} />
//                    <Route path="/productos/:id/editar" element={<FormProductos />} />
//                    <Route path="/productos/:id/detalles" element={<FormProductos />} />

//                    {/* PEDIDOS */}
//                    <Route path="/pedidos" element={<ListaPedidos />} />

//                    {/* FACTURAS Y CATEGORÍAS */}
//                    <Route path="/facturas" element={<Facturas />} />
//                    <Route path="/categorias" element={<Categorias />} />

//                    {/* HOME */}
//                    <Route path="/" element={<Categorias />} />

//                    {/* Crear nueva cotización para una orden */}
//                    <Route
//                        path="/presupuestos/nuevo/:ordenId"
//                        element={<FormPresupuestos />}
//                    />

//                    {/* Cargar cotizaciones existentes de un proveedor para esa orden */}
//                    <Route
//                        path="/presupuestos/cargar/:ordenId/:proveedorId"
//                        element={<FormPresupuestos />}
//                    />

//                    {/* Listar todas las cotizaciones de una orden */}
//                    <Route
//                        path="/cotizaciones/:ordenId"
//                        element={<ListaPresupuestos />}
//                    />
//                </Routes>
//            </div>
//        </div>
//    );
//}

//import React from 'react';
//import { Routes, Route } from 'react-router-dom';

//import { SideBar } from './Components/SideBar.jsx';

//import { ListaProveedores } from './Pages/ListaProveedores.jsx';
//import { FormProveedores } from './Pages/FormProveedores.jsx';
//import { DetallesProveedores } from './Pages/DetallesProveedores.jsx';
//import { Categorias } from './Pages/Categorias.jsx';

//import  ListaProductos from './Pages/ListaProductos.jsx';
//import  FormProductos from './Pages/FormProductos.jsx';

//import  ListaPedidos from './Pages/ListaPedidos.jsx';
//import  FormPedidos  from './Pages/FormPedidos.jsx';

//import { Facturas } from './Pages/Facturas.jsx';
//import Stock from './Pages/Stock.jsx';

//import ListaPresupuestos from './Pages/ListaPresupuestos.jsx'
//import FormPresupuestos from './Pages/FormPresupuestos.jsx'



//import './App.css';

//export default function App() {
//    return (
//        <div className="flex h-screen overflow-hidden">
//            <SideBar />
//            <div className="flex-1 overflow-auto bg-gray-50">
//                <Routes>
//                    {/* PESTAÑA STOCK */}
//                    <Route path="/stock" element={<Stock />} />
//                    <Route path="/stock" element={<Stock />}>
//                        <Route path="nuevo" element={<FormPedidos />} />
//                        <Route path=":id" element={<FormPedidos />} />
//                    </Route>


//                    {/* PROVEEDORES */}
//                    <Route path="/proveedores" element={<ListaProveedores />} />
//                    <Route path="/proveedores/nuevo" element={<FormProveedores />} />
//                    <Route path="/proveedores/:id/detalles" element={<DetallesProveedores />} />
//                    <Route path="/proveedores/:id/editar" element={<FormProveedores />} />

//                    {/* PRODUCTOS */}
//                    <Route path="/productos" element={<ListaProductos />} />
//                    <Route path="/productos/nuevo" element={<FormProductos />} />
//                    <Route path="/productos/:id/editar" element={<FormProductos />} />
//                    <Route path="/productos/:id/detalles" element={<FormProductos />} />

//                    {/* PEDIDOS */}
//                    <Route path="/pedidos" element={<ListaPedidos />} />
//                    <Route path="/pedidos/nuevo" element={<FormPedidos />} />
//                    <Route path="/pedidos/:id" element={<FormPedidos />} />

//                    {/* FACTURAS Y CATEGORÍAS */}
//                    <Route path="/facturas" element={<Facturas />} />
//                    <Route path="/categorias" element={<Categorias />} />

//                    {/* HOME */}
//                    <Route path="/" element={<Categorias />} />

//                    {/* Crear nueva cotización para una orden */}
//                    <Route
//                        path="/presupuestos/nuevo/:ordenId"
//                        element={<FormPresupuestos />}
//                    />

//                    {/* Cargar cotizaciones existentes de un proveedor para esa orden */}
//                    <Route
//                        path="/presupuestos/cargar/:ordenId/:proveedorId"
//                        element={<FormPresupuestos />}
//                    />

//                    {/* Listar todas las cotizaciones de una orden */}
//                    <Route
//                        path="/cotizaciones/:ordenId"
//                        element={<ListaPresupuestos />}
//                    />




//                </Routes>
//            </div>
//        </div>
//    );
//}

// src/App.jsx
import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
//                          ↑↑↑ agrega aquí Navigate
import { SideBar } from './Components/SideBar.jsx';

import { ListaProveedores } from './Pages/ListaProveedores.jsx';
import { FormProveedores } from './Pages/FormProveedores.jsx';
import { DetallesProveedores } from './Pages/DetallesProveedores.jsx';
import { Categorias } from './Pages/Categorias.jsx';

//import Stock from './pages/Stock';
//import ListaProductos from './pages/ListaProductos';
//import ListaOrdenes from './pages/ListaOrdenes';
//import FormOrden from './pages/FormOrden';
//import ProveedoresParaOrden from './pages/ProveedoresParaOrden';
//import ListaPresupuestosPorOrden from './pages/ListaPresupuestosPorOrden';
//import FormPresupuestos from './pages/FormPresupuestos';
//import ListaPedidosFinales from './pages/ListaPedidosFinales';
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
import './App.css';

export default function App() {
    return (
        <div className="flex h-screen overflow-hidden">
            <SideBar />
            <div className="flex-1 overflow-auto bg-gray-50">
                <Routes>
                    {/* Rutas principales */}
                    {/*<Route path="/" element={<Navigate to="/stock" />} />*/}
                    {/*<Route path="/stock" element={<Stock />} />*/}
                    {/*<Route path="/productos" element={<ListaProductos />} />*/}
                    {/*<Route path="/ordenes" element={<ListaOrdenes />} />*/}
                    {/*<Route path="/ordenes/nueva" element={<FormOrden />} />*/}
                    {/*<Route path="/ordenes/:ordenId/proveedores" element={<ProveedoresParaOrden />} />*/}
                    {/*<Route path="/ordenes/:ordenId/presupuestos" element={<ListaPresupuestosPorOrden />} />*/}
                    {/*<Route path="/ordenes/:ordenId/presupuestos/:provId" element={<FormPresupuestos />} />*/}

                    {/*<Route path="/pedidos" element={<ListaPedidosFinales />} />*/}
                    {/* PROVEEDORES */}
                    {/*<Route*/}
                    {/*    path="/proveedores"*/}
                    {/*    element={<ListaProveedores />}*/}
                    {/*/>*/}
                    {/*<Route*/}
                    {/*    path="/proveedores/nuevo"*/}
                    {/*    element={<FormProveedores />}*/}
                    {/*/>*/}
                    {/*<Route*/}
                    {/*    path="/proveedores/:id/detalles"*/}
                    {/*    element={<DetallesProveedores />}*/}
                    {/*/>*/}
                    {/*<Route*/}
                    {/*    path="/proveedores/:id/editar"*/}
                    {/*    element={<FormProveedores />}*/}
                    {/*/>*/}

                    {/* CATEGORÍAS */}
                    {/*<Route path="/categorias" element={<Categorias />} />*/}

                    {/* AGREGAR AQUÍ las demás rutas (Facturas, etc.) */}

                    {/* Si no coincide, redirige a Stock */}
                    {/*<Route path="*" element={<Navigate to="/stock" />} />*/}
                    <Route path="/" element={<Navigate to="/stock" />} />
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

                    {/* Redirección por defecto */}
                    <Route path="*" element={<Navigate to="/stock" />} />
                </Routes>
            </div>
        </div>
    );
}

