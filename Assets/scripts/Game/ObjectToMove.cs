using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectToMove : MonoBehaviour
{
    List<Unit> Units = new List<Unit>();
    public int UnitsNeeded { get;} = 3;

    //circle around the object
    [SerializeField][Range(0.2f,1.0f)]float radius = 0.25f;
    float angleStep;
    float currentAngle = 0.0f;
    float LiftingSpeed = 2.3f;
    Coroutine CurrentCoroutine; //to save the running coroutine 

    private void Start()
    {
        angleStep = 360f / UnitsNeeded;
    }

    public void AddUnit(Unit unit)
    {
        if (!Units.Contains(unit))
        {           
            //skeleton.SetTarget(null);
            Units.Add(unit);
            //Debug.Log($"unit ajouté pour le déplacement de {this.name}");
            MoveToAroundObjectPosition(Units);                                           
        }
        if (Units.Count >= UnitsNeeded)
        {
            //Debug.Log($"all {UnitsNeeded} required Units Ready");
            if (unit is Skeleton)
            {
                LiftObject();
                Debug.Log("les skeletons lancent l'animation");
            }
        }
    }
    private void MoveToAroundObjectPosition(List<Unit> Units)
    {
        currentAngle = 0.0f;
        for (int i = 0; i < Units.Count; i++)
        {           
            //calcule des positions autour de l'objet
            float posX = transform.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
            float posZ = transform.position.z + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;

            Vector3 vector3 = new Vector3(posX, Units[i].transform.position.y, posZ);            
            //on apllique les nouvelles positions aux squelettes
            Units[i].transform.position = vector3;
            
            // Faire en sorte que le squelette regarde l'objet
            Units[i].transform.LookAt(transform.position);

            // Avancer l'angle pour le prochain Pikmin
            currentAngle += angleStep;            
        }

    }

    private void LiftObject()
    {
        foreach (Unit U in Units) 
        {
            U.ActionOnAllUnitsReady();            
        }
        MovePieceWhenLifted();
    }
    /// <summary>
    /// move the object physically
    /// </summary>
    private void MovePieceWhenLifted()
    {        
        CurrentCoroutine = StartCoroutine(IMovePieceWhenLifted());
    }
    /// <summary>
    /// to physically move up the piece by 1.5f
    /// </summary>
    /// <returns></returns>
    private IEnumerator IMovePieceWhenLifted()
    {
        Vector3 targetPos = transform.position + new Vector3(0, 1.0f, 0);
        while (Vector3.Distance(transform.position, targetPos) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, LiftingSpeed * Time.deltaTime);
            yield return null;
        }

        //call the leave method when the piece movement animation is finished
        SetObjectAsUnitsParent();
        Leave();

    }

    /* it's this piece which manage all the unit used to move it.
     * so it's the piece which trigger the event
     * */
    private void Leave()
    {
        bool isJ1 = GetComponent<Piece>().IsJ1;
        Vector3 target = ServiceLocator.Get<PieceManager>().GetRandomPositionInPieceCamp(isJ1);
        Debug.Log($"la piece sera déposé à {target}");
        StartCoroutine(Ileave(target));       
    }

    private IEnumerator Ileave(Vector3 target)
    {
        /* check distance while moving to the target        
         * */
        while (Vector3.Distance(transform.position, target) > 0.3f)
        {
            Vector3 movedir = Vector3.MoveTowards(transform.position, target, Units[0].speed * Time.deltaTime);
            transform.SetPositionAndRotation(movedir,Quaternion.identity);
            yield return null;
        }
        /* when the target is reached
         * set back the unitManager as Units Parent         
         * */
        foreach (Unit U in Units)
        {
            print("ici");
            U.SetIsMovingBool(false);
            U.SetStateBool1(false);
            U.HandleLayerMaskWeight();
            U.transform.parent = U.unitManager;
        }
        /* delete the piece;         
         * */
        Piece obj = GetComponent<Piece>();
        int cX = obj.CaseX;
        int cY = obj.CaseY;
        EventHandler.endEatPieceAnimation(cX,cY);
    }

    /// <summary>
    /// set the object as parent of the skeleton units
    /// </summary>
    private void SetObjectAsUnitsParent()
    {       
        foreach (Unit U in Units)
        {
            U.transform.parent = transform;
            U.SetStateBool(false);
            U.SetStateBool1(true);
            U.SetIsMovingBool(true);
            U.HandleLayerMaskWeight();
        }

        
    }
}
