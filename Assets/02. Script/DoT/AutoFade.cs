using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class AutoFade : MonoBehaviour
{
    public Image fadeImage;
    public UnityEvent ChangeEvent;
    public float fadeDurationTime = 1f;

    int eventCount = 0;
    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, fadeDurationTime).SetEase(Ease.Linear);
    }
    public void FadeOut()
    {
        fadeImage.DOFade(0, fadeDurationTime).SetEase(Ease.Linear);
    }
    public void FadeInOut()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeCoroutine());
    }
    IEnumerator FadeCoroutine()     //FadeIn -> FadeOut
    {
        FadeIn();
        yield return new WaitForSeconds(fadeDurationTime +0.4f);   //fade time �̿ܿ� 0.4�� ���
        //���������� ���� �̺�Ʈ ����Ǵ� ��� ���� ����
        FadeOut();
        yield return new WaitForSeconds(fadeDurationTime);
        fadeImage.gameObject.SetActive(false);
    }
}
