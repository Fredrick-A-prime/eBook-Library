eBook Management API Documentation
Welcome to the eBook Management API repository! This API provides functionalities for managing and interacting with eBooks, as well as user management and other features. As development progresses, the repository will be thoroughly documented to ensure clarity and ease of use.

Authentication and Authorization
Our API includes robust user authentication and authorization mechanisms to ensure secure access to resources. We currently support user registration, login, and role-based access control. Below are the details of the authentication endpoints:

1. User Registration
Endpoint: POST /api/register

Description: This endpoint allows new users to register an account. A successful registration will require a unique email address and a secure password. The password must meet the security requirements, and the user will be assigned the default role upon registration.

Request Body:

JSON

{
  "username": "string",
  "email": "string",
  "password": "string"
}
Responses:

201 Created: The user is successfully registered.
400 Bad Request: Invalid input, such as missing fields or an already existing email.
Postman Collection: Will be made available later on 

2. User Login
Endpoint: POST /api/login

Description: This endpoint allows registered users to log in to the application. Upon successful authentication, a JWT (JSON Web Token) is provided, which must be included in the Authorization header for accessing protected resources.

Request Body:

JSON 

{
  "email": "string",
  "password": "string"
}
Responses:

200 OK: The user is successfully authenticated, and a JWT is returned.
401 Unauthorized: Invalid credentials.
Postman Collection: Will be made available later on

Authorization and Roles
Our API uses role-based authorization to control access to specific resources. Users are assigned roles upon registration, and these roles determine the level of access to various endpoints. Currently, the system supports roles like "Admin", "Members", "Librarian" and "Guests".

Role Management:

Admin: Elevated privileges, including the ability to manage eBooks and user roles.

Librarian: Manages book inventory, handles book check-ins/check-outs, and assists users.

Member: Can search for books, borrow books, and manage their account details.

Guest: Limited access, mainly for browsing the catalog and viewing basic information.

Each endpoint in the API may require specific roles to access. The necessary roles and permissions will be documented alongside the respective endpoints as they are developed.

Future Plans

As we continue to develop the API, we will include detailed documentation for additional features such as eBook management, user profile management, and other functionalities. Stay tuned for updates and expanded documentation.

Note: Links to Postman collections for testing the endpoints will be provided as they become available.

Thank you for using the eBook Management API! Please refer to this documentation for all your development and testing needs.
