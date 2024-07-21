using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Transplan.Common.BusinessObjects.SystemComponents;
// Besoin de CatalogueInfo pour les dimension
//

public class EquipementScript : MonoBehaviour
{
    private IService1 service = ServiceScript.getInstance();
    private ButtonInstance buttonInstance;
    public float scale = 0.001f;
    public GameObject imageRawObject; 


    // Start is called before the first frame update
    void Start()
    {
        buttonInstance = FindAnyObjectByType<ButtonInstance>();
        Debug.Log(buttonInstance.GetEquipementId());
        var catalog = service.GetCatInfoByUserId(ServiceScript.user.Id, service.GetSystemInfo(ServiceScript.user.Id, buttonInstance.GetEquipementId()).CaId);
        //ResizeCube(catalog.Catalog.Longueur, catalog.Catalog.Largeur, catalog.Catalog.Hauteur);
        var rackImage = service.GetRackImageByUserId(ServiceScript.user.Id, catalog.Catalog.Url);
        var canvas = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        
        var image = FindAnyObjectByType<UnityEngine.UI.RawImage>();
        image.texture = LoadTexture(rackImage);
        image.transform.localScale = new Vector3(catalog.Catalog.Largeur, catalog.Catalog.Hauteur) * scale;
        canvas.rect.Set(canvas.rect.x, canvas.rect.y, catalog.Catalog.Largeur * scale, catalog.Catalog.Hauteur * scale);
        var height = HeightOfSlotInRack(catalog);
        var rackingZoneWidth = catalog.Catalog.RackingZones[0].X2 - catalog.Catalog.RackingZones[0].X1;
        GetImageOfAllEquipment((float)rackingZoneWidth,height , catalog);
        
    }
    
    void GetImageOfAllEquipment(float rackingZoneWidth ,double slotHeight , CatalogInfos mainEquipementCatalogue)
    {
      
        foreach(Transplan.Common.BusinessObjects.SystemComponents.SystemInfo elem in service.GetAllSubSystemInfoList(ServiceScript.user.Id,buttonInstance.GetEquipementId()))
            {
            var instance = Instantiate(imageRawObject,gameObject.transform.GetChild(0).GetChild(1));
            instance.name = elem.Name + " " + elem.Id + " "+elem.FaceInParent;
            
            var obj = instance.GetComponent<UnityEngine.UI.RawImage>();
            // On Recupere le Catalogue de l'element 
            CatalogInfos catalogue = service.GetCatInfoByUserId(ServiceScript.user.Id, elem.CaId);
            var image = service.GetRackImageByUserId(ServiceScript.user.Id, catalogue.Catalog.Url);
            obj.texture = LoadTexture(image);
            obj.transform.localScale = new Vector3(rackingZoneWidth, catalogue.Catalog.Hauteur) * scale ;
            Vector3 position = PositionInBaie(elem, mainEquipementCatalogue, slotHeight);
            
            
            if (elem.FaceInParent == "Avant")
            instance.GetComponent<RectTransform>().localPosition = new Vector3(position.x,(float) elem.BaiePosition, 0) * 0.01f ;
            else
            instance.GetComponent<RectTransform>().localPosition = new Vector3(position.x, (float)elem.BaiePosition, catalogue.Catalog.Largeur) * 0.01f;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ResizeCube(float longeur, float largeur, float hauteur)
    {
        transform.localScale = new Vector3(longeur, hauteur, largeur) *0.01f;
    }

    void AttachImageToCube(byte[] image)
    {
        var component = GetComponent<Renderer>();
        if (component == null || component.material == null) return;

        
        foreach (var material in GetComponent<Renderer>().materials)
        {
            material.mainTexture = LoadTexture(image);
            Debug.Log("Main Texture");
        }
    }

    Texture2D LoadTexture(byte[] image)
    {
        Texture2D texture = new Texture2D(2, 2);
        //Debug.Log(image.Length);
        texture.LoadImage(image);
        
        return texture;
    }

    public double HeightOfSlotInRack(CatalogInfos elem)
    {
      
        
     
        var y1 = elem.Catalog.RackingZones[0].Y1;
        var y2 = elem.Catalog.RackingZones[0].Y2;

        Debug.Log("y1 " + y1 + " y2 " + y2 + " hu" +( (y2 - y1) / elem.Catalog.UHauteur));
        if (elem.Catalog.IsETSI)
        {
            Debug.Log("IsEtsi  " + ((y2 - y1) / elem.Catalog.UHauteur) / 3);
            return ((y2 - y1) / elem.Catalog.UHauteur) / 3;
        }
        else
        {
            Debug.Log("NotEtsi  " + ((y2 - y1) / elem.Catalog.UHauteur) / 1);
            return ((y2 - y1) / elem.Catalog.UHauteur) / 1;
        }  
    }
    public Vector3 PositionInBaie(Transplan.Common.BusinessObjects.SystemComponents.SystemInfo systemInfo, CatalogInfos cat , double slotHeight )
    {
     

        var Y1 = cat.Catalog.RackingZones[0].Y1;
        var Y2 = cat.Catalog.RackingZones[0].Y2;
        if (cat.Catalog.IsAscending)
            //Dans le cas 
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)(Y2 - (systemInfo.BaiePosition != null ? systemInfo.BaiePosition : 0 * slotHeight * scale)));
        else
            return new Vector3((float)cat.Catalog.RackingZones[0].X1, (float)(Y1 + (systemInfo.BaiePosition != null ? systemInfo.BaiePosition : 0 * slotHeight * scale)));

    }
}
