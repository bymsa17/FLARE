using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3D : MonoBehaviour {

    private void OnMouseDown()
    {
        // Al pulsar click encima. Se ejecuta 1 frame.
        Debug.Log(transform.name + " Down");
    }

    private void OnMouseEnter()
    {
        // Al entrar en el collider. 1 frame.
        Debug.Log(transform.name + " Enter");
    }

    void OnMouseExit()
    {
        // Al salir del collider. 1 frame.
        Debug.Log(transform.name + " Exit");
    }

    private void OnMouseUp()
    {
        // Primero has hecho click en el collider, y ahora dejas de hacer click en cualquier sitio. No tiene porque ser en el collider. 1 frame.
        Debug.Log(transform.name + " Up");
    }

    private void OnMouseUpAsButton()
    {
        // Has hecho click en el collider, y has dejado de hacer click en el collider. 1 frame.
        Debug.Log(transform.name + " UpAsButton");
    }

    private void OnMouseDrag()
    {
        // Click en el collider y arrastrar.
        Debug.Log(transform.name + " Drag");
    }

    private void OnMouseOver()
    {
        // Raton encima del collider.
        Debug.Log(transform.name + " Over");
    }
}
