import {useState, useEffect} from 'react';
import axios from 'axios';

const App = () => {
    const [topics, setTopics] = useState([]);

    useEffect(() => {
        const loadTopics = async () => {
            const response = await axios.get('https://urchin-app-6crcv.ondigitalocean.app/api/scripts');
            //const response = await axios.get('http://localhost:3001/api/scripts');
            console.log(response.data.resource);
            setTopics(response.data.resource);
        };

        loadTopics();
    }, []);

    return (
        <>
        {topics.map((topic, i) => (
            <p key={i}>{topic.resource.title} {topic.resource.description}</p>
        ))}
        </>
    )
}

export default App;