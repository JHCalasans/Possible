O projeto contém tudo que foi pedido no pdf passado por email.

Na tela de login é possível escolher se vai utilizar o banco local(Sqlite) ou o serviço WCF(Depois que esta escolha for feita todas as operações seguirão a conexão selecionada, podendo alterar apenas se efetuar o logoff, durante o uso do WCF os dados são mantidos apenas em memória no serviço) - Bônus 2.

Na tela de login também foi deixado propositalmente dois campos 'Entry' distintos, o do campo 'Nome' está utilizando um Custon Renderer(Bônus 1), o do campo senha está utilizando um componente normal do Xamarin.Forms.

Para conseguir logar no aplicativo é preciso antes criar um usuário, esta opção também está disponível na tela de login.

A tela principal após incluir uma 'Lista' nova, poderá excluir esta lista com todas as 'Tarefas' que ela contem através da opção "Remove" e poderá incluir 'Tarefas' novas nesta lista através da opção "Add".

Para remover uma 'Tarefa' basta selecioná-la na lista e confirmar a exclusão.

Na tela principal abaixo do botão de adicionar listas encontra-se o filtro pedido(Today, This Week, This Month e Open-Ended).

São dois projetos separados, o projeto do aplicativo e o projeto contendo serviços WCF(lembrando que para estes serviços funcionarem devem estar rodando em IIS).

IMPORTANTE - PARA PODER UTILIZAR OS SERVIÇOS WCF A URL DEVE SER ALTERADA NA CLASSE 'VIEWMODELBASE' NO MÉTODO 'GETURL()', DEVE SER COLOCADO A URL ONDE O SERVIÇO ESTÁ PUBLICADO.  

OBS - NO SERVIÇO WCF NÃO ESTÃO SENDO PERSISTIDOS DADOS, OS DADOS SÃO MANTIDOS EM MEMÓRIA.
