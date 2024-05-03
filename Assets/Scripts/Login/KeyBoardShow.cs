using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardShow : MonoBehaviour
{

    [SerializeField]
    private TouchScreenKeyboard keyBoard;



    public void OpenSystemKeyboard()
    {
        keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }
    // Start is called before the first frame update
    void Start()
    {
        OpenSystemKeyboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
