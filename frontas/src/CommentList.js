import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './CommentList.css';

const CommentList = ({ characterId }) => {
    const [comments, setComments] = useState([]);

    useEffect(() => {
        const fetchComments = async () => {
            const response = await axios.get(`https://urchin-app-6crcv.ondigitalocean.app/api/characters/${characterId}/comments`);
            setComments(response.data);
        };

        fetchComments();
    }, [characterId]);

    return (
        <div className="comment-list">
            <h4>Comments</h4>
            <ul>
                {comments.map(comment => (
                    <li key={comment.id}>
                        {comment.content}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default CommentList;
