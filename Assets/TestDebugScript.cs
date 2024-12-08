using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestDebugScript : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private PieceManager pieceManager;

    public int _unitTypeID;
    public int UnitTypeID 
    {
        get { return _unitTypeID; }
        set 
        {
            _unitTypeID = Mathf.Clamp(value, 0, 1);
        } 
    }

    private void Start()
    {    
        pieceManager = ServiceLocator.Get<PieceManager>();
    }

    /// <summary>
    /// event pour tester les différente animation d'élimination d'une piece
    /// le type d'Unit définie quelle animation sera jouer
    /// </summary>
    /// <param name="unitID"></param>
    public delegate void SettingUnitType(int unitID);
    public static event SettingUnitType OnSetUnitType;  

    /// <summary>
    /// trouver une piece au hasard dans le jeu et l'élimine
    /// </summary>
    private void selectRandomPiece()
    {
        //print(TerrainManager.Instance.PiecesInGame.Count); // pour vérifier si random prend même la dernière valeur en compte, je ne suis jamais sur
        int random = UnityEngine.Random.Range(0, pieceManager.PiecesInGame.Count-1); //s'il faut mettre -1 ou pas
        Piece piece = pieceManager.PiecesInGame[random].GetComponent<Piece>();
        if (piece != null) 
        {
            EventHandler.OnEatPieceFromCase(piece.CaseX, piece.CaseY);
        }
        else
        {
            print("piece introuvable");
        }


    }
    /// <summary>
    /// affiche les mouvements disponible de toutes les pieces
    /// </summary>
    private void showAllPieceMouv()
    {
        List<Piece> allPieces = pieceManager.PiecesInGame;
        foreach (Piece piece in allPieces)
        {
            pieceManager.ShowAllPossibleMouv(piece);
        }
    }
    public void  OnPointerClick(PointerEventData eventData)
    {
        /* //on set le type de personnages pour l'animation d'élimination
         * OnSetUnitType?.Invoke(UnitTypeID);
         * //on choisie une piece au hasard a éliminer
         * selectRandomPiece();
         * */
        showAllPieceMouv();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
