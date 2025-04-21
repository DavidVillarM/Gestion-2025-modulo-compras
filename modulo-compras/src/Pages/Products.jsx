import React from 'react'
import DataTable from 'react-data-table-component';
import { MdOutlineDelete } from "react-icons/md";
import { FaRegEdit } from "react-icons/fa";
import { FiTrash2 } from "react-icons/fi";

export const Products = () => {
    const columns = [
        {
            name: 'ID',
            selector: row => row.id,  // clave de la propiedad de datos
            sortable: true,  // habilita la ordenación
        },
        {
            name: 'Nombre',
            selector: row => row.nombre,
            sortable: true,
        },
        {
            name: 'Categoria',
            selector: row => row.categoria,
            sortable: true,

        },
        {
            name: 'Cantidad',
            selector: row => row.cantidad,
            sortable: true,

        },
        {
            name: 'Ultima Compra',
            selector: row => row.ultimaCompra,
            sortable: true,

        },
        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-2">
                    <button
                        className="text-black px-2 py-1 rounded-lg text-[23px] hover:text-blue-500"

                    >
                        <FaRegEdit />
                    </button>
                    <button
                        className="text-black px-2 py-1 rounded-lg text-[25px] hover:text-red-500"
                    >
                        <FiTrash2 />
                    </button>
                </div>
            ),
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
        },
    ];

    const data = [
        {
            id: 1,
            nombre: 'Jabón líquido para manos',
            categoria: 'Higiene personal',
            cantidad: 30,
            ultimaCompra: '2025-03-20',
        },
        {
            id: 2,
            nombre: 'Toallas de baño',
            categoria: 'Ropa blanca',
            cantidad: 50,
            ultimaCompra: '2025-03-18',
        },
        {
            id: 3,
            nombre: 'Desinfectante multisuperficie',
            categoria: 'Limpieza',
            cantidad: 20,
            ultimaCompra: '2025-03-15',
        },
        {
            id: 4,
            nombre: 'Camas queen size',
            categoria: 'Muebles',
            cantidad: 5,
            ultimaCompra: '2025-02-28',
        },
        {
            id: 5,
            nombre: 'Shampoo y acondicionador',
            categoria: 'Higiene personal',
            cantidad: 40,
            ultimaCompra: '2025-03-22',
        },
        {
            id: 6,
            nombre: 'Papel higiénico',
            categoria: 'Higiene personal',
            cantidad: 100,
            ultimaCompra: '2025-03-19',
        },
        {
            id: 7,
            nombre: 'Sillas de comedor',
            categoria: 'Muebles',
            cantidad: 12,
            ultimaCompra: '2025-03-05',
        },
        {
            id: 8,
            nombre: 'Detergente para pisos',
            categoria: 'Limpieza',
            cantidad: 25,
            ultimaCompra: '2025-03-12',
        }
    ];


    return (
        <div class="bg-gray-200 w-full h-screen">Products
            <div class="rounded  pl-4">
                <button class="text-black bg-gray-100 rounded p-2 hover:bg-sky-600">General</button>
                <button class="rounded p-2 ">Pedidos</button>
                <button class="rounded p-2 ">Poco Stock</button>
            </div>
            <div>
                <div className="pl-4 pr-4 rounded-lg">
                    <DataTable
                        //title="Productos"
                        columns={columns}
                        data={data}

                        highlightOnHover
                        responsive
                    />
                </div>

            </div>
        </div>
    )
}
