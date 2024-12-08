using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    /* ce fichier sert au calcul des mouv pour chaque piece
     * elle fait les calculs et assigne les données  
     * */

    public List<Piece> PiecesInGame { get; set; }
    public int J1ActivePiece { get; set; } = 0;
    public int J2ActivePiece { get; set; } = 0;

    Vector3 RandomReturnPos { get; set; } // la position sur laquelles les units vont emmenée la piece mangée

    private GridManager gridManager;
    private PlaneManager uIManager;

    private void Awake()
    {
        ServiceLocator.Register<PieceManager>(this);
        PiecesInGame = new List<Piece>();
    }
    private void Start()
    {
        gridManager = ServiceLocator.Get<GridManager>();
        uIManager = ServiceLocator.Get<PlaneManager>();
    }

    #region fonctions

    /// <summary>
    /// bouge lastSelectedpiece au coordonnée fournit
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void MoveLastSelectedPieceTo(int caseX, int caseY)
    {
        Piece piece = gridManager.LastSelectedpiece;
        //on met la case occupé par la piece a false, donc innocupé

        gridManager.CaseOccuped[piece.CaseX, piece.CaseY] = false;

        //on modif les valeur de la piece au nouvelle valeur
        piece.CaseX = caseX;
        piece.CaseY = caseY;

        //on modif la position du pion
        Vector3 caseCenter = gridManager.GetCaseCenter(caseX, caseY);
        piece.gameObject.transform.position = new Vector3(caseCenter.x, 0.0f, caseCenter.z);


        //on annonce que la nouvelle case est occupé
        gridManager.CaseOccuped[piece.CaseX, piece.CaseY] = true;

    }
   
    /// <summary>
    /// renvoie le gameobject au coordonnée fournit
    /// </summary>
    /// <param name="pieceX"></param>
    /// <param name="pieceY"></param>
    /// <returns></returns>
    public GameObject GetPieceCase(int pieceX, int pieceY)
    {
        foreach (Piece item in PiecesInGame)
        {
            Piece piece = item.GetComponent<Piece>();


            if (piece != null)
            {
                if (piece.CaseX == pieceX && piece.CaseY == pieceY)
                {
                    //Debug.Log("la piece rechercher est "+piece.name);
                    return item.gameObject;
                }

            }

        }
        Debug.Log("pas de piece trouvé");
        return null;

    }

    /// <summary>
    /// calcul les mouvement possible d'une piece qui n'est pas une reine
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void CheckPiecePossibleMouvement(Piece piece)
    {
        //données temporaire utilisées pour les calcules
        //la liste des coordonnées de mouvements possible ,utilisées pour les calcules
        List<Vector3> caseOnGridPossibleMouvPosition = new List<Vector3>();
        int nbPossibleMouv = 0;

        //la liste de position des pions qui peuvent etre manger et leur nombres ,
        //et la pisotion de cette piece après avoir manger 
        List<Vector3> caseOnGridPossibleAfterEatMouvPosition = new List<Vector3>();
        int nbCaseMangeable = 0;
        Dictionary<Vector2Int, Vector2Int> caseOnGridEatedPiecesPosition = new Dictionary<Vector2Int, Vector2Int>();

        //pour regarder seulement devant
        int direction = (piece.IsJ1 ? 1 : -1);

        //pour regarder a gauche et a droite
        for (int i = -1; i <= 1; i += 2)
        {
            int j = direction;

            //les variable du mouvement 
            int eatCaseX = piece.CaseX + i;
            int eatCaseY = piece.CaseY + j;

            //on s'assure que la case a checker est dan sles limites du plateau
            if (gridManager.IsWithinGrid(eatCaseX, eatCaseY))
            {

                //si la case est vide, le mouvement est possible 
                if (!gridManager.GetifCaseIsOccuped(eatCaseX, eatCaseY))
                {
                    nbPossibleMouv++;
                    caseOnGridPossibleMouvPosition.Add(new Vector3(eatCaseX, 0, eatCaseY));
                    //Debug.Log("appel terrainManager : affiche les cases on GRid non occupé valide
                    //pour déplacement: newX/newY : "+ newX +" / " +newY);
                }
                //si elle n'est pas vide
                else
                {
                    //on regarde si la case d'après l'est
                    int afterEatCaseX = eatCaseX + i;
                    int afterEatCaseY = eatCaseY + j;

                    // il faut que la case soit tjrs dans la limite bord
                    if (gridManager.IsWithinGrid(afterEatCaseX, afterEatCaseY))
                    {
                        //si elle n'est pas occupé
                        if (!gridManager.GetifCaseIsOccuped(afterEatCaseX, afterEatCaseY))
                        {
                            //on check si c'est bien un pion de l'adversaire que m'on essaie de manger
                            if (GetPieceCase(eatCaseX, eatCaseY) != null)
                            {
                                GameObject pieceObj = GetPieceCase(eatCaseX, eatCaseY);
                                Piece eatedPiece = pieceObj.GetComponent<Piece>();

                                if (eatedPiece.IsJ1 != piece.IsJ1)
                                {
                                    /* On arrive bien a acceder a la piece sur la position voulu et la faire réagir
                                     * Debug.Log(pieceObj.name);
                                     * pieceObj.SetActive(false);*/

                                    //on ajoute les coordonné de la piece a manger avec comme clé le plane correspondant
                                    caseOnGridEatedPiecesPosition.Add(new Vector2Int(afterEatCaseX, afterEatCaseY)
                                        , new Vector2Int(eatCaseX, eatCaseY));

                                    //Debug.Log($"clé :plane [{afterEatCaseX},{afterEatCaseY}] / valeur :case [{eatCaseX},{eatCaseY}] ajouté");
                                    nbCaseMangeable++;
                                    piece.CanEat = true;
                                    //puis le nb de mouv possible ainsi que leur position

                                    caseOnGridPossibleAfterEatMouvPosition.Add(new Vector3(afterEatCaseX, 0, afterEatCaseY));
                                    //Debug.Log("appel terrainManager :affiche les cases on on grid a manger: X/Y : " + eatedCaseX + " / " + eatedCaseY);
                                }
                            }
                        }
                    }
                }


            }

        }
        //represent all the possible mouv for a piece
        piece.caseOnGridPossibleMouvPosition = caseOnGridPossibleMouvPosition;
        //represent all the possible position after a capture mouv for a piece
        piece.caseOnGridPossibleAfterEatMouvPosition = caseOnGridPossibleAfterEatMouvPosition;
        //represent the eaten piece position for each mouv position 
        piece.caseOnGridEatedPiecesPosition = caseOnGridEatedPiecesPosition;
        //number of eatable piece
        piece.NbCapturePossible = nbCaseMangeable;
        //number of possible mouv
        piece.NbPossibleMouv = nbPossibleMouv;
        #region check
        /*Debug.Log("move");
        Debug.Log("nb de case mangeable : " + NbCaseMangeable);
        for (int i = 0; i < nbPossibleMouv; i++)
        {
            Debug.Log(caseOnGridPossibleMouvPosition[i]);
        }

        Debug.Log("eat");
        Debug.Log(NbCaseMangeable);        
        foreach (var item in caseOnGridEatedPiecesPosition)
        {
            Debug.Log("pour "+caseX+"/"+caseY+" : clé :"+item.Key +" /vaeur :" +item.Value);
        }
    */
        #endregion
    }

    /// <summary>
    /// calcule tous les mouvements d'une piece qui est une reine.
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void CheckQueenPossibleMouvement(Piece piece)
    {
        int nbCaseMangeable = 0;
        List<Vector3> caseOnGridQueenMouvPos = new List<Vector3>();
        List<Vector3> caseOnGridQueenAfterEatPos = new List<Vector3>();
        Dictionary<Vector2Int, Vector2Int> caseOnGridPiecesEatedByQueenPos = new Dictionary<Vector2Int, Vector2Int>();

        int[] direction = { -1, 1 };

        foreach (int dx in direction)
        {
            foreach (int dy in direction)
            {
                int x = piece.CaseX + dx;
                int y = piece.CaseY + dy;

                while (gridManager.IsWithinGrid(x, y))
                {
                    if (!gridManager.GetifCaseIsOccuped(x, y))
                    {
                        caseOnGridQueenMouvPos.Add(new Vector3Int(x, 0, y));
                        x += dx;
                        y += dy;
                    }
                    else
                    {
                        Piece nextPiece = GetPieceCase(x, y).GetComponent<Piece>();
                        if (piece.IsJ1 != nextPiece.IsJ1)
                        {
                            int nextX = x + dx;
                            int nextY = y + dy;
                            if (gridManager.IsWithinGrid(nextX, nextY) && !gridManager.GetifCaseIsOccuped(nextX, nextY))
                            {
                                // Ajouter la case libre après la pièce ennemie aux mouvements possibles après une attaque
                                caseOnGridQueenAfterEatPos.Add(new Vector3(nextX, 0.0f, nextY));
                                caseOnGridPiecesEatedByQueenPos.Add(new Vector2Int(nextX, nextY), new Vector2Int(x, y));
                                nbCaseMangeable++;
                                piece.CanEat = true;
                            }
                            // Arrêter de vérifier plus loin dans cette direction après avoir traité la pièce ennemie
                            break;
                        }
                        else
                            break;
                    }
                }
            }
        }
        piece.caseOnGridQueenMouvPos = caseOnGridQueenMouvPos;
        piece.caseOnGridQueenAfterEatPos = caseOnGridQueenAfterEatPos;
        piece.caseOnGridPiecesEatedByQueenPos = caseOnGridPiecesEatedByQueenPos;
    }


    /// <summary>
    /// affiche tout les mouvements et Eats possible d'un pion aux coordonnée CaseX,CaseY
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void ShowAllPossibleMouv(Piece piece)
    {
        CheckPossibleMouvement(piece);
        ShowMouvement(piece);
        ShowEatMouvement(piece);

    }
    /// <summary>
    /// affiche tous les mouvs disponibles d'une piece ou d'une reine
    /// </summary>
    /// <param name="piece"></param>
    public void ShowMouvement(Piece piece)
    {
        /// on a ajouté une séparation entre un pion et une reine si on veut ajouter des choses ici
        if (!piece.IsQueen)
        {
            //Debug.Log("showmouv appel ShowPieceMouv");
            ShowPieceMouvement(piece);
        }
        else
        {
            //Debug.Log("on appel showmouv queen");
            ShowQueenMouvement(piece);

        }
    }

    /// <summary>
    /// affiche uniquement les mouv possibles d'une piece. 
    /// </summary>
    /// <param name="piece"></param>
    private void ShowPieceMouvement(Piece piece)
    {
        uIManager.ShowMouvPlane(piece);
    }

    /// <summary>
    /// affiche uniquement les mouv possibles d'une reine. 
    /// </summary>
    /// <param name="piece"></param>
    public void ShowQueenMouvement(Piece piece)
    {
        uIManager.ShowMouvPlane(piece);
    }

    /// <summary>
    /// affiche les Mouvement d'attaque disponible quelque soit la piece
    /// </summary>
    /// <param name="piece"></param>
    public void ShowEatMouvement(Piece piece)
    {
        if (!piece.IsQueen)
        {
            //Debug.Log("on appel showEatMouv appel ShowPieceEat");
            ShowPieceEatMouvement(piece);

        }
        else
        {
            ShowQueenEatMouvement(piece);
        }
    }

    /// <summary>
    /// affiche les Mouvement d'attaque disponible d'une piece
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void ShowPieceEatMouvement(Piece piece)
    {
        uIManager.ShowEatPlane(piece);
    }

    /// <summary>
    /// affiche les Mouvement d'attaque disponible d'une reine
    /// </summary>
    /// <param name="piece"></param>
    public void ShowQueenEatMouvement(Piece piece)
    {

        uIManager.ShowEatPlane(piece);

    }

    /// <summary>
    /// check all the possible mouv of a piece from a position on the grid
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void CheckPossibleMouvement(int caseX, int caseY)
    {
        Piece piece = GetPieceCase(caseX, caseY).GetComponent<Piece>();
        CheckPossibleMouvement(piece);
    }

    /// <summary>
    /// check all the possible mouv of a piece from the Piece script
    /// </summary>
    /// <param name="piece"></param>
    public void CheckPossibleMouvement(Piece piece)
    {
        uIManager.HidePossibleMouv(); // le problème dans plans qui se désactivent viens surement d'ici, a modifié plus tard
        if (!piece.IsQueen)
        {
            //Debug.Log("CheckPossibleMouvement appel CheckPiecePossMouv");
            CheckPiecePossibleMouvement(piece);

        }
        else
        {
            CheckQueenPossibleMouvement(piece);
        }
    }

    /// <summary>
    /// manger la piece
    /// </summary>
    /// <param name="caseX"></param>
    /// <param name="caseY"></param>
    public void EatPiece(int caseX, int caseY)
    {
        GameObject PieceObj = GetPieceCase(caseX, caseY);
        Piece piece = PieceObj.GetComponent<Piece>();
        
        PieceObj.SetActive(false);
        PiecesInGame.Remove(piece);
        if (piece.IsJ1)
        {
            J1ActivePiece--;
        }
        else
        {
            J2ActivePiece--;
        }

        //Debug.Log($"Piece active j1: {J1ActivePiece} / j2: {J2ActivePiece}");
        ServiceLocator.Get<TurnManager>().CheckEndGame();

    }

    public void CheckPromotion(Piece piece)
    {
        if ((piece.IsJ1 && piece.CaseY == 9) || (!piece.IsJ1 && piece.CaseY == 0))
        {
            PromoteToQueen(piece);
        }
    }

    public void PromoteToQueen(Piece piece)
    {
        piece.IsQueen = true;
        piece.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f) * gridManager.TailleCase;
        piece.name = $"Dame {piece.name}";
        Debug.Log("une Dame est apparue");
    }

    /// <summary>
    /// renvoie une postion hors plateau au harsard dans le camps isj1Piece 
    /// </summary>
    /// <param name="isJ1Piece"></param>
    /// <returns></returns>
    public Vector3 GetRandomPositionInPieceCamp(bool isJ1Piece)
    {
        float randomX = UnityEngine.Random.Range(0, gridManager.GridX);
        float randomY = isJ1Piece ? 
            UnityEngine.Random.Range(-5, -3) : UnityEngine.Random.Range(gridManager.GridY + 3, gridManager.GridY + 5);
        RandomReturnPos = new Vector3(randomX, 1, randomY);
        return RandomReturnPos;
    }

    #endregion

    #region event listener
    private void OnEnable()
    {
        EventHandler.OnEndEatPieceAnimation += EatPiece;

    }
    private void OnDisable()
    {
        EventHandler.OnEndEatPieceAnimation -= EatPiece;

    }

    #endregion
}
