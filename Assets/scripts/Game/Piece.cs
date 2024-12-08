using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class Piece : ClickableObject
{
    #region Champs
    public bool IsJ1 = true;
    public bool IsActive { get; set; } = true;
    public bool IsQueen = false;
    public bool CanEat { get; set; } = false;   
    
    //number of possible mouv
    [SerializeField]private int nbPossibleMouv = 0;
    public int NbPossibleMouv { get => nbPossibleMouv;  set => nbPossibleMouv = value; }
    //number of eatable piece
    [SerializeField]private int nbCapturePossible = 0;
    public int NbCapturePossible { get => nbCapturePossible;  set => nbCapturePossible = value; }
    


    //represent all the possible mouv for a piece
    public List<Vector3> caseOnGridPossibleMouvPosition { get; set; } = new List<Vector3>();
    //represent all the possible position after a capture mouv for a piece
    public List<Vector3> caseOnGridPossibleAfterEatMouvPosition { get; set; } = new List<Vector3>();
    //represent the eaten piece position for each capture mouv position 
    public Dictionary<Vector2Int, Vector2Int> caseOnGridEatedPiecesPosition { get; set; } = new Dictionary<Vector2Int, Vector2Int>();

    public List<Vector3> caseOnGridQueenMouvPos { get; set; } = new List<Vector3>();
    public List<Vector3> caseOnGridQueenAfterEatPos { get; set; } = new List<Vector3>();
    public Dictionary<Vector2Int, Vector2Int> caseOnGridPiecesEatedByQueenPos { get; set; } = new Dictionary<Vector2Int, Vector2Int>();

    #endregion

    #region Constructeur
    public Piece() { }
    public Piece(int x, int y)
    {
        CaseX=x; CaseY=y;
        ServiceLocator.Get<SpawnerManager>().SpawnPiece(CaseX,CaseY);
    }
    #endregion

    private void Awake()
    {
        HoovedColor = Color.yellow;
        SelectedColor = Color.green;
    }

    #region Foncion

    private void PossibleMovement(int caseX, int caseY)
    {

        //Debug.Log("PossibleMovement appel Show all possible Mouve");
        pieceManager.ShowAllPossibleMouv(this);
    }

    private void HandleEatPieceEvent(int eventCaseX, int eventCaseY)
    {
        /* on implémente une réaction a un event 
         * si c'est bien cette piece qui se fait capturer :
         * on annonce qu'elle sera éliminer et on met canEat a false pour que 
         * elle ne soit plus selectionnable 
         * */
        if (CaseX == eventCaseX && CaseY == eventCaseY)
        {
            IsActive = false;
            Debug.Log($"la piece {this.name} va se faire éliminer");

            CanEat = false;
            Debug.Log($"La pièce en {CaseX}, {CaseY} ne peut plus manger.");

            //on assigne la case à non occupé
            gridManager.CaseOccuped[CaseX, CaseY] = false;
        }
    }
    #endregion

    #region installation Interact Event
    private void OnEnable()
    {
        EventHandler.OnEatingFromCase += HandleEatPieceEvent;
        
    }

    private void OnDisable()
    {
        EventHandler.OnEatingFromCase -= HandleEatPieceEvent; 
    }
    #endregion
   

    #region Installation Event Mouse
    /// <summary>
    /// quand la souris survole la piece elle se met jaune
    /// </summary>
    /// <param name="pointerEventData"></param>
    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!IsActive) return; 

        if (IsJ1 == turnManager.isJ1Turn)
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            GetComponent<MeshRenderer>().material.color = HoovedColor;
        }
    }

    /// <summary>
    /// quand la souris arret de survolr la piece elle se remet a sa couleur d'origine
    /// sauf si la case a été selectionner
    /// </summary>
    /// <param name="pointerEventData"></param>
    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        if (IsJ1 == turnManager.isJ1Turn)
        {
            if (EventSystem.current.currentSelectedGameObject != this.gameObject)
                GetComponent<MeshRenderer>().material.color = BaseColor;
        }
    }

    /// <summary>
    /// quand la souris survole la piece et click, on selectionne la piece
    /// et on appel OnSelect, si on reclick on appel deselect,
    /// </summary>
    /// <param name="pointerEventData"></param>
    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        //si isActive est false ça veut dire que la piece à été éliminer.
        if (!IsActive) return;

        //Peit rappel: le EventSystem ici n'a rien ç voir avec les events créer via script,
        //ici il s'agit de la gestion des clicks sur un objet et autres Event d'unity
        if (IsJ1 == turnManager.isJ1Turn)
        {
            
            if (EventSystem.current.currentSelectedGameObject == this.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
                uIManager.HidePossibleMouv();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
                gridManager.LastSelectedpiece = this;
                
                
                //Debug.Log("on appelle methode PossibleMouvement de piece lors du click");
                PossibleMovement(CaseX, CaseY);

            }
        }
    }
    /// <summary>
    /// quand la piece est selectionner elle passe en vert
    /// </summary>
    /// <param name="baseEventData"></param>
    public override void OnSelect(BaseEventData baseEventData)
    {
        if (IsJ1 == turnManager.isJ1Turn)
            GetComponent<MeshRenderer>().material.color = SelectedColor;
    }

    /// <summary>
    /// quand on (selectionne)click sur une autre piece, l'ancinne reprend la couleur de base  
    /// </summary>
    /// <param name="baseEventData"></param>
    public override void OnDeselect(BaseEventData baseEventData)
    {
        if (IsJ1 == turnManager.isJ1Turn)
            GetComponent<MeshRenderer>().material.color = BaseColor;
        

    }

    #endregion


}
