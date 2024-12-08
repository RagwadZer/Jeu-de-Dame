using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plane : ClickableObject
{
    /// <summary>
    /// un plan apparait seulement après avoir selectionner une piece
    /// on veut que quand le plan est selectionner le pion se déplace
    /// </summary>
    /// <param name="pointerEventData"></param>   
    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        Piece selected = gridManager.LastSelectedpiece;

        if (selected != null)
        {
            //on choisi le dictionnaire qui contient les données de mouvement en fonction de si le pion est une reine
            Dictionary<Vector2Int, Vector2Int> cond = selected.IsQueen ? 
                selected.caseOnGridPiecesEatedByQueenPos : selected.caseOnGridEatedPiecesPosition;

            
            // Vérifier si une pièce peut être mangée à la position cliquée
            //on gros en regarde si c'est attaque ou un simple mouvement
            bool canEat = cond.TryGetValue(new Vector2Int(CaseX, CaseY), out Vector2Int caseToEatPosition);

            if (canEat)
            {
                //on bouge le pion 
                pieceManager.MoveLastSelectedPieceTo(CaseX, CaseY);
                //on élimine le plan enemie
                EventHandler.OnEatPieceFromCase(caseToEatPosition.x, caseToEatPosition.y);

                //on regarde si la piece peut recevoir une promotion après le mouveemnt
                pieceManager.CheckPromotion(selected);
                uIManager.HidePossibleMouv();
                // on reinit les planes pour verifier si on peut attaquer de nouveau
                pieceManager.CheckPossibleMouvement(CaseX, CaseY);

                //s'il n'y as pas de attaque possible , fin de tour
                if (selected.NbCapturePossible == 0)
                {
                    //le changement de tour doit se faire avant l'evenement pour le calucul des mouv des piece adverse du prochain tours                   
                    EventHandler.EndTurn();
                    EventSystem.current.SetSelectedGameObject(null);
                }
                else
                {
                    //on calcule les nouveaux mouv
                    //on montre les mouv d'attaque
                    pieceManager.ShowEatMouvement(selected);
                }


            }
            else
            {
                pieceManager.MoveLastSelectedPieceTo(CaseX, CaseY);
                pieceManager.CheckPromotion(selected);

                uIManager.HidePossibleMouv();
                //le changement de tour doit se faire avant l'evenement pour le calucul des mouv des piece adverse du prochain tours               
                EventHandler.EndTurn();

            }
            
           
        }
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        GetComponent<Renderer>().material.color = base.HoovedColor;
    }

    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        GetComponent<Renderer>().material.color = baseColor ;
        
    }


    private void OnDisable()
    {
        Debug.Log($"{name} se desactive");
    }

}
