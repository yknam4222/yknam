using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    //Scene �信�� ��Ÿ���� Game �信�� �� ��Ÿ��.
    private void OnDrawGizmos()
    {
        //Gizmos�� ���� �����Ѵ�.
        Gizmos.color = Color.red;

        //Gizmos�� �׸���.
        Gizmos.DrawSphere(this.transform.position, 0.2f);
    }
}
