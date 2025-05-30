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



function App() {
  return (
    <Router>
      <div className="flex h-screen overflow-hidden">
        <SideBar />
        <div className="flex-1 overflow-y-auto bg-gray-100">
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/categorias" element={<Categorias />} />
            <Route path="/productos" element={<Products />} />
            <Route path="/recepcion" element={<RecepcionProductos />} />
            <Route path="/ordenes-pago" element={<OrdenesPago />} />
            <Route path="/ordenes-presupuesto/" element={<OrdenesPresupuesto />} />
            <Route path="/ordenes-vista/:id" element={<OrdenesVista />} />
           <Route path="/ordenes-presupuesto-final" element={<OrdenesPresupuestoFinal />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;