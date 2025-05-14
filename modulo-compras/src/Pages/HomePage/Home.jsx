import React from 'react'
import { SideBar } from '../../Components/SideBar.jsx';

import { Outlet } from 'react-router-dom'

export const Home = () => {
    return (
        <div className="flex h-screen">

            <SideBar />
 
            <div className="w-screen">
                <Outlet /> 
            </div>
        </div>
    )
}
