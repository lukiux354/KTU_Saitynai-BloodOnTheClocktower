import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CommentList from './CommentList';
import './CharacterList.css';

const CharacterList = ({ scriptId }) => {
    const [characters, setCharacters] = useState([]);

    useEffect(() => {
        const fetchCharacters = async () => {
            const response = await axios.get(`https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters`);
            setCharacters(response.data);
        };

        fetchCharacters();
    }, [scriptId]);

    return (
        <div className="character-list">
            <h3>Characters</h3>
            <ul>
                {characters.map(character => (
                    <li key={character.id}>
                        {character.title}
                        <CommentList characterId={character.id} />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default CharacterList;
