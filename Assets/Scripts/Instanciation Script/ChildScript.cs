
using UnityEngine;
using UnityEngine.UI;
using SystemInfo = Transplan.Common.BusinessObjects.SystemComponents.SystemInfo;

public class ChildScript : MonoBehaviour
{
    [SerializeField]
    private long idSousEquipement;
    private IService1 service = ServiceScript.getInstance();
    [SerializeField]
    private GameObject ImagePrefab;

    public long IdSousEquipement
        {
        get { return idSousEquipement; }
        set { idSousEquipement = value; }
        }
    
    // Start is called before the first frame update
    private void Awake()
    {
        ImagePrefab = GameObject.Find("EquipementImage");
    }
    void Start()
    {
        CatalogInfos maincat = service.GetCatInfoByUserId(ServiceScript.user.Id, service.GetSystemInfo(ServiceScript.user.Id , idSousEquipement).CaId);
        Debug.Log(maincat.Catalog.Id + " "+ idSousEquipement);
   
            HeightOfSlotInRack(maincat);
        
            foreach (SystemInfo systemInfo in service.GetAllSubSystemInfo(ServiceScript.user.Id, idSousEquipement))
            {
                var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, systemInfo.CaId);
                var imageObject = Instantiate(ImagePrefab, transform);
                
                imageObject.name = systemInfo.Name + " " + systemInfo.Id;
                var imageTexture = LoadTexture(service.GetRackImageByUserId(ServiceScript.user.Id, catalog.Catalog.Url));

               
                imageObject.GetComponent<RawImage>().texture = imageTexture;

                var position = Vector3.zero;
                if (systemInfo.FaceInParent == "Avant")
                    imageObject.GetComponent<RectTransform>().anchoredPosition = position;
                else
                    imageObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(position.x, position.y, maincat.Catalog.Largeur);

                
                imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);

            }
        
    }


    public void HeightOfSlotInRack(CatalogInfos elem)
    {


        if (elem.Catalog.RackingZones == null)
            return ;

       

        foreach (var item in elem.Catalog.RackingZones)
        {
            Debug.Log("y1 " + item.Y1+ " y2 " + item.Y2 + " x1 " + item.X1 + " X2 " + item.X2);
        }

        //Debug.Log("y1 " + y1 + " y2 " + y2 + " hu" + ((y2 - y1) / elem.Catalog.UHauteur));
        //if (elem.Catalog.IsETSI)
        //{
        //    Debug.Log("IsEtsi  " + ((y2 - y1) / elem.Catalog.UHauteur) / 1);
        //    return ((y2 - y1) / elem.Catalog.UHauteur) / 1;
        //}
        //else
        //{
        //    Debug.Log("NotEtsi  " + ((y2 - y1) / elem.Catalog.UHauteur) / 3);
        //    return ((y2 - y1) / elem.Catalog.UHauteur) / 3;
        //}
    }
    public Vector3 PositionInBaie(SystemInfo systemInfo, CatalogInfos cat, double slotHeight)
    {

        if (slotHeight == 14)
            return Vector3.zero;

        var Y1 = cat.Catalog.RackingZones[0].Y1;
        var Y2 = cat.Catalog.RackingZones[0].Y2;

        if (cat.Catalog.IsAscending)
        {
            //Dans le cas
            Debug.Log("ascending " + Y2);
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)((systemInfo.BaiePosition != null ? systemInfo.BaiePosition * slotHeight : 0) - Y2));
        }
        else
        {
            
            Debug.Log("not ascending");
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)(Y2 - (systemInfo.BaiePosition != null ? systemInfo.BaiePosition * slotHeight : 0)));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    Texture2D LoadTexture(byte[] image)
    {
        Texture2D texture = new Texture2D(2, 2);
        //Debug.Log(image.Length);
        texture.LoadImage(image);

        return texture;
    }
}
