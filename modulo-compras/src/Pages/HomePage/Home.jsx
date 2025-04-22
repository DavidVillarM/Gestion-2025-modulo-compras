import React from 'react'
import { SideBar } from '../../components/SideBar'
import { Outlet } from 'react-router-dom'

export const Home = () => {
    return (
        <div className="flex h-screen">

            <SideBar />
 
            <div className="w-screen">
                <Outlet /> {/* Las rutas hijas se renderizarán aquí */}
            </div>
        </div>
    )
}
