# KTU Saitynų modulio projektas - Blood on the Clocktower script duomenų bazė.

# 1. Uždavinio aprašymas 
Šiame stalo žaidime yra apie 150 skirtingų veikėjų, iš kurių kiekvienas turi unikalią galią. Žaidimas yra panašus į Mafia, Town of salem ir kitus socialinės dedukcijos žaidimus. Žaidimą gali žaisti nuo 5 iki 20 žmonių. Žaidimo tikslas – geriečiams rasti žudiką demoną, o blogiečiams – išžudyti miestelį.
Rolių rinkiniai naudojami tam, kad žaidėjai žinotų, kokios rolės gali egzistuoti žaidžiamam žaidime. Kiekvienam rolių rinkinyje įprastai būna apie 25 skirtingi veikėjai, tie veikėjai visada būna 4 skirtingų tipų.
Originaliame oficialiame žaidime šiuo metu egzistuoja 3 skirtingi rolių rinkiniai, tačiau šio žaidimo žaidėjai jau ilgai kuria savo „custom“ (padarytus ranka) rinkinius tam, kad paįvairinti žaidimą, sukurti įdomių sąveikų (interactions) tarp skirtingų veikėjų, kurie oficialiuose rinkiniuose niekada nebūna kartu.

# Sistemos paskirtis 
Turėti vieną vietą, kurioje visi žmonės gali kurti, talpinti ir rasti kitų žmonių sukurtus rolių rinkinius, dalintis patarimais ir vertinti vieni kitų „kūrinius“.

# Funkciniai reikalavimai
Vienas iš funkcinių reikalavimų – galimybė valdyti visus tris resursus (script, character, comment) atitinkamai pagal savo roles. Taip pat reikia realizuoti vartotojo autentifikaciją ir autorizaciją.

# Overview
This API project is designed to manage a hierarchy of entities inspired by the board game Blood on the Clocktower. The hierarchy is structured as follows:

- Script: Represents a script, a fundamental entity that contains characters.
- Character: Represents a character within a script. Each character is associated with one script.
- Comment: Represents a comment on a character. Each comment is associated with one character.

The API allows authenticated users to interact with these entities through standard CRUD (Create, Read, Update, Delete) operations. JWT tokens are used for authorization, ensuring that only authorized users can modify or access certain resources.

# Endpoint overview
Each entity in the hierarchy has its own set of endpoints. Permissions vary based on user roles (e.g., creator, admin, or general user).

### Script endpoints
- GET /api/script: Retrieves a list of all scripts.
- GET /api/script/{id}: Retrieves a specific script by ID.
- POST /api/script: Creates a new script.
- PUT /api/script/{id}: Updates a specific script by ID.
- DELETE /api/script/{id}: Deletes a specific script by ID.
> Authorization: Only the creator or an admin can edit or delete a script. Any user can create a new script.

### Character endpoints
- GET /api/script/{script_id}/character: Retrieves a list of all characters for a specific script.
- GET /api/script/{script_id}/character/{id}: Retrieves a specific character by ID within a script.
- POST /api/script/{script_id}/character: Creates a new character within a specific script.
- PUT /api/script/{script_id}/character/{id}: Updates a specific character by ID within a script.
- DELETE /api/script/{script_id}/character/{id}: Deletes a specific character by ID within a script.
> Authorization: Only the original creator of the script or an admin can add characters. Only the creator or an admin can edit or delete characters.

### Comment endpoints
- GET /api/script/{script_id}/character/{character_id}/comment: Retrieves a list of all comments for a specific character.
- GET /api/script/{script_id}/character/{character_id}/comment/{id}: Retrieves a specific comment by ID within a character.
- POST /api/script/{script_id}/character/{character_id}/comment: Creates a new comment for a specific character.
- PUT /api/script/{script_id}/character/{character_id}/comment/{id}: Updates a specific comment by ID within a character.
- DELETE /api/script/{script_id}/character/{character_id}/comment/{id}: Deletes a specific comment by ID within a character.
> Authorization: Only the creator of the comment or an admin can edit or delete a comment. Any authenticated user can create a comment.

