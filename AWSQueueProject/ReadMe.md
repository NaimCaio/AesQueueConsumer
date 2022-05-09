# Criar imagem Dorck

>docker build -t consumer-aws-image -f Dockerfile .

# Rodar utilizando as variáveis de ambiente no arquivo Dockerfile.env

>docker run --name=filesContainer  -it --env-file ./Dockerfile.env consumer-aws-image

Os campos key e secret são para colocar o id e senha do usuário aws ( não enviei com o meu por questão se segurança)
os campos queuename a timout são o nome da fila e seu timeout de visibilidade respectivamente

Para conectar com a base mysql eu estou rodando a mesma publicamente em um servidor publico no heroku
(connectionString já está correta)

O código não está deletando a mensagem após ler mas a função foi criada, caso seja necessário só precisa descomentar a linha 84 do sqsQueueService.cs
