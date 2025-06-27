import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import logoImg from "../assets/logo.webp";

export const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      const response = await fetch("http://localhost:5000/api/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) throw new Error("Credenciales inválidas");

      const data = await response.json();
      localStorage.setItem("token", data.token);
      navigate("/ordenes-pago");
    } catch (error) {
      alert(error.message);
    }
  };

  return (
    <div className="w-screen h-screen bg-white flex items-center justify-center">
      <div className="w-full max-w-5xl h-auto shadow-lg rounded-lg overflow-hidden flex flex-col lg:flex-row">
        {/* Panel izquierdo */}
        <div className="w-full lg:w-1/2 p-10 flex flex-col justify-center items-center bg-white">
          <img
            src={logoImg} // Reemplazar por la ruta real del logo
            alt="SICOM Logo"
            className="w-80 h-auto mb-6"
          />
          <h2 className="text-xl font-semibold mb-6 text-center text-gray-800">
            Somos SICOM
          </h2>

          <div className="w-full max-w-sm">
            <p className="mb-4 text-gray-600 text-sm">
              Inicia Sesión con tu cuenta
            </p>

            <input
              type="text"
              placeholder="Nombre de Usuario"
              className="w-full border border-gray-300 rounded px-3 py-2 mb-4 text-sm focus:outline-none focus:ring-2 focus:ring-pink-300"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />

            <input
              type="password"
              placeholder="Contraseña"
              className="w-full border border-gray-300 rounded px-3 py-2 mb-4 text-sm focus:outline-none focus:ring-2 focus:ring-pink-300"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            <button
              onClick={handleLogin}
              className="w-full text-white font-semibold py-2 rounded mb-3 text-sm transition"
              style={{
                background:
                  "linear-gradient(to right, #5FC9D7, #58BDE8, #66A4ED, #66A4ED)",
              }}
            >
              INICIA SESIÓN
            </button>

            <div className="text-center text-sm text-gray-600 mb-4">
              <a href="#" className="hover:underline">
                ¿Olvidaste tu contraseña?
              </a>
            </div>

            <div className="flex items-center justify-center gap-2 text-sm text-gray-700">
              <p>¿No tienes una cuenta?</p>
              <button className="border border-[#d8363a] text-[#d8363a] px-4 py-1 rounded text-xs hover:bg-gray-100 transition">
                REGISTRATE
              </button>
            </div>
          </div>
        </div>

        {/* Panel derecho con degradado */}
        <div
          className="hidden lg:flex w-full lg:w-1/2 items-center justify-center text-white p-10"
          style={{
            background:
              "linear-gradient(to right,#5FC9D7, #58BDE8, #66A4ED, #66A4ED)",
          }}
        >
          <div className="max-w-md">
            <h2 className="text-2xl font-semibold mb-4">
              Somos mas que un sistema de compras
            </h2>
            <p className="text-sm leading-relaxed">
              Bienvenido a SICOM, una plataforma diseñada para optimizar y centralizar la gestión de adquisiciones 
              dentro de tu organización. Desde la solicitud hasta la recepción de productos, 
              este sistema permite un control eficiente de órdenes de compra, proveedores, 
              facturación y stock, todo desde una interfaz intuitiva y segura. Ideal para 
              instituciones que buscan transparencia, trazabilidad y eficiencia en sus 
              procesos de compra.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};
