 # **Jeu de dame** 🎮

> **Un projet Unity conçu pour démontrer mes compétences en programmation gameplay.**  
> **Type de projet** : Jeu de dames avec animations et interface utilisateur.

---

## **📖 Description**

Ce dépôt contient un **jeu de dames complet**, réalisé en utilisant Unity et C#.  
Les objectifs principaux de ce projet sont :  
- Montrer mes compétences en programmation gameplay avec **Unity**.  
- Illustrer ma capacité à gérer des **mécaniques de jeu complexes** (logique de déplacement, capture).  
- Mettre en œuvre une **interface utilisateur fonctionnelle et intuitive**.

---

## **🛠️ Fonctionnalités principales**

- **Mécanique de jeu complète** :
  - Déplacement valide des pièces selon les règles du jeu de dames.
  - Capture des pièces adverses avec animations.  

- **Interface utilisateur basique** :
  - Affichage des scores. 

- **Système d'événements (GameManager)** :
  - Gestion centralisée des états du jeu (début, tour des joueurs, fin de partie).  
  - Utilisation d’événements pour une communication efficace entre les objets Unity.

---

## **🧑‍💻 Structure du projet**

### 1. **Scripts**
Les scripts principaux se trouvent dans le dossier `Assets/Scripts`.  
- **GameManager.cs** : Contrôle les états et les événements du jeu.  
- **Piece.cs** : Gère les déplacements, les captures et l'état des pièces.  
- **BoardManager.cs** : Logique du plateau (placement, vérification des règles).  

### 2. **Organisation des assets**
- **Scenes/** : Contient la scène principale du jeu.  
- **Prefabs/** : Modèles des pièces et du plateau.  
- **UI/** : Éléments d'interface utilisateur.  

---

## **📂 Installation**

1. **Cloner le dépôt** :  
   ```bash
   git clone https://github.com/RagwadZer/Jeu-de-Dame.git
Ouvrir dans Unity :
Chargez le projet dans Unity (version recommandée : Unity 2022.3.34f1 ou supérieur).

Exécuter la scène principale :
Naviguez dans le dossier Scenes et ouvrez MainScene.unity.

🛠️ Technologies utilisées
Unity (C#) pour le moteur de jeu.
UVCS pour la gestion de version.
Animations Unity pour les transitions visuelles.

🎯 Objectif du projet
Ce projet sert à démontrer mes compétences pour des postes de programmeur gameplay.

Maîtrise des systèmes Unity (scènes, gestion d’événements, interaction UI).
Gestion de projets complets avec une architecture de code claire.
Développement d’un gameplay engageant et fonctionnel.

👨‍💻 Auteur
RAFANOMEZANTSOA Nicolas Yoan
[https://ragwadzer.itch.io/]
[https://www.linkedin.com/in/nicolas-yoan-rafanomezantsoa-486917178/]
[nicolasrafano@yahoo.com]

## Diagramme de Classes UML
Représente les relations entre les classes principales du projet.
```mermaid
classDiagram
    class ClickableObject {
        <<abstract>>
        +BaseColor: Color
        +HoovedColor: Color
        +SelectedColor: Color
        +CaseX: int
        +CaseY: int
        +CaseOnGrid: Vector2Int
        +OnPointerClick()
        +OnPointerEnter()
        +OnPointerExit()
        +OnSelect()
        +OnDeselect()
    }
    class Piece {
        +IsJ1: bool
        +IsActive: bool
        +IsQueen: bool
        +NbPossibleMouv: int
        +NbCapturePossible: int
        +caseOnGridPossibleMouvPosition: List<Vector3>
        +CheckPossibleMouvement()
        +PromoteToQueen()
    }
    class Plane {
        +CaseX: int
        +CaseY: int
        +BaseColor: Color
        +HoovedColor: Color
        +OnPointerClick()
        +OnPointerEnter()
        +OnPointerExit()
    }
    class TurnManager {
        +isJ1Turn: bool
        +OnTurnBegin()
        +OnEndTurn()
        +ChangeCamera()
    }
    class PieceManager {
        +PiecesInGame: List<Piece>
        +MoveLastSelectedPieceTo()
        +CheckPossibleMouvement()
        +CheckQueenPossibleMouvement()
    }
    class GridManager {
        +NbCaseLongueur: int
        +NbCaseHauteur: int
        +CaseOccuped: bool[,]
        +GetCaseCenter()
        +IsWithinGrid()
    }
    class EventHandler {
        +EatPieceFromCase()
        +EndEatPieceAnimation()
        +OnTurnEnding()
        +OnGameEnd()
    }
    class PlaneManager {
        +ShowMouvPlane()
        +ShowEatPlane()
        +HidePossibleMouv()
    }
    class ServiceLocator {
        +Register<T>()
        +Get<T>()
    }
    class Unit {
        <<abstract>>
        +unitManager: Transform
        +targetObject: GameObject
        +speed: float
        +isMoving: bool
        +SetTarget()
        +MoveToTarget()
        +OnObjectReached()
    }
    class Skeleton {
        +ReadyToLift: bool
        +LiftAnObject: bool
        +DecideNextAction()
        +ActionOnAllUnitsReady()
        +HandleLayerMaskWeight()
    }
    class ScifiSoldier {
        +IsAiming: bool
        +IsShooting: bool
        +DecideNextAction()
        +ActionOnAllUnitsReady()
        +HandleAnimation()
    }

    ClickableObject <|-- Piece
    ClickableObject <|-- Plane
    Unit <|-- Skeleton
    Unit <|-- ScifiSoldier
    Piece --> PieceManager
    Plane --> PlaneManager
    TurnManager --> PieceManager
    ServiceLocator --> TurnManager
    ServiceLocator --> PieceManager
    ServiceLocator --> GridManager
    Skeleton --> ObjectToMove
    ScifiSoldier --> ObjectToMove


