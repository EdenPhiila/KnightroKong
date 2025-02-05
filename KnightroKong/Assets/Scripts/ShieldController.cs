using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.gameObject.tag == "Player")
        {
            if (_other.gameObject.GetComponent<PlayerController>().invincible == false)
            {
                Destroy(gameObject);

                _other.gameObject.GetComponent<PlayerController>().Shield();
            }
        }
    }
}
