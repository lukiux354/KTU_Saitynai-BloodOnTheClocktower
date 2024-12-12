import {useState, useEffect} from 'react';
import axios from 'axios';

const App = () => {
    const [scripts, setScripts] = useState([]);

    useEffect(() => {
        const loadScripts = async () => {
            const response = await axios.get('https://urchin-app-6crcv.ondigitalocean.app/api/scripts');
            //const response = await axios.get('http://localhost:3001/api/scripts');
            console.log(response.data.resource);
            setScripts(response.data.resource);
        };

        loadScripts();
    }, []);

    return (
        <>
        {scripts.map((script, i) => (
            <p key={i}>{script.resource.title} {script.resource.description}</p>
        ))}
        </>
    )
}

export default App;