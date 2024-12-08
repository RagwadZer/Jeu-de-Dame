using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    public List<GameObject> mouvPlanes { get; set; }
    public List<GameObject> eatPlanes { get; set; }

    private void Awake()
    {
        ServiceLocator.Register<PlaneManager>(this);
    }

    public void ShowMouvPlane(Piece piece)
    {
        /* //* Test *\\ : j'ai cette fonction qui doit afficher des plans sur certaines positions,
         * les positions sont contenues dans le script piece
         * dans la valeur caseOnGridPossibleMouvPosition pour notre cas. nous avons déjà 30 plans a disposition
         * il y aura plusieurs pieces qui devront potentiellement afficher plusieurs plans.
         * ce que je veux c'est que utiliser les plans inactifs uniquement pour ne pas desactivé les plans des autres pieces, 
         * pour ça,j'ai activesMouvPlane qui se crée a partir des plan inactifs dans mouvPlanes
         * selon piece.nbPossibleMouv , et activeMouvPlanes qui stockera les planes activées pour chaque piece.
         * */

        //on arrete tout si la piece n'as pas de mouvement pour ne pas faire de calcul pour rien
        if (piece.NbPossibleMouv == 0) return;

        // Récupérer uniquement les plans inactifs parmi mouvPlanes
        //a la base j'ai utilisé ceci :
        List<GameObject> inactiveMouvPlanes = mouvPlanes.Where(plane => !plane.activeSelf).Take(piece.NbPossibleMouv).ToList();

        //mais j'ai gardé ceci a cause des plans activés qui se faisait écraser, ça ma parraissait plus claire 
        /*List<GameObject> inactiveMouvPlanes = new List<GameObject>();
        foreach (GameObject plan in mouvPlanes)
        {
            if(inactiveMouvPlanes.Count >= piece.nbPossibleMouv) break;
            if (!plan.activeSelf)
            {
                if (!inactiveMouvPlanes.Contains(plan))
                {
                    inactiveMouvPlanes.Add(plan);
                }
            }
        }*/

        Debug.Log($"la {piece.name} demande {piece.NbPossibleMouv} planes, " +
            $"on a prit {inactiveMouvPlanes.Count} planes inactifs sur les {mouvPlanes.Count} total");

        // Boucle pour activer les plans nécessaires
        for (int i = 0; i < piece.NbPossibleMouv; i++)
        {
            ///implémenter la capture obligatoire, le bug est peut etre ici
            ///
            ///
            ///
            // Vérifier que nous avons suffisamment de plans inactifs pour les mouvements possibles
            if (inactiveMouvPlanes.Count < piece.NbPossibleMouv)
            {
                Debug.LogError($"Pas assez de plans disponibles pour afficher tous les mouvements possibles pour la pièce {piece.name}");
                return;
            }

            //on récupère le plan lans la liste des planes inactifs
            GameObject planeObject = inactiveMouvPlanes[i];
            Plane plane = planeObject.GetComponent<Plane>();

            // Récupérer la position de mouvement
            List<Vector3> possibleMouv = piece.IsQueen ? piece.caseOnGridQueenMouvPos : piece.caseOnGridPossibleMouvPosition;
            Vector3 caseCenter = ServiceLocator.Get<GridManager>().GetCaseCenter(possibleMouv[i].x, possibleMouv[i].z);
            Vector3 caseWorldPos = new Vector3(caseCenter.x, 0.0f, caseCenter.z);

            // Initialiser le plane
            plane.CaseX = (int)possibleMouv[i].x;
            plane.CaseY = (int)possibleMouv[i].z;
            plane.BaseColor = piece.BaseColor;
            plane.HoovedColor = piece.HoovedColor;
            plane.GetComponent<MeshRenderer>().material.color = piece.BaseColor;

            // Positionner et activer le plane
            planeObject.transform.position = caseWorldPos;
            planeObject.SetActive(true);

            Debug.Log($"Plan {plane.name} activé pour la {piece.name} : Position {caseWorldPos} / actif : {plane.gameObject.activeSelf}");
        }
    }

    public void ShowEatPlane(Piece piece)
    {
        for (int i = 0; i < piece.NbCapturePossible; i++)
        {
            if (i >= mouvPlanes.Count)
            {
                Debug.Log("Index hors des limites de la liste planes");
                break;
            }
            //on choisit la liste a utilisé en fonction de si la piece est une reine
            List<Vector3> listToUse = piece.IsQueen ? piece.caseOnGridQueenAfterEatPos : piece.caseOnGridPossibleAfterEatMouvPosition;
            Vector3 caseCenter = ServiceLocator.Get<GridManager>().GetCaseCenter(listToUse[i].x, listToUse[i].z);
            Vector3 worldPos = new Vector3(caseCenter.x, 0.0f, caseCenter.z);

            Plane plane = eatPlanes[i].GetComponent<Plane>();
            plane.CaseX = (int)piece.caseOnGridPossibleAfterEatMouvPosition[i].x;
            plane.CaseY = (int)piece.caseOnGridPossibleAfterEatMouvPosition[i].z;
            plane.BaseColor = piece.BaseColor;
            plane.HoovedColor = piece.HoovedColor;
            plane.GetComponent<MeshRenderer>().material.color = piece.BaseColor;


            plane.transform.position = worldPos;
            plane.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// reset tous les planes
    /// </summary>
    public void HidePossibleMouv()
    {
        foreach (GameObject item in mouvPlanes)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in eatPlanes)
        {
            item.SetActive(false);
        }
    }


}
