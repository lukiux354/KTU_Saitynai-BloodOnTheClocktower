import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './ScriptList.css';

const ScriptList = ({ onSelectScript }) => {
    const [scripts, setScripts] = useState([]);

    useEffect(() => {
        const fetchScripts = async () => {
            const response = await axios.get('https://urchin-app-6crcv.ondigitalocean.app/api/scripts');
            setScripts(response.data);
        };

        fetchScripts();
    }, []);

    return (
        <div className="script-list">
            <h2>Scripts</h2>
            <ul>
                {scripts.map(script => (
                    <li key={script.id} onClick={() => onSelectScript(script)}>
                        {script.title}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default ScriptList;
