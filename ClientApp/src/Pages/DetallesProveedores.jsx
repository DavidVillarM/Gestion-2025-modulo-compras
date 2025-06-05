// src/Pages/DetallesProveedores.jsx
import React, { useEffect, useState } from 'react';
import { fetchProveedor } from '../api/proveedores';
import { useParams, useNavigate } from 'react-router-dom';

export function DetallesProveedores() {
    const { id } = useParams();
    const [prov, setProv] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        (async () => {
            try {
                setProv(await fetchProveedor(id));
            } catch (e) {
                console.error(e);
            }
        })();
    }, []);

    if (!prov) return <div>Cargando…</div>;

    return (
        <div className="p-6 bg-gray-50 min-h-screen">
            <h2 className="text-xl font-bold text-sky-600 mb-6">Detalles de Proveedor</h2>
            <dl className="grid grid-cols-2 gap-4 bg-white p-6 rounded shadow">
                <div>
                    <dt className="font-medium">Nombre</dt>
                    <dd>{prov.nombre}</dd>
                </div>
                <div>
                    <dt className="font-medium">Teléfono</dt>
                    <dd>{prov.telefono}</dd>
                </div>
                <div>
                    <dt className="font-medium">Categoría</dt>
                    <dd>{prov.categoria}</dd>
                </div>
                <div>
                    <dt className="font-medium">Correo</dt>
                    <dd>{prov.correo}</dd>
                </div>
                <div>
                    <dt className="font-medium">RUC</dt>
                    <dd>{prov.ruc}</dd>
                </div>
                <div>
                    <dt className="font-medium">Dirección</dt>
                    <dd>{prov.direccion}</dd>
                </div>
                <div>
                    <dt className="font-medium">Timbrado</dt>
                    <dd>{prov.timbrado}</dd>
                </div>
                <div className="col-span-2">
                    <dt className="font-medium">Observaciones</dt>
                    <dd>{prov.observaciones}</dd>
                </div>
            </dl>

            <div className="mt-6">
                <button
                    onClick={() => navigate(-1)}
                    className="px-4 py-2 bg-gray-400 text-white rounded"
                >
                    Volver
                </button>
            </div>
        </div>
    );
}
