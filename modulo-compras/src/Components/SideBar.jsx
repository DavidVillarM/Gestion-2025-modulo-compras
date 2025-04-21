import React from 'react'
import { FaHome, FaHotel, FaPeopleCarry } from 'react-icons/fa'
import { IoHomeOutline } from "react-icons/io5";

export const SideBar = () => {
    return (
        <div className="flex h-screen">
            {/* Sidebar */}
            <aside className="w-64 bg-sky-600 text-white p-4 ">
                <p class=" flex mb-[10px] items-center justify-center">
                    <h1 class="text-[100px] text-black"><FaHotel /></h1>
                </p>

                <nav className="flex flex-col gap-4">

                    <button className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
                        <FaHome class="mr-[10px]" />
                        <h2>Inicio</h2>
                    </button>
                    <button className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
                        <FaPeopleCarry class="mr-[10px]" />
                        <h2>Proveedores</h2>
                    </button>

                    <button className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">Stock</button>
                    <button className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">Historial</button>
                    <button className="text-left hover:bg-sky-800 p-2 rounded text-[20px]">Ordenes de pago</button>
                </nav>
            </aside>

        </div>

    )
}
