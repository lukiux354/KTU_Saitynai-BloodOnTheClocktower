# Blood on the Clocktower Script Database API

## Summary

This project is a web-based application designed for the social deduction board game *Blood on the Clocktower*. The game features approximately 150 different characters, each with unique abilities, and is played by 5 to 20 people. The core objective is for the "good" team to find the demon, while the "evil" team aims to eliminate the townsfolk.

To manage the complexity of the game, players use "scripts"—curated lists of characters that can appear in a specific game session. A standard script typically contains about 25 characters of four different types. While the official game offers three base scripts, the community actively creates custom scripts to introduce new interactions and variety.

**System Purpose:**
The goal of this system is to provide a centralized platform where users can create, store, and discover custom role scripts, share advice, and rate each other's creations.

## Functional Requirements

The system is built to manage three core resources:
1.  **Scripts**
2.  **Characters**
3.  **Comments**

A key functional requirement is role-based access control, allowing users to manage these resources according to their permissions. The system implements secure user authentication and authorization using JWT tokens.

## Overview

This API manages a hierarchical structure of entities:

* **Script:** The fundamental entity representing a collection of characters.
* **Character:** Represents a specific game role. Each character belongs to one script.
* **Comment:** Represents user feedback on a specific character.

The API exposes standard CRUD (Create, Read, Update, Delete) operations for these entities.

## Technology Stack

* **Frontend:** React.js
* **Backend:** .NET Core Web API
* **Database:** PostgreSQL
* **Authentication:** JSON Web Tokens (JWT)
* **Architecture:** RESTful API

**System Architecture:**
![System Architecture](https://github.com/user-attachments/assets/31dbeb99-3d02-4cb2-a821-104499a22425)

## Endpoint Overview

Permissions vary based on user roles (General User, Creator, Admin).

### Script Endpoints
* `GET /api/script`: Retrieve all scripts.
* `GET /api/script/{id}`: Retrieve a specific script.
* `POST /api/script`: Create a new script.
* `PUT /api/script/{id}`: Update a script.
* `DELETE /api/script/{id}`: Delete a script.
> **Authorization:** Any user can create a script. Only the creator or an admin can edit/delete it.

### Character Endpoints
* `GET /api/script/{script_id}/character`: Retrieve characters for a script.
* `GET /api/script/{script_id}/character/{id}`: Retrieve a specific character.
* `POST /api/script/{script_id}/character`: Add a character to a script.
* `PUT /api/script/{script_id}/character/{id}`: Update a character.
* `DELETE /api/script/{script_id}/character/{id}`: Remove a character.
> **Authorization:** Only the script creator or an admin can add, edit, or delete characters within that script.

### Comment Endpoints
* `GET /api/script/{script_id}/character/{character_id}/comment`: Retrieve comments for a character.
* `GET /api/script/{script_id}/character/{character_id}/comment/{id}`: Retrieve a specific comment.
* `POST /api/script/{script_id}/character/{character_id}/comment`: Add a comment.
* `PUT /api/script/{script_id}/character/{character_id}/comment/{id}`: Update a comment.
* `DELETE /api/script/{script_id}/character/{character_id}/comment/{id}`: Delete a comment.
> **Authorization:** Any authenticated user can comment. Only the comment creator or an admin can edit/delete it.

## Authentication & Authorization

The API uses **JWT (JSON Web Tokens)**. A valid token must be included in the request header to access protected endpoints.

### Roles & Permissions
* **General User:** Can create scripts and comment on any character.
* **Script Creator:** (Contextual role) Can manage their own scripts and characters.
* **Admin:** Has full control to edit or delete any resource across the system.

### Response Codes
* **200 OK:** Request successful.
* **201 Created:** Resource created.
* **204 No Content:** Request processed successfully (often for delete).
* **400 Bad Request:** Invalid request.
* **401 Unauthorized:** Invalid or missing authentication.
* **403 Forbidden:** Insufficient permissions.
* **404 Not Found:** Resource not found.
* **500 Internal Server Error:** Server-side error.

## User Interface

### 1. Main Page
**Wireframe:**
![Main Page Wireframe](https://github.com/user-attachments/assets/8d823ec5-3998-4c2a-a44e-a73444876f21)

**Implementation:**
![Main Page Real](https://github.com/user-attachments/assets/fb8043a9-1e45-4a7b-94c6-0427b260d145)

### 2. Character Page
**Wireframe:**
![Character Page Wireframe](https://github.com/user-attachments/assets/dadf9657-3088-4608-99ad-fad202f1b13c)

**Implementation:**
![Character Page Real](https://github.com/user-attachments/assets/06e375c1-93db-4086-8c39-9801a1a8d14c)

### 3. Authentication
**Wireframe:**
![Login Wireframe](https://github.com/user-attachments/assets/2108fdeb-9e5d-417f-84fe-5827abc8fa51)

**Login Implementation:**
![Login Real](https://github.com/user-attachments/assets/f3b069da-5ced-41f0-8c43-6c38c292b9a7)

**Register Implementation:**
![Register Real](https://github.com/user-attachments/assets/f50d7377-3987-4e78-93d5-22b902758467)

### 4. Creation Forms
**Wireframe:**
![Creation Form Wireframe](https://github.com/user-attachments/assets/63fb2e07-e8a0-4936-bc42-d2b2febb5399)

**Create Script:**
![Create Script Real](https://github.com/user-attachments/assets/a496f92a-1ce7-48ee-8e24-99b2fc154aca)

**Edit Script:**
![Edit Script Real](https://github.com/user-attachments/assets/ee7c1154-a706-437c-85d3-4fe50d6a1b25)

**Create Character:**
![Create Character Real](https://github.com/user-attachments/assets/d06005f1-2889-4574-a2d1-d99c85add22e)

**Edit Character:**
![Edit Character Real](https://github.com/user-attachments/assets/425eb310-d554-408d-8bff-16bac2892aad)

## API Specification Examples

### Scripts
* `GET /api/scripts` - List all scripts.
* `GET /api/scripts/{id}` - Get script details.
* `POST /api/scripts` - Create a script.
    ```json
    {
      "title": "Testing",
      "description": "Third script."
    }
    ```

### Characters
* `GET /api/scripts/{id}/characters` - List characters in a script.
* `POST /api/scripts/{id}/characters` - Add a character.
    ```json
    {
      "title": "Empath",
      "body": "Neighbors evil SECOND"
    }
    ```

### Auth
* `POST /api/accounts` - Register a new user.
* `POST /api/login` - Log in and receive a JWT token.

*(See full documentation for all endpoints and response bodies)*

## Conclusions

1.  A robust forum/database API was developed using **REST principles** and **.NET Core**.
2.  **PostgreSQL** was successfully integrated as the persistence layer.
3.  A reactive client-side application was built using **React.js**.
4.  Secure authorization was implemented using **JWT tokens**.
5.  Both client and server components are deployed and accessible via the cloud.
6.  Comprehensive documentation and reporting were provided.

## Installation

To run this project locally:

1.  **Backend:**
    * Navigate to the backend solution folder.
    * Update `appsettings.json` with your PostgreSQL connection string.
    * Run migrations: `dotnet ef database update`.
    * Start the API: `dotnet run`.

2.  **Frontend:**
    * Navigate to the frontend folder.
    * Install dependencies: `npm install`.
    * Start the development server: `npm start`.

## License

This project is provided for educational and demonstration purposes.
