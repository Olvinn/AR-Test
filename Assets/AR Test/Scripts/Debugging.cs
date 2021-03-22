using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Debugging : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] ImageTargetBehaviour target;

    SkinnedMeshRenderer[] _modelRenderers;
    float _dissolve = 1;

    // Start is called before the first frame update
    void Awake()
    {
        _modelRenderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mr in _modelRenderers)
        {
            mr.material.SetFloat("_Dissolve", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.position = Vector3.Lerp(model.transform.position, target.transform.position, Time.deltaTime * 2);
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, target.transform.rotation, Time.deltaTime * .5f);
    }

    public void TrackIn()
    {
        model.transform.position = target.transform.position;
        model.transform.rotation = transform.transform.rotation;
        model.transform.localScale = transform.transform.localScale;
        StopAllCoroutines();
        StartCoroutine(Appear());
    }

    public void TrackOut()
    {
        StopAllCoroutines();
        StartCoroutine(Disappear());
    }

    IEnumerator Appear()
    {
        while (_dissolve > 0)
        {
            _dissolve -= Time.deltaTime;
            foreach (SkinnedMeshRenderer mr in _modelRenderers)
            {
                mr.material.SetFloat("_Dissolve", _dissolve);
            }
            yield return new WaitForEndOfFrame();
        }
        _dissolve = 0;
    }

    IEnumerator Disappear()
    {
        while (_dissolve < 1)
        {
            _dissolve += Time.deltaTime;
            foreach (SkinnedMeshRenderer mr in _modelRenderers)
            {
                mr.material.SetFloat("_Dissolve", _dissolve);
            }
            yield return new WaitForEndOfFrame();
        }
        _dissolve = 1;
    }
}
