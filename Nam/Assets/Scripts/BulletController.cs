using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //�Ѿ��� ���ư��� �ӵ�
    private float Speed;

    // �Ѿ��� �浹�� Ƚ��
    private int hp;

    // ����Ʈȿ�� ����
    public GameObject fxPrefab;

    //�Ѿ��� ���ư��� �� ����
    public Vector3 Direction { get; set; }

    private void Start()
    {
        //�ʱⰪ
        Speed = 10.0f;

        //�浹 Ƚ���� 3���� �����Ѵ�.
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
        //�浹Ƚ�� ����.
        --hp;

        //����Ʈȿ�� ����.
        GameObject Obj = Instantiate(fxPrefab);

        //����ȿ���� ������ ������ ����
        GameObject camera = new GameObject("Camera Test");
        
        //���� ȿ�� ��Ʈ�ѷ� ����
        camera.AddComponent<CameraController>();

        //����Ʈȿ���� ��ġ�� ����
        Obj.transform.position = transform.position;

        //collision = �浹�� ���
        //�浹�� ����� �����Ѵ�.
        Destroy(collision.transform.gameObject);

        //�Ѿ��� �浹 Ƚ���� 0�� �Ǹ� �Ѿ� ����.
        if(hp == 0)
            Destroy(this.gameObject);
    }
}
