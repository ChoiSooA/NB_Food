using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ON_Enable : MonoBehaviour
{
    public AudioClip mentClip; //��Ʈ Ŭ�� �迭
    private void OnEnable()
    {
        Audio_Manager.Instance.PlayMent(mentClip);
    }
    private void OnDisable()
    {
        Audio_Manager.Instance.Ment_audioSource.Stop();
    }
}
