using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{

    public string levelToLoad;

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
        if (_other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            _other.gameObject.GetComponent<PlayerController>().nextLevel = levelToLoad;
            _other.gameObject.GetComponent<PlayerController>().win = true;
            _other.gameObject.GetComponent<PlayerController>().NextLevel();
        }
    }

}
