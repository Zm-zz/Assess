using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    [Title("��λ����")]
    public Transform pointsParent;

    private CameraBasicMove motion;

    public Dictionary<string, Transform> pointsDic = new Dictionary<string, Transform>();

    public void Initialize()
    {
        List<Transform> points = pointsParent.GetComponentsInChildren<Transform>(true).ToList();

        foreach (Transform t in points)
        {
            pointsDic.Add(t.name, t);
        }

        motion = FindObjectOfType<CameraBasicMove>();
        motion.Initialize();
    }

    /// <summary>
    /// ����ƶ���Ŀ��λ��
    /// </summary>
    public void MoveToTarget(string pointName, float motionTime = 0, UnityAction action = null, bool canMove = true, bool canLift = true)
    {
        if (pointsDic.ContainsKey(pointName))
        {
            MoveToTarget(pointsDic[pointName], motionTime, action, canMove, canLift);
            Debug.Log($"<size=13><color=yellow>�л��ӽǣ�</color></size>{pointName}");
        }
        else
        {
            Debug.Log($"<size=13><color=red>�������ӽǣ�</color></size>{pointName}");
        }
    }

    /// <summary>
    /// �ƶ���Ŀ��Ϊֹ������վ���Ժ�����¼�
    /// </summary>
    public void MoveToTarget(Transform target, float motionTime, UnityAction moveEvent = null, bool canMove = true, bool canLift = true)
    {
        // ��ֹ��������������Tween
        int tweenNumber = DOTween.Kill(transform);

        motion.canCameraMove = false;
        motion.canLifting = false;

        motion.transform.DORotate(target.rotation.eulerAngles, motionTime);

        motion.transform.DOMove(target.position, motionTime).OnComplete(() =>
        {
            motion.canCameraMove = canMove;
            motion.canLifting = canLift;

            moveEvent?.Invoke();
        });
    }

}
