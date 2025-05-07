using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Mission1 : MonoBehaviour
{
    public TMP_Text ment;
    public TMP_Text how_many_left;

    int totalCount = 9;
    int healthTotalCount = 4;
    int curruntCount = 0;
    int healthCount = 0;

    public GameObject SuccessAll;

    public Transform goodGroup;
    public Transform badGroup;

    public ParticleSystem popParticle;

    public AudioClip[] right_wrong; //0: ����, 1: Ʋ��
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        how_many_left.text = $"{healthCount}/{healthTotalCount}"; 
        if (goodGroup != null)
        {
            foreach (Transform child in goodGroup)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<Collider>().enabled = true; // Ŭ�� �����ϰ�
            }
        }
        if (badGroup != null)
        {
            foreach (Transform child in badGroup)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<Collider>().enabled = true; // Ŭ�� �����ϰ�
                child.GetChild(0).gameObject.SetActive(false);      //�̰� xǥ�� ���ִ� �̴ϴ� �׷��� true���� ����
                child.GetChild(1).gameObject.SetActive(true);      //�̰� ��ƼŬ ���ִ� �̴ϴ� �׷��� true���� ����
            }
        }
        
        // ī��Ʈ �ʱ�ȭ
        curruntCount = 0;
        healthCount = 0;
        how_many_left.text = $"{healthCount}/{healthTotalCount}"; // ���� ������Ʈ

        // ����â �����
        if (SuccessAll != null)
            SuccessAll.transform.GetChild(0).GetComponent<DoTEffect>().Close();
    }
    private void OnDisable()
    {
        audioSource.Stop();
    }

    public void IsItRight()
    {
        GameObject touchObj = TouchObjectDetector.touchObj;
        string particle = GetPostPosition_EunNeun(touchObj.name);
        ment.DOKill();
        ment.text = "";

        if (touchObj.transform.parent.name == "Good")
        {
            AnimationController.Instance.PlayOneTime("Clap");
            Audio_Manager.Instance.PlayEffect(1);
            popParticle.transform.position = touchObj.transform.position;
            popParticle.Play();
            PlaySoundExclusive(right_wrong[0]);
            ment.DOText($"{touchObj.name}{particle} ���� ���� ������ �¾ƿ�!", 1f);
            touchObj.gameObject.SetActive(false);
            Debug.Log("�����Դϴ�.");
            healthCount++;
            how_many_left.text = $"{healthCount}/{healthTotalCount}"; // ���� ������Ʈ
            if (healthCount == healthTotalCount)
            {
                AnimationController.Instance.PlayOneTime("Cheer");
                Audio_Manager.Instance.PlayEffect(3);
                StartCoroutine(AllDone());
            }
        }
        else if (touchObj.transform.parent.name == "Bad")
        {
            AnimationController.Instance.PlayOneTime("SideToSide");
            Audio_Manager.Instance.PlayEffect(2);
            touchObj.transform.GetChild(0).gameObject.SetActive(true);          //X ǥ�� ���ִ� ����
            touchObj.transform.GetChild(1).gameObject.SetActive(false);         //��ƼŬ ���ִ� ����
            touchObj.GetComponent<Collider>().enabled = false;       //Xǥ�ð� ������ ���̻� Ŭ�� ���ϰ� ���°���
            PlaySoundExclusive(right_wrong[1]);
            ment.DOText($"{touchObj.name}{particle} ���� ���� ������ �ƴϿ���!", 1f);
            Debug.Log("�����Դϴ�.");
        }
        else
        {
            return;
        }
        curruntCount++;

        if (curruntCount == totalCount)
        {
            StartCoroutine(AllDone());
            Debug.Log("��� ������Ʈ�� Ȯ���߽��ϴ�.");
        }
    }

    string GetPostPosition_EunNeun(string word)
    {
        if (string.IsNullOrEmpty(word)) return "";

        char lastChar = word[word.Length - 1];
        int unicode = lastChar - 0xAC00;

        if (unicode < 0 || unicode > 11171) return "��";

        int jongseongIndex = unicode % 28;

        return (jongseongIndex == 0) ? "��" : "��";
    }

    public void PlaySoundExclusive(AudioClip clip)
    {
        if (audioSource.clip!=null)
        {
            Audio_Manager.Instance.Ment_audioSource.Stop();      // ���� ��� ���� ���� ����
        }
        Audio_Manager.Instance.Ment_audioSource.clip = clip;
        Audio_Manager.Instance.Ment_audioSource.Play();      // ���ο� Ŭ�� ���
    }

    IEnumerator AllDone()   //������ ������Ʈ�� Ȯ���� �� ������ �ð��� ������ ����â�� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(2f);
        SuccessAll.SetActive(true);
    }
}
