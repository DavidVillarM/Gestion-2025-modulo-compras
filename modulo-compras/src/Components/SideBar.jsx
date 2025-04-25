import React, { useState } from 'react'
import { FaHistory, FaHome, FaHotel, FaPeopleCarry } from 'react-icons/fa'
import { FaCartShopping } from "react-icons/fa6"
import { RiFileList3Fill } from "react-icons/ri"
import { Link } from 'react-router-dom'

export const SideBar = () => {
    const [show, setShow] = useState(true)

    // Clases comunes reutilizables
    const navItemClass = "flex items-center hover:bg-sky-800 p-2 rounded text-[20px]"
    const iconClass = "mr-[10px]"

    return (
        <aside className="w-64 bg-sky-600 text-white p-4 h-full">
            <div className="flex mb-[10px] items-center justify-center">
                <h1 className="text-[100px] text-white"><FaHotel /></h1>
            </div>

            <nav className="flex flex-col gap-4 mt-15">

                <Link to="/" className={navItemClass}>
                    <FaHome className={iconClass} />
                    <span>Inicio</span>
                </Link>

                <Link to="/Proveedores" className={navItemClass}>
                    <FaPeopleCarry className={iconClass} />
                    <span>Proveedores</span>
                </Link>

                <Link to="/Stock" className={navItemClass}>
                    <FaCartShopping className={iconClass} />
                    <span>Stock</span>
                </Link>

                <button className={navItemClass}>
                    <FaHistory className={iconClass} />
                    <span>Historial</span>
                </button>

                <Link to="/Ordenes" className={navItemClass}>
                    <RiFileList3Fill className={iconClass} />
                    <span>Ordenes</span>
                </Link>

            </nav>
        </aside>
    )
}
