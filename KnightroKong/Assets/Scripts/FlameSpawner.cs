using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpawner : MonoBehaviour
{
    public GameObject redFlame;
    public GameObject greenFlame;
    public GameObject blueFlame;
    public GameObject lastFlame;
    public float interval = 100;
    private float counter = 0;
    private int choice;
    [SerializeField] AudioClip flameSpawn;

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += 1;

        if (counter >= interval)
        {
            GetComponent<AudioSource>().PlayOneShot(flameSpawn);
            ChooseFlame();
            counter = 0;

        }
    }

    void ChooseFlame() 
    {
        choice = Random.Range(0, 3);

        if (choice == 0)
        {
            lastFlame = Instantiate(redFlame, transform.position, transform.rotation);
            lastFlame.SetActive(true);
        }
        else if (choice == 1)
        {
            lastFlame = Instantiate(blueFlame, transform.position, transform.rotation);
            lastFlame.SetActive(true);
        }
        else if (choice == 2)
        {
            lastFlame = Instantiate(greenFlame, transform.position, transform.rotation);
            lastFlame.SetActive(true);
        }
    }
}
  