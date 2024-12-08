using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class EventHandler : MonoBehaviour
{
    #region install InteractionEventSystem
    ///j'ai dabord commencé avec des events déclarés avec un delegate puis un static event pour l'exercice
    ///nous utiliserons arpès des static Action 
    
    ///Event qui gère lorsque la capture d'une piece les case (X,Y) est déclaré
    ///elle lance les animation, le déplacemnts etc
    public delegate void EatPieceFromCase(int caseX, int CaseY);
    public static event EatPieceFromCase OnEatingFromCase;

    public static void OnEatPieceFromCase(int caseX, int CaseY)
    {
        OnEatingFromCase?.Invoke(caseX,CaseY);
    }
    
    /// <summary>
    /// marque la fin des animations lors de la capture d'une piece
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="CaseY"></param>
    public delegate void EndEatPieceAnimation(int caseX, int CaseY);
    public static event EndEatPieceAnimation OnEndEatPieceAnimation;
    public static void endEatPieceAnimation(int caseX, int CaseY)
    {
        OnEndEatPieceAnimation?.Invoke(caseX, CaseY);
    }

    public delegate void TurnEnding();
    public static event TurnEnding OnTurnEnding;

    public static void EndTurn()
    {
        OnTurnEnding?.Invoke();
    }

    public delegate void GameEnd();
    public static event GameEnd OnGameEnd;

    public static void EndGame()
    {
        OnGameEnd?.Invoke();
    }
    

    public static Action<int, int> OnGetCase;
    public void RaiseGetCase(int caseX, int CaseY) => OnGetCase?.Invoke(caseX,CaseY);

    #endregion


}
