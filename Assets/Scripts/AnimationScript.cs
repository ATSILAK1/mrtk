using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using UnityEngine;
using TMPro;


public class AnimationScript : MonoBehaviour
{
    
    public TMP_Text text; 
    // Start is called before the first frame update
    void Start()
    {
        
        var service1 = ServiceScript.getInstance();
        
        var user = ServiceScript.getInstance().GetUser("PJJD4552", "Nourra123456@", "LDAP://vipadyleg.si.francetelecom.fr:636/DC=ad,DC=francetelecom,DC=fr", "AD");

        //var systeme = ServiceScript.getInstance().GetSystemWithAllSubSystemAsync(user.Id, 271158);

       
        Debug.Log(user.Name != null ? "plein" : "Rien");
        
        //var cat = service1.GetCatInfoByUserId(user.Id, 13125).Catalog;
        //text.text  = "Nom :" + cat.Name;
        //text.text += "\nUrl :" + cat.Url;
        //text.text += "\nTemperature Max :" + cat.TemperatureMax;
        //text.text += "\nCommentaire :" + cat.Commentaire;
        //text.text += "\nPuissance :" + cat.Puissance;
        //text.text += "\nState :" + cat.State;
        
        
        

     //   text.text += "\n =========================== Sub System ==================================";

        Debug.Log(service1.GetChildrenOfSystem(user.Id, 271158).GetValue(1));
        foreach (var elem in service1.GetChildrenOfSystem(user.Id, 271158))
            Debug.Log(elem);
      
        //foreach(var elem in service1.GetAllSubSystemInfo(284309))
        //{
        //    Debug.Log(elem.Id + "  " + elem.Name);
        //}

      
    }

   
}
