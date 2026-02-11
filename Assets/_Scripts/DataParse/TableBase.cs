using System.Collections.Generic;
using UnityEngine;

//T는 클래스, ID가 필요, 생성 가능(인자값 없이)
public class TableBase<T> where T : class, ITableData, new()
{
    //데이터 저장 딕셔너리 (Key: id스트링, Val: 데이터 객체)
    public Dictionary<string, T> dataMap = new Dictionary<string, T>();

    //인자값으로 파일 경로를 받아서 데이터를 채워넣는 메서드
    public void Load(string filePath)
    {
        //CsvParser에게 시켜서 리스트로 데이터 받아오기
        List<T> list = CsvParser.Parse<T>(filePath);

        //딕셔너리 초기화 (재로드 대비)
        dataMap.Clear();

        //리스트를 딕셔너리로 변환
        foreach (T item in list)
        {
            //id가 비어있지 않고 중복된 id가 아닐 때만 등록
            if (!string.IsNullOrEmpty(item.PrimaryID) && !dataMap.ContainsKey(item.PrimaryID))
            {
                dataMap.Add(item.PrimaryID, item);
            }
        }
    }

    //ID로 데이터 꺼내는 함수
    public T Get(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        //딕셔너리 확인
        if (dataMap.TryGetValue(id, out T value))
        {
            return value;
        }
        return null; // 못 찾으면 null 반환
    }

    //도감 같은 곳에서 전체 목록을 반환하는 메서드
    public List<T> GetAll()
    {
        return new List<T>(dataMap.Values);
    }
}
