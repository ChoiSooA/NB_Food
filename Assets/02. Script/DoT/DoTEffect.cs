/*
 * 사이즈는 작은 사이즈로 만들어둬야함, 피봇은 사이즈가 퍼지게 할 위치로 설정해야함
 * 바운드와 무브는 원래 위치가 아닌 등장할 위치로 설정해야함
 * 알파는 기본적으로 0으로 설정해두어야함(나타나는게 시작, 사라지는 게 끝)
 * 이 스크립트를 사용한 오브젝트는 활성화가 되어있어야함(비활성화나 활성화 모두 작동하게 하기 위해 ActiveSet의 trueActive에 넣어둬야함)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class DoTEffect : MonoBehaviour
{
    public enum State
    {
        Size,
        Bound,
        Move,
        Slide,
        Blink,
    }
    public State state;                 //사이즈, 바운드, 무브, 알파 등 사용할 효과
    public float durationTime = 1f;     //지속시간
    public Vector3 targetVector;        //타겟 벡터(사이즈, 위치 등)
    Vector3 OriginalSize;               //원래 사이즈
    Vector3 OriginalPosition;           //원래 위치
    public GameObject activeOff;        //효과가 끝난 후 비활성화할 오브젝트

    bool backMove = false;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        OriginalSize = transform.localScale;
        OriginalPosition = transform.localPosition;
        if (activeOff != null)
            activeOff.SetActive(true);
        StartDoT();
    }
    public void Close()
    {
        EndDoT();
    }
    public void OriginalPosClose()
    {
        StartCoroutine(OriginalEnd());
    }

    public void StartDoT()
    {
        backMove = false;
        switch (state)
        {
            case State.Size:
                SizeStart();
                break;
            case State.Bound:
                BoundStart();
                break;
            case State.Move:
                MoveStart();
                break;
            case State.Slide:
                MoveStart();
                break;
            case State.Blink:
                BlinkStart();
                break;
        }
    }
    void EndDoT()
    {
        switch (state)
        {
            case State.Size:
                StartCoroutine(SizeEnd());
                break;
            case State.Slide:
                StartCoroutine(SlideEnd());
                break;
            default:
                StartCoroutine(OriginalEnd());
                break;
        }
    }

    void SizeStart()
    {
        transform.DOScale(targetVector, durationTime);
    }
    void BoundStart()
    {
        transform.DOLocalMove(targetVector, durationTime).SetEase(Ease.OutBounce);
    }
    void MoveStart()
    {
        transform.DOLocalMove(targetVector, durationTime);
    }
    void BlinkStart()
    {
        transform.GetComponent<TextMeshProUGUI>().DOFade(0f, durationTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
    IEnumerator SizeEnd()
    {
        transform.DOScale(OriginalSize, durationTime);
        yield return new WaitForSeconds(durationTime * 0.8f);
        transform.localScale = OriginalSize;
        if(activeOff != null)
            activeOff.SetActive(false);
        yield return null;
    }
    IEnumerator OriginalEnd()
    {
        transform.DOLocalMove(OriginalPosition, durationTime);
        yield return new WaitForSeconds(durationTime * 0.8f);
        transform.localPosition = OriginalPosition;
        if (activeOff != null)
            activeOff.gameObject.SetActive(false);
        yield return null;
    }
    IEnumerator SlideEnd()
    {
        transform.DOLocalMove(new Vector3(-OriginalPosition.x, OriginalPosition.y, OriginalPosition.z), durationTime);
        yield return new WaitForSeconds(durationTime);
        transform.localPosition = new Vector3(-OriginalPosition.x, OriginalPosition.y, OriginalPosition.z);
        if (activeOff != null)
            activeOff.gameObject.SetActive(false);
        yield return null;
    }
}
