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

## API Specification

### GET /api/scripts

Fetches a list of scripts.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:

```http
GET http://localhost:5000/api/scripts
```

### Response

```http
Status 200
[
    {
        "id": 5,
        "title": "Trouble Brewing",
        "description": "This is an original BOTC script. For beginners",
        "createdOn": "2024-12-17T17:06:05.035702+00:00"
    },
    {
        "id": 10,
        "title": "Bad Moon Rising",
        "description": "More advanced script where death is information. Recommended for intermediate and expert players.",
        "createdOn": "2024-12-19T18:10:48.304944+00:00"
    },
    {
        "id": 11,
        "title": "Sects & Violets",
        "description": "More advanced script with madness and a lot of misinformation. Recommended for intermediate players.",
        "createdOn": "2024-12-19T18:11:59.753005+00:00"
    }
]
```

---

### GET /api/scripts/{scriptId}

Fetches details for a specific script.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:

```http
GET http://localhost:5000/api/scripts/10
```

### Response

```http
Status 200
{
    "id": 10,
    "title": "Bad Moon Rising",
    "description": "More advanced script where death is information. Recommended for intermediate and expert players.",
    "createdOn": "2024-12-19T18:10:48.304944+00:00"
}
```

---

### POST /api/scripts

Creates a new script.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Body:

```http
{
  "title": "Testing",
  "description": "Third script."
}
```

### Example Request:

```http
POST http://localhost:5000/api/scripts
```

### Response

```http
Status 201
{
    "id": 12,
    "title": "Testing",
    "description": "Third script.",
    "createdOn": "2024-12-19T19:39:35.3378733+00:00"
}
```

---

### PUT /api/scripts/{scriptId}

Updates the description of an existing script.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Body:

```http
{
  "description": "Updated for demo.."
}
```

### Example Request:

```http
PUT http://localhost:5000/api/scripts/32
```

### Response

```http
Status 200
{
    "id": 12,
    "title": "Testing",
    "description": "Updated for demo..",
    "createdOn": "2024-12-19T19:39:35.337873+00:00"
}
```

---

### DELETE /api/scripts/{scriptId}

Deletes a specific script.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Example Request:

```http
DELETE http://localhost:5000/api/scripts/12
```

### Response

```http
Status 204
```

---

## Recipies

### GET /api/scripts/{scriptId}/characters

Fetches a list of characters.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:

```http
GET http://localhost:5000/api/scripts/10/characters
```

### Response

```http
Status 200
[
    {
        "id": 30,
        "title": "Lunatic",
        "body": "You think you are a Demon, but you are not. The Demon knows who you are & who you choose at night.",
        "createdOn": "2024-12-19T18:13:18.407073+00:00"
    },
    {
        "id": 31,
        "title": "Tinker",
        "body": "You might die at any time.",
        "createdOn": "2024-12-19T18:13:31.464228+00:00"
    },
    {
        "id": 32,
        "title": "Pukka",
        "body": "Each night, choose a player: they are poisoned. The previously poisoned player dies then becomes healthy.",
        "createdOn": "2024-12-19T18:13:48.31974+00:00"
    }
]
```

---

### GET /api/scripts/{scriptId}/characters/{characterId}

Fetches details for a specific character.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:

```http
GET http://localhost:5000/api/scripts/10/characters/32
```

### Response

```http
Status 200
{
    "id": 32,
    "title": "Pukka",
    "body": "Each night, choose a player: they are poisoned. The previously poisoned player dies then becomes healthy.",
    "createdOn": "2024-12-19T18:13:48.31974+00:00"
}
```

---

### POST  /api/scripts/{scriptId}/characters

Creates a new character.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Body:

```http
{
  "title": "Empath",
  "body": "Neighbors evil SECOND"
}
```

### Example Request:

```http
POST http://localhost:5000/api/scripts/{scriptId}/characters
```

### Response

```http
Status 201
{
    "id": 33,
    "title": "Empath",
    "body": "Neighbors evil SECOND",
    "createdOn": "2024-12-19T19:49:01.2639625+00:00"
}
```

---

### PUT  /api/scripts/{scriptId}/characters/{characterId}

Updates the description of an existing character.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Body:

```http
{
  "body": "This role doesnt exist."
}
```

