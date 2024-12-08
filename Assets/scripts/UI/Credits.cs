

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{
    TextMeshProUGUI text;
    List<Createur> createurs = new List<Createur>();

    Createur Tarodev = new Createur("Tarodev", "les Sprites du PlayButton", "https://www.youtube.com/watch?v=lF26yGJbsQk");
    Createur Kandles = new Createur("Art by Kandles", "les Sprites des SettingButton", "https://assetstore.unity.com/packages/2d/gui/icons/basic-icons-139575");
    Createur CheapProFonts = new Createur("CheapProFonts", "Texte Font", "https://www.fontsquirrel.com/fonts/cowboy-hippie-pro");

    struct Createur
    {
        public string Name ;
        public string AssetDescription ;
        public string UrlSource;

        public Createur(string name, string asset, string Url)
        {
            this.Name = name ;
            this.AssetDescription = asset ;
            this.UrlSource = Url ;
        }

    }
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        ListCreateur();
        AfficherCreateur();
    }

    private void AddCreateur(Createur createur)
    {
        createurs.Add(createur);
    }
    private void AddCreateur(string Name, string Asset, string Url)
    {
        Createur name = new Createur(Name, Asset, Url);
        AddCreateur(name);
    }
    /// <summary>
    /// c'est ici qu'il faut ajouter les créateur a afficher
    /// </summary>
    private void ListCreateur()
    {
        AddCreateur(Tarodev);
        AddCreateur(Kandles);
        AddCreateur(CheapProFonts);
        AddCreateur("Zololgo", "Tous les models 3D", "https://assetstore.unity.com/packages/3d/props/flatpoly-chess-and-checkers-104559");
    }
    private void AfficherCreateur()
    {
        string CreateurText = "";
        foreach (Createur crea in createurs)
        {
            CreateurText += $"<u>{crea.Name}</u> : {crea.AssetDescription}.\n {crea.UrlSource} \n \n";
        }

        text.text = CreateurText;
       
    }


  
}
