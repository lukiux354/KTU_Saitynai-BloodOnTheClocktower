import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Button, Col, Modal, Form } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa'; 
import { Link } from 'react-router-dom';

const CharacterList = ({ scriptId }) => {
    const [characters, setCharacters] = useState([]);
    const [role, setRole] = useState(localStorage.getItem('role')); // Get role from localStorage
    const [showModal, setShowModal] = useState(false); // State for showing the modal
    const [newCharacterName, setNewCharacterName] = useState('');
    const [newCharacterDescription, setNewCharacterDescription] = useState('');

    useEffect(() => {
        const fetchCharacters = async () => {
            try {
                const response = await axios.get(`https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters`);
                setCharacters(response.data);
            } catch (error) {
                console.error('Error fetching characters:', error);
            }
        };

        if (scriptId) {
            fetchCharacters();
        }
    }, [scriptId]);

    const truncateText = (text, maxLength) => {
        if (!text) return ''; // Handle undefined or null descriptions
        return text.slice(0, maxLength) + '...';
    };

    const handleCreateCharacter = async () => {
        if (!newCharacterName || newCharacterName.length > 20) {
            alert('Character name must be between 1 and 20 characters.');
            return;
        }

        if (!newCharacterDescription || newCharacterDescription.length > 300) {
            alert('Character description must be between 1 and 300 characters.');
            return;
        }

        try {
            const token = localStorage.getItem('accessToken'); // Retrieve the JWT token
            await axios.post(
                `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters`, // Fixed the URL
                { title: newCharacterName, body: newCharacterDescription },
                {
                    headers: {
                        Authorization: `Bearer ${token}`, // Include the token in the request
                    },
                }
            );

            alert('Character created successfully!');
            setShowModal(false); // Close the modal after successful creation
            setNewCharacterName('');
            setNewCharacterDescription('');

            // Refresh the Characters list after creation
            const response = await axios.get(`https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setCharacters(response.data); // Update the character state
        } catch (error) {
            console.error('Error creating character:', error);
            alert('Failed to create character.');
        }
    };

    return (
        <>
            {/* Admin's create button for Characters */}
            {['Admin', 'ForumUser'].includes(role) && (
                <Button
                    variant="success"
                    onClick={() => setShowModal(true)}
                    className="mb-3"
                    style={{ fontSize: '14px', padding: '8px 12px' }}
                >
                    <FaPlus /> New Character
                </Button>
            )}

            {characters.length > 0 ? (
                characters.map((character) => (
                    <Col sm={12} md={6} lg={4} key={character.id} className="mb-4">
                        <Card className="h-100">
                            {/* Optional: Add an image here */}
                            <Card.Body>
                                <Card.Title>{character.title}</Card.Title>
                                <Card.Text>{character.body}</Card.Text>

                                <Button
                                    variant="info"
                                    href={`/scripts/${scriptId}/characters/${character.id}`}
                                    className="mr-2"
                                >
                                    Read More
                                </Button>
                            </Card.Body>
                        </Card>
                    </Col>
                ))
            ) : (
                <p>No characters available.</p>
            )}

            {/* Modal for creating a new character */}
            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Create New Character</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group>
                            <Form.Label>Character Name</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Enter character name"
                                value={newCharacterName}
                                onChange={(e) => setNewCharacterName(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Character Description</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                placeholder="Enter character description"
                                value={newCharacterDescription}
                                onChange={(e) => setNewCharacterDescription(e.target.value)}
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Cancel
                    </Button>
                    <Button variant="primary" onClick={handleCreateCharacter}>
                        Create Character
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default CharacterList;
