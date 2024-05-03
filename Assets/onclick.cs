using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onclick : MonoBehaviour
{
    [SerializeField]
    private Stack<GameObject> visualStack = new Stack<GameObject>();
    [SerializeField]
    private List<GameObject> uiList = new List<GameObject>();
    [SerializeField]
    public  GameObject dialogue;

    private void Start()
    {
        visualStack.Push(GameObject.Find("MainMenu"));
    }
    public void onClickOnMenu(string name)
    {
        
        GameObject gameObject = null ;
        
        foreach (GameObject elements in uiList)
        {
            if (elements.name.Equals(name))
            {
                
            }
        }

    }

}
