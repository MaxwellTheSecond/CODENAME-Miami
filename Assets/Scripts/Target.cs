using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Shader _dissolveShader;
    [SerializeField] float _dissolveDuration = 1f;
    [SerializeField] float _health = 100f;
    public void TakeDamage(float amount)
    {
        _health-=amount;
        if(_health<=0)
        {
            Die();
        }
    }
    private void Die()
    {   
        Color oldColor = gameObject.GetComponent<Renderer>().material.color;
        Material newMaterial = gameObject.GetComponent<Renderer>().material;
        newMaterial.shader = _dissolveShader;
        gameObject.GetComponent<Renderer>().material=newMaterial;
        //gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_OldColor",oldColor);
        gameObject.GetComponent<Renderer>().material.SetFloat("_DissolveAmount",0f);
        StartCoroutine("Dissolve");
    }

    IEnumerator Dissolve()
    {
        float dissolveAmount = 0f;
        while(dissolveAmount<1f)
        {
            dissolveAmount += 0.01f;
            dissolveAmount = Mathf.Clamp01(dissolveAmount);
            gameObject.GetComponent<Renderer>().material.SetFloat("_DissolveAmount",dissolveAmount);
            yield return new WaitForSeconds(_dissolveDuration/100f);
        }
        Destroy(gameObject);
    }
}
