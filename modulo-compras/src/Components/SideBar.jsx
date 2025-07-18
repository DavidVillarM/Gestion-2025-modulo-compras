import React from 'react';
import {
  FaHome,
  FaPeopleCarry,
  FaBoxes,
  FaBoxOpen,
  FaTags,
  FaClipboardList,
  FaMoneyCheckAlt,
  FaHotel,
  FaFileInvoice,
  FaChair // <- nuevo ícono para Asientos
} from "react-icons/fa";
import logoImg from "../assets/logo-removebg-preview (1).png";
import { Link } from 'react-router-dom';

export const SideBar = () => {
  return (
    <div className="flex h-screen fix-top">
      {/* Sidebar */}
     <aside className="w-64 bg-sky-600 text-white p-4 overflow-y-auto">
        <div className="flex justify-center mb-6">
          <h1 className="text-[100px] text-black">
            <img src={logoImg} alt="lOGO" />
          </h1>
        </div>
       <nav className="flex flex-col gap-4">
  {/* Gestión General */}
  <div>
    <h2 className="uppercase text-sm text-gray-200 mb-2">Gestión General</h2>
    <Link to="/dashboard" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaHome className="mr-2" />
      <span>Inicio</span>
    </Link>
    <Link to="/proveedores" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaPeopleCarry className="mr-2" />
      <span>Proveedores</span>
    </Link>
  </div>

  {/* Productos y Stock */}
  <div>
    <h2 className="uppercase text-sm text-gray-200 mb-2">Productos y Stock</h2>
    <Link to="/stock" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaBoxes className="mr-2" />
      <span>Stock</span>
    </Link>
    <Link to="/productos" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaBoxOpen className="mr-2" />
      <span>Productos</span>
    </Link>
    <Link to="/categorias" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaTags className="mr-2" />
      <span>Categorías</span>
    </Link>
  </div>

  {/* Órdenes y Finanzas */}
  <div>
    <h2 className="uppercase text-sm text-gray-200 mb-2">Órdenes y Finanzas</h2>
    <Link to="/ordenes" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaClipboardList className="mr-2" />
      <span>Órdenes</span>
    </Link>
    <Link to="/presupuestos" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaClipboardList className="mr-2" />
      <span>Presupuesto</span>
    </Link>
    <Link to="/ordenes-pago" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaMoneyCheckAlt className="mr-2" />
      <span>Gestión Pedidos</span>
    </Link>
    <Link to="/facturas" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaFileInvoice className="mr-2" />
      <span>Facturas</span>
    </Link>
    <Link to="/notas-credito" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaFileInvoice className="mr-2" />
      <span>Nota de Crédito</span>
    </Link>
  </div>

  {/* Otras funciones */}
  <div>
    <h2 className="uppercase text-sm text-gray-200 mb-2">Otros</h2>
    <Link to="/recepcion" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaBoxes className="mr-2" />
      <span>Recepción</span>
    </Link>
    <Link to="/asientos" className="flex items-center hover:bg-sky-800 p-2 rounded text-[20px]">
      <FaChair className="mr-2" />
      <span>Asientos</span>
    </Link>
  </div>
</nav>
      </aside>
    </div>
  );
};
