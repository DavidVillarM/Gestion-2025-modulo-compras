import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { BrowserRouter as Router, Routes, Route, RouterProvider, createBrowserRouter } from "react-router-dom";
import { SideBar } from './Components/SideBar.jsx'


import RecepcionProductos from './Pages/RecepcionProductos.jsx'
import { Home } from './Pages/HomePage/Home'
import Dashboard from './Pages/HomePage/Dashboard.jsx';

import { Proveedores } from './Pages/Proveedores/index.jsx'
import { ListaProveedores } from './Pages/Proveedores/ListaProveedores.jsx';
import { FormProveedores } from './Pages/Proveedores/FormProveedores.jsx'
import { OrdenesPago } from './Pages/Ordenes/OrdenesPago.jsx';
import { OrdenesVista } from './Pages/Ordenes/OrdenesVista.jsx';
import { OrdenesPresupuestoFinal } from './Pages/Ordenes/OrdenesPresupuestoFinal.jsx';
import { OrdenesPresupuesto } from './Pages/Ordenes/OrdenesPresupuesto.jsx';
import { Ordenes } from './Pages/Ordenes/index.jsx';
import { Stock } from './Pages/Stock/index.jsx';
import { Products } from './Pages/Stock/General.jsx';
import { ModalProveedores } from './Pages/Proveedores/ModalProveedores.jsx';


const router = createBrowserRouter([
  {
    path: "/",
    element: <Home />,
    children: [
      {path: "", element: <Dashboard/>},

      {
        path: "Proveedores",
        element: <Proveedores />,
        children: [
          {
            path: "",
            element: <ListaProveedores />
          },
          {/*
            path: "AgregarProveedor",
            element: <ModalProveedores />,
*/
          }
        ],


      },
      {
        path: "Ordenes",
        element: <Ordenes />,
        children: [
          {
            path: "",
            element: <OrdenesPago />
          },
          {
            path: "Vistas",
            element: <OrdenesVista />,
          },
          {
            path: "OrdenesPresupuesto/:id",
            element: <OrdenesPresupuesto />,
          },
          {
            path: "/OrdenesPresupuestoFinal/:id",
            element: <OrdenesPresupuestoFinal />,
          }
        ],


      },
      {
        path: "Stock",
        element: <Stock />,
        children: [{
          path: "",
          element: <Products />,
        }]
      }]
  },

]);
function App() {

  const [isOpen, setIsOpen] = useState(false);

  return (
    <RouterProvider router={router} />

  );
}

export default App;