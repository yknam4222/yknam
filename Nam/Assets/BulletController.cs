using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //�Ѿ��� ���ư��� �ӵ�
    private float Speed;

    private int hp;

    public GameObject fxPrefab;

    //�Ѿ��� ���ư��� �� ����
    public Vector3 Direction { get; set; }

    private void Start()
    {
        //�ʱⰪ
        Speed = 10.0f;

        hp = 3;
    }
    void Update()
    {
        //�������� �ӵ���ŭ ��ġ�� ����
        transform.position += Direction * Speed * Time.deltaTime;   
    }

    //�浹ü�� ���������� ���Ե� ������Ʈ�� �ٸ� �浹ü�� �浹�Ѵٸ� ����Ǵ� �Լ� 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        --hp;

        GameObject Obj = Instantiate(fxPrefab);

        GameObject camera = new GameObject("Camera Test");
        camera.AddComponent<CameraController>();

        Obj.transform.position = transform.position;

        DestroyObject(collision.transform.gameObject);

        if(hp == 0)
            DestroyObject(this.gameObject);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    print("bbbbbbbbbbb");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    print("ccccccccccccc");
    //}
}
