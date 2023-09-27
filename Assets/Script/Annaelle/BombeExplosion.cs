using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombeExplosion : MonoBehaviour
{
    [SerializeField] private GameObject rayonArriere;
    [SerializeField] private GameObject rayonAvant;
    [SerializeField] private GameObject rayonGauche;
    [SerializeField] private GameObject rayonDroit;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScaleRayon(rayonDroit, Vector3.right));
        StartCoroutine(ScaleRayon(rayonArriere, Vector3.down));
        StartCoroutine(ScaleRayon(rayonAvant, Vector3.up));
        StartCoroutine(ScaleRayon(rayonGauche, Vector3.left));
    }
    IEnumerator ScaleRayon(GameObject go, Vector3 direction)
    {
        Vector3 localScaleA = go.transform.localScale;
        Vector3 localPos = go.transform.localPosition;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 5; i++)
        {
            localScaleA += new Vector3(0, 0.7f, 0);
            localPos += direction;
            go.transform.localPosition = localPos;
            go.transform.localScale = localScaleA;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
