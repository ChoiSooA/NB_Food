using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TouchSelf : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;
    public bool isTouching = false;
    public Vector3 originalPos;
    private void Awake()
    {
        originalPos = transform.position;
    }

    public void OnClick()
    {
        onClick.Invoke();
        Debug.Log($"{name} onclick ½ÇÇàµÊ");
    }

}