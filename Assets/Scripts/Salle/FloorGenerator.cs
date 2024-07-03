using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FloorGenerator : MonoBehaviour
{
    private long idRoom = 8856 ;
    private IService1 service = ServiceScript.getInstance();
    [SerializeField]
    private GameObject plane;
    
    public long IdRoom {
        get { return idRoom; } }
    // Start is called before the first frame update
    void Start()
    {
        GenerateFloorInRoom(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenerateFloorInRoom()
    {
        DalleInfo[] dalle = service.GetDalleInfoByRoom(ServiceScript.user.Id, idRoom);
       
        foreach (var item in dalle)
        {
            var obj = Instantiate(plane);
            obj.name = item.Id.ToString();      
            obj.transform.position = new Vector3(item.PosX, 0, item.PosY)*0.001f;
            obj.transform.localScale = new Vector3(item.Height, 1, item.Width)*0.001f;
            

        }
    }

   
}
