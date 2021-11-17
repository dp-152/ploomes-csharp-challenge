# API Challenge for Ploomes Application
### Developed by Mizael Almeida

This is a simple implementation of a "chat" API. Users can register, create private chats, group chats and send messages.
#### Considerations:

 - A user cannot start a conversation with another user directly. A private chat must be created first (POST /api/chat/private)
 - A pair of users can have more than one private chat
 - User Authentication is done by the Authorization header, which should contain the token generated at registration
 - There is currently no way to recover a token once generated

## Endpoints
#### \*\* OpenAPI/Swagger data about available endpoints can be found at `/swagger/v1/swagger.json`\*\*
---
`GET /api/user/{ID}`\
Gets a user by ID

**Returns:**\
The requested user. HTTP 404 if user does not exist
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the user |
| FirstName | string | First name of the user |
| LastName | string | (optional) Last name of the user |
| Username | string | Unique username for the user |

---
`GET /api/user/username/{username}`\
Gets a user by username

 **Returns:**\
The requested user. HTTP 404 if user does not exist
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the user |
| FirstName | string | First name of the user |
| LastName | string | (optional) Last name of the user |
| Username | string | Unique username for the user |

---
`POST /api/user`\
Registers a new user

**Request body:**
| Field | Type | Description |
|--|--|--| 
| FirstName | string | First name of the user. **Max 30 characters** |
| LastName | string | (optional) Last name of the user. **Max 30 characters** |
| Username | string | Unique username for the user. **Max 30 characters** |

**Returns:**\
The newly created user.\
Location header contains the URI to the new user.
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the user |
| FirstName | string | First name of the user |
| LastName | string | (optional) Last name of the user |
| Username | string | Unique username for the user |
| AuthToken | string | Unique authorization token. **CANNOT BE RECOVERED**

---
`POST /api/message/send/{chatID}`\
Sends a message to the chat specified in `chatID`.\
Requires authentication.

**Request body:**
| Field | Type | Description |
|--|--|--| 
| MessageBody | string | Message to be sent |

**Returns:**\
The sent message.\
Location header contains the URI to the new message.

| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the message |
| ChatID | int| ID of the chat the message has been sent to |
| SenderID | int | ID of the message sender |
| MessageBody | string | Message contents |
| Created | DateTime | Timestamp for the message creation |
| LastChanged | DateTime | Timestamp for the message's last modification |

---
`GET /api/message/{ID}`\
Gets a message by ID.\
Requires authentication.\
Only retrieves messages from chats you are a member of.

**Returns:**\
The requested message. HTTP 404 if the message does not exist.

| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the message |
| ChatID | int| ID of the chat the message has been sent to |
| SenderID | int | ID of the message sender |
| MessageBody | string | Message contents |
| Created | DateTime | Timestamp for the message creation |
| LastChanged | DateTime | Timestamp for the message's last modification |

---
`GET /api/message/chat/{chatID}`\
Gets all messages for the chat specified in `chatID`.\
Requires authentication.\
Only retrieves messages from chats you are a member of.

**Returns:**\
A JSON-serialized array containing the requested messages. HTTP 404 if the chat does not exist.

| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the message |
| ChatID | int| ID of the chat the message has been sent to |
| SenderID | int | ID of the message sender |
| MessageBody | string | Message contents |
| Created | DateTime | Timestamp for the message creation |
| LastChanged | DateTime | Timestamp for the message's last modification |

---
`PATCH /api/message/{id}`\
Updates the contents of a message.\
Requires authentication.\
Only updates messages you have sent.

**Request body:**\
A JSON PATCH document containing the following fields:
| Operation | Path | Type | Description |
|--|--|--|--|
| Replace | MessageBody | string | Message contents |

**Returns:**\
HTTP 204

---
`DELETE /api/message/{id}`\
Deletes a message.\
Requires authentication.\
Only deletes messages you have sent or from groups you are an admin of.