# Authentication and Authorization
This API uses JWT (JSON Web Tokens) for authentication and authorization. When a user logs in, they receive a token which must be included in the headers of requests to access protected endpoints.

## Roles and Permissions
### General User:
Can create a new script.
Can add comments to characters in any script.

### Script Creator:
> This is not an actual role, it's just mentioned here for the purposes of explaining what a user can't do on another users work.
Can edit and delete their own scripts.
Can add, edit, and delete characters in their own scripts.
Can add, edit, and delete their own comments on characters.

### Admin:
Has full permissions over all scripts, characters, and comments.
Can edit and delete any script, character, or comment.

# Response Codes
- 200 OK: The request was successful.
- 201 Created: The resource was successfully created.
- 204 Created : The resource was created and returned.
- 400 Bad Request: The request was invalid.
- 401 Unauthorized: The request did not include valid authentication credentials.
- 403 Forbidden: The user does not have permission to access the resource.
- 404 Not Found: The specified resource was not found.
- 500 Internal Server Error: An error occurred on the server.
> Error 500 should not be found in this project, otherwise it's a mistake.


# 2. Technologijų aprašymas

# 3. Naudotojo sąsaja
## 3.1 Pagrindinis puslapis
### Wireframe
![image (1)](https://github.com/user-attachments/assets/8d823ec5-3998-4c2a-a44e-a73444876f21)

### Realus pavyzdys
![image](https://github.com/user-attachments/assets/fb8043a9-1e45-4a7b-94c6-0427b260d145)

## 3.2 Atitinkamo CHARACTER puslapis
### Wireframe
![image (2)](https://github.com/user-attachments/assets/dadf9657-3088-4608-99ad-fad202f1b13c)

### Realus pavyzdys
![image](https://github.com/user-attachments/assets/06e375c1-93db-4086-8c39-9801a1a8d14c)

## 3.3 Prisijungimo forma
### Wireframe
![image (4)](https://github.com/user-attachments/assets/2108fdeb-9e5d-417f-84fe-5827abc8fa51)

Panašaus principo wireframe pritaikomas ir registracijai, tačiau atsiranda papildomas email langas.
### Realus pavyzdys (prisijungimas)
![image](https://github.com/user-attachments/assets/f3b069da-5ced-41f0-8c43-6c38c292b9a7)

### Realus pavyzdys (registracija)
![image](https://github.com/user-attachments/assets/f50d7377-3987-4e78-93d5-22b902758467)

## 3.4 SCRIPT, CHARACTER kūrimo forma
### Wireframe
![image (3)](https://github.com/user-attachments/assets/63fb2e07-e8a0-4936-bc42-d2b2febb5399)

Panašaus principo wireframe pritaikomas ir kitiems kūrumo/redagavimo langams
### Realus pavyzdys (kuriamas naujas SCRIPT)
![image](https://github.com/user-attachments/assets/a496f92a-1ce7-48ee-8e24-99b2fc154aca)

### Realus pavyzdys (redaguojamas SCRIPT)
![image](https://github.com/user-attachments/assets/ee7c1154-a706-437c-85d3-4fe50d6a1b25)

### Realus pavyzdys (kuriamas naujas CHARACTER)
![image](https://github.com/user-attachments/assets/d06005f1-2889-4574-a2d1-d99c85add22e)

### Realus pavyzdys (redaguojamas CHARACTER)
![image](https://github.com/user-attachments/assets/425eb310-d554-408d-8bff-16bac2892aad)

# 4 API Specifikacija

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


# 5. Išvados
1. Sukurtas forum API pasinaudojant REST principais su .NET
2. Duomenų bazei panaudotas PostgresSQL.
3. Klientinei daliai sukurti panaudotas React.js
4. Autorizacijai panaudoti JWT tokenai.
5. Kliento ir serverio dalys yra pasiekiamos per debesis.
6. Pateikta detali ataskaita.



