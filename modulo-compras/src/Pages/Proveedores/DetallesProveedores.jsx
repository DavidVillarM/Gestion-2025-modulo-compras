import React from 'react'

export const DetallesProveedores = () => {
    return (

        <div class="flex flex-col h-[470px] w-full bg-white ">
            <h1 class="ml-14 mt-2 text-sky-400 font-medium text-[25px]">Detalles</h1>
            <div class="flex  h-full w-full  items-row">

                <div class="flex flex-col h-64 w-70 mt-10 ml-12">
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Nombre:</label>
                        <h1>Mandioca SA.</h1>

                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <h4 htmlFor="categoria" class="font-medium">Categoria:</h4>
                        <h1>Comestibles</h1>
                    </p>
                    <p class="flex items-center m-2 justify-between">

                        <label htmlFor="nombre" class="font-medium">Ruc:</label>
                        <h1>Mandioca SA.</h1>
                    </p>

                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Timbrado:</label>
                        <h1>Mandioca SA.</h1>
                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Observaciones:</label>
                        <h1>Mandioca SA.</h1>
                    </p>


                </div>
                <div class="flex flex-col h-64 w-80 mt-10 ml-10">
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Telefono:</label>
                        <h1>0981234567</h1>
                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Correo:</label>
                        <h1>mandioca_sa@gmail.com</h1>
                    </p>
                    <p class="flex items-center m-2 justify-between">
                        <label htmlFor="nombre" class="font-medium">Dirección:</label>
                        <h1>Barrio San Pedro - Encarnacion</h1>
                    </p>

                </div>

            </div>
            <div class="flex w-full justify-center border-t-2">
                <button class="m-4 bg-gray-400 rounded h-[30px] w-[100px] hover:scale-110">Cancelar</button>
                <button class="m-4 bg-sky-400 rounded h-[30px] w-[100px] hover:scale-110">Aceptar</button>
            </div>
        </div>

    )
}
