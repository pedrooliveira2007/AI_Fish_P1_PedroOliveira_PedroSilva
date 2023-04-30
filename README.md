# AI_Fish_P1_PedroOliveira_PedroSilva

# Alunos:
## Pedro Silva
## Pedro Oliverira

# 3 - Responsabilidades:
## Pedro Silva:
- Criação dos diferentes peixes
- Implementação dos métodos de ação básica de cada peixe
- 

## Pedro Oliveira:
- Implementação da state machine


# 4 - Introdução
 O grupo decidiu utilizar state machine para o projeto. Cada peixe, dependendo do seu tamanho teria funcionalidades diferentes. Dentro de um aquário, existem 3 tipos de peixes diferentes, pequeno, médio e grande (implementado por enumerações) e algas no fundo que sobem com o tempo e volta a aparecer passado determinado tempo. Os peixes pequenos consomem algas, os médios consomem peixes pequenos e/ou algas e os peixes grandes consomem peixes médios e/ou pequenos. Os peixes têm preferência por comer outros peixes, sendo que os grandes têm preferência pelos médios e os médios têm preferência por pequenos. Ao consumirem outro peixe, os peixes recebem energia. Os peixes chegando a 75 de energia reproduzem-se e criam um novo peixe e sempre que detetam um peixe inimigo próximo tentam fugir.

# 5 - Metodologia
 O grupo utilizado state machine para a implementação de decisões dos agentes (Evade,Consume,Wander,Pursue,Reproduce), que funciona da seguinte forma:
## Peixe pequeno:
- Procura pela alga mais próxima "GetClosestAlgae()"
- Se não tiver inimigo próximo dirige-se até à alga e come-a, caso não exista alga fica em "Wander()" pelo aquário, caso contrário foge do inimigo "Evade()".

## Peixe médio:
- Procura pelo peixe mais próximo "GetClosestFish()"
- Se não tiver inimigo próximo dirige-se até ao peixe e come-o, caso não exista peixe, procura alga e caso não exista alga fica em modo "Wander()", contudo, caso encontre um inimigo entra em estado "Evade()".

## Peixe grande:
- Procura pelo peixe mais próximo "GetClosestFish()"s

## Alga:
- Sobre continuamente no aquário até chegar ao teto, acabando por se destruir.

# 6 - Resultados e discussão
## 

# 7 - Conclusões
##

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

