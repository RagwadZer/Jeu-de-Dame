using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerManager : MonoBehaviour
{

    #region Champs
    private float GrassPlatformOffset = -1.0f;
    private List<GameObject> unitTypeList = new List<GameObject>();

    GameObject piecePrefab;
    GameObject whiteGrassPrefab;
    GameObject blackGrassPrefab;
    GameObject skeletonPrefab;
    GameObject scifiSoldierPrefab;

    GameObject grassPlatformParent;
    public GameObject UnitsParent { get; set; }
    public GameObject PiecesParent { get; set; }

    private GridManager gridManager;
    private PieceManager pieceManager;
    #endregion

    private void Start()
    {
        Init();
        LoadAllPrefabs();
        SpawnAllPieces();
        SpawnEnvironnement();

        SpawnUnits();
    }

    private void Init()
    {       
        PiecesParent = new GameObject("Pieces");
        //PiecesParent.AddComponent<PieceManager>();

        grassPlatformParent = new GameObject("GrassPlatforms");
        UnitsParent = new GameObject("UnitsParent");
        UnitsParent.AddComponent<UnitManager>(); //je voulais l'ajouter manuellement juste pour essayer

        gridManager = ServiceLocator.Get<GridManager>();
        pieceManager = ServiceLocator.Get<PieceManager>();
    }

    private void LoadAllPrefabs()
    {
        piecePrefab = Resources.Load<GameObject>("Prefab/checker");
        whiteGrassPrefab = Resources.Load<GameObject>("Prefab/chess_platform_grass_White");
        blackGrassPrefab = Resources.Load<GameObject>("Prefab/chess_platform_grass_black");
        
        skeletonPrefab = Resources.Load<GameObject>("Prefab/mini simple skeleton");
        unitTypeList.Add(skeletonPrefab);
        scifiSoldierPrefab = Resources.Load<GameObject>("Prefab/Scifi soldier");
        unitTypeList.Add(scifiSoldierPrefab);
        
    }

    #region Piece & Environnement

    /// <summary>
    /// appliqué la couleur de la piece selon le joueur
    /// </summary>
    /// <param name="myP"></param>
    private void SetPieceColor(Piece myP)
    {
        if (myP.CaseY <= gridManager.GridY / 2)
        {
            myP.IsJ1 = true;
            myP.BaseColor = myP.GetComponent<MeshRenderer>().material.color;
            myP.GetComponent<MeshRenderer>().material.color = myP.BaseColor;

            pieceManager.J1ActivePiece++;
            //Debug.Log($"appel SetpieceColor piece[{myP.CaseX},{myP.CaseX}] : baseColor = white {myP.BaseColor}");
        }
        else
        {
            myP.IsJ1 = false;
            myP.BaseColor = Color.black;
            myP.GetComponent<MeshRenderer>().material.color = myP.BaseColor;
            pieceManager.J2ActivePiece++;
            
            //Debug.Log($"appel SetpieceColor piece [{myP.CaseX},{myP.CaseX}] baseColor = black {myP.BaseColor}");
        }
    }

    /// <summary>
    /// Creer une piece (prefabs) via le dossier Resources,
    /// lui fixe une position et sa couleur 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SpawnPiece(int X, int Y)
    {
        //on veut que x et y reste tjrs dans la grille
        if (X >= gridManager.NbCaseLongueur)
        {
            Debug.LogWarning($"X sort de la grille");
            X = gridManager.NbCaseLongueur - 1;
        }

        if (Y >= gridManager.NbCaseHauteur)
        {
            Debug.LogWarning($"Y sort de la grille");
            Y = gridManager.NbCaseHauteur - 1;
        }


        //s'il n'a pas déjà de pion sur la case de spawn
        if (!gridManager.CaseOccuped[X, Y])
        {
            /* on creer des variables pour prendre en compte
             * la taille d'ue case pour le postionnement des pieces
             * */
            float posX = X * gridManager.TailleCase;
            float posY = Y * gridManager.TailleCase;
            Vector3 position = gridManager.GetCaseCenter(posX, posY);

            /* on va chercher le prefabs cylinder dans les ressources (Asset) du projet
             * */



            //s'il y a bien une prefab de ce dans le dossier ressource
            if (piecePrefab != null)
            {
                //on créer le pion
                GameObject pieceUnit = Instantiate(piecePrefab, position, Quaternion.identity);
                Piece piece = pieceUnit.GetComponent<Piece>();
                piece.CaseX = X;
                piece.CaseY = Y;

                //Debug.Log($"piece.CaseX: {piece.CaseX},piece.CaseY: {piece.CaseY}");*/
                gridManager.CaseOccuped[X, Y] = true;



                pieceManager.PiecesInGame.Add(piece);

                SetPieceColor(piece);

                pieceUnit.name = $"piece[{X},{Y}]";

                gridManager.CaseOccuped[X, Y] = true;
                //Debug.Log($"case({X},{Y}) à un nouveau occupant : " + pieceUnit.name);
                //Debug.Log($"appel SpawnPiece ,piece[{X},{Y}]: couleur : {pieceUnit.GetComponent<MeshRenderer>().material.color}");

                pieceUnit.transform.SetParent(PiecesParent.transform, true);
            }
            else Debug.Log("les préfabs sont introuvables dans le dossier demandé");
        }
        else Debug.Log($"vous essayé de spawn sur la case[{X},{Y}] mais elle est occupé");



    }

    /// <summary>
    /// spawn toutes les pieces
    /// </summary>
    void SpawnAllPieces()
    {
        for (int i = 0; i < gridManager.NbCaseLongueur; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if ((i + j) % 2 != 0) // Positionner les pions seulement sur les cases noires (alternées)
                {
                    SpawnPiece(i, j);
                }
            }
            for (int j = gridManager.NbCaseHauteur - 1; j > gridManager.NbCaseHauteur - 5; j--)
            {
                if ((i + j) % 2 != 0) // Positionner les pions seulement sur les cases noires (alternées)
                {
                    SpawnPiece(i, j);
                }
            }
        }
    }

    private void SpawnEnvironnement()
    {
        for (int i = 0; i < gridManager.NbCaseLongueur; i++)
        {
            for (int j = 0; j < gridManager.NbCaseHauteur; j++)
            {
                SpawnGrass(i, j);
            }
        }
    }

    private void SpawnGrass(float x, float y)
    {
        float newX = x * gridManager.TailleCase;
        float newY = y * gridManager.TailleCase;
        Vector3 caseCenter = gridManager.GetCaseCenter(newX, newY);
        Vector3 worldPos = new Vector3(caseCenter.x,0.0f, caseCenter.z);

        GameObject grass = ((x + y) % 2 != 0) ? whiteGrassPrefab : blackGrassPrefab;
        if ((x + y) % 2 != 0)
        {
            var go = Instantiate(whiteGrassPrefab, worldPos, Quaternion.identity);
            go.transform.SetParent(grassPlatformParent.transform, true);
        }
        else
        {
            worldPos += new Vector3(1, 0, 0);
            var go = Instantiate(blackGrassPrefab, worldPos, Quaternion.identity);
            go.transform.SetParent(grassPlatformParent.transform, true);
        }        
        
    }

    #endregion

    #region AnimationUnit
    /// <summary>
    /// Spawn a Skeleton Unit a random Pos: X e [1,8] , Y e [-2,-7] 
    /// </summary>
    private void SpawnUnits()
    {
        foreach(GameObject unitType in unitTypeList)
        {
            //spawn 5 of each type of unit
            for (int i = 0; i < 5; i++)
            {
                float randomX = UnityEngine.Random.Range(1.0f, 8.0f);
                float randomY = UnityEngine.Random.Range(-7.0f, -2.0f);
                Vector3 SpawnPos = new Vector3(randomX, 0, randomY);
                GameObject Unit = Instantiate(unitType, SpawnPos, Quaternion.identity);                
                Unit.transform.SetParent(UnitsParent.transform, true);                            
            }
        }
    }


    #endregion
}


