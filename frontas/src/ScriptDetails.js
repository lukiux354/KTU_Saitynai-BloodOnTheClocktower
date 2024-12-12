import React from 'react';
import CharacterList from './CharacterList';
import './ScriptDetails.css';

const ScriptDetails = ({ script }) => {
    if (!script) {
        return <div className="script-details">Select a script to see details</div>;
    }

    return (
        <div className="script-details">
            <h2>{script.title} ID:{script.id}</h2>
            <p>{script.description}</p>
            <CharacterList scriptId={script.id} />
        </div>
    );
}

export default ScriptDetails;
