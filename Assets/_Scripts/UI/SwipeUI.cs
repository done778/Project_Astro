using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private Image[] dots; // 하단 점 UI 배열
    [SerializeField] private Color activeColor; // 활성화된 점 색상
    [SerializeField] private Color inactiveColor; // 비활성화된 점 색상

    private float[] pagePositions;
    private int pageCount;
    private int currentPage = 0;

    void Start()
    {
        pageCount = content.childCount;
        pagePositions = new float[pageCount];

        // 각 페이지의 스크롤 위치값 계산 (0~1 사이)
        for (int i = 0; i < pageCount; i++)
        {
            pagePositions[i] = (float)i / (pageCount - 1);
        }
    }

    void Update()
    {
        // 드래그 중이 아닐 때 가장 가까운 페이지로 보간(Lerp) 이동
        if (!Input.GetMouseButton(0))
        {
            float targetPos = pagePositions[currentPage];
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPos, Time.deltaTime * 10f);
        }

        UpdateDots();
    }

    void UpdateDots()
    {
        // 현재 스크롤 위치에서 가장 가까운 페이지 인덱스 찾기
        float currentPos = scrollRect.horizontalNormalizedPosition;
        float minDistance = float.MaxValue;

        for (int i = 0; i < pageCount; i++)
        {
            float distance = Mathf.Abs(currentPos - pagePositions[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentPage = i;
            }
            dots[i].color = inactiveColor;
        }
        dots[currentPage].color = activeColor;
    }
}
