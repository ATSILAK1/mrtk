using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using UnityEngine;

public class ServiceScript : MonoBehaviour
{
    //PlayerPrefs.GetString("login"), PlayerPrefs.GetString("motdepasse"),
    // Start is called before the first frame update
    static private Service1Client clientService;
    static public UserInfo user;



    private ServiceScript() { }


    static public Service1Client getInstance()
    {
       
               if (clientService == null)
                {

                    var httpTransportBindingElement = new HttpTransportBindingElement
                    {
                        MaxReceivedMessageSize = int.MaxValue,
                        MaxBufferPoolSize = int.MaxValue,
                        MaxBufferSize = int.MaxValue,
                        
                        
                    };

                    TextMessageEncodingBindingElement textMessageEncodingBindingElement = new TextMessageEncodingBindingElement
                    {
                        MessageVersion = MessageVersion.Soap11,
                        WriteEncoding = Encoding.Default,
                        
                    };

                    // 65536 * 50;
                    var binding = new CustomBinding(
                        textMessageEncodingBindingElement,
                        httpTransportBindingElement
                    );

                    clientService = new Service1Client(binding, new EndpointAddress("http://localhost:55303/Service1.svc"));

                    

                 }
               // il faut changer Le login et le mot de passe par le playerPrefs
               user =  clientService.GetUser("PJJD4552", "Nourra123456@", "LDAP://vipadyleg.si.francetelecom.fr:636/DC=ad,DC=francetelecom,DC=fr", "AD");
        return clientService;
           
       
    }
   
}
