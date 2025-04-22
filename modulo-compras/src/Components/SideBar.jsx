import React from 'react';
import { FaHome, FaHotel, FaPeopleCarry, FaBoxes } from 'react-icons/fa';
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
          <Link to="/" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            <FaHome className="mr-2" />
            <span>Inicio</span>
          </Link>

          <Link to="/proveedores" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            <FaPeopleCarry className="mr-2" />
            <span>Proveedores</span>
          </Link>

          <Link to="/stock" className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            Stock
          </Link>

          <Link to="/productos" className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            Productos
          </Link>

          <Link to="/categorias" className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            Categorias
          </Link>

          <Link to="/historial" className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            Historial
          </Link>

          <Link to="/ordenes-pago" className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">
            Ordenes de pago
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
