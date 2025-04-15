using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2 : MonoBehaviour
{
    public GameObject vectorSet;
    Vector3[] position;
    bool[] isFilled;
    Dictionary<GameObject, int> objectIndexMap = new Dictionary<GameObject, int>();

    public static int howManyObjects = 0;
    public static int score = 0;

    public static List<string> goodNames = new List<string>();
    public static List<string> badNames = new List<string>();

    public GameObject CompleteButton;

    private void OnDisable()
    {
        foreach (var pair in objectIndexMap)
        {
            GameObject obj = pair.Key;
            TouchSelf touchSelf = obj.GetComponent<TouchSelf>();

            obj.transform.DOKill();
            obj.transform.DOMove(touchSelf.originalPos, 0.5f);

            obj.transform.DOMove(touchSelf.originalPos + new Vector3(
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.1f, 0.1f),
                0f), Random.Range(2f, 3f))
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            touchSelf.isTouching = false;
        }

        for (int i = 0; i < isFilled.Length; i++)
            isFilled[i] = false;

        objectIndexMap.Clear();
        howManyObjects = 0;
        score = 0;

        goodNames.Clear();
        badNames.Clear();

        if (CompleteButton != null)
            CompleteButton.SetActive(false);
    }

    private void Start()
    {
        int count = vectorSet.transform.childCount;
        position = new Vector3[count];
        isFilled = new bool[count];

        for (int i = 0; i < count; i++)
        {
            position[i] = vectorSet.transform.GetChild(i).position;
            isFilled[i] = false;
        }
    }

    public void isItHealthy()
    {
        GameObject touchObj = TouchObjectDetector.touchObj;
        TouchSelf touchSelf = touchObj.GetComponent<TouchSelf>();

        if (!touchSelf.isTouching)
        {
            int targetIndex = FindFirstEmptyIndex();
            if (targetIndex == -1)
            {
                Debug.Log("�� �ڸ��� �����ϴ�.");
                return;
            }

            touchSelf.isTouching = true;
            touchObj.transform.DOKill();
            touchObj.transform.DOMove(position[targetIndex], 0.5f).SetEase(Ease.OutBack);

            howManyObjects++;
            isFilled[targetIndex] = true;
            objectIndexMap[touchObj] = targetIndex;

            if (touchObj.transform.parent.name == "Good")
            {
                score++;
                goodNames.Add(touchObj.name);
                Debug.Log("�����Դϴ�.");
            }
            else if (touchObj.transform.parent.name == "Bad")
            {
                badNames.Add(touchObj.name);
                Debug.Log("�����Դϴ�.");
            }

            if (howManyObjects >= 3)
            {
                Debug.Log("3�� �̻� ���");
                if (CompleteButton != null)
                    CompleteButton.SetActive(true);
            }
        }
        else
        {
            touchSelf.isTouching = false;
            StartCoroutine(MoveOriginPos(touchObj));
            howManyObjects--;

            if (objectIndexMap.ContainsKey(touchObj))
            {
                int idx = objectIndexMap[touchObj];
                isFilled[idx] = false;
                objectIndexMap.Remove(touchObj);
            }

            if (touchObj.transform.parent.name == "Good")
            {
                score--;
                goodNames.Remove(touchObj.name);
                Debug.Log("���� ��ҵ�");
            }
            else if (touchObj.transform.parent.name == "Bad")
            {
                badNames.Remove(touchObj.name);
            }

            Debug.Log("���� �ȿ� " + howManyObjects + "�� ����");

            if (howManyObjects < 3)
            {
                Debug.Log("3�� �Ʒ��� ������");
                if (CompleteButton != null)
                    CompleteButton.SetActive(false);
            }
        }
    }

    int FindFirstEmptyIndex()
    {
        for (int i = 0; i < isFilled.Length; i++)
        {
            if (!isFilled[i])
                return i;
        }
        return -1;
    }

    IEnumerator MoveOriginPos(GameObject touchObj)
    {
        Vector3 origin = touchObj.GetComponent<TouchSelf>().originalPos;
        touchObj.transform.DOMove(origin, 0.5f);
        yield return new WaitForSeconds(0.5f);

        touchObj.transform.DOMove(touchObj.transform.position + new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            0f), Random.Range(2f, 3f))
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
