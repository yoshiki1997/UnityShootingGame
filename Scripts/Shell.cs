using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifeTime = 4;
    float fadeTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        myRigidbody.AddForce(transform.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifeTime);

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer> (). material;
        Color initialColor = mat.color;

        while (percent <= 1)
        {
            percent += fadeSpeed * Time.deltaTime;
            mat.color = Color.Lerp (initialColor, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}
