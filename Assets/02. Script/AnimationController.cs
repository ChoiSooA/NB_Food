using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class AnimationController : MonoBehaviour
{
    public GameObject Character;

    public Animator animator;

    public Vector3 resetPosition;
    public Vector3 resetRotation;
    public Vector3 resetScale;

    public static AnimationController Instance { get; private set; }


    private void Awake()
    {
        animator = Character.GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        resetPosition = Character.transform.localPosition;
        resetRotation = Character.transform.localRotation.eulerAngles;
        resetScale = Character.transform.localScale;
    }

    public void ResetVector()
    {
        Character.transform.localPosition = resetPosition;
        Character.transform.localRotation = Quaternion.Euler(resetRotation);
        Character.transform.localScale = resetScale;
    }
    private void OnDisable()
    {
        PlayAnimation("Idle");
        ResetVector();
    }

    void PlayAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }
    public void PlayOneTime(string animationName)
    {
        StartCoroutine(PlayOneTimeCo(animationName));
    }
    public void playAnimSet(int animnum)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        switch (animnum)
        {
            case 0:
                StartCoroutine(AnimOne());
                break;
            case 1:
                StartCoroutine(AnimSide());
                break;
            case 2:
                StartCoroutine(AnimTwo());
                break;
            case 3:
                StartCoroutine(AnimThree());
                break;
        }
    }

    void jump()
    {
        PlayAnimation("Jump");
        Character.transform.DOMoveY(-1.5f, 0.7f).SetEase(Ease.Linear);  //DOMoveY�� y������ �̵��ϴ� �Լ�, Y������ -1.5��ŭ�� �ƴ϶� Y�� -1.5�� �������
    }

    public void moveSide()
    {
        Character.transform.DOLocalMove(new Vector3(-1.45f,-1.7f,6.24f), 0.7f).SetEase(Ease.Linear);  //DOMoveX�� x������ �̵��ϴ� �Լ�
        Character.transform.DOLocalRotate(new Vector3(0f,167.5f,0f), 0.7f).SetEase(Ease.Linear);
    }
    
    IEnumerator AnimOne()                                   //0�� �ִϸ��̼�
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);   //�ִϸ��̼� ���� �� ��� �ð�
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Hi");
        yield return new WaitForSeconds(1.5f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(7f);   //������ nice ������ ��� �ð�
        PlayAnimation("Nice");
        yield return new WaitForSeconds(3f);
        PlayAnimation("Idle");
    }
    IEnumerator AnimSide()                                  //1�� �ִϸ��̼�
    {
        Character.transform.localPosition = new Vector3(-1.45f, -4f, 6.24f);
        yield return new WaitForSeconds(0.01f);   //�ִϸ��̼� ���� �� ��� �ð�
        Character.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(0.5f);   //�ִϸ��̼� ���� �� ��� �ð�
        jump();
        moveSide();
    }                                                   
    IEnumerator AnimTwo()                                   //2�� �ִϸ��̼�
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);   //�ִϸ��̼� ���� �� ��� �ð�
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Clap");
        yield return new WaitForSeconds(2f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(4f);    //������ cheer ������ ��� �ð�
        PlayAnimation("Cheer");
    }
    IEnumerator AnimThree()                                 //3�� �ִϸ��̼�
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);   //�ִϸ��̼� ���� �� ��� �ð�
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Clap");
        Audio_Manager.Instance.PlayEffect(3);
        yield return new WaitForSeconds(2.2f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(9f);    //������ clap ������ ��� �ð�
        PlayAnimation("Hi");
    }

    IEnumerator PlayOneTimeCo(string animName)
    {
        StopCoroutine(PlayOneTimeCo(animName)); //�ִϸ��̼��� ���� ���� �� �ٽ� �����ϸ� �ִϸ��̼��� �ߺ����� ����Ǵ� ���� ����
        PlayAnimation("idle");
        PlayAnimation(animName);
        yield return new WaitForSeconds(0.01f);     //�ִϸ��̼��� ���� �� �ִϸ��̼� �ð��� �о�� �� �־� ������� ���� ���� ���
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(animName + " �ִϸ��̼� ����");
        PlayAnimation("Idle");
    }
}
