import React, { useEffect, useState } from 'react';
import axios from 'axios';

const CommentList = ({ scriptId, characterId }) => {
    const [comments, setComments] = useState([]);

    useEffect(() => {
        const fetchComments = async () => {
            try {
                const response = await axios.get(`https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments`);
                setComments(response.data);
            } catch (error) {
                console.error('Error fetching comments:', error);
            }
        };

        fetchComments();
    }, [scriptId, characterId]);

    return (
        <div>
            <h2>Comments</h2>
            <ul>
                {comments.map(comment => (
                    <li key={comment.id}>{comment.content}</li>
                ))}
            </ul>
        </div>
    );
};

export default CommentList;
