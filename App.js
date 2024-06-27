import React from 'react';
import Artists from './components/Artists';
import Exhibitions from './components/Exhibitions';
import Facts from './components/Facts';
import './App.css';

const App = () => {
    return (
        <div className="App">
            <header className="App-header">
                <h1>Art Gallery of Aboriginal Art of Australia</h1>
            </header>
            <main>
                <Facts/>
                <Artists />
                <Exhibitions />
            </main>
        </div>
    );
};

export default App;
