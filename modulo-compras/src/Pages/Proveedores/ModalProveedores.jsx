import React from 'react'
import { Link } from 'react-router-dom'

export const ModalProveedores = ({ onClose }) => {
    return (
        <div className="flex items-center justify-center bg-gray-600/50 fixed inset-0">
            <div className="bg-white p-10 rounded-lg shadow-lg transform transition duration-300 ease-out scale-100 opacity-100">
                <div className="flex h-full w-full items-row">
                    <div className="flex flex-col h-64 w-90">
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="nombre" className="font-medium">Nombre:</label>
                            <input id="nombre" type="text" className="ml-8 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="categoria" className="font-medium">Categoria:</label>
                            <input id="categoria" type="text" className="ml-8 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="ruc" className="font-medium">Ruc:</label>
                            <input id="ruc" type="text" className="ml-8 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="timbrado" className="font-medium">Timbrado:</label>
                            <input id="timbrado" type="text" className="ml-8 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="observaciones" className="font-medium">Observaciones:</label>
                            <input id="observaciones" type="text" className="ml-8 border border-gray-400 bg-gray-100 rounded-sm h-20" />
                        </p>
                    </div>
                    <div className="flex flex-col h-64 w-80 ml-20">
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="telefono" className="font-medium">Telefono:</label>
                            <input id="telefono" type="text" className="ml-8 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="correo" className="font-medium">Correo:</label>
                            <input id="correo" type="text" className="ml-2 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                        <p className="flex items-center m-2 justify-between">
                            <label htmlFor="direccion" className="font-medium">Dirección:</label>
                            <input id="direccion" type="text" className="ml-6 border border-gray-400 bg-gray-100 rounded-sm h-8" />
                        </p>
                    </div>
                </div>
                <div className="flex w-full justify-center mt-20">
                    <Link to="/Proveedores">
                        <button className=" shadow-xl/20 text-white  m-4 bg-gray-400 rounded h-[30px] w-[100px] hover:scale-110" onClick={onClose}>Cancelar</button>
                    </Link>
                    <Link to="/Proveedores">
                        <button className="shadow-xl/20 text-white  m-4 bg-sky-400 rounded h-[30px] w-[100px] hover:scale-110">Aceptar</button>
                    </Link>
                </div>
            </div>
        </div>
    )
}
