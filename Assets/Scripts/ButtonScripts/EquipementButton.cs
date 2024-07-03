using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipementButton : MonoBehaviour
{
    [SerializeField]
    private long idEquipement;
    [SerializeField]
    private TMP_Text catalogueText;
    private GameObject textGameObject;
    private IService1 service = ServiceScript.getInstance();
    
    public long IdEquipement {
        get { return idEquipement; }
        set { idEquipement = value; }
    }
    private void Start()
    {
       textGameObject = GameObject.Find("EquipementText");
       catalogueText = GameObject.Find("dataCenterText").GetComponent<TMP_Text>();
     

    }
    public void OnClickButtonEquipement()
    {
        var systeme = service.GetSystemInfo(ServiceScript.user.Id, idEquipement);

        var elem = textGameObject.GetComponent<TMP_Text>();
        elem.text  = "Nom : "+systeme.Name;
        elem.text += "\nId Parent: " + systeme.PereId;
        elem.text += "\nCatalogue ID :" + systeme.CaId;
        elem.text += "\nCode 26E : "+ (systeme.Code26E != null ? systeme.Code26E : "Indisponible" );
        elem.text += "\nCommentaire" + systeme.Comment;
        elem.text += "\nPos X :" + systeme.OffsetX + "Pos Y :" + systeme.OffsetY + "Pos Z :" + systeme.OffsetZ;
        elem.text += "\nDate Update :" + systeme.DateUpdate;
        elem.text += "\nLabel X et Label Y" + systeme.LabelX + " " + systeme.LabelY;

        var cat = service.GetCatInfoByUserId(ServiceScript.user.Id, systeme.CaId).Catalog;

        catalogueText.text  = "Nom :" + cat.Name;
        catalogueText.text += "\nId :" + cat.Id;
        catalogueText.text += "\nUrl :" + cat.Url;
        catalogueText.text += "\nTemperature Max :" + cat.TemperatureMax;
        catalogueText.text += "\nCommentaire :" + cat.Commentaire;
        catalogueText.text += "\nPuissance :" + cat.Puissance;
        catalogueText.text += "\nState :" + cat.State;
        catalogueText.text += "\nLongueur     hauteur  largeur" + cat.Longueur + " " + cat.Hauteur+" "+cat.Largeur;

        foreach(var component in Component.FindObjectsOfType<Outline>())
        {
            component.enabled = false; 
        }
        GameObject.Find(systeme.Id.ToString()).GetComponent<Outline>().enabled = true ;


        
    }

}
