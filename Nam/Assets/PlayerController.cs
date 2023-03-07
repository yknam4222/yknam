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
    
    // ����Ƽ �⺻ ���� �Լ�
    // �ʱⰪ�� ������ �� ���
    void Start()
    {
        //�ӵ��� �ʱ�ȭ
        Speed = 5.0f;

        // player�� Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();

        onAttack = false;
        onHit = false;
        onJump = false;
        onDash = false;
        onDead = false;
    }

    // ����Ƽ �⺻ ���� �Լ�
    // �����Ӹ��� �ݺ������� ����Ǵ� �Լ�
    void Update()
    {
        // �Ǽ����� IEEE 754

        // Input GetAxis = -1 ~ +1 ������ ���� ��ȯ��.
        float Hor = Input.GetAxis("Horizontal");
        float Ver = Input.GetAxis("Vertical");
        
        Movement =  new Vector3(
        Hor * Time.deltaTime * Speed, 
        Ver * Time.deltaTime * Speed, 
        0.0f);

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
        if (Input.GetKey(KeyCode.F))
            OnDash();

        //���
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



