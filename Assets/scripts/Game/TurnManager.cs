using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventHandler;

public class TurnManager : MonoBehaviour
{
    public bool isJ1Turn { get; set; } = true;
    private CinemachineVirtualCamera camJ1;
    
    private PieceManager pieceManager;

    private void Awake()
    {
        ServiceLocator.Register<TurnManager>(this);        
    }
    // Start is called before the first frame update
    private void Start()
    {
        pieceManager = ServiceLocator.Get<PieceManager>();
        camJ1 = GetComponentInChildren<CinemachineVirtualCamera>();


        OnTurnBegin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTurnBegin()
    { /* pour implémenter une capture obligatoire
         * on vérifie en début de tour si une capture est possible 
         * */
        //UpdatePiecePossibleMouv();
    }

    private void UpdatePiecePossibleMouv()
    {
        CheckAllPieceMouv();
    }

    private void OnEndTurn()
    {
        isJ1Turn = !isJ1Turn;
        ChangeCamera();
        OnTurnBegin();
    }

    /// <summary>
    /// check les mouveemnt disponible pour toutes les pieces
    /// </summary>
    private void CheckAllPieceMouv()
    {
        Debug.Log("je lance un check des mouv possibles");
        foreach (Piece piece in pieceManager.PiecesInGame)
        {
            pieceManager.CheckPossibleMouvement(piece);
            //pieceManager.ShowMouvement(piece); //c'est ici que l'erreur de référence null se lance
            pieceManager.ShowMouvement(piece);
            //Debug.Log($"{piece.name} a {piece.nbPossibleMouv} mouv et {piece.nbCaseMangeable} capture possible.");
            if (piece.CanEat)
            {
            }
        }
    }

    public void CheckEndGame()
    {
        if (pieceManager.J1ActivePiece == 0 || pieceManager.J2ActivePiece == 0)
        {
            EventHandler.EndGame();
            string Winner = pieceManager.J1ActivePiece > 0 ? "j1" : "j2";
            Debug.Log($"le gagnant est le {Winner}");
        }
    }


    public void ChangeCamera()
    {
        StartCoroutine(IChangeCamera());
    }

    public IEnumerator IChangeCamera()
    {
        if (camJ1.Priority == 11)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            camJ1.Priority = 9;
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.5f);
            camJ1.Priority = 11;
        }
    }
    private void OnEnable()
    {
        OnTurnEnding += OnEndTurn;
        
    }
    private void OnDisable()
    {
        OnTurnEnding -= OnEndTurn;
       
    }
}
