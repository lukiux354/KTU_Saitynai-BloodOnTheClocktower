import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Button, Col, Modal, Form } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';

const CommentsList = ({ scriptId, characterId }) => {
    const [comments, setComments] = useState([]);
    const [role, setRole] = useState(localStorage.getItem('role')); // Get role from localStorage
    const [showModal, setShowModal] = useState(false); // State for showing the modal
    const [newCommentText, setNewCommentText] = useState('');

    // Fetch comments when component mounts or scriptId/characterId changes
    useEffect(() => {
        const fetchComments = async () => {
            try {
                const response = await axios.get(
                    `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments`
                );
                setComments(response.data); // Populate comments state
            } catch (error) {
                console.error('Error fetching comments:', error);
            }
        };

        if (scriptId && characterId) {
            fetchComments();
        }
    }, [scriptId, characterId]);

    // Handle new comment creation
    const handleCreateComment = async () => {
        if (!newCommentText || newCommentText.length > 300) {
            alert('Comment text must be between 1 and 300 characters.');
            return;
        }

        try {
            const token = localStorage.getItem('accessToken'); // Retrieve the JWT token
            await axios.post(
                `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments`,
                { body: newCommentText },
                {
                    headers: {
                        Authorization: `Bearer ${token}`, // Include the token in the request
                    },
                }
            );

            alert('Comment created successfully!');
            setShowModal(false); // Close the modal
            setNewCommentText(''); // Reset the input field

            // Refresh the comments list after creation
            const response = await axios.get(
                `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
            setComments(response.data); // Update the comments state
        } catch (error) {
            console.error('Error creating comment:', error);
            alert('Failed to create comment.');
        }
    };

    return (
        <>
            {/* Admin's create button for Comments */}
            {['Admin', 'ForumUser'].includes(role) && (
                <Button
                    variant="success"
                    onClick={() => setShowModal(true)}
                    className="mb-3"
                    style={{ fontSize: '14px', padding: '8px 12px' }}
                >
                    <FaPlus /> New Comment
                </Button>
            )}

            {comments.length > 0 ? (
                comments.map((comment) => (
                    <Col sm={12} key={comment.id} className="mb-4">
                        <Card className="h-100">
                            <Card.Body>
                                <Card.Text>{comment.body}</Card.Text>
                            </Card.Body>
                        </Card>
                    </Col>
                ))
            ) : (
                <p>No comments available.</p>
            )}

            {/* Modal for creating a new comment */}
            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Create New Comment</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group>
                            <Form.Label>Comment Text</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                placeholder="Enter comment text"
                                value={newCommentText}
                                onChange={(e) => setNewCommentText(e.target.value)}
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Cancel
                    </Button>
                    <Button variant="primary" onClick={handleCreateComment}>
                        Create Comment
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default CommentsList;
