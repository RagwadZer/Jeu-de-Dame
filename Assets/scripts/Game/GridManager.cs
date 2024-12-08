using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    

    #region champs
    [SerializeField] [Range(1.0f, 3.0f)]
    GameObject EatMouvPLanesParent;
    GameObject SimpleMouvPlanesParent;
   
    Piece lastSelectedpiece;
    public Piece LastSelectedpiece { get => lastSelectedpiece; set => lastSelectedpiece = value; }
    
    float offset;
    public float Offset
    {
        get { return offset; }
        private set { offset = value; }
    }
    
    int tailleCase = 1;
    public int TailleCase
    {
        get { return tailleCase;}        
    }
    
    public int NbCaseLongueur { get; } = 10;
    
    public int NbCaseHauteur { get; } = 10;
    
    bool[,] caseOccuped;
    public bool[,] CaseOccuped
    {
        get { return caseOccuped; }
        private set { caseOccuped = value; }
    }

    float gridX;
    /// <summary>
    /// taille de case fois nb de case en X
    /// </summary>
    public float GridX { get => gridX; private set => gridX = value; }

    float gridY;
    /// <summary>
    ///  taille de case fois nb de case en Z
    /// </summary>
    public float GridY { get => gridY; private set => gridY = value; }

    #endregion

    private void Awake()
    {
        Init();       
    }
    private void Start()
    {
        
        InitPlanes();
    }
    private void Update()
    {
        /* A servit lors des phases de prototypage
         * //DrawTerrain();
         * //GetMousePositionOnGrid();
         * */
    }

    #region Fonctions

    /// <summary>
    /// pour initialiser les valeurs qui doivent l'être 
    /// </summary>
    private void Init()
    {
        ServiceLocator.Register<GridManager>(this);
        CaseOccuped = new bool[NbCaseLongueur, NbCaseHauteur];
        ClearAllCase();
        
        Offset = TailleCase / 2.0f;
        GridX = NbCaseLongueur * TailleCase;
        GridY = NbCaseHauteur * TailleCase;

        EatMouvPLanesParent = new GameObject("EatMouvPlanes");
        SimpleMouvPlanesParent = new GameObject("SimplesMouvPlanes");
    }

    private void InitPlanes()
    {
        // Initialiser les plans pour les mouvements possibles
        int maxPossibleMoves = Mathf.FloorToInt((float)(Math.Sqrt(2.0) * GridX) + 1) * 2; // Ajustez ce nombre selon vos besoins
       
        //on récupère les planes
        PlaneManager uIManager = ServiceLocator.Get<PlaneManager>();
        uIManager.mouvPlanes = new List<GameObject>();
        uIManager.eatPlanes = new List<GameObject>();

        GameObject planePrefab = Resources.Load<GameObject>("Prefab/Plane");

        for (int i = 0; i < maxPossibleMoves; i++)
        {
            //set all param for mouvPlanes and create it
            GameObject plane = Instantiate(planePrefab);
            plane.transform.localScale = new Vector3(1.0f, 0.2f, 1.0f) * TailleCase;
            plane.SetActive(false);
            plane.name = $"mouvPlane {i}";
            uIManager.mouvPlanes.Add(plane);

            //for hierachie better organisation
            plane.transform.SetParent(SimpleMouvPlanesParent.transform);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject plane = Instantiate(planePrefab);
            plane.transform.localScale = new Vector3(1.0f, 0.2f, 1.0f) * TailleCase;
            plane.SetActive(false);
            plane.name = $"eatPlane {i}";
            uIManager.eatPlanes.Add(plane);

            plane.transform.SetParent(EatMouvPLanesParent.transform);
        }
    }

    /// <summary>
    /// met false sur toute les cases
    /// </summary>
    void ClearAllCase()
    {
        for (int i = 0; i < NbCaseLongueur; i++)
        {
            for (int j = 0; j < NbCaseHauteur; j++)
            {
                CaseOccuped[i, j] = false;
            }
        }
    }

    public bool IsWithinGrid(int x, int y)
    {
        if ((x >= 0 && x < GridX) && (y >= 0 && y < GridY))
            return true;
        else return false;
    }

    /// <summary>
    /// pour obtenir la position au centre de la case [x,z]
    /// </summary>
    /// <param name="x"></param> 
    /// <param name="z"></param> 
    /// <returns></returns>
    public Vector3 GetCaseCenter(float x, float z)
    {
        Vector3 origine = Vector3.zero;
        origine.x += x + Offset;
        origine.z += z + Offset;
        return origine;
    }

    /// <summary>
    /// affiche si la case[x,y] est occupée sur le plateau
    /// </summary>
    /// <param name="plateau"></param>
    public bool GetifCaseIsOccuped(int caseX, int caseY)
    {
        //Debug.Log($"la case[{caseX},{caseY}] est occupé ? : {caseOccuped[caseX,caseY]}");
        return caseOccuped[caseX, caseY];

    }
    
    public void SetCaseOccuped(int x, int y, bool isOccupied) => caseOccuped[x, y] = isOccupied;


    #endregion
    #region fonctions non utilisées
    /* a servi pour le protypage mais n'est plus utilisé
     * */
   
    /// <summary>
    /// pour dessiner le terrain de jeu, visible dans la scene 
    /// </summary>
    private void DrawTerrain()
    {

        Debug.DrawLine(new Vector3(GridX, 0, 0), new Vector3(GridX, 0, GridY), Color.red, 5.0f);
        Debug.DrawLine(new Vector3(0, 0, GridY), new Vector3(GridX, 0, GridY), Color.red, 5.0f);
        for (int i = 0; i < NbCaseLongueur; i++)
        {
            for (int j = 0; j < NbCaseHauteur; j++)
            {
                int posX = i * TailleCase;
                int posY = j * TailleCase;
                Debug.DrawLine(new Vector3(posX, 0, posY),
                    new Vector3((i + 1) * tailleCase, 0, posY), Color.red);

                Debug.DrawLine(new Vector3(posX, 0, posY),
                    new Vector3(posX, 0, (j + 1) * tailleCase), Color.red);

            }
        }



    }

    /// <summary>
    /// on veut la position de la souris sur l'écran
    /// et sur quelle case elle se trouve quand elle survole
    /// le plateau de jeu. peut etre désactivé sans incidence.
    /// 
    /// 
    /// très limité en fonction de la position de la camera
    /// on va la garder car elle donne des info qui peuvent etre utilent mais tout est fait avec le 
    /// eventSyteme qui est plus précise et efficace
    /// </summary>
    /// <returns></returns>
    void GetMousePositionOnGrid()
    {
        Vector3 mousePos = Input.mousePosition;
        /* ça regle le pb worldPos qui reste tjrs a la mm valeur
         * le pb étant que mousePos.z est de base =0 vu que c'est la pos de la souris(vector2)
         * */
        mousePos.z = Camera.main.transform.position.y;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        /* on divise WorldPos par la taille d'une casse pour obtenir le numéro
         * de la case quelque soit la taille de la case
         * !!!: caseY s'obtient avec worldPose.z (non pas .y)
         * */
        int caseX = Mathf.FloorToInt(worldPos.x / TailleCase);
        int caseY = Mathf.FloorToInt(worldPos.z / TailleCase);
        //si les valeurs sont dans la grille
        if (caseX >= 0 && caseX <= NbCaseLongueur
            && caseY >= 0 && caseY <= NbCaseHauteur)
        {
            ///mettre du code
            ///si la souris
        }
        //sinon elle sont hors grille, on les mets a -1, parce qu'il faut une valeur
        else
        {

        }

    }
    #endregion
}
