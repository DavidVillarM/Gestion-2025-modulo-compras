import React, { useState } from 'react'
import { FaHistory, FaHome, FaHotel, FaPeopleCarry } from 'react-icons/fa'
import { IoHomeOutline } from "react-icons/io5";
import { CiShoppingCart } from "react-icons/ci";
import { FaCartShopping } from "react-icons/fa6";
import { RiFileList3Fill } from "react-icons/ri";
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link
} from 'react-router-dom';

export const SideBar = () => {
    const [show, setShow] = useState(true);
    return (

        <aside className="w-64 bg-sky-600 text-white p-4 h-full">
            <p class=" flex mb-[10px] items-center justify-center">
                <h1 class="text-[100px] text-white"><FaHotel /></h1>
            </p>

            <nav className="flex flex-col gap-4">
                <Link to="/" className="flex items-center hover:bg-sky-800 p-2 rounded">
                    <FaHome className="mr-[10px]" />
                    <span className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">Inicio</span>
                </Link>

                <Link to="/Proveedores" className="flex items-center hover:bg-sky-800  rounded">
                    <button className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
                        <FaPeopleCarry class="mr-[10px]" />
                        <h2>Proveedores</h2>
                    </button>
                </Link>

                <Link to="/Stock" className="flex items-center hover:bg-sky-800 p-2 rounded">
                    <FaCartShopping className="mr-[10px]" />
                    <span className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">Stock</span>
                </Link>
                <button className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
                    <FaHistory class="mr-[10px] " />
                    <h2>Historial</h2>
                </button>
                <Link to="/Ordenes" className="flex items-center hover:bg-sky-800 p-2 rounded">

                    <button className=" flex items-center text-left hover:bg-sky-800 p-2 rounded text-[20px]">
                        <RiFileList3Fill class="mr-[10px] " />
                        <h2>Ordenes de pago</h2>
                    </button>
                </Link>



            </nav>
        </aside >



    )
}
