/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSound : MonoBehaviour
{
    //Spaces in inspecter to set the different sounds for different layers
    public AudioClip grassFootstepSound;
    public AudioClip concreteFootstepSound;
    public AudioClip woodFootstepSound;

    //Ray that detects the layer
    public float raycastDistance = 0.1f;

    //Spaces in inspecter to set the different layers for detection
    public LayerMask grassLayer;
    public LayerMask concreteLayer;
    public LayerMask woodLayer;

    //audio source which will be storing and playing the clips
    AudioSource aud;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound()
    {
        // Check if the object is on the grass layer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, grassLayer))
        {
            //Set the grass footstep sound
            //aud.Stop();
            aud.clip = grassFootstepSound;
        }
        else if ((Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, concreteLayer)))
        {
            // if player is on concrete
            //aud.Stop();
            aud.clip = concreteFootstepSound;
        }
        else if ((Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, woodLayer)))
        {
            //if player is on wood
            //aud.Stop();
            aud.clip = woodFootstepSound;
        }
        else
        {
            //if no layer detected
            aud.clip = null;
        }

        //Randomizes pitch and volume of the footsteps
        aud.pitch = Random.Range(0.5f, 1.5f);
        aud.volume = Random.Range(0.5f, 1.5f);

        // Play the footstep sound
        aud.Play();
    }
}
*/