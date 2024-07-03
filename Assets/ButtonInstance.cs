using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MixedReality.Toolkit.UX;

// ce script permet de cree des button de chaque equipement dans une baie 
public class ButtonInstance : MonoBehaviour
{

    public GameObject preFab;
    private Service1Client service = ServiceScript.getInstance();
    private UserInfo user;
    private long systemId = 1067457//1075953
; // ;

    public long GetEquipementId()
    {
        return systemId;
    }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.GetString("login"), PlayerPrefs.GetString("motdepasse")
        user = service.GetUser("PJJD4552", "Nourra123456@", "LDAP://vipadyleg.si.francetelecom.fr:636/DC=ad,DC=francetelecom,DC=fr", "AD");
        InitButtonForEquipment(user.Id, systemId);//271158 - 1070627

        //foreach(Transplan.Common.BusinessObjects.SystemComponents.SystemInfo elem in service.GetAllSubSystemInfoList(ServiceScript.user.Id , 1070627))
        //{
        //    Debug.Log(elem.Id + " baie position " + elem.);
        //    var cat = service.GetCatInfoByUserId(ServiceScript.user.Id, elem.CaId);
        //    foreach(var cat1 in service.GetCatInfoByUserId(ServiceScript.user.Id , elem.CaId).Catalog.RackingZones)
        //    {
        //        Debug.Log(cat1.X1 + " " + cat1.X2 + " " + cat1.Y1 + " " + cat1.Y2);
        //    }
        //}
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
            //Debug.Log(elem.Name);
        }
    }

}