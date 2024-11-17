using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateBanknotes : MonoBehaviour
{
    public GameObject PrefabBanknote; 
    public Button ButtonClick;
    public RectTransform ButtonUpTransform;
    public float InitialSpeed = 5f; // Начальная скорость вылета купюры
    public float FallSpeed = 7f; // Скорость падения купюры

    void Start()
    {
        if (ButtonClick != null)
        {
            ButtonClick.onClick.AddListener(SpawnAtMousePosition); // Добавляем слушатель
        }
        else
        {
            Debug.LogError("ButtonClick не назначен в инспекторе!");
        }
    }

    void SpawnAtMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition; // Координаты мыши на экране
        mousePosition.z = 90.0f; 

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Создаём объект на месте мыши
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
            Debug.LogError("PrefabBanknote не назначен в инспекторе!");
        }
    }
}

public class BanknoteMove : MonoBehaviour
{
    private float TopY; // Позиция индикатора уровня апгрейда
    private float InitSpeed; // Начальная скорость
    private float FallSpeed; // Скорость падения
    private bool IsFalling = false; // Началось ли падение
    private Vector3 RandomDirection; // Случайное направление движения
    private float RandomRotation; // Случайное вращение

    public void Init(float _TopY, float _InitSpeed, float _FallSpeed)
    {
        this.TopY = _TopY;
        this.InitSpeed = _InitSpeed;
        this.FallSpeed = _FallSpeed;

        // Генерация случайного направления и вращения
        RandomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), 0).normalized; 
        RandomRotation = Random.Range(-360f, 360f);
    }

    void Update()
    {
        if (!IsFalling)
        {
            transform.position += RandomDirection * InitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward * RandomRotation * Time.deltaTime); // Создаём вращение вокруг оси z

            // Проверка на достижение границ экрана
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            if ((screenPosition.x <= 0) || (screenPosition.x >= Screen.width)) // Левая или правая граница
            {
                RandomDirection.x = -RandomDirection.x; // Меняем направление по X
            }

            if ((screenPosition.y <= 0) || (screenPosition.y >= Screen.height)) // Верхняя или нижняя граница
            {
                RandomDirection.y = -RandomDirection.y; // Меняем направление по Y
            }


            if (transform.position.y < TopY)
            {
                IsFalling = true;
            }
        }
        else
        {
            // Падение вниз
            transform.position += Vector3.down * FallSpeed * Time.deltaTime;
        }

        // Удаление, если купюра скрылась за нижней частью экрана
        if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y)
        {
            Destroy(gameObject);
        }

    }

}

