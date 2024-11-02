<body>
    <h1>Learning Code - Authentication API 🔒</h1>

 <p>This API is part of the <strong>Learning Code</strong> microservice, an e-learning software. It is responsible for user authentication in the system, including user creation, login, generation and renewal of <em>JWT</em> and <em>Refresh Tokens</em>, deletion and querying of users by <code>Id</code>, as well as receiving messages via <strong>RabbitMQ</strong> for updating a user's <code>Role</code>.</p>

 <h2>Security 👮</h2>
    <p>Security was a top priority in the development of this API. We implemented best security practices to protect user data, including:</p>
    <ul>
        <li><strong>JWT</strong> with robust encryption algorithms to ensure secure authentication and authorization.</li>
        <li><strong>Secret Key</strong>: In addition to the JWT, you need a security secret key to access the endpoints.</li>
        <li><strong>Refresh Tokens</strong> to enhance security and maintain controlled access.</li>
        <li>Strict <strong>Entity Framework</strong> configurations to prevent SQL Injection and database vulnerabilities.</li>
        <li>Secure integration with <strong>RabbitMQ</strong> for reliable service communication.</li>
    </ul>

 <h2>Architecture</h2>
    <p>The architecture of the API was designed by strictly following the <strong>Onion Architecture</strong>, with the following layers:</p>
    <ul>
        <li><strong>API</strong></li>
        <li><strong>Application</strong></li>
        <li><strong>Domain</strong></li>
        <li><strong>Infrastructure</strong></li>
    </ul>

 <img src="https://github.com/user-attachments/assets/d4542b45-69c8-482e-8fa2-afa954a7a106" alt="API Architecture">

<h2>Patterns and Technologies</h2>
    <p>The following patterns and technologies were adopted for the development of the API:</p>
    <ul>
        <li><strong>CQRS and MediatR</strong> for command and query handling.</li>
        <li><strong>Minimal APIs</strong> for performance optimization.</li>
        <li>Developed in <strong>.NET</strong> with a <strong>SQL Server</strong> database and <strong>Entity Framework</strong> as ORM.</li>
        <li>Asynchronous integration with <strong>RabbitMQ</strong> for service communication.</li>
    </ul>

 <h2>Endpoints</h2>

 <img src="https://github.com/user-attachments/assets/0faf7028-d3fe-4cd1-9722-747120c66f35" alt="API Endpoints">

 <h3>1. Register</h3>
    <p><strong>POST /api/users/register</strong></p>
    <p>Creates a new user.</p>
    <pre>
{
  "fullName": "string",
  "phone": "string",
  "document": "string",
  "email": "string",
  "password": "string",
  "birthDate": "2024-11-01T19:24:52.987Z"
}
    </pre>

<h3>2. Delete</h3>
    <p><strong>DELETE /api/users/{id}</strong></p>
    <p>Deletes a user by <code>Id</code>.</p>

<h3>3. Get User</h3>
    <p><strong>GET /api/users/{id}</strong></p>
    <p>Queries a user's information by <code>Id</code>.</p>

 <h3>4. Refresh Token</h3>
    <p><strong>POST /api/users/refresh-token</strong></p>
    <p>Generates a new access token from a <em>Refresh Token</em>.</p>
    <pre>
{
  "token": "string",
  "refreshToken": "string"
}
    </pre>

   <h3>5. Login</h3>
    <p><strong>POST /api/users/login</strong></p>
    <p>Authenticates a user.</p>
    <pre>
{
  "email": "string",
  "password": "string"
}
    </pre>
</body>
