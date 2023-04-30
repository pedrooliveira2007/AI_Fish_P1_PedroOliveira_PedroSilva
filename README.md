# AI_Fish_P1_PedroOliveira_PedroSilva

# Alunos:
## Pedro Silva
## Pedro Oliverira - a21705187

# 3 - Responsabilidades:
## Pedro Silva:
- Criação dos diferentes peixes
- Implementação dos métodos de ação básica de cada peixe
- 

## Pedro Oliveira:

- Implementação da state machine
- debugging do código
- Implementação do movimento
- Implementação da rotação dos peixes
- Implementação da geração do tanque
- Implementação da lógica dos States
- Implementação da lógica das Transitions
- Modelo do peixe
- Pesquisa sobre a state machine
- Testes de lógica e do projeto no geral
- Conclusão de Pedro Oliveira no relatório


# 4 - Introdução
##
 O grupo decidiu utilizar state machine para o projeto onde cada peixe, dependendo do seu tamanho teria funcionalidades diferentes assim como solicitado no enunciado.
 
 Dentro de um aquário, existem 3 tipos de peixes diferentes, pequeno, médio e grande (implementado por enumerações) e algas no fundo que sobem com o tempo e volta a aparecer passado determinado tempo.
 
 Os peixes pequenos consomem algas, os médios consomem peixes pequenos e/ou algas e os peixes grandes consomem peixes médios e/ou pequenos. Os peixes têm preferência por comer outros peixes, sendo que os grandes têm preferência pelos médios e os médios têm preferência por pequenos. Ao consumirem outro peixe, os peixes recebem energia. Os peixes chegando a 75 de energia reproduzem-se e criam um novo peixe e sempre que detetam um peixe inimigo próximo tentam fugir.

# 5 - Metodologia
 O grupo utilizado state machine para a implementação de decisões dos agentes (Evade,Consume,Wander,Pursue,Reproduce), que funciona da seguinte forma:
## Peixe pequeno:
- Procura por inimigos no raio de deteção, caso encontre foge.
- Procura pela alga mais próxima "GetClosestAlgae()", caso encontre a persegue e a consome caso chegue próximo o suficiente.
- caso não encontre nenhum alvo, anda para um ponto aleatório
- Caso tenha energia o suficiente, reproduz

## Peixe médio:
- Procura pelo peixe mais próximo "GetClosestFish()" ou pela alga mais próxima "GetClosestAlgae()", caso encontre a persegue e a consome caso chegue próximo o suficiente.
- Procura por inimigos no raio de deteção, caso encontre foge.
- caso não encontre nenhum alvo, anda para um ponto aleatório
- Caso tenha energia o suficiente, reproduz

## Peixe grande:
- Procura pelo peixe mais próximo "GetClosestFish()"
- Caso tenha energia o suficiente, reproduz

## Alga:
- Sobre continuamente no aquário até chegar ao teto, acabando por se destruir.

# 6 - Resultados e discussão
##
Neste projeto foi possível realizar um sistema básico de AI ao utilizar uma máquina de estados finitos (FSM), no qual visava reproduzir o comportamento de peixes de diferentes tamanhos. 

 Cada peixe tem seus alvos de consumo, assim como inimigos. O projeto não teve toda a sua capacidade avaliada, a levar em consideração os erros de código e de lógica que não foram possíveis serem corrigidos a tempo da entrega. Mais precisamente, não foi testado de maneira exaustiva o sistema de reprodução, bem como o sistema de movimento aleatório não foi terminado da maneira esperada.
 
 Ainda sim, é possível ver o comportamento de fuga, perseguição e de consumo dentre os peixes.
 O projeto foi testado com 70 entidades de peixes diversos. Repara-se que há uma queda na quantidade de frames em certas ocasiões, provavelmente causada pela quantidade de verificações feitas por cada peixe.

 Com mais tempo para este projeto, as prioridades de implementação e de correção seriam:
 - Reprodução
 - Movimentação "Wander"
 - Implementação do gráfico
 - Sistema de "Flocking"
 - Melhora do sistema de detecção de entidades

# 7 - Conclusões
## 
Ao levar em consideração o projeto como está no momento, é possível concluír que a implementação de movimentação de um peixe em unity não é complexa em si, porém a implementação dos vários estados de um peixe em uma IA é complexa e tem de ter uma ótima organização de estados e transições, pois mesmo com poucos estados as interações entre os diverss peixes é custosa para o cumputador, tendo assim que ser otimizada para não existir perda de processamento.
 O projeto não ficou concluído da maneira que era solicitado, teria de ser melhor distribuído entre as partes, de maneira que nenhum dos realizadores ficasse sobrecarregado.

# 9 - Referências
## Fish animation
### https://www.youtube.com/watch?v=oS703kCYwTA&t=781s

## FSM, States and Transition
### https://github.com/nunofachada/AIUnityExamples/blob/main/SimpleFSMs/Assets/Scripts/FSMs/Transition.cs

## Action Delegate
### https://www.tutorialsteacher.com/csharp/csharp-action-delegate

## Animation Parameters
### https://docs.unity3d.com/Manual/AnimationParameters.html

## Dúvidas sobre erros de compilação e duvidas sobre rotação do peixe:
### ChatGPT

