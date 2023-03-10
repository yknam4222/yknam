using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    //Scene 뷰에만 나타나고 Game 뷰에는 안 나타남.
    private void OnDrawGizmos()
    {
        //Gizmos의 색을 변경한다.
        Gizmos.color = Color.red;

        //Gizmos를 그린다.
        Gizmos.DrawSphere(this.transform.position, 0.2f);
    }
}
