using System.ServiceModel;
using System.Threading.Tasks;
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
            // user =  ServiceScript.getInstance().GetUserAsync(login, motdepasse, "Link", "AD");
            Task task = new Task(async () =>
            {
              UserInfo user =  await ServiceScript.getInstance().GetUserAsync(login, motdepasse, "link", "AD");
            });
            task.Start();

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
