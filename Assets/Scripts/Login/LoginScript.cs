using System.ServiceModel;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField loginTMP;
    [SerializeField]
    private TMP_InputField motDePasseTMP;
    [SerializeField]
    private TMP_Text false_Password;
   

   public async void SeConnecter()
    {
        string login = loginTMP.text;
        string motdepasse = motDePasseTMP.text;

        try
        {
            UserInfo user =  ServiceScript.getInstance().GetUser(login, motdepasse, "LDAP://vipadyleg.si.francetelecom.fr:636/DC=ad,DC=francetelecom,DC=fr", "AD");
        }catch(FaultException e)
        {
            false_Password.text = "Mot de Passe Incorrect";
            Debug.LogError(e.Message);
            return;
        }

        PlayerPrefs.SetString("login", login);
        PlayerPrefs.SetString("motdepasse", motdepasse);
        
        SceneManager.LoadScene(1);
         
    }
    
}
