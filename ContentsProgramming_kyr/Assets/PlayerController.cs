using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public string playerName = "김철수";
    public int playerLevel = 1;
    
    // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
    private Animator animator;
    
    void Start()
    {
        // 게임 시작 시 한 번만 - Animator 컴포넌트 찾아서 저장
        animator = GetComponent<Animator>();
        
        Debug.Log("안녕하세요, " + playerName + "님!");
         
        //if (animator != null)
        //{
           // Debug.Log("Animator 컴포넌트를 찾았습니다!");
        //}
        //else
        //{
           // Debug.LogError("Animator 컴포넌트가 없습니다!");
        //}

        
    }

    
   void Update()
{
    // 이동 벡터 계산
    Vector3 movement = Vector3.zero;
    float currentMoveSpeed = moveSpeed;


    if (Input.GetKey(KeyCode.A))
    {
        movement += Vector3.left;
        transform.localScale = new Vector3(-1, 1, 1); // X축 뒤집기
    }
 
    if (Input.GetKey(KeyCode.D))
    {
        movement += Vector3.right;
        transform.localScale = new Vector3(1, 1, 1); // 원래 크기
    }

    if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed * 2f;
            Debug.Log("달리는 중!");
    }

    // 점프 입력 (한 번만 실행되어야 하므로 GetKeyDown!)
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (animator != null)
            {
                animator.SetBool("Jump", true);
                Debug.Log("점프하는 중!");
        }
    }

    // 실제 이동 적용
    if (movement != Vector3.zero)
    {
        transform.Translate(movement * currentMoveSpeed * Time.deltaTime);
    }
    
    // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
    float currentSpeed = movement != Vector3.zero ? currentMoveSpeed : 0f;
    
    // Animator에 속도 전달
    if (animator != null)
    {
        animator.SetFloat("Speed", currentSpeed);
    }
    }
}