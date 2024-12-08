using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    Unit[] Units;
    List<Skeleton> skeletons = new List<Skeleton>();
    List<ScifiSoldier> scifiers = new List<ScifiSoldier>();
    [SerializeField]private int UnitUsedID;

    private PieceManager pieceManager;

    private void Start()
    {
        
        Init();
        
    }

    private void Init()
    {
        pieceManager = ServiceLocator.Get<PieceManager>();

        Units = transform.GetComponentsInChildren<Unit>();
        foreach (Unit U in Units)
        {
            if (U is Skeleton)
            {
                skeletons.Add((Skeleton)U);               
            }
            if (U is ScifiSoldier)
            {
                scifiers.Add(U as ScifiSoldier);
            }
        }
        //Debug.Log($"il y a {Units.Length} units disponible pour l'animation");


    }

    private void OnEnable()
    {
        EventHandler.OnEatingFromCase += OrderGoToTargetToUnits;
        TestDebugScript.OnSetUnitType += setUnitTypeCalled;
    }


    private void OnDisable()
    {
        EventHandler.OnEatingFromCase -= OrderGoToTargetToUnits;
        TestDebugScript.OnSetUnitType -= setUnitTypeCalled;
       
    }
    /// <summary>
    /// utilisé pour les tests, pour appelé une troupe en particulier
    /// </summary>
    /// <param name="unitID"></param>
    public void setUnitTypeCalled(int unitID)
    {
       UnitUsedID = unitID;       
    }
  
    /* reçoit les coordonnés de la piece à éliminer par l'event OnEatingFromCase
     * */
    /// <summary>
    /// appelle la coroutine ISetTargetToUnits
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="CaseY"></param>
    private void OrderGoToTargetToUnits(int caseX, int CaseY)
    {       
        StartCoroutine(IOrderGoToTargetToUnits(caseX, CaseY));        
        
    }

    /// <summary>
    /// ordonne au nombre de unit requis pour déplacer la piece de bouger
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="CaseY"></param>
    /// <returns></returns>
    private IEnumerator IOrderGoToTargetToUnits(int caseX, int CaseY)
    {
        //pour appeler une troupe au hasard
        //int random = UnityEngine.Random.Range(0, 2);
        
        ObjectToMove obj = pieceManager.GetPieceCase(caseX, CaseY).GetComponent<ObjectToMove>();
        if (obj != null)
        {
            print($"Les {(UnitUsedID /*random*/ == 0 ? "squelettes" : "scifi Soldiers")} sont Appelés");
            if (UnitUsedID /*random*/ == 0)
            {
                for (int i = 0; i < obj.UnitsNeeded; i++)
                {
                    if (i < skeletons.Count)
                    {
                        yield return new WaitForSecondsRealtime(0.6f);
                        skeletons[i].SetTarget(obj.gameObject);
                    }
                    else
                    {
                        Debug.Log("pas assez de unit pour bouger l'objet");
                    }
                }
            } 
            if (UnitUsedID /*random*/== 1)
            {
                for (int i = 0; i < obj.UnitsNeeded; i++)
                {
                    if (i < scifiers.Count)
                    {
                        yield return new WaitForSecondsRealtime(0.6f);
                        scifiers[i].SetTarget(obj.gameObject);
                    }
                    else
                    {
                        Debug.Log("pas assez de unit pour bouger l'objet");
                        print("scCount: " + scifiers.Count + " ,i: "+i);
                    }
                }
            }
        }
    }

    
}
