import React from 'react'
import { FaUser } from 'react-icons/fa';
export const Login = () => {
    return (
        <div class="flex flex-col bg-sky-700 items-center justify-center w-screen h-screen">
            <div class="flex flex-col item-center justify-centrer  w-50 ">
                <p class=" flex mb-[10px] items-center justify-center w-80">
                    <h1 class="text-[100px] text-white"><FaUser /></h1>
                </p>
                <p class="mb-[10px]">
                    <input class="bg-white border-gray-700 rounded h-[35px] w-80" type="text" placeholder="Correo electronico" />
                </p>
                <p class="mt-[10px] h-[35px] w-80">
                    <input class="bg-white border-gray-700 rounded h-[35px] w-80" type="password" placeholder="Contraseña" />
                </p>
                <p class="flex mt-[10px] items-center justify-center h-[35px] w-80">
                    <button class="bg-white w-[100px] rounded mt-8 h-[35px] w-80" type="submit">Iniciar Sesion</button>
                </p>
            </div>
        </div>
    )
}
