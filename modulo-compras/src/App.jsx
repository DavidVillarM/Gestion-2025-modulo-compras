import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { BrowserRouter as Router, Routes, Route, RouterProvider, createBrowserRouter } from "react-router-dom";
import { SideBar } from './Components/SideBar.jsx'
import { Products } from './Pages/Products.jsx'
import { Categorias } from './Pages/Categorias.jsx'
import RecepcionProductos from './Pages/RecepcionProductos.jsx'
import { Home } from './Pages/HomePage/Home'
import { Proveedores } from './Pages/Proveedores/index.jsx'
import { ListaProveedores } from './Pages/Proveedores/ListaProveedores.jsx';
import { FormProveedores } from './Pages/Proveedores/FormProveedores.jsx'
import { OrdenesPago } from './Pages/Ordenes/OrdenesPago.jsx';
import { OrdenesVista } from './Pages/Ordenes/OrdenesVista.jsx';
import { OrdenesPresupuestoFinal} from './Pages/Ordenes/OrdenesPresupuestoFinal.jsx';
const router = createBrowserRouter([
  {
    path: "/",
    element: <Home />,
    children: [{
      path: "Proveedores",
      element: <Proveedores />,
      children: [
        {
          path: "",
          element: <ListaProveedores />
        },
        {
          path: "AgregarProveedor",
          element: <FormProveedores />,
        }
      ],


    },
    {
      path: "Ordenes",
      element: <Proveedores />,
      children: [
        {
          path: "",
          element: <OrdenesPago/>
        },
        {
          path: "Vistas",
          element: <OrdenesVista />,
        },
        {
          path: "OrdenesPresupuesto/:id",
          element: <OrdenesPresupuestoFinal />,
        }
      ],


    }]
  },

]);
function App() {
  return (
    <RouterProvider router={router} />
  );
}

export default App;