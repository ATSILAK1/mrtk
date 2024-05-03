using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField]
    private int id { get; set; }
    [SerializeField]
    private string serverName { get; set; }
    
    private float memoryUsage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.id = 1;
        this.serverName = "Server 1";
        this.memoryUsage = 59;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
