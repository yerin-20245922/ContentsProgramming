using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;
    
    [Header("점프 설정")]
    public float jumpForce = 10.0f;
    
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private int score = 0;  // 점수 추가

    private Animator animator;

    // 리스폰용 시작 위치
    private Vector3 startPosition;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 게임 시작 시 위치를 저장
        startPosition = transform.position;
        Debug.Log("시작 위치 저장:" + startPosition);
    }
    
    void Update()
    {

        // 좌우 이동
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        
        // ⭐ 애니메이션 처리 부분
        // 이동 중인지 여부를 스피드로 전달 (절댓값으로!)
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        // 캐릭터가 움직이는 방향에 따라 좌우 반전
        if (moveX != 0)
        {
            transform.localScale = new Vector3(moveX > 0 ? 1 : -1, 1, 1);
        }

        // 점프 (지난 시간에 배운 내용)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("Jump", true);
        }
        
    }

    // 바닥 충돌 감지 (Collision)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에 착지!");
            isGrounded = true;
        }

        // 장애물 충돌 감지 - 새로 추가!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");

            // 시작 위치로 순간이동
            transform.position = startPosition;

            // 속도 초기화 (안 하면 계속 날아감)
            rb.linearVelocity = new Vector2(0, 0);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에서 떨어짐");
            isGrounded = false;
        }
    }
    
    // 아이템 수집 감지 (Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;  // 점수 증가
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);  // 코인 제거
        }

            // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");
            
            // 캐릭터 조작 비활성화
            enabled = false;
        }
    }
}