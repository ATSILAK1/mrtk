using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MixedReality.Toolkit.UX;

public class ButtonInstance : MonoBehaviour
{

    public GameObject preFab;
    private Service1Client service = ServiceScript.getInstance();
    private UserInfo user;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.GetString("login"), PlayerPrefs.GetString("motdepasse")
        user = service.GetUser("PJJD4552", "Nourra123456@", "LDAP://vipadyleg.si.francetelecom.fr:636/DC=ad,DC=francetelecom,DC=fr", "AD");
        InitButtonForEquipment(user.Id, 1067457);// 
    }

    public void InitButtonForEquipment(long userId, long sysId)
    {

        foreach (var elem in service.GetAllSubSystemInfo(userId, sysId))
        {
            var obj = Instantiate(preFab, transform);
            EquipementButton component = obj.GetComponent<EquipementButton>();
            component.IdEquipement = elem.Id;
            var child = obj.transform.Find("Frontplate/AnimatedContent/Icon/Label").gameObject.GetComponent<TMP_Text>();
            child.text = elem.Name + " ";
            Debug.Log(elem.Name);
        }
    }

}