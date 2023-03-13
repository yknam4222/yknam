using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //움직이는 속도
    private float Speed;

    //움직임을 저장하는 벡터
    private Vector3 Movement;

    //플레이어의 Animator 구성요소를 받아오기위해
    private Animator animator;

    //플레이어의 SpriteRenderer 구성요소를 받아오기 위해
    private SpriteRenderer spriteRenderer;

    //상태체크
    private bool onAttack;
    private bool onHit;
    private bool onJump;
    private bool onDash;
    private bool onDead;

    //복사할 총알 원본
    private GameObject BulletPrefab;

    //복사할 FX 원본
    private GameObject fxPrefab;

    public GameObject[] stageBack = new GameObject[7];

    //복제된 총알의 저장 공간
    private List<GameObject> Bullets = new List<GameObject>();

    //플레이어가 마지막으로 바라본 방향
    private float Direction;

    public bool DirLeft;
    public bool DirRight;

    private void Awake()
    {
        // player의 Animator를 받아온다.
        animator = this.GetComponent<Animator>();

        // player의 SpriteRenderer을 받아온다.
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        // [Resources] 폴더에서 사용할 리소스를 들고온다.
        BulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        fxPrefab = Resources.Load("Prefabs/Fx/Smoke") as GameObject;
    }

    // 유니티 기본 제공 함수
    // 초기값을 설정할 때 사용
    void Start()
    {
        //속도를 초기화
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

    // 유니티 기본 제공 함수
    // 프레임마다 반복적으로 실행되는 함수
    void Update()
    {
        // 실수연산 IEEE 754

        // Input GetAxis = -1 ~ +1 사이의 값을 반환함.
        float Hor = Input.GetAxisRaw("Horizontal"); // -1, 0, 1 셋 중에 하나 반환
        //float Ver = Input.GetAxis("Vertical"); // -` ~ 1 까지 실수로 반환

        //입력받은 값으로 플레이어를 움직인다.
        Movement = new Vector3(
        Hor * Time.deltaTime * Speed,
        // Ver * Time.deltaTime * Speed, 
        0.0f,
        0.0f);

        //Hor이 0이라면 멈춰있는 상태이기 때문에 예외처리를 해줌.
        if (Hor != 0)
            Direction = Hor;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //플레이어의 좌표가 0보다 작을 때 플레이어만 움직인다.
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

            //플레이어의 좌표가 -15.0 보다 클 때
            if(transform.position.x > -15.0f)
                transform.position += Movement; 
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = false;
        }

        //플레이어가 바라보고있는 방향에 따라 이미지 플립 설정
        if (Direction < 0)
        {
            spriteRenderer.flipX = DirLeft = true;

            //실제 플레이어를 움직인다.
            
        }
        else if (Direction > 0)
        {
            spriteRenderer.flipX = false;
            DirRight = true;
        }


        // spriteRenderer.flipX = (Hor < 0) ? true : false;

        //공격
        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        //피격
        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        //점프
        if (Input.GetKey(KeyCode.Space))
            OnJump();

        //구르기
        if (Input.GetKey(KeyCode.C))
            OnDash();

        //사망
        if (Input.GetKey(KeyCode.Q))
            OnDead();

        if (Input.GetKeyDown(KeyCode.X))
        {
            OnAttack();
            //총알원본을 복제한다.
            GameObject Obj = Instantiate(BulletPrefab);

            //복제된 총알의 위치를 현재 플레이어의 위치로 초기화한다.
            Obj.transform.position = transform.position;

            //총알의 BulletController 스크립트를 받아온다.
            BulletController Controller = Obj.AddComponent<BulletController>();

            //Controller.Direction = (spriteRenderer.flipX) ? new Vector3(-1.0f, 0.0f, 0.0f) : new Vector3(1.0f, 0.0f, 0.0f);
            //Obj.GetComponent<SpriteRenderer>().flipY = (spriteRenderer.flipX) ? true : false;

            //총알의 스크립트내부의 방향번수를 현재 플레이어의 방향변수로 설정한다.
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            //총알 스크립트내부의 FX Prefab을 설정한다.
            Controller.fxPrefab = fxPrefab;

            //총알의 SpriteRenderer을 받아온다.
            SpriteRenderer bulletRenderer = Obj.GetComponent<SpriteRenderer>();

            //총알의 이미지 반전 상태를 플레이어의 이미지 반전 상태로 설정한다.
            bulletRenderer.flipY = spriteRenderer.flipX;

            //모든 설정이 종료되었다면 저장소에 보관한다.
            Bullets.Add(Obj);
        }

        //플레이어의 움직임에 따라 이동 모션을 실행 한다.
        animator.SetFloat("Speed", Hor);
        //animator.SetFloat("Climb", Ver);


    }

    private void OnAttack()
    {
        //이미 공격모션이 진행중이라면
        if (onAttack)
            //함수를 종료시킨다.
            return;

        //함수가 종료되지 않았다면 공격상태를 활성화하고 공격모션을 실행시킨다.
        onAttack = true;
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
        //함수가 실행되면 공격모션이 비활성화 된다.
        //함수는 애니메이션 클립의 이벤트 프레임으로 삽입된다.
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



