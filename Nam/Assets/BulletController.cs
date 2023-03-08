using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //�Ѿ��� ���ư��� �ӵ�
    private float Speed;

    //�Ѿ��� ���ư��� �� ����
    public Vector3 Direction { get; set; }

    private void Start()
    {
        //�ʱⰪ
        Speed = 10.0f;
    }
    void Update()
    {
        //�������� �ӵ���ŭ ��ġ�� ����
        transform.position += Direction * Speed * Time.deltaTime;   
    }

    //�浹ü�� ���������� ���Ե� ������Ʈ�� �ٸ� �浹ü�� �浹�Ѵٸ� ����Ǵ� �Լ� 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Wall")
        Destroy(this.gameObject);
    }
}
