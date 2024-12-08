using UnityEngine.SceneManagement;
using UnityEngine;

public class ReloadButtonScript : UIClickable
{
    public override void OnClickHandler()
    {
        ReloadScene();
    }

   private void ReloadScene()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   } 
}
