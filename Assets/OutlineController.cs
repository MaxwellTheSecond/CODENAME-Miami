using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    [SerializeField] float outlineRange = 100f;
    private Material oldMaterial;
    private Renderer objectRenderer;
    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast (ray, out hit, outlineRange) && hit.transform.tag == "Gun")
        {
            objectRenderer = hit.transform.GetComponent<Renderer>();
            if(oldMaterial == null)
            {
                oldMaterial = hit.transform.GetComponent<Renderer>().material;
            }
            objectRenderer.material = highlightMaterial;
            
        }
        else
        {
          if(oldMaterial != null && objectRenderer != null)
            {
                objectRenderer.material = oldMaterial;
                oldMaterial = null;
                objectRenderer = null;
            }
        }
    }
}
