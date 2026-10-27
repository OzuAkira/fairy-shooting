using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_move : MonoBehaviour
{
    Rigidbody2D rb ;//rb.velocity�œ������p
    public Vector2 axis;//OnMove�̓��͒l��ۑ�

    public float moveSpeed = 1;
    private float neutral_Speed , half_Speed;


    
    private PlayerInput _playerInput;// ���͂��󂯎��PlayerInput
    [SerializeField] private string _slowActionName = "Slow";// �A�N�V������
    private InputAction _slowAction;// �A�N�V�����̃R���|�[�l���g

    GameObject child_Obj;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _slowAction = _playerInput.actions[_slowActionName];// �A�N�V������PlayerInput����擾
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        child_Obj = gameObject.transform.GetChild(0).gameObject;

        neutral_Speed = moveSpeed;//��E�ᑬ���̈ړ����x
        half_Speed = moveSpeed/2;//�ᑬ���̈ړ����x
    }
    void OnMove(InputValue value)
    {
        axis = value.Get<Vector2>();
    }
    private void Update()
    {
        //�ᑬ�ړ��̃t���O����
        if (_slowAction != null)
        {
            // �U���{�^���̉�����Ԏ擾
            var isPressed = _slowAction.IsPressed();

            if (isPressed)moveSpeed = half_Speed;
            else moveSpeed = neutral_Speed;
        }
        else
        {
            Debug.Log("_slowAction ��Null�ł��I");
            return;
        }

        //�ړ�����
        rb.linearVelocity = new Vector2(axis.x, axis.y) * moveSpeed;
        child_Obj.transform.position = gameObject.transform.position;
    }
}
