# Troll App

Fork de aplicativo Trojan.Android-CampusParty criado pelo Erick Wendel para a Campus Party de 2017.

O App Mobile com Ionic foi excluído e foi criado um app com ReactJs.

# Projeto TrollApp
Este é o core da aplicação, ele deve ser executado na maquina cliente para que o socket seja aberto e possamos trolar nossos amigos. Por padrão ele inicializa na porta 15345, mais caso deseje pode iniciar o executavel em outra porta, passando a porta como parâmetro do executável.

Com o intuito de enganar o usuario, o executavel gerado tem o nome "Microsoft App Services", para enganar o usuario ao acessar o gerenciador de tarefas.

# Projeto Desktop Control
É um aplicativo Desktop Windows feito para comunicar com a maquina invadida, basta informar o IP do cliente e a porta, conectar se e começar a brincadeira.

#Projeto Get System Info
É um aplicativo Fake que é utilizado para instalar e executar o TroolApp.Control em segundo plano, o app inclui automaticamente o executavel na pasta Startup do Windows.

# Projeto Hacker Web
É um aplicativo Web feito em ReactJs para que você possa acessar o hospedeiro e executar as suas traquinagens