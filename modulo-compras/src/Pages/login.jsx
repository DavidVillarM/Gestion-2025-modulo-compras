import React, { useState } from 'react';
import { FaUser } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

export const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });
            
            if (!response.ok) {
                throw new Error('Credenciales inválidas');
            }

            const data = await response.json();
            localStorage.setItem('token', data.token);
            console.log(data);
            navigate('/ordenes-pago'); 
        } catch (error) {
            alert(error.message);
        }
    };

    return (
        <div className="flex flex-col bg-sky-700 items-center justify-center w-screen h-screen">
            <div className="flex flex-col items-center justify-center w-50 ">
                <div className="flex mb-[10px] items-center justify-center w-80">
                    <h1 className="text-[100px] text-white"><FaUser /></h1>
                </div>
                <div className="mb-[10px]">
                    <input
                        className="bg-white border-gray-700 rounded h-[35px] w-80 px-2"
                        type="text"
                        placeholder="Nombre de usuario"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </div>
                <div className="mt-[10px] h-[35px] w-80">
                    <input
                        className="bg-white border-gray-700 rounded h-[35px] w-80 px-2"
                        type="password"
                        placeholder="Contraseña"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>
                <div className="flex mt-[10px] items-center justify-center h-[35px] w-80">
                    <button
                        className="bg-white rounded mt-8 h-[35px] w-80 font-bold"
                        onClick={handleLogin}
                    >
                        Iniciar Sesión
                    </button>
                </div>
            </div>
        </div>
    );
};