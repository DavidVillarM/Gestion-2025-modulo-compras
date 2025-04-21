import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import {SideBar} from './Components/SideBar.jsx'
import {Products} from './Pages/Products.jsx'
import { Categorias } from './Pages/Categorias.jsx'


function App() {
  //const [count, setCount] = useState(0)

  return (
    <>
      <div className="flex">
        <SideBar></SideBar>
        <Categorias></Categorias>
      </div>
    </>
      
  );
}

export default App
