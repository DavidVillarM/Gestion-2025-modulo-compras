//// src/Pages/Stock.jsx
//import React, { useState } from 'react'
//import ListaProductos from './ListaProductos.jsx'
//import ListaPedidos from './ListaPedidos.jsx'

//export default function Stock() {
//    const [tab, setTab] = useState('general')

//    return (
//        <div className="p-6 bg-gray-100 min-h-screen">
//            {/* Cabecera de pestañas */}
//            <div className="flex justify-start space-x-4 border-b mb-6 bg-white p-2">
//                <button
//                    onClick={() => setTab('general')}
//                    className={`px-4 py-2 font-medium ${tab === 'general'
//                        ? 'border-b-2 border-blue-600 text-blue-600'
//                        : 'text-gray-600 hover:text-blue-600'
//                        }`}
//                >
//                    Productos
//                </button>
//                <button
//                    onClick={() => setTab('pedidos')}
//                    className={`px-4 py-2 font-medium ${tab === 'pedidos'
//                        ? 'border-b-2 border-blue-600 text-blue-600'
//                        : 'text-gray-600 hover:text-blue-600'
//                        }`}
//                >
//                    Pedidos
//                </button>
//                <button
//                    onClick={() => setTab('poco')}
//                    className={`px-4 py-2 font-medium ${tab === 'poco'
//                        ? 'border-b-2 border-blue-600 text-blue-600'
//                        : 'text-gray-600 hover:text-blue-600'
//                        }`}
//                >
//                    Poco stock
//                </button>
//            </div>

//            {/* Contenido de cada pestaña */}
//            {tab === 'general' && <ListaProductos />}
//            {tab === 'pedidos' && <ListaPedidos />}
//            {tab === 'poco' && <ListaProductos lowStockOnly={true} />}
//        </div>
//    )
//}

// src/Pages/Stock.jsx
// src/pages/Stock.jsx
// src/pages/Stock.jsx
import React, { useState } from 'react';
import ListaProductos from './ListaProductos';
import ListaOrdenes from './ListaOrdenes';
import PocoStock from './PocoStock';

export default function Stock() {
    const [tabActivo, setTabActivo] = useState('productos');

    return (
        <div>
            <nav className="mb-6 border-b border-gray-300 bg-white">
                <ul className="flex space-x-4 px-6">
                    <li>
                        <button
                            className={`px-3 py-2 ${tabActivo === 'productos'
                                    ? 'border-b-2 border-blue-600 font-semibold'
                                    : 'text-gray-600 hover:text-blue-600'
                                }`}
                            onClick={() => setTabActivo('productos')}
                        >
                            Productos
                        </button>
                    </li>
                    <li>
                        <button
                            className={`px-3 py-2 ${tabActivo === 'ordenes'
                                    ? 'border-b-2 border-blue-600 font-semibold'
                                    : 'text-gray-600 hover:text-blue-600'
                                }`}
                            onClick={() => setTabActivo('ordenes')}
                        >
                            Órdenes
                        </button>
                    </li>
                    <li>
                        <button
                            className={`px-3 py-2 ${tabActivo === 'poco-stock'
                                    ? 'border-b-2 border-blue-600 font-semibold'
                                    : 'text-gray-600 hover:text-blue-600'
                                }`}
                            onClick={() => setTabActivo('poco-stock')}
                        >
                            Poco Stock
                        </button>
                    </li>
                </ul>
            </nav>

            {tabActivo === 'productos' && <ListaProductos />}
            {tabActivo === 'ordenes' && <ListaOrdenes />}
            {tabActivo === 'poco-stock' && <PocoStock />}
        </div>
    );
}



