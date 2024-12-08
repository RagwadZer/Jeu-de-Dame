using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    public Color baseColor { get; set; }
    public Color HoovedColor { get; set; }
    public Color SelectedColor { get; set; }

    [SerializeField]
    private Vector2Int caseOnGrid;
    #region caseOnGrid propriétés
    public int CaseX
    {
        get { return caseOnGrid.x; }
        set
        {
            caseOnGrid.x = value;
        }
    }
    public int CaseY
    {
        get { return caseOnGrid.y; }
        set
        {
            caseOnGrid.y = value;
        }
    }
    public Vector2Int CaseOnGrid
    {
        get { return caseOnGrid; }
        set { caseOnGrid = value; }
    }
    #endregion
    
    protected PlaneManager uIManager;
    protected TurnManager turnManager;
    protected GridManager gridManager;
    protected PieceManager pieceManager;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        uIManager = ServiceLocator.Get<PlaneManager>();
        turnManager = ServiceLocator.Get<TurnManager>();
        gridManager = ServiceLocator.Get<GridManager>();
        pieceManager = ServiceLocator.Get<PieceManager>();
    }

    public Color BaseColor { get => baseColor; set => baseColor = value; }


    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);

    public virtual void OnSelect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    public virtual void OnDeselect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
