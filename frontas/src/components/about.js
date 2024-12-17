import React, { useState } from 'react';
import { Container, Form, Button } from 'react-bootstrap';
import aboutUsImage from './AboutUs.jpg'; // Update the path accordingly

const About = () => {
  const [formData, setFormData] = useState({
    textField: '',
    numberField: '',
    dateField: '',
  });

  const [submittedData, setSubmittedData] = useState(null);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmittedData(formData);
    setFormData({
      textField: '',
      numberField: '',
      dateField: '',
    });
  };

  return (
    <div className="about-page">
      <Container>
        <h2>About Us</h2>
        <img
          src={aboutUsImage}
          alt="About Us"
          style={{
            maxWidth: '25%',
            height: 'auto',
            display: 'block',
            margin: '20px auto',
          }}
        />
        <p>
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla.
        </p>
        <p>
          <strong>Our Mission:</strong> Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.
        </p>
        <p>
          <strong>Our Vision:</strong> At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga.
        </p>
        <p>
          <strong>Contact Information:</strong> You can reach us at email@example.com. We are always happy to hear from you!
        </p>

        {/* Form for different field types */}
        <h3>Form Submission</h3>
        <Form onSubmit={handleSubmit}>
          <Form.Group controlId="formTextField">
            <Form.Label>Text Field</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter some text"
              name="textField"
              value={formData.textField}
              onChange={handleChange}
              required
            />
          </Form.Group>

          <Form.Group controlId="formNumberField">
            <Form.Label>Number Field</Form.Label>
            <Form.Control
              type="number"
              placeholder="Enter a number"
              name="numberField"
              value={formData.numberField}
              onChange={handleChange}
              required
            />
          </Form.Group>

          <Form.Group controlId="formDateField">
            <Form.Label>Date Field</Form.Label>
            <Form.Control
              type="date"
              name="dateField"
              value={formData.dateField}
              onChange={handleChange}
              required
            />
          </Form.Group>

          <Button variant="primary" type="submit">
            Submit
          </Button>
        </Form>

        {/* Display submitted data below */}
        {submittedData && (
          <div className="submitted-data mt-4">
            <h4>Submitted Data:</h4>
            <p><strong>Text Field:</strong> {submittedData.textField}</p>
            <p><strong>Number Field:</strong> {submittedData.numberField}</p>
            <p><strong>Date Field:</strong> {submittedData.dateField}</p>
          </div>
        )}
      </Container>
    </div>
  );
};

export default About;
