//import React from 'react';
//import { FaHome, FaHotel, FaPeopleCarry, FaBoxes } from 'react-icons/fa';
//import { FaCartShopping } from "react-icons/fa6"
//import { RiFileList3Fill } from "react-icons/ri"
//import { Link } from 'react-router-dom';

//export const SideBar = () => {
//    const navItemClass = "flex items-center hover:bg-sky-800 p-2 rounded text-[20px]"
//    const iconClass = "mr-[10px]"
//    return (
//        <div className="flex h-screen fix-top">
//            {/* Sidebar */}
//            <aside className="w-64 bg-sky-600 text-white p-4 h-full">
//                <div className="flex mb-[10px] items-center justify-center">
//                    <h1 className="text-[100px] text-white"><FaHotel /></h1>
//                </div>

//                <nav className="flex flex-col gap-4">
//                    <Link to="/" className="flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
//                        <FaHome className="mr-2" />
//                        <span>Inicio</span>
//                    </Link>

//                    <Link to="/proveedores" className={navItemClass}>
//                        <FaPeopleCarry className={iconClass} />
//                        <span>Proveedores</span>
//                    </Link>
//                    <Link to="/facturas" className={navItemClass}>
//                        <span>Facturas</span>
//                    </Link>
//                    <Link to="/stock" className={navItemClass}>
//                        <FaCartShopping className={iconClass} />
//                        <span>Stock</span>
//                    </Link>

//                    <Link to="/productos" className={navItemClass}>
//                        Productos
//                    </Link>

//                    <Link to="/categorias" className={navItemClass}>
//                        Categorias
//                    </Link>

//                    <Link to="/historial" className={navItemClass}>
//                        Historial
//                    </Link>

//                    <Link to="/ordenes-pago" className={navItemClass}>
//                        Ordenes de pago
//                    </Link>
//                    {/* Nueva opción para recepción de productos */}
//                    <Link to="/recepcion" className={navItemClass}>
//                        <FaBoxes className={iconClass} />
//                        <span>Recepción</span>
//                    </Link>
//                    // SideBar.jsx (agregar debajo de “Inicio”)
//                    <Link to="/stock">
//                        <li className="px-4 py-2 hover:bg-gray-200 cursor-pointer flex items-center gap-2">
//                            <span>Stock</span>
//                        </li>
//                    </Link>

//                </nav>
//            </aside>
//        </div>
//    );
//};

// src/Components/SideBar.jsx
// src/Components/SideBar.jsx
// src/Components/SideBar.jsx
import React from 'react'
import { NavLink } from 'react-router-dom'
import {
    FaHome,
    FaCartPlus,
    FaListUl,
    FaPeopleCarry,
    FaMoneyCheckAlt,
    FaBoxes,
    FaTags,
    FaHotel,
    FaHistory
} from 'react-icons/fa'

const navItems = [
    { to: '/', label: 'Inicio', icon: <FaHome /> },
    { to: '/pedidos', label: 'Pedidos', icon: <FaCartPlus /> },
    { to: '/presupuestos', label: 'Cotizaciones', icon: <FaListUl /> },
    { to: '/proveedores', label: 'Proveedores', icon: <FaPeopleCarry /> },
    { to: '/facturas', label: 'Facturas', icon: <FaMoneyCheckAlt /> },
    { to: '/stock', label: 'Stock', icon: <FaBoxes /> },
    { to: '/ordenes', label: 'Órdenes', icon: <FaCartPlus /> },
    { to: '/productos', label: 'Productos', icon: <FaTags /> },
    { to: '/categorias', label: 'Categorías', icon: <FaTags /> },
    { to: '/historial', label: 'Historial', icon: <FaHistory /> }
]

export const SideBar = () => {
    return (
        <aside className="w-64 bg-sky-600 text-white flex flex-col h-screen p-4 space-y-4">
            <div className="flex justify-center mb-6">
                <FaHotel className="text-6xl" />
            </div>
            <nav className="flex-1">
                {navItems.map(item => (
                    <NavLink
                        key={item.to}
                        to={item.to}
                        end
                        className={({ isActive }) =>
                            `flex items-center p-2 rounded text-lg hover:bg-sky-800 ${isActive ? 'bg-sky-800 font-semibold' : ''
                            }`
                        }
                    >
                        <span className="mr-3">{item.icon}</span>
                        {item.label}
                    </NavLink>
                ))}
            </nav>
        </aside>
    );
}
