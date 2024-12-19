import React, { useState, useRef } from 'react';
import axios from 'axios';
import { Form, Button, Alert, Modal } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const LoginForm = ({ setAuthToken, handleCloseModal }) => {
  const [userName, setUserName] = useState('');
  const [userId, setUserId] = useState('');
  const [password, setPassword] = useState('');
  const [email, setEmail] = useState(''); // For registration
  const [errorMessage, setErrorMessage] = useState('');
  const [showSuccessModal, setShowSuccessModal] = useState(false); // State to control success modal visibility
  const [role, setRole] = useState(''); // State to store the role
  const [isRegistering, setIsRegistering] = useState(false); // Toggle between login and register
  const navigate = useNavigate();
  const modalRef = useRef(null);

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('https://urchin-app-6crcv.ondigitalocean.app/api/login', {
        userName,
        password,
      });

      // On successful login, store the JWT token, username, and role in localStorage
      const { accessToken, refreshToken } = response.data;

      // Decode the token to extract the role
      const decodedToken = jwtDecode(accessToken);
      
      // Extract the role from the custom claim
      const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      const role = Array.isArray(userRole) ? userRole[1] : userRole;

      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('refreshToken', refreshToken);
      localStorage.setItem('userName', userName);
	  localStorage.setItem('userId', userId);
      localStorage.setItem('role', role); // Store the role

      setRole(role);
      setAuthToken(accessToken);
      setShowSuccessModal(true); // Show the success modal
    } catch (error) {
      setErrorMessage('Invalid username or password');
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('https://urchin-app-6crcv.ondigitalocean.app/api/accounts', {
        userName,
        email,
        password,
      });
      
      alert('Registration successful! You can now log in.');
      setIsRegistering(false); // Switch back to login form after successful registration
    } catch (error) {
      setErrorMessage(error.response?.data || 'Registration failed');
    }
  };

  const handleCloseSuccessModal = () => {
    setShowSuccessModal(false); // Close the success modal
    navigate('/'); // Redirect to home page after the modal is closed
    window.location.reload(); // This will refresh the page after login
    handleCloseModal(); // Close the login modal as well
  };

  return (
    <>
      {/* Display error message if login/register fails */}
      {errorMessage && <Alert variant="danger">{errorMessage}</Alert>}

      <Form onSubmit={isRegistering ? handleRegister : handleLogin}>
        {isRegistering && (
          <Form.Group controlId="formEmail">
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </Form.Group>
        )}
        
        <Form.Group controlId="formUserName">
          <Form.Label>Username</Form.Label>
          <Form.Control
            type="text"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            required
          />
        </Form.Group>

        <Form.Group controlId="formPassword">
          <Form.Label>Password</Form.Label>
          <Form.Control
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </Form.Group>

        <Button variant="primary" type="submit" style={{ marginTop: '10px' }}>
          {isRegistering ? 'Register' : 'Login'}
        </Button>
      </Form>

      <div className="mt-3">
        <Button variant="link" onClick={() => setIsRegistering(!isRegistering)}>
          {isRegistering ? 'Already have an account? Login' : 'Don\'t have an account? Register'}
        </Button>
      </div>

      {/* Success Modal */}
      <Modal ref={modalRef} show={showSuccessModal} onHide={handleCloseSuccessModal}>
        <Modal.Header closeButton>
          <Modal.Title>Login Successful!</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>You have successfully logged in!</p>
          <p>Your role: {role}</p> {/* Display the role */}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseSuccessModal}>
            OK
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};

export default LoginForm;
