using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateBanknotes : MonoBehaviour
{
    public GameObject PrefabBanknote; 
    public Button ButtonClick;
    public RectTransform ButtonUpTransform;
    public float InitialSpeed = 5f; // ��������� �������� ������ ������
    public float FallSpeed = 7f; // �������� ������� ������

    void Start()
    {
        if (ButtonClick != null)
        {
            ButtonClick.onClick.AddListener(SpawnAtMousePosition); // ��������� ���������
        }
        else
        {
            Debug.LogError("ButtonClick �� �������� � ����������!");
        }
    }

    void SpawnAtMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition; // ���������� ���� �� ������
        mousePosition.z = 90.0f; 

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // ������ ������ �� ����� ����
        if (PrefabBanknote != null)
        {
            GameObject NewBanknote = Instantiate(PrefabBanknote, worldPosition, Quaternion.identity);
            BanknoteMove Move = NewBanknote.AddComponent<BanknoteMove>();
            Vector3[] worldCorners = new Vector3[4];

            ButtonUpTransform.GetWorldCorners(worldCorners);
            float TopY = worldCorners[1].y;
            Move.Init(TopY, InitialSpeed, FallSpeed);

        }
        else
        {
            Debug.LogError("PrefabBanknote �� �������� � ����������!");
        }
    }
}

public class BanknoteMove : MonoBehaviour
{
    private float TopY; // ������� ���������� ������ ��������
    private float InitSpeed; // ��������� ��������
    private float FallSpeed; // �������� �������
    private bool IsFalling = false; // �������� �� �������
    private Vector3 RandomDirection; // ��������� ����������� ��������
    private float RandomRotation; // ��������� ��������

    public void Init(float _TopY, float _InitSpeed, float _FallSpeed)
    {
        this.TopY = _TopY;
        this.InitSpeed = _InitSpeed;
        this.FallSpeed = _FallSpeed;

        // ��������� ���������� ����������� � ��������
        RandomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), 0).normalized; // ?
        RandomRotation = Random.Range(-360f, 360f);
    }

    void Update()
    {
        if (!IsFalling)
        {
            transform.position += RandomDirection * InitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * RandomRotation * Time.deltaTime); // ������ �������� ������ ��� z

            // �������� �� ���������� ������ ������
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            if ((screenPosition.x <= 0) || (screenPosition.x >= Screen.width)) // ����� ��� ������ �������
            {
                RandomDirection.x = -RandomDirection.x; // ������ ����������� �� X
            }

            if ((screenPosition.y <= 0) || (screenPosition.y >= Screen.height)) // ������� ��� ������ �������
            {
                RandomDirection.y = -RandomDirection.y; // ������ ����������� �� Y
            }


            if (transform.position.y < TopY)
            {
                IsFalling = true;
            }
        }
        else
        {
            // ������� ����
            transform.position += Vector3.down * FallSpeed * Time.deltaTime;
        }

        // ��������, ���� ������ �������� �� ������ ������ ������
        if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y)
        {
            Destroy(gameObject);
        }

    }

}

