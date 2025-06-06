import React, { useEffect, useState } from "react";
import DataTable from "react-data-table-component";
import { FaRegEdit, FaTrash } from "react-icons/fa";

export function Categorias() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [mostrarFormulario, setMostrarFormulario] = useState(false);
  const [nuevaCategoria, setNuevaCategoria] = useState({
    nombre: "",
    descripcion: "",
  });

  useEffect(() => {
    fetch("http://localhost:5000/api/categorias")
      .then((response) => {
        if (!response.ok) throw new Error("Error al cargar categorías");
        return response.json();
      })
      .then((data) => {
        setCategories(data);
        setLoading(false);
      })
      .catch((err) => {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  const columns = [
    {
      name: "ID",
      selector: (row) => row.idCategoria,
      sortable: true,
      width: "80px",
    },
    {
      name: "Nombre",
      selector: (row) => row.nombre,
      sortable: true,
    },
    {
      name: "Descripción",
      selector: (row) => row.descripcion,
    },
    {
      name: "Acciones",
      cell: (row) => (
        <div className="flex gap-2">
          <button
            className="text-gray-600 hover:text-sky-600"
            onClick={() => alert(`Editar categoría ID ${row.idCategoria}`)}
          >
            <FaRegEdit size={20} />
          </button>
          <button
            className="text-gray-600 hover:text-red-600 px-3 py-1 rounded"
            onClick={() => alert(`Eliminar categoría ID ${row.idCategoria}`)}
            title="Eliminar"
          >
            <FaTrash size={18} />
          </button>
        </div>
      ),
      ignoreRowClick: true,
      allowOverflow: true,
      button: true,
      center: true,
    },
  ];

  const handleChange = (e) => {
    setNuevaCategoria({
      ...nuevaCategoria,
      [e.target.name]: e.target.value,
    });
  };

  const handleNuevaCategoria = (e) => {
    e.preventDefault();

    fetch("http://localhost:5000/api/categorias", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(nuevaCategoria),
    })
      .then((response) => {
        if (!response.ok) throw new Error("Error al guardar la categoría");
        return response.json();
      })
      .then((data) => {
        setCategories([...categories, data]);
        setNuevaCategoria({ nombre: "", descripcion: "" });
        setMostrarFormulario(false);
      })
      .catch((err) => {
        alert("Error: " + err.message);
      });
  };

  return (
    <div className="p-6 w-full  mx-auto">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Gestión de Categorías</h1>
        <button
          className="bg-sky-500 text-white px-4 py-2 rounded hover:bg-sky-600"
          onClick={() => setMostrarFormulario(!mostrarFormulario)}
        >
          {mostrarFormulario ? "Cancelar" : "Nueva Categoría"}
        </button>
      </div>

      {mostrarFormulario && (
        <form
          onSubmit={handleNuevaCategoria}
          className="mb-6 bg-white p-4 rounded shadow-md"
        >
          <div className="mb-4">
            <label className="block mb-1 font-semibold">Nombre</label>
            <input
              type="text"
              name="nombre"
              value={nuevaCategoria.nombre}
              onChange={handleChange}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-sky-500"
              placeholder="Ingrese el nombre"
            />
          </div>
          <div className="mb-4">
            <label className="block mb-1 font-semibold">Descripción</label>
            <textarea
              name="descripcion"
              value={nuevaCategoria.descripcion}
              onChange={handleChange}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-sky-500"
              placeholder="Ingrese la descripción"
            />
          </div>
          <button
            type="submit"
            className="bg-sky-600 text-white px-4 py-2 rounded hover:bg-sky-700"
          >
            Guardar
          </button>
        </form>
      )}

      {loading ? (
        <p>Cargando...</p>
      ) : error ? (
        <p className="text-red-500">Error: {error}</p>
      ) : (
        <DataTable
          columns={columns}
          data={categories}
          pagination
          highlightOnHover
          striped
          responsive
          className="rounded-md shadow-md bg-white"
        />
      )}
    </div>
  );
}