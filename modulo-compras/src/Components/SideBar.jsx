import React from 'react';
import { FaHome, FaPeopleCarry, FaBoxes, FaBoxOpen, FaTags, FaClipboardList, FaMoneyCheckAlt, FaHotel, FaFileInvoice } from "react-icons/fa";

import { Link } from 'react-router-dom';

export const SideBar = () => {
  return (
    <div className="flex h-screen fix-top">
      {/* Sidebar */}
      <aside className="w-64 bg-sky-600 text-white p-4">
        <div className="flex justify-center mb-6">
          <h1 className="text-[100px] text-black">
            <FaHotel />
          </h1>
        </div>
<nav className="flex flex-col gap-4">
  <Link to="/dashboard" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaHome className="mr-2" />
    <span>Inicio</span>
  </Link>

  <Link to="/proveedores" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaPeopleCarry className="mr-2" />
    <span>Proveedores</span>
  </Link>

  <Link to="/stock" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaBoxes className="mr-2" />
    <span>Stock</span>
  </Link>

  <Link to="/productos" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaBoxOpen className="mr-2" />
    <span>Productos</span>
  </Link>

  <Link to="/categorias" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaTags className="mr-2" />
    <span>Categorias</span>
  </Link>

  <Link to="/ordenes" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaClipboardList className="mr-2" />
    <span>Ordenes</span>
  </Link>
  <Link to="/facturas" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaFileInvoice className="mr-2" />
    <span>Facturas</span>
  </Link>
  <Link to="/presupuestos" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaClipboardList className="mr-2" />
    <span>Presupuesto</span>
  </Link>
  <Link to="/ordenes-pago" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
    <FaMoneyCheckAlt className="mr-2" />
    <span>Gestion Pedidos</span>
  </Link>

          {/* Nueva opción para recepción de productos */}
          <Link to="/recepcion" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            <FaBoxes className="mr-2" />
            <span>Recepción</span>
          </Link>
        </nav>
      </aside>
    </div>
  );
};