**Returns:**\
HTTP 204

---
`GET /api/chat`\
Gets all existing chats.\
Does not contain any information about chat members.

**Returns:**\
A JSON-serialized array containing the requested chats:
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the chat |
| Title | string | Title of the chat |
| Type | string | Group or Private |

---
`GET /api/chat/{chatID}`\
Gets the chat specified by `chatID`\
Does not contain any information about chat members.

**Returns:**\
The requested chat. HTTP 404 if the chat does not exist.
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the chat |
| Title | string | Title of the chat |
| Type | string | Group or Private |

---
`DELETE /api/chat/{chatID}`\
Deletes a chat.\
Requires authentication.\
Only applies for group chats which you are an admin of.

**Returns:**\
HTTP 204

---
`POST /api/chat/private`
Creates a private chat with another user.\
Requires authentication.\
A pair of users can have more than one private chat.

**Request body:**
| Field | Type | Description |
|--|--|--| 
| SecondPartyID | int | ID of the user you want to start the chat with |

**Returns:**\
The newly created chat.\
Location header contains the URI to the new chat.
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the chat |
| Title | string | Title of the chat (\<youruser\> x \<seconduser\>) |
| Type | string | Private |

---
`POST /api/chat/group`\
Creates a group chat.\
Requires authentication.\
This will automatically add your user as an admin of the chat.\
All other members have to be added afterwards (`POST /api/chat/{chatID}/members/{memberID}`)

**Request body:**
| Field | Type | Description |
|--|--|--| 
| Title | string | Title of the chat |

**Returns:**\
The newly created chat.\
Location header contains the URI to the new chat.
| Field | Type | Description |
|--|--|--| 
| ID | int | Unique ID for the chat |
| Title | string | Title of the chat (\<youruser\> x \<seconduser\>) |
| Type | string | Private |

---
`GET /api/chat/{chatID}/members`\
Gets all members from the chat specified by `chatID`.\
Requires authentication.\
Only retrieves members from chats you are a member of.

**Returns:**\
A JSON-Serialized array containing the membership data for all members.
| Field | Type | Description |
|--|--|--| 
| ChatID | int | ID of the requested chat |
| UserID | int | ID of the member |
| IsAdmin | boolean | Whether the user is an admin of the chat or not |

---
`GET /api/chat/mine`\
Gets all chats you are currently a member of.\
Requires authentication.

**Returns:**\
A JSON-Serialized array containing the membership data for all chats.
| Field | Type | Description |
|--|--|--| 
| ChatID | int | ID of the requested chat |
| UserID | int | Your ID |
| IsAdmin | boolean | Whether you are an admin of the chat or not |

---
`POST /api/chat/{chatID}/members/{memberID}`\
Adds the user specified by `memberID` to the chat specified by `chatID`.\
Requires authentication.\
Only applies to group chats which you are an admin of.

**Request body:**\
Empty.

**Returns:**\
HTTP 204

---
`DELETE /api/chat/{chatID}/members/{memberID}`\
Removes the user specified by `memberID` from the chat specified by `chatID`.\
Requires authentication.\
Only applies to group chats which you are an admin of.

**Returns:**\
HTTP 204

---
`POST /api/chat/{chatID}/members/{memberID}/admin`\
Sets the admin status of the member specified by `memberID` in the chat specified by `chatID`.\
Requires authentication.\
Only applies to group chats which you are an admin of.

**Request body:**
| Field | Type | Description |
|--|--|--| 
| IsAdmin | boolean | "true" to grant admin permissions to the user, "false" to revoke it. |

**Returns:**\
HTTP 204

---
## Deployment
The project makes use of a user secret to store the full connection string to the database.\
Make sure you are using Microsoft SQL Server or Azure SQL, then generate a user secret by running `dotnet user-secrets init`, and add your connection string by running `dotnet user-secrets set "PloomesChallenge:ConnectionString" <your connection string>`.