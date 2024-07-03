using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGenerator : MonoBehaviour
{

    private IService1 service = ServiceScript.getInstance();
    [SerializeField]
    private GameObject baie ; 
    // Start is called before the first frame update
    void Start()
    {
        GeneratorEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GeneratorEquipment()
    {
       long idroom = GetComponent<FloorGenerator>().IdRoom;
        Debug.Log(idroom);
        var results = service.GetRoomSystem(ServiceScript.user.Id, idroom);

        foreach (var item in results)
        {
            //Debug.Log(item.DalleId.ToString());
            //GameObject gameObject = GameObject.Find(item.DalleId.ToString());
            //Debug.Log(gameObject.name);
            var obj = Instantiate(baie);
            obj.transform.GetChild(0).GetComponent<BaieScript>().equipmentProperty = item.Id;
            
            //obj.transform.position = gameObject.transform.position;
        }

    }
}
