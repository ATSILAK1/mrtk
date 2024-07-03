using System.Collections;
using System.Collections.Generic;
using Transplan.Common.BusinessObjects.SystemComponents; 
using UnityEngine;
using UnityEngine.UI;
using SystemInfo = Transplan.Common.BusinessObjects.SystemComponents.SystemInfo;

public class BaieScript : MonoBehaviour
{
    private IService1 service = ServiceScript.getInstance();
    [SerializeField]

    private long equipement;
    [SerializeField]
    private GameObject ImagePrefab;
    private Time Time;

    public long equipmentProperty {
        get { return equipement; }
        set { equipement = value; }
                }
    
    // Start is called before the first frame update
    void Start()
    {
        float debutStart, finStart;
        debutStart = Time.unscaledTime;
        Debug.LogError(debutStart);
        
        GetBaie();
        finStart = debutStart - Time.unscaledTime;

        Debug.LogError(finStart);
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
        
    void GetBaie()
    {

        //il faut changer cette ligne pour l'adapter 
        //equipement = FindAnyObjectByType<ButtonInstance>().GetEquipementId();
        float DebutGetBaie, finGetBaie;
        DebutGetBaie = Time.unscaledTime;

        
        Debug.Log(DebutGetBaie);
        
        //changement de la taille du canvas
        var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, service.GetSystemInfo(ServiceScript.user.Id, equipement).CaId);
        GetComponent<RectTransform>().sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);
        // application de l'image 
        Texture2D texture = LoadTexture(service.GetRackImageByUserId(ServiceScript.user.Id, catalog.Catalog.Url));
        var baie = transform.GetChild(0);
        var image = baie.GetComponent<RawImage>();
        if (texture == null)
            Debug.Log("texture null");
        else
            Debug.Log("plein");
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

        finGetBaie = Time.unscaledTime;
        Debug.LogError("fin getbaie :" +(finGetBaie - DebutGetBaie ));
    }
    void GetAllEquipmentInBaie(float slotHeight , CatalogInfos maincat)
    {
        float DebutGetBaie, finGetBaie;
        DebutGetBaie = Time.unscaledTime;

        Debug.LogError("debut getAllEquipmentinBaie "+DebutGetBaie);
        foreach (var systemInfo in  service.GetAllSubSystemInfoList(ServiceScript.user.Id , equipement))
        {
            float subsys = Time.unscaledTime;
            Debug.LogError("debut getAllsubsysteminfo " + subsys);

            var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, systemInfo.CaId);
            var imageObject = Instantiate(ImagePrefab , transform.GetChild(0)  ) ;
            foreach( var item in service.GetAllSubSystemInfo(ServiceScript.user.Id , systemInfo.Id)  )
            {
                float subsys2 = Time.unscaledTime;
                Debug.LogError("debut getAllsubsysteminfo2 " + subsys2);
                if (equipement == item.Id)
                    continue;
                var child =Instantiate(ImagePrefab, imageObject.transform);
                child.name =item.Name+" "+ item.Id.ToString();
                var childCat = service.GetCatInfoByUserId(ServiceScript.user.Id, item.CaId); 
                child.GetComponent<RawImage>().texture = LoadTexture(service.GetRackImageByUserId(ServiceScript.user.Id,childCat.Catalog.Url));
                child.GetComponent<RectTransform>().sizeDelta = new Vector2(childCat.Catalog.Largeur , childCat.Catalog.Hauteur);
               
                IEnumerator enumerator = catalog.Containers.GetEnumerator();
                
                while (enumerator.MoveNext())
                {
                    var element = (CatConteneurInfo)enumerator.Current;
                    Debug.Log(element.Name + " "+ item.Name);

                    if(element.Name == item.Name.Substring(item.Name.Length - 2 ))
                    {
                        child.GetComponent<RectTransform>().localPosition = new Vector2(element.CacX, -childCat.Catalog.Hauteur-element.CacY);
                    }
                    Debug.LogError("fin getAllsubsysteminfo2 " + (Time.unscaledTime - subsys2));

                }
                //child.transform.SetParent(null);
                

            }

            imageObject.name = systemInfo.Id.ToString();
            var imageTexture = LoadTexture(service.GetRackImageByUserId(ServiceScript.user.Id, catalog.Catalog.Url));
            
            imageObject.GetComponent<RawImage>().texture = imageTexture;



            var position = PositionInBaie(systemInfo, maincat, slotHeight);
        
            if (systemInfo.FaceInParent == "Avant")
                imageObject.GetComponent<RectTransform>().anchoredPosition = position;
            else
            {
                imageObject.GetComponent<RectTransform>().localRotation = new Quaternion(0,180,0,0);
                imageObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(maincat.Catalog.Largeur-position.x, position.y, maincat.Catalog.Longueur);
                
            }
            imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(catalog.Catalog.Largeur, catalog.Catalog.Hauteur);

            // partie qui attribue l 'id du sous equipement 
            Debug.LogError("fin subsys EquipmentinBaie " + (Time.unscaledTime -  subsys));

        }
        finGetBaie = Time.unscaledTime;
        Debug.LogError("fin allequipemntgetbaie :" + (finGetBaie - DebutGetBaie));
    }

  


    public double HeightOfSlotInRack(CatalogInfos elem)
    {



        var y1 = elem.Catalog.RackingZones[0].Y1;
        var y2 = elem.Catalog.RackingZones[0].Y2;

        Debug.Log("y1 " + y1 + " y2 " + y2 + " hu" + ((y2 - y1) / elem.Catalog.UHauteur));
        if (elem.Catalog.IsETSI)
        {
            Debug.Log("IsEtsi  " + ((y2 - y1) / elem.Catalog.UHauteur) / 1);
            return ((y2 - y1) / elem.Catalog.UHauteur) / 1;
        }
        else
        {
            Debug.Log("NotEtsi  " + ((y2 - y1) / elem.Catalog.UHauteur) / 3);
            return ((y2 - y1) / elem.Catalog.UHauteur) / 3;
        }
    }
    public Vector3 PositionInBaie(SystemInfo systemInfo, CatalogInfos cat, double slotHeight)
    {


        var Y1 = cat.Catalog.RackingZones[0].Y1;
        var Y2 = cat.Catalog.RackingZones[0].Y2;
        if (cat.Catalog.IsAscending)
        {
            //Dans le cas
            Debug.Log("ascending " + Y2);
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)( (systemInfo.BaiePosition != null ? systemInfo.BaiePosition * slotHeight : 0) - Y2));
        }
        else
        {
            Debug.Log("not ascending");
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)(Y2-  (systemInfo.BaiePosition != null ? systemInfo.BaiePosition * slotHeight : 0)));
        }
    }
}
