# Jaeger com ASP.NET Core e .NET 5.0

Exemplo de tracing distribuído utilizando Jaeger com ASP.NET Core e .NET 5.0.

## Estrutura do Projeto

```bash
.
├── README.md
├── docker-compose.yaml
└── src
    ├── Common         # Extensões de tracing
    ├── Order.API      # Serviço A
    └── Payment.API    # Serviço B
```

## Excutando

Para subir todos os containers das aplicações e os serviços do Jaeger, basta executar a seguinte instrução no prompt de comando:

```bash
$ docker-compose up
```

|  app |  url |
|------|------|
|  Order.API (Serviço A)| http://localhost:5001/api/values  |
|  Payment.API (Serviço B)| http://localhost:5002/api/values |
|  Jaeger UI | http://localhost:16686/search  |