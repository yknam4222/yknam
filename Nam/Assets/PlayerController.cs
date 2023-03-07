using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float Speed;
    private Vector3 Movement;

    private Animator animator;

    private bool onAttack;
    private bool onHit;
    private bool onJump;
    private bool onDash;
    private bool onDead;
    
    // 유니티 기본 제공 함수
    // 초기값을 설정할 때 사용
    void Start()
    {
        //속도를 초기화
        Speed = 5.0f;

        // player의 Animator를 받아온다.
        animator = this.GetComponent<Animator>();

        onAttack = false;
        onHit = false;
        onJump = false;
        onDash = false;
        onDead = false;
    }

    // 유니티 기본 제공 함수
    // 프레임마다 반복적으로 실행되는 함수
    void Update()
    {
        // 실수연산 IEEE 754

        // Input GetAxis = -1 ~ +1 사이의 값을 반환함.
        float Hor = Input.GetAxis("Horizontal");
        float Ver = Input.GetAxis("Vertical");
        
        Movement =  new Vector3(
        Hor * Time.deltaTime * Speed, 
        Ver * Time.deltaTime * Speed, 
        0.0f);

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
        if (Input.GetKey(KeyCode.F))
            OnDash();

        //사망
        if (Input.GetKey(KeyCode.Q))
            OnDead();

        animator.SetFloat("Speed", Hor);
        animator.SetFloat("Climb", Ver);

        if(!onDead)
            transform.position += Movement;
    }

    private void OnAttack()
    {
        if (onAttack)
            return;

        onAttack = true;
        animator.SetTrigger("Attack");
    }

    private void SetAttack()
    {
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



