<body>
    <h1>Learning Code - API de Autenticação 🔒</h1>

   <p>Esta API faz parte do microsserviço <strong>Learning Code</strong>, um software de e-learning. Ela é responsável pela autenticação dos usuários no sistema, incluindo a criação de usuários, login, geração e renovação de <em>JWT</em> e <em>Refresh Tokens</em>, exclusão e consulta de usuários por <code>Id</code>, além de receber mensagens via <strong>RabbitMQ</strong> para atualização de <code>Role</code> de um usuário.</p>

   <h2>Segurança 👮</h2>
    <p>A segurança foi uma prioridade máxima no desenvolvimento desta API. Utilizamos as melhores práticas de segurança para proteger os dados dos usuários, incluindo:</p>
    <ul>
        <li><strong>JWT</strong> com algoritmos de criptografia robustos para garantir autenticação e autorização seguras.</li>
       <li><strong>Secret Key</strong>, além de um JWT você precisa da chave secreta de segurança para acessar os endpoints</li>
        <li><strong>Refresh Tokens</strong> para melhorar a segurança e manter o acesso controlado.</li>
        <li>Configurações rigorosas de <strong>Entity Framework</strong> para evitar SQL Injection e vulnerabilidades de banco de dados.</li>
        <li>Integração segura com <strong>RabbitMQ</strong> para comunicação confiável entre serviços.</li>
    </ul>
    
   <h2>Arquitetura</h2>
    <p>A arquitetura da API foi projetada seguindo rigorosamente a <strong>Arquitetura Cebola (Onion Architecture)</strong>, com as camadas:</p>
    <ul>
        <li><strong>API</strong></li>
        <li><strong>Application</strong></li>
        <li><strong>Domain</strong></li>
        <li><strong>Infrastructure</strong></li>
    </ul>


   ![image](https://github.com/user-attachments/assets/d4542b45-69c8-482e-8fa2-afa954a7a106)

   <h2>Padrões e Tecnologias</h2>
    <p>Os seguintes padrões e tecnologias foram adotados para o desenvolvimento da API:</p>
    <ul>
        <li><strong>CQRS e MediatR</strong> para manipulação de comandos e consultas.</li>
        <li><strong>Minimal APIs</strong> para otimização de desempenho.</li>
        <li>Desenvolvido em <strong>.NET</strong> com banco de dados <strong>SQL Server</strong> e <strong>Entity Framework</strong> como ORM.</li>
        <li>Integração assíncrona com <strong>RabbitMQ</strong> para comunicação entre serviços.</li>
    </ul>

   <h2>Endpoints</h2>

   ![image](https://github.com/user-attachments/assets/0faf7028-d3fe-4cd1-9722-747120c66f35)


   <h3>1. Register</h3>
    <p><strong>POST /api/users/register</strong></p>
    <p>Cria um novo usuário.</p>
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
    <p>Exclui um usuário pelo <code>Id</code>.</p>

   <h3>3. Get User</h3>
    <p><strong>GET /api/users/{id}</strong></p>
    <p>Consulta as informações de um usuário pelo <code>Id</code>.</p>

   <h3>4. Refresh Token</h3>
    <p><strong>POST /api/users/refresh-token</strong></p>
    <p>Gera um novo token de acesso a partir de um <em>Refresh Token</em>.</p>
    <pre>
{
  "token": "string",
  "refreshToken": "string"
}
    </pre>

   <h3>5. Login</h3>
    <p><strong>POST /api/users/login</strong></p>
    <p>Realiza a autenticação de um usuário.</p>
    <pre>
{
  "email": "string",
  "password": "string"
}
    </pre>
</body>
