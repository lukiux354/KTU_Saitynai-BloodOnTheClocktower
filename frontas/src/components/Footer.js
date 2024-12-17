import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { FaFacebook, FaTwitter, FaInstagram, FaLinkedin } from 'react-icons/fa'; // Importing icons from react-icons
import '../Footer.css'; // Add a CSS file for styling if needed
import { Fa0, FaArrowUpRightDots } from 'react-icons/fa6';

const Footer = () => {
    return (
        <footer className="footer bg-dark text-white py-3 mt-auto">
            <Container>
                <Row className="justify-content-between">
                    {/* Social Media Links */}
                    <Col md={3} className="text-center">
                        <h5>Follow Us</h5>
                        <ul className="social-links list-unstyled d-flex justify-content-center">
                            <li className="mx-3">
                                <a href="https://www.facebook.com" target="_blank" rel="noopener noreferrer">
                                    <FaFacebook size={30} className="social-icon" />
                                </a>
                            </li>
                            <li className="mx-3">
                                <a href="https://www.twitter.com" target="_blank" rel="noopener noreferrer">
                                    <FaTwitter size={30} className="social-icon" />
                                </a>
                            </li>
                            <li className="mx-3">
                                <a href="https://www.instagram.com" target="_blank" rel="noopener noreferrer">
                                    <FaInstagram size={30} className="social-icon" />
                                </a>
                            </li>
                            <li className="mx-3">
                                <a href="https://www.linkedin.com" target="_blank" rel="noopener noreferrer">
                                    <FaLinkedin size={30} className="social-icon" />
                                </a>
                            </li>
                            <li className="mx-3">
                                <a href="http://localhost:3000" target="_blank" rel="noopener noreferrer">
                                    <FaArrowUpRightDots size={30} className="social-icon" />
                                </a>
                            </li>
                        </ul>
                    </Col>

                    {/* Contact Information */}
                    <Col md={3} className="text-center">
                        <h5>Contact Us</h5>
                        <p>Email: contact@botc.com</p>
                    </Col>
                </Row>
            </Container>
        </footer>
    );
};

export default Footer;
