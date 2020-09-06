using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    GravityAttractor planet;
    Rigidbody rbody;

    void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
        rbody = GetComponent<Rigidbody>();

        // Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rbody.useGravity = false;
        rbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Allow this body to be influenced by planet's gravity
        planet.Attract(rbody);
    }
}
