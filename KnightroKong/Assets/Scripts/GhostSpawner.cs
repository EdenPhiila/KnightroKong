using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghost;
    public GameObject lastGhost;
    public float interval = 200;
    private float counter = 0;
    private int choice;
    private bool active = false;
    [SerializeField] AudioClip ghostAudio;

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += 1;

        if (active)
        {

            if (counter >= interval)
            {
                lastGhost = Instantiate(ghost, transform.position, transform.rotation);
                lastGhost.SetActive(true);
                GetComponent<AudioSource>().PlayOneShot(ghostAudio);
                counter = 0;

            }
        }

        else 
        {
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.gameObject.CompareTag("Flame"))
        { 
            active = true;
            Destroy(_other.gameObject);
        }
    }

}
  