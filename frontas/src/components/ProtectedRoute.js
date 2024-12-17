// src/components/ProtectedRoute.js
import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ element, authToken }) => {
  if (!authToken) {
    // Redirect to login if not authenticated
    return <Navigate to="/login" />;
  }
  return element;
};

export default ProtectedRoute;
