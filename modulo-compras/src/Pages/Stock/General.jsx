import React, { use, useEffect, useState } from 'react'
import DataTable from 'react-data-table-component';
import { MdOutlineDelete } from "react-icons/md";
import { FaEye, FaRegEdit } from "react-icons/fa";
import { FiTrash2 } from "react-icons/fi";
import axios from "../../axios";

export const Products = () => {
    const [productos, setProductos] = useState([]);
    const columns = [
        {
            name: 'ID',
            selector: row => row.idProducto,  // clave de la propiedad de datos
            sortable: true,  // habilita la ordenación
            width: "80px"
        },
        {
            name: 'Nombre',
            selector: row => row.nombre,
            sortable: true,
            width: "300px"
        },
        {
            name: 'Marca',
            selector: row => row.marca,
            sortable: true,
            width: "200px"
        },
        {
            name: 'Categoria',
            selector: row => row.IdCategoria,
            sortable: true,
            width: "200px"

        },
        {
            name: 'Cantidad',
            selector: row => row.cantidadTotal,
            sortable: true,
            width: "200px"

        },

        {
            name: 'Acciones',
            cell: row => (
                <div className="flex gap-2">
                    <button
                        className="text-black px-2 py-1 rounded-lg text-[23px] hover:text-green-500"

                    >
                        <FaEye />
                    </button>
                    <button
                        className="text-black px-2 py-1 rounded-lg text-[23px] hover:text-blue-500"                   >
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
    useEffect(() => {
        axios.get("Producto").then((response) => {
            console.log(response.data);
            setProductos(response.data);

        }).catch((error) => { console.log("Hubo un error", error) });
    }, []);


    return (
        <div class="bg-gray-200 w-full h-screen">

            <div class="flex justify-between pt-8  items-center  ">
                <div class="rounded  pl-4">
                    <button class="text-black bg-gray-100 rounded p-2 hover:bg-sky-600">General</button>
                    <button class="text-black bg-gray-100 rounded p-2 hover:bg-sky-600">Pedidos</button>
                    <button class="text-black bg-gray-100 rounded p-2 hover:bg-sky-600 ">Poco Stock</button>
                </div>
                <div class=" flex justify-end m-2 items-center">
                    <label htmlFor="categorias" class="block mb-1 text-sm font-medium text-gray-700">
                        Categorias
                    </label>
                    <input type="text" class="bg-white m-2" />
                    <label htmlFor="buscar" class="block mb-1 text-sm font-medium text-gray-700">
                        Buscar
                    </label>
                    <input id='buscar' type="text" class="bg-white m-2" />
                </div>

            </div>


            <div>
                <div className="pl-4 pr-4  rounded-lg">
                    <DataTable className='shadow-lg/20 rounded-lg'
                        //title="Productos"
                        columns={columns}
                        data={productos}

                        highlightOnHover
                        responsive
                    />
                </div>

            </div>
        </div>
    )
}