### Example Request:

```http
PUT http://localhost:5000/api/scripts/10/characters/33
```

### Response

```http
Status 200
{
    "id": 33,
    "title": "Empath",
    "body": "This role doesnt exist.",
    "createdOn": "2024-12-19T19:49:01.263962+00:00"
}
```

---

### DELETE  /api/scripts/{scriptId}/characters/{characterId}

Deletes a specific character.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Example Request:

```http
DELETE http://localhost:5000/api/scripts/10/characters/33
```

### Response

```http
Status 204
```

---

## Comments

### GET /api/scripts/{scriptId}/characters/{characterId}/comments

Fetches a list of comments.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:

```http
GET http://localhost:5000/api/scripts/5/characters/7/comments
```

### Response

```http
Status 200
[
    {
        "id": 5,
        "content": "I think this character is pretty boring..",
        "createdOn": "2024-12-17T23:29:11.483055+00:00"
    },
    {
        "id": 6,
        "content": "Classic",
        "createdOn": "2024-12-17T23:29:19.386892+00:00"
    }
]
```

---

### GET /api/scripts/{scriptId}/characters/{characterId}/comments/{commentId}

Fetches details for a specific comment.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:

```http
GET http://localhost:5000/api/scripts/5/characters/7/comments/5
```

### Response

```http
Status 200
{
   "id": 5,
    "content": "I think this character is pretty boring..",
    "createdOn": "2024-12-17T23:29:11.483055+00:00"
}
```

---

### POST  /api/scripts/{scriptId}/characters/{characterId}/comments

Creates a new comment.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Body:

```http
{
  "content": "Demo comment 2"
}
```

### Example Request:

```http
POST http://localhost:5000/api/scripts/5/characters/7/comments
```

### Response

```http
Status 201
{
    "id": 8,
    "content": "Demo comment 2",
    "createdOn": "2024-12-19T20:02:00.8110099+00:00"
}
```

---

### PUT  /api/scripts/{scriptId}/characters/{characterId}/comments/{commentId}

Updates the description of an existing comment.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Body:

```http
{
  "content": "I love this character.... Not really"
}
```

### Example Request:

```http
PUT http://localhost:5000/api/scripts/5/characters/7/comments/8
```

### Response

```http
Status 200
{
    "id": 8,
    "content": "I love this character.... Not really",
    "creationDate": "2024-12-19T20:02:00.8110099+00:00"
}
```

---

### DELETE  /api/scripts/{scriptId}/characters/{characterId}/comments/{commentId}

Deletes a specific comment.

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | Yes       |

### Example Request:

```http
DELETE http://localhost:5000/api/characters/5/recipies/7/comments/8
```

### Response

```http
Status 204
```

---

## Autorizacija

### POST api/accounts

Registers new user

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Body
```http
{
    "userName": "lukas",
    "Email": "luksonas@email.com",
    "Password": "Lukas1?sunkus"
}
```

### Example Request:
```http
POST http://localhost:5000/api/accounts
```

### Response

```http
Status 201
```

---

### POST api/login

Logs a user in

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Body
```http
{
    "userName": "lukas",
    "Password": "Lukas1?sunkus"
}
```

### Example Request:
```http
POST http://localhost:5000/api/login
```

### Response

```http
Status 200
{
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibHVrYXNuYXZpY2thcyIsImp0aSI6ImQxMjc3ZjFmLWQyYTUtNGYyOC04NjdiLWQxNjhkN2QzMGM0OSIsInN1YiI6IjFhM2NiM2E4LTBiZGYtNDJkNi04MjkwLWJkM2M4OTE1MDM1MCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkZvcnVtVXNlciIsImV4cCI6MTczNDY0MDAxMywiaXNzIjoiTHVrYXMiLCJhdWQiOiJUcnVzdGVkQ2xpZW50In0.mzOwPzG1LzgpkNzSL0tNI6GWZtOk41HxbF0ibMvMrS4"
}
```

---

### POST api/logout

Logs a user out

### Resource Information:

| Property                | Value     |
|-------------------------|-----------|
| Response format         | JSON      |
| Requires authentication | No        |

### Example Request:
```http
POST http://localhost:5000/api/logout
```

### Response

```http
Status 200
```


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
