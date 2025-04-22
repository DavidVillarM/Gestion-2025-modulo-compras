import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import {SideBar} from './Components/SideBar.jsx'
import {Products} from './Pages/Products.jsx'
import { Categorias } from './Pages/Categorias.jsx'
import RecepcionProductos from './Pages/RecepcionProductos.jsx'


function App() {
  return (
    <Router>
      <div className="flex">
        <SideBar />
        <div className="flex-1 p-4">
          <Routes>
            <Route path="/categorias" element={<Categorias />} />
            <Route path="/productos" element={<Products />} />
            <Route path="/recepcion" element={<RecepcionProductos />} />
            {/* Agregá más rutas si las tenés */}
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;