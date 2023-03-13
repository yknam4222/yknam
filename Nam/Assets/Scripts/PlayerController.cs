using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //�����̴� �ӵ�
    private float Speed;

    //�������� �����ϴ� ����
    private Vector3 Movement;

    //�÷��̾��� Animator ������Ҹ� �޾ƿ�������
    private Animator animator;

    //�÷��̾��� SpriteRenderer ������Ҹ� �޾ƿ��� ����
    private SpriteRenderer spriteRenderer;

    //����üũ
    private bool onAttack;
    private bool onHit;
    private bool onJump;
    private bool onDash;
    private bool onDead;

    //������ �Ѿ� ����
    private GameObject BulletPrefab;

    //������ FX ����
    private GameObject fxPrefab;

    public GameObject[] stageBack = new GameObject[7];

    //������ �Ѿ��� ���� ����
    private List<GameObject> Bullets = new List<GameObject>();

    //�÷��̾ ���������� �ٶ� ����
    private float Direction;

    public bool DirLeft;
    public bool DirRight;

    private void Awake()
    {
        // player�� Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();

        // player�� SpriteRenderer�� �޾ƿ´�.
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        // [Resources] �������� ����� ���ҽ��� ���´�.
        BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        fxPrefab = Resources.Load("Prefabs/Fx/Smoke") as GameObject;
    }

    // ����Ƽ �⺻ ���� �Լ�
    // �ʱⰪ�� ������ �� ���
    void Start()
    {
        //�ӵ��� �ʱ�ȭ
        Speed = 5.0f;

        onAttack = false;
        onHit = false;
        onJump = false;
        onDash = false;
        onDead = false;

        Direction = 1.0f;

        for (int i = 0; i < 7; ++i)
            stageBack[i] = GameObject.Find(i.ToString());
    }

    // ����Ƽ �⺻ ���� �Լ�
    // �����Ӹ��� �ݺ������� ����Ǵ� �Լ�
    void Update()
    {
        // �Ǽ����� IEEE 754

        // Input GetAxis = -1 ~ +1 ������ ���� ��ȯ��.
        float Hor = Input.GetAxisRaw("Horizontal"); // -1, 0, 1 �� �߿� �ϳ� ��ȯ
        //float Ver = Input.GetAxis("Vertical"); // -` ~ 1 ���� �Ǽ��� ��ȯ

        //�Է¹��� ������ �÷��̾ �����δ�.
        Movement = new Vector3(
        Hor * Time.deltaTime * Speed,
        // Ver * Time.deltaTime * Speed, 
        0.0f,
        0.0f);

        //Hor�� 0�̶�� �����ִ� �����̱� ������ ����ó���� ����.
        if (Hor != 0)
            Direction = Hor;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //�÷��̾��� ��ǥ�� 0���� ���� �� �÷��̾ �����δ�.
            if (transform.position.x < 0)
                transform.position += Movement;
            else 
            { 
            ControllerManager.GetInstance().DirRight = true;
            ControllerManager.GetInstance().DirLeft = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = true;

            //�÷��̾��� ��ǥ�� -15.0 ���� Ŭ ��
            if(transform.position.x > -15.0f)
                transform.position += Movement; 
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = false;
        }

        //�÷��̾ �ٶ󺸰��ִ� ���⿡ ���� �̹��� �ø� ����
        if (Direction < 0)
        {
            spriteRenderer.flipX = DirLeft = true;

            //���� �÷��̾ �����δ�.
            
        }
        else if (Direction > 0)
        {
            spriteRenderer.flipX = false;
            DirRight = true;
        }


        // spriteRenderer.flipX = (Hor < 0) ? true : false;

        //����
        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        //�ǰ�
        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        //����
        if (Input.GetKey(KeyCode.Space))
            OnJump();

        //������
        if (Input.GetKey(KeyCode.C))
            OnDash();

        //���
        if (Input.GetKey(KeyCode.Q))
            OnDead();

        if (Input.GetKeyDown(KeyCode.X))
        {
            OnAttack();
            //�Ѿ˿����� �����Ѵ�.
            GameObject Obj = Instantiate(BulletPrefab);

            //������ �Ѿ��� ��ġ�� ���� �÷��̾��� ��ġ�� �ʱ�ȭ�Ѵ�.
            Obj.transform.position = transform.position;

            //�Ѿ��� BulletController ��ũ��Ʈ�� �޾ƿ´�.
            BulletController Controller = Obj.AddComponent<BulletController>();

            //Controller.Direction = (spriteRenderer.flipX) ? new Vector3(-1.0f, 0.0f, 0.0f) : new Vector3(1.0f, 0.0f, 0.0f);
            //Obj.GetComponent<SpriteRenderer>().flipY = (spriteRenderer.flipX) ? true : false;

            //�Ѿ��� ��ũ��Ʈ������ ��������� ���� �÷��̾��� ���⺯���� �����Ѵ�.
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            //�Ѿ� ��ũ��Ʈ������ FX Prefab�� �����Ѵ�.
            Controller.fxPrefab = fxPrefab;

            //�Ѿ��� SpriteRenderer�� �޾ƿ´�.
            SpriteRenderer bulletRenderer = Obj.GetComponent<SpriteRenderer>();

            //�Ѿ��� �̹��� ���� ���¸� �÷��̾��� �̹��� ���� ���·� �����Ѵ�.
            bulletRenderer.flipY = spriteRenderer.flipX;

            //��� ������ ����Ǿ��ٸ� ����ҿ� �����Ѵ�.
            Bullets.Add(Obj);
        }

        //�÷��̾��� �����ӿ� ���� �̵� ����� ���� �Ѵ�.
        animator.SetFloat("Speed", Hor);
        //animator.SetFloat("Climb", Ver);


    }

    private void OnAttack()
    {
        //�̹� ���ݸ���� �������̶��
        if (onAttack)
            //�Լ��� �����Ų��.
            return;

        //�Լ��� ������� �ʾҴٸ� ���ݻ��¸� Ȱ��ȭ�ϰ� ���ݸ���� �����Ų��.
        onAttack = true;
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        //�Լ��� ����Ǹ� ���ݸ���� ��Ȱ��ȭ �ȴ�.
        //�Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Եȴ�.
        onAttack = false;
    }

    private void OnHit()
    {
        if (onHit)
            return;

        onHit = true;
        animator.SetTrigger("Hit");
    }

    private void SetHit()
    {
        onHit = false;
    }

    private void OnJump()
    {
        if (onJump)
            return;

        onJump = true;
        animator.SetTrigger("Jump");
    }

    private void SetJump()
    {
        onJump = false;
    }

    private void OnDash()
    {
        if (onDash)
            return;

        onDash = true;
        animator.SetTrigger("Dash");
    }

    private void SetDash()
    {
        onDash = false;
    }

    private void OnDead()
    {
        if (onDead)
            return;

        onDead = true;
        animator.SetTrigger("Dead");
    }
}



