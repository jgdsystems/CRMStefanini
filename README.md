# CRMStefanini
Protótipo CRM para empresa stefanini

Procedimentos para implantação:

1. Restaurar a base de dados a partir do Backup localizado no diretório DATABASE arquivo CRMDBJULIODUARTE.bak, 
esse backup já possui usuários cadastrados e registros de exemplo, 
o arquivo de banco de dados fornecido não foi utilizado essa estrutura foi criada do zero.

2. WebConfig
O Arquivo de configuração está localizado em CRM\Web.config alterar a linha 72 para indicar as informações de conexão com servidor de 
banco de dados

3. Versão e suporte
CRM MVC-5 Versão 1.0
.Net Framework 4.5.1
Autor: Julio Duarte
Email para julio.pes@gmail.com

Melhorias pendentes:

* Filtro entre Cidades e regiões teve um inicio de implementação mas não foi finalizado;
* Arquitetura centralizada na camada de Front-End MVC o ideal é criar um projeto de Domínio como Back-End juntamente com um projeto de serviços de infra.
* Um exemplo de teste unitário foi criado mas não foi muito explorado.
* Atualizar projeto para .Net Core (tecnologia mais recente).
* Atualizar versão do Bootstrap (Por medidas de segurança).


