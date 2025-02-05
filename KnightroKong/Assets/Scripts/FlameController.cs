using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            _other.gameObject.GetComponent<PlayerController>().Hit();
        }
    }
}
