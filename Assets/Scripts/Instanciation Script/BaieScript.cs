using System.Collections;
using System.Collections.Generic;
using Transplan.Common.BusinessObjects.SystemComponents;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SystemInfo = Transplan.Common.BusinessObjects.SystemComponents.SystemInfo;
using System.Timers;
public class BaieScript : MonoBehaviour
{

    // Attribut 
    private IService1 service = ServiceScript.getInstance();
    [SerializeField]
    
    private long equipement;
    [SerializeField]
    private GameObject ImagePrefab;
    // Propriete 
    public long equipmentProperty {
                get { return equipement; }
                set { equipement = value; }
                }
    // Start is called before the first frame update
    void Start()
    {
        //resultInfo = service.LoadFullSystemeInfo(ServiceScript.user.Id, new long[equipement]);
        var watch = System.Diagnostics.Stopwatch.StartNew();
        // the code that you want to measure comes here
     

        GetBaie();
     watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log(" start method final " + elapsedMs / 1000);
    }

    // Update is called once per frame
 
    Texture2D LoadTexture(byte[] image)
    {
        Texture2D texture = new Texture2D(2, 2);
        //Debug.Log(image.Length);
        texture.LoadImage(image);

        return texture;
    }
        
    void GetBaie()
    {

        // the code that you want to measure comes here

        //il faut changer cette ligne pour l'adapter 
        //equipement = FindAnyObjectByType<ButtonInstance>().GetEquipementId(); 
        
        //changement de la taille du canvas
        var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, service.GetSystemInfo(ServiceScript.user.Id, equipement).CaId);
        GetComponent<RectTransform>().sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);
        // application de l'image 
        Texture2D texture = Resources.Load("ElementsImages/" + catalog.Catalog.Url.Substring(0, catalog.Catalog.Url.Length - 4)) as Texture2D ;//service.GetRackImageByUserId(ServiceScript.user.Id, catalog.Catalog.Url));
        var baie = transform.GetChild(0);
        var image = baie.GetComponent<RawImage>();
      
        image.texture = texture;


        var rectTransform = baie.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);
        var slotheight = HeightOfSlotInRack(catalog);

        //var cube = transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(catalog.Catalog.Largeur, catalog.Catalog.Hauteur, catalog.Catalog.Longueur) ;
        GetAllEquipmentInBaie((float)slotheight, catalog);
        var cube = transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        cube.localScale = new Vector3(catalog.Catalog.Largeur, catalog.Catalog.Hauteur, catalog.Catalog.Longueur-1);
        cube.anchoredPosition3D = new Vector3(cube.anchoredPosition3D.x,cube.anchoredPosition3D.y,catalog.Catalog.Longueur / 2);

        transform.localScale *= 0.001f;
        
        
    }
    //void GetAllEquipmentInBaie(float slotHeight , CatalogInfos maincat)
    //{
    //    var result = service.GetAllSubSystemInfoList(ServiceScript.user.Id, equipement);


    //    foreach (var systemInfo in  result)
    //    {

    //        var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, systemInfo.CaId);
    //        var imageObject = Instantiate(ImagePrefab , transform.GetChild(0)  ) ;
    //        var subResult = service.GetAllSubSystemInfo(ServiceScript.user.Id, systemInfo.Id);
    //        foreach ( var item in  subResult)
    //        {

    //            if (equipement == item.Id)
    //                continue;
    //            var child =Instantiate(ImagePrefab, imageObject.transform);
    //            child.name =item.Name+" "+ item.Id.ToString();
    //            var childCat = service.GetCatInfoByUserId(ServiceScript.user.Id, item.CaId); 
    //            child.GetComponent<RawImage>().texture =Resources.Load(("ElementsImages/" + childCat.Catalog.Url.Substring(0, childCat.Catalog.Url.Length - 4))) as Texture2D ;
    //            child.GetComponent<RectTransform>().sizeDelta = new Vector2(childCat.Catalog.Largeur , childCat.Catalog.Hauteur);

    //            IEnumerator enumerator = catalog.Containers.GetEnumerator();

    //            while (enumerator.MoveNext())
    //            {
    //                var element = (CatConteneurInfo)enumerator.Current;
    //                Debug.Log(element.Name + " "+ item.Name);

    //                if(element.Name == item.Name.Substring(item.Name.Length - 2 ))
    //                {
    //                    child.GetComponent<RectTransform>().localPosition = new Vector2(element.CacX, -childCat.Catalog.Hauteur-element.CacY);
    //                }


    //            }
    //            //child.transform.SetParent(null);

    //        }

    //        imageObject.name = systemInfo.Id.ToString();
    //        var imageTexture = Resources.Load("ElementsImages/" + catalog.Catalog.Url.Substring(0, catalog.Catalog.Url.Length - 4)) as Texture2D;

    //        imageObject.GetComponent<RawImage>().texture = imageTexture;



    //        var position = PositionInBaie(systemInfo, maincat, slotHeight);

    //        if (systemInfo.FaceInParent == "Avant")
    //            imageObject.GetComponent<RectTransform>().anchoredPosition = position;
    //        else
    //        {
    //            imageObject.GetComponent<RectTransform>().localRotation = new Quaternion(0,180,0,0);
    //            imageObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(maincat.Catalog.Largeur-position.x, position.y, maincat.Catalog.Longueur);

    //        }
    //        imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);


    //    }


    //}


    void GetAllEquipmentInBaie(float slotHeight, CatalogInfos maincat)
    {
        var result = service.GetAllSubSystemInfoList(ServiceScript.user.Id, equipement);
        foreach (var systemInfo in result)
        {
            ProcessSystemInfo(systemInfo, maincat, slotHeight);
        }
    }

    void ProcessSystemInfo(SystemInfo systemInfo, CatalogInfos maincat, float slotHeight)
    {
        var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, systemInfo.CaId); 
        var imageObject = Instantiate(ImagePrefab, transform.GetChild(0));
        ////var subResult = service.GetAllSubSystemInfo(ServiceScript.user.Id, systemInfo.Id);
        //watch2.Stop();
        //Debug.Log("getallsubinfo " + systemInfo.Id + " " + watch2.ElapsedMilliseconds / 1000);
        //var subResult = resultInfo.Systemes.Where(elem => elem.ParentId == systemInfo.Id);
        //foreach (var item in subResult)
        //{
        //    if (equipement == item.Id)
        //        continue;
        //        ProcessSubSystemItem(item, catalog, imageObject);
        //}
        SetImageObjectProperties(imageObject, catalog, systemInfo, maincat, slotHeight);
    }

    void ProcessSubSystemItem(SystemInfo item, CatalogInfos catalog, GameObject imageObject)
    {
       
        var child = Instantiate(ImagePrefab, imageObject.transform);
        child.name = $"{item.Name} {item.Id}"; 
        
        var childCat = service.GetCatInfoByUserId(ServiceScript.user.Id, item.CaId);

        SetChildProperties(child, childCat, item);
        //PositionChildInContainer(child, catalog, item, childCat);
 
    }

    void SetChildProperties(GameObject child, CatalogInfos childCat, SystemInfo item)
    {
        var texturePath = "ElementsImages/" + childCat.Catalog.Url.Substring(0, childCat.Catalog.Url.Length - 4);
        child.GetComponent<RawImage>().texture = Resources.Load(texturePath) as Texture2D;
        child.GetComponent<RectTransform>().sizeDelta = new Vector2(childCat.Catalog.Largeur, childCat.Catalog.Hauteur);
    }

    void PositionChildInContainer(GameObject child, CatalogInfos catalog, SystemInfo item, CatalogInfos childCat)
    {
        foreach (CatConteneurInfo element in catalog.Containers)
        {
            // element name A1 
            // item.name xxxx-xxxxx-xxxxx-xxxx-A1
            if (element.Name == item.Name.Substring(item.Name.Length - 2))
            {
                child.GetComponent<RectTransform>().localPosition = new Vector2(element.CacX, -childCat.Catalog.Hauteur - element.CacY);
                break;
            }
        }
    }

    void SetImageObjectProperties(GameObject imageObject, CatalogInfos catalog, SystemInfo systemInfo, CatalogInfos maincat, float slotHeight)
    {
        
        imageObject.name = systemInfo.Id.ToString();
        var texturePath = "ElementsImages/" + catalog.Catalog.Url.Substring(0, catalog.Catalog.Url.Length - 4);
        imageObject.GetComponent<RawImage>().texture = Resources.Load(texturePath) as Texture2D;


        var position = PositionInBaie(systemInfo, maincat, slotHeight);

        var rectTransform = imageObject.GetComponent<RectTransform>();
        if (systemInfo.FaceInParent == "Avant")
        {
            rectTransform.anchoredPosition = position;
        }
        else
        {
            rectTransform.localRotation = Quaternion.Euler(0, 180, 0);
            rectTransform.anchoredPosition3D = new Vector3(maincat.Catalog.Largeur - position.x, position.y, maincat.Catalog.Longueur);
        }

        rectTransform.sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);
        
    }


    public double HeightOfSlotInRack(CatalogInfos elem)
    {
        
        if (elem.Catalog.CategoryName != "Baie")
            return 1;
        
        var y1 = elem.Catalog.RackingZones[0].Y1;
        var y2 = elem.Catalog.RackingZones[0].Y2;

    
        if (elem.Catalog.IsETSI)
        {
          
            return ((y2 - y1) / elem.Catalog.UHauteur) / 1;
        }
        else
        {

            return ((y2 - y1) / elem.Catalog.UHauteur) / 3;
           
        }
       
    }
    public Vector3 PositionInBaie(SystemInfo systemInfo, CatalogInfos cat, double slotHeight)
    {
      
        var Y1 = cat.Catalog.RackingZones[0].Y1;
        var Y2 = cat.Catalog.RackingZones[0].Y2;
        if (cat.Catalog.IsAscending)
        {
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)( (systemInfo.BaiePosition != null ? systemInfo.BaiePosition * slotHeight : 0) - Y2));
        }
        else
        {
         
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)(Y2-  (systemInfo.BaiePosition != null ? systemInfo.BaiePosition * slotHeight : 0)));
        }
    }
}
