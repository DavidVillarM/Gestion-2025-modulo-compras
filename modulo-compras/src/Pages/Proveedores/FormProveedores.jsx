import React from 'react'

export const FormProveedores = () => {
    return (
        <div class="flex flex-col h-[500px] w-full bg-white pt-10">
            <h1 class="ml-14 mt-2 text-sky-400 font-medium text-[25px]">Agregar Proveedor</h1>
            <div class="flex  h-full w-full  items-row pl-15  mb-10">

                <div class="flex flex-col h-64 w-90 mt-10 ml-12">
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Nombre:</label>
                        <input id='nombre' class="ml-8 bg-gray-300 rounded-sm   h-8" type="text" />

                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <h4 htmlFor="categoria" class="font-medium">Categoria:</h4>
                        <input id='categoria' type="text" class="ml-8 bg-gray-300 rounded-sm h-8" />

                    </p>
                    <p class="flex items-center m-2 justify-between">

                        <label htmlFor="nombre" class="font-medium">Ruc:</label>
                        <input type="text" class="ml-8 mt-2 bg-gray-300 rounded-sm h-8" />

                    </p>

                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Timbrado:</label>
                        <input type="text" class="ml-8 mt-2 bg-gray-300 rounded-sm h-8" />

                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Observaciones:</label>
                        <input type="text" class="ml-8 mt-2 bg-gray-300 rounded-sm h-20" />
                    </p>


                </div>
                <div class="flex flex-col h-64 w-80 mt-10 ml-20">
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Telefono:</label>
                        <input type="text" class="ml-8 mt-2 bg-gray-300 rounded-sm h-8" />
                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Correo:</label>
                        <input type="text" class="ml-2 mt-2 bg-gray-300 rounded-sm h-8" />
                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Dirección:</label>
                        <input type="text" class="ml-6 mt-2 bg-gray-300 rounded-sm h-8" />
                    </p>

                </div>

            </div>
            <div class="flex w-full justify-center mt-10">
                <button class="m-4 bg-gray-400 rounded h-[30px] w-[100px] hover:scale-110">Cancelar</button>
                <button class="m-4 bg-sky-400 rounded h-[30px] w-[100px] hover:scale-110">Aceptar</button>
            </div>
        </div>
    )
}
