 <h1>API - Microsserviço Learning Code</h1>
        <p>Este projeto é uma API que faz parte do microsserviço <strong>Learning Code</strong>. Ela é responsável pelas seguintes operações relacionadas aos usuários da aplicação:</p>
  <ul>
            <li>Login</li>
            <li>Criação de usuários</li>
           <li>Exclusão lógica de usuários</li>
       </ul>

        

  <h2>Autenticação</h2>
        <p>Para acessar os endpoints, são necessárias duas autenticações que devem ser incluídas no cabeçalho (Header) das requisições:</p>
        <ul>
            <li><strong>AcessSecretKey:</strong> Utilizada para acessar os endpoints de criação e login de usuários.</li>
            <li><strong>Token:</strong> Este token é retornado pelo endpoint de login e, juntamente com a <code>AcessSecretKey</code>, permite o acesso a todos os outros endpoints da API.</li>
        </ul>

  <h2>Endpoints</h2>
        <p>A API possui os seguintes endpoints:</p>


![image](https://github.com/user-attachments/assets/978d7438-5e60-4f67-878a-9a3336a5ccbd)



<h3>Exemplo de Requisição: /api/users/register</h3>
        <pre>
        POST /api/users/register
        Body:
        {
          "fullName": "string",
          "phone": "string",
          "document": "string",
          "email": "string",
          "password": "string",
          "birthDate": "2024-10-08T23:07:35.965Z"
        }
        </pre>

  <h3>Exemplo de Requisição: /api/users/login</h3>
        <pre>
        POST /api/users/login
        Body:
        {
          "email": "string",
          "password": "string"
        }
        </pre>

  <h2>Observações</h2>
  <p>Certifique-se de sempre enviar o <strong>AcessSecretKey</strong> e o <strong>Token</strong> corretos, caso contrário, o acesso aos endpoints será negado.</p>


  <h1>Arquitetura e Fluxograma</h1>

          
  ![AYc3o9il0nNSAAAAAElFTkSuQmCC](https://github.com/user-attachments/assets/de2bff55-b337-4dab-9f2d-9effa6281351)
  
  
   ![Fluxograma Pedido Online Amarelo Marrom](https://github.com/user-attachments/assets/6a537f55-6c90-4617-b8d6-ee41df37d44f)
