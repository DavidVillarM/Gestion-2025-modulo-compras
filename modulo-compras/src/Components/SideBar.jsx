import React from 'react';
import { FaHome, FaPeopleCarry, FaBoxes, FaBoxOpen, FaTags, FaClipboardList, FaMoneyCheckAlt, FaHotel } from "react-icons/fa";

import { Link } from 'react-router-dom';

export const SideBar = () => {
  return (
    <div className="flex h-screen fix-top">
      {/* Sidebar */}
<aside className="w-62 bg-sky-600 text-white p-4">
  <div className="flex justify-center mb-4">
    <h1 className="text-6xl text-black">
      <FaHotel />
    </h1>
  </div>
  <nav className="flex flex-col gap-3">
    <Link to="/dashboard" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaHome className="mr-2 text-2xl" />
      <span>Inicio</span>
    </Link>

    <Link to="/proveedores" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaPeopleCarry className="mr-2 text-2xl" />
      <span>Proveedores</span>
    </Link>

    <Link to="/stock" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaBoxes className="mr-2 text-2xl" />
      <span>Stock</span>
    </Link>

    <Link to="/productos" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaBoxOpen className="mr-2 text-2xl" />
      <span>Productos</span>
    </Link>

    <Link to="/categorias" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaTags className="mr-2 text-2xl" />
      <span>Categorías</span>
    </Link>

    <Link to="/ordenes" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaClipboardList className="mr-2 text-2xl" />
      <span>Órdenes</span>
    </Link>

    <Link to="/presupuestos" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaClipboardList className="mr-2 text-2xl" />
      <span>Presupuesto</span>
    </Link>

    <Link to="/ordenes-pago" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaMoneyCheckAlt className="mr-2 text-2xl" />
      <span>Gestión</span>
    </Link>

    <Link to="/recepcion" className="flex items-center hover:bg-sky-800 p-2 rounded text-lg">
      <FaBoxes className="mr-2 text-2xl" />
      <span>Recepción</span>
    </Link>
  </nav>
</aside>

    </div>
  );
};
