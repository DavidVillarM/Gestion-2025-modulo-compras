import React from 'react'
import DataTable from 'react-data-table-component'
import { FaRegEdit } from "react-icons/fa"
import { FiTrash2 } from "react-icons/fi"
import { Link, Outlet } from 'react-router-dom'

export const ListaProveedores = () => {
    const columns = [
        {
            name: 'Nombre',
            selector: row => row.nombre,
            sortable: true,
        },
        {
            name: 'Categoría',
            selector: row => row.categoria,
            sortable: true,
        },
        {
            name: 'Contacto',
            selector: row => row.contacto,
            sortable: true,
        },
        {
            name: 'RUC',
            selector: row => row.ruc,
            sortable: true,
        },
        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-2">
                    <button className="text-black px-2 py-1 rounded-lg text-[23px] hover:text-blue-500">
                        <FaRegEdit />
                    </button>
                    <button className="text-black px-2 py-1 rounded-lg text-[25px] hover:text-red-500">
                        <FiTrash2 />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
        },
    ]

    const data = [
        {
            nombre: 'Distribuidora López',
            categoria: 'Limpieza',
            contacto: '0981 123 456',
            ruc: '80012345-6',
        },
        {
            nombre: 'ProveMax',
            categoria: 'Higiene personal',
            contacto: '0982 654 321',
            ruc: '80098765-2',
        },
        {
            nombre: 'Muebles Express',
            categoria: 'Muebles',
            contacto: '0971 456 789',
            ruc: '80111223-9',
        },
        {
            nombre: 'White Textile',
            categoria: 'Ropa blanca',
            contacto: '0983 321 123',
            ruc: '80233445-0',
        },
    ]

    return (
        <div className="bg-gray-200 w-full h-screen pt-10">

            <h2 className="ml-14 mt-2 text-sky-400 font-medium text-[25px]">Proveedores</h2>
            <div className="flex justify-between m-2 items-center">

                <div className="flex justify-end m-2 items-center">
                    <label htmlFor="categorias" className="block mb-1 text-sm font-medium text-gray-700">
                        Categorías
                    </label>
                    <input type="text" className="bg-white m-2" />
                    <label htmlFor="buscar" className="block mb-1 text-sm font-medium text-gray-700">
                        Buscar
                    </label>
                    <input id='buscar' type="text" className="bg-white m-2" />
                </div>
            </div>

            <div className="pl-4 pr-4 rounded-lg pb-10">
                <DataTable
                    columns={columns}
                    data={data}
                    highlightOnHover
                    responsive
                />
            </div>
            <div class="flex w-full justify-center ">
                <Link to="AgregarProveedor">
                    <button class="text-white m-4 bg-sky-400 rounded h-[30px] w-[200px] hover:scale-110">Nuevo Proveedor</button>
                </Link>
            </div>
        </div>
    )
}
