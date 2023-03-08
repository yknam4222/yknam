using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //총알이 날아가는 속도
    private float Speed;

    //총알이 날아가야 할 방향
    public Vector3 Direction { get; set; }

    private void Start()
    {
        //초기값
        Speed = 10.0f;
    }
    void Update()
    {
        //방향으로 속도만큼 위치를 변경
        transform.position += Direction * Speed * Time.deltaTime;   
    }

    //충돌체와 물리엔진이 포함된 오브젝트가 다른 충돌체와 충돌한다면 실행되는 함수 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Wall")
        Destroy(this.gameObject);
    }
}
