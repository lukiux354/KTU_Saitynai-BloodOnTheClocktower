import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Button, Row, Col, Modal, Form } from 'react-bootstrap';
import { FaEdit, FaTrashAlt, FaPlus } from 'react-icons/fa';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import CharacterList from './CharacterList';
import '../App.css'; // Add CSS for animations
import '../animations.css';

const ScriptList = () => {
    const [scripts, setScripts] = useState([]);
    const [selectedScript, setSelectedScript] = useState(null);
    const [role, setRole] = useState(localStorage.getItem('role'));
	const [userName, setUserName] = useState(localStorage.getItem('userName'));
	const [userId, setUserId] = useState(localStorage.getItem('userId'));
    const [showModal, setShowModal] = useState(false);
    const [newScriptName, setNewScriptName] = useState('');
    const [newScriptDescription, setNewScriptDescription] = useState('');
    const [editingScript, setEditingScript] = useState(null); // New state for editing
    const [showEditModal, setShowEditModal] = useState(false); // New modal state

    useEffect(() => {
        const fetchScripts = async () => {
            try {
                const response = await axios.get('https://urchin-app-6crcv.ondigitalocean.app/api/scripts');
                setScripts(response.data);
            } catch (error) {
                console.error('Error fetching scripts:', error);
            }
        };
        fetchScripts();
    }, []);

    const handleScriptClick = (script) => setSelectedScript(script);

    const handleCreateScript = async () => {
        if (!newScriptName || newScriptName.length > 20) {
            alert('Script name must be between 1 and 20 characters.');
            return;
        }

        if (!newScriptDescription || newScriptDescription.length > 100) {
            alert('Script description must be between 1 and 100 characters.');
            return;
        }

        try {
            const token = localStorage.getItem('accessToken');
            const response = await axios.post(
                'https://urchin-app-6crcv.ondigitalocean.app/api/scripts',
                { title: newScriptName, description: newScriptDescription },
                { headers: { Authorization: `Bearer ${token}` } }
            );
            alert('Script created successfully!');
            setScripts((prevScripts) => [...prevScripts, response.data]);
            setShowModal(false);
            setNewScriptName('');
            setNewScriptDescription('');
        } catch (error) {
            console.error('Error creating Script:', error);
            alert('Failed to create Script.');
        }
    };

    const handleEditScript = (script) => {
        setEditingScript(script); // Set meal to edit
        setShowEditModal(true); // Open edit modal
    };

    const handleDeleteScript = async (scriptId) => {
        if (window.confirm('Are you sure you want to delete this script?')) {
            try {
                const token = localStorage.getItem('accessToken');
                await axios.delete(`https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                alert('Script deleted successfully!');
                setScripts((prevScripts) => prevScripts.filter((script) => script.id !== scriptId));
            } catch (error) {
                console.error('Error deleting script:', error);
                alert('Failed to delete script.');
            }
        }
      };

    return (
        <div className="container mt-3">
            {/* Script List */}
            <div className="row">
                <div className="col-md-4">
                    <h2 className="text-pink">Script list</h2>
                    {(role === 'Admin' || role === 'ForumUser') && (
                        <Button
                            variant="success"
                            onClick={() => setShowModal(true)}
                            className="mb-3"
                            style={{ fontSize: '14px', padding: '8px 12px' }}
                        >
                            <FaPlus /> New Script
                        </Button>
                    )}
                    <TransitionGroup className="list-group">
                        {scripts.map((script) => (
                            <CSSTransition key={script.id} timeout={300} classNames="script">
                                <Card
                                    className="mb-3 shadow-sm"
                                    onClick={() => handleScriptClick(script)}
                                    style={{ cursor: 'pointer', backgroundColor: '#f8f9fa' }}
                                >
                                    <Card.Body>
                                        <div className="d-flex justify-content-between align-items-center">
                                            <Card.Title className="text-coral">{script.title}</Card.Title>
                                            {(true) && (
                                                <div>
                                                    <Button
                                                        variant="warning"
                                                        size="sm"
                                                        onClick={(e) => {
                                                            e.stopPropagation(); // Prevent triggering the Card's onClick
                                                            handleEditScript(script);
                                                        }}
                                                        style={{ marginLeft: '10px' }}
                                                    >
                                                        <FaEdit />
                                                    </Button>
                                                    <Button
                                                        variant="danger"
                                                        size="sm"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            handleDeleteScript(script.id);
                                                        }}
                                                        style={{ marginLeft: '5px' }}
                                                    >
                                                        <FaTrashAlt />
                                                    </Button>
                                                </div>
                                            )}
                                        </div>
                                    </Card.Body>
                                </Card>
                            </CSSTransition>
                        ))}
                    </TransitionGroup>
                </div>

                {/* Details Section */}
                <div className="col-md-8">
                    {selectedScript ? (
                        <div>
                            <h2>{selectedScript.name}</h2>
                            <p>{selectedScript.description}</p>
                            <h4>Characters:</h4>
                            <Row>
                                <CharacterList scriptId={selectedScript.id} />
                            </Row>
                        </div>
                    ) : (
                        <div>
                            <p>Select a script to view characters.</p>
                            <img
                            //    src="cute.png"
                            //    alt="Basic script"
                            //    className="cute-image"
                            //    style={{ marginTop: '20px' }}
                            />
                        </div>
                    )}
                </div>
            </div>

            {/* Create Script Modal */}
            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Create New Script</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group>
                            <Form.Label>Script Name</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Enter script name"
                                value={newScriptName}
                                onChange={(e) => setNewScriptName(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Script Description</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                placeholder="Enter script description"
                                value={newScriptDescription}
                                onChange={(e) => setNewScriptDescription(e.target.value)}
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Cancel
                    </Button>
                    <Button variant="primary" onClick={handleCreateScript}>
                        Create Script
                    </Button>
                </Modal.Footer>
            </Modal>

            {/* Edit Script Modal */}
            <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Edit Script</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mt-3">
                            <Form.Label>Script Description</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                placeholder="Enter script description"
                                value={editingScript?.description || ''}
                                onChange={(e) =>
                                    setEditingScript((prev) => ({ ...prev, description: e.target.value }))
                                }
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowEditModal(false)}>
                        Cancel
                    </Button>
                    <Button
                        variant="primary"
                        onClick={async () => {
                            try {
                                const token = localStorage.getItem('accessToken');
                                await axios.put(
                                    `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${editingScript.id}`,
                                    editingScript,
                                    { headers: { Authorization: `Bearer ${token}` } }
                                );
                                setScripts((prevScripts) =>
                                    prevScripts.map((script) =>
                                        script.id === editingScript.id ? editingScript : script
                                    )
                                );
                                alert('Script updated successfully!');
                                setShowEditModal(false);
                            } catch (error) {
                                console.error('Error updating script:', error);
                                alert('Failed to update script.');
                            }
                        }}
                    >
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default ScriptList;
