import React, { useState } from 'react';
import Header from './Header';
import Footer from './Footer';
import ScriptList from './ScriptList';
import ScriptDetails from './ScriptDetails';
import './App.css';

const App = () => {
    const [selectedScript, setSelectedScript] = useState(null);

    return (
        <div className="app">
            <Header />
            <main className="content">
                <ScriptList onSelectScript={setSelectedScript} />
                <ScriptDetails script={selectedScript} />
            </main>
            <Footer />
        </div>
    );
}

export default App;
