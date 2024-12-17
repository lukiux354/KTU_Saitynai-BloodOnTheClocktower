import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import { Modal, Button } from "react-bootstrap";
import "./CharacterDetails.css"; // Import the CSS file

const CharacterDetail = () => {
  const { scriptId, characterId } = useParams();
  const [character, setCharacter] = useState(null);
  const [userNamee, setUserNamee] = useState(localStorage.getItem('userName'));
  const [userId, setUserId] = useState(localStorage.getItem('userId'));
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState("");
  const [showEditModal, setShowEditModal] = useState(false); // State for modal visibility
  const [updatedDescription, setUpdatedDescription] = useState(""); // State for new description
  const userName = localStorage.getItem("userName") || "Anonymous";
  const userRole = localStorage.getItem("role");

  useEffect(() => {
    const fetchCharacter = async () => {
      try {
		console.log(`Fetching character for scriptId: ${scriptId}, characterId: ${characterId}`);
        const response = await axios.get(
          `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}`
        );
        setCharacter(response.data);
        setUpdatedDescription(response.data.description); // Initialize with current description

        const commentsResponse = await axios.get(
          `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments`
        );
        setComments(commentsResponse.data);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchCharacter();
  }, [scriptId, characterId]);

  const handleAddComment = async (e) => {
    e.preventDefault();
    try {
      const token = localStorage.getItem("accessToken"); // Retrieve the JWT token
      if (!token) {
        alert("You must be logged in to add a comment.");
        return;
      }

      const response = await axios.post(
        `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments`,
        {
          content: newComment,
          username: userName, // Replace with actual user data
          avatar: "https://via.placeholder.com/50", // Replace with actual user avatar URL
        },
        {
          headers: {
            Authorization: `Bearer ${token}`, // Include the token in the request
          },
        }
      );

      setComments([...comments, response.data]); // Add the new comment to the state
      setNewComment(""); // Clear the input field
    } catch (error) {
      console.error("Error adding comment:", error.response?.data || error.message);
      alert("Failed to add comment. Please try again.");
    }
  };

  const handleDeleteComment = async (commentId) => {
    try {
      const token = localStorage.getItem('accessToken');
      await axios.delete(
        `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}/comments/${commentId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      // Update state to remove the deleted comment
      setComments((prevComments) => prevComments.filter((comment) => comment.id !== commentId));
    } catch (error) {
      console.error("Error deleting comment:", error.response?.data || error.message);
      alert("Failed to delete the comment. Please try again.");
    }
  };

  const handleDeleteCharacter = async () => {
    if (window.confirm("Are you sure you want to delete this character?")) {
      try {
        const token = localStorage.getItem("accessToken");
        await axios.delete(
          `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        alert("Character deleted successfully!");
        window.location.href = `/`;
      } catch (error) {
        console.error("Error deleting character:", error.response?.data || error.message);
        alert("Failed to delete the character.");
      }
    }
  };

  const handleEditCharacter = async () => {
    try {
      const token = localStorage.getItem("accessToken");
      await axios.put(
        `https://urchin-app-6crcv.ondigitalocean.app/api/scripts/${scriptId}/characters/${characterId}`,
        { body: updatedDescription },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Character updated successfully!");
      setCharacter((prev) => ({ ...prev, body: updatedDescription }));
      setShowEditModal(false); // Close the modal
    } catch (error) {
      console.error("Error updating character:", error.response?.data || error.message);
      alert("Failed to update the character.");
    }
  };

  return (
    <div className="container mt-3">
      {character ? (
        <>
          <h2 className="mb-3">{character.title}</h2>
          <p>{character.body}</p>

          {/* Conditional rendering for Edit/Delete buttons */}
          {(userRole === "Admin" || userRole === "ForumUser") && (
            <div className="mb-3">
              <button
                className="btn btn-warning me-2"
                onClick={() => setShowEditModal(true)}
              >
                Edit Character
              </button>
              <button className="btn btn-danger" onClick={handleDeleteCharacter}>
                Delete Character
              </button>
            </div>
          )}

          {/* Edit Modal */}
          <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
            <Modal.Header closeButton>
              <Modal.Title>Edit Character</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <textarea
                className="form-control"
                rows="5"
                value={updatedDescription}
                onChange={(e) => setUpdatedDescription(e.target.value)}
              />
            </Modal.Body>
            <Modal.Footer>
              <Button variant="secondary" onClick={() => setShowEditModal(false)}>
                Cancel
              </Button>
              <Button variant="primary" onClick={handleEditCharacter}>
                Save Changes
              </Button>
            </Modal.Footer>
          </Modal>

          <h3 className="mt-4">Comments</h3>
          <div className="comments-section">
            <ul className="list-unstyled mb-4">
              {comments.map((comment) => (
                <li key={comment.id} className="d-flex align-items-start mb-3">
                  <img
                  //  src={comment.avatar || "https://via.placeholder.com/50"}
                  //  alt={`${comment.username}'s avatar`}
                  //  className="rounded-circle me-3"
                  //  width="50"
                  //  height="50"
                  />
                  <div>
                    <strong>{comment.userId || "Anonymous"}</strong>
                    <p className="mb-1">{comment.content}</p>

                    {/* Show delete button for admin/forum */}
                    {(userRole === "Admin" || userRole === "ForumUser") && (
                      <button
                        className="btn btn-danger btn-sm"
                        onClick={() => handleDeleteComment(comment.id)}
                      >
                        Delete
                      </button>
                    )}
                  </div>
                </li>
              ))}
            </ul>

            {/* Comment form */}
            <form onSubmit={handleAddComment} className="mb-3">
              <div className="mb-3">
                <label htmlFor="newComment" className="form-label">
                  Add a comment
                </label>
                <input
                  type="text"
                  id="newComment"
                  className="form-control"
                  value={newComment}
                  onChange={(e) => setNewComment(e.target.value)}
                  required
                  disabled={!localStorage.getItem("accessToken")} // Disable comment form if not logged in
                />
              </div>
              <button type="submit" className="btn btn-primary" disabled={!localStorage.getItem("accessToken")}>
                Add Comment
              </button>
            </form>
          </div>
        </>
      ) : (
        <p>Loading...</p>
      )}
    </div>
  );
};

export default CharacterDetail;
