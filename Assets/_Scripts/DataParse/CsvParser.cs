using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

//CSV 파일을 읽어서 C# 객체로 찍어내기
public static class CsvParser
{
    //Parse<T> 메서드
    //어떤 데이터 타입이든 처리, where new T()로 만들 수 있는 클래스만
    public static List<T> Parse<T>(string csvFileName) where T : new()
    {
        List<T> list = new List<T>();

        //Resources에서 파일 읽어오기
        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);


        if (csvData == null)
        {
            Debug.LogError($"[CsvParser] 파일 없다: {csvFileName}");
            return list;
        }

        //줄바꿈 처리
        string[] lines = csvData.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        //헤더가 3줄이니까 4줄은 넘겨야 데이터가 존재
        if (lines.Length < 4)
        {
            return list;
        }

        //Csv 2번째 줄 = 변수명 = Header = 컬럼명
        string[] headers = SplitCsvLine(lines[1]);

        //캐싱 해두기
        FieldInfo[] fieldCache = new FieldInfo[headers.Length];
        Type type = typeof(T);

        for (int i = 0; i < headers.Length; i++)
        {
            //Trim()도 여기서 미리 수행하여 GC 스파이크 방지
            string fieldName = headers[i].Trim();
            //딱 한 번만 찾기
            fieldCache[i] = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
        }

        //4번째 줄(Index 3)부터 실제 데이터
        for (int i = 3; i < lines.Length; i++)
        {
            //쉼표 자르기
            string[] values = SplitCsvLine(lines[i]);
            if (values.Length == 0)
            {
                continue;
            }

            //빈 껍데기 객체 생성(ex. new MonsterData()) 
            T entry = new T();

            //값 채워넣기
            for (int j = 0; j < headers.Length; j++)
            {
                if (j >= values.Length)
                {
                    break;
                }
                //캐싱된 변수 쓰기
                FieldInfo field = fieldCache[j];

                //헤더와 일치하는 변수가 있고, 값도 있다면
                if (field != null)
                {
                    string value = values[j].Replace("\"\"\"", "").Replace("\"", "").Trim();

                    try
                    {
                        object finalValue = ConvertValue(value, field.FieldType);
                        field.SetValue(entry, finalValue);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[CsvParser] 파싱 에러파트 파일:{csvFileName}, 줄:{i + 1}, 컬럼:{headers[j]}, 값:{value}\n에러:{e}");
                    }
                }
            }
            //객체를 리스트에 추가
            list.Add(entry);
        }
        return list;
    }

    //CSV 쉼표 분리 (따옴표 내부 쉼표 무시)
    private static string[] SplitCsvLine(string line)
    {
        return Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
    }

    //타입 변환기
    //인자값 value는 string, type은 목표 타입
    //출력은 int bool 등 모든 타입이 될 수 있는 object
    private static object ConvertValue(string value, Type type)
    {
        if (type == typeof(int))
        {
            return int.TryParse(value, out int i) ? i : 0;
        }
        if (type == typeof(float))
        {
            return float.TryParse(value, out float f) ? f : 0f;
        }
        if (type == typeof(bool)) //기획서 상엔 t, f > (Jihoo) 데이터 테이블에서 0, 1로 저장된 것으로 확인
        {
            // Jihoo
            // 데이터 테이블 형식에 따라 "t"를 "1"로 변경
            return value == "1";
        }
        if (type.IsEnum) //Fire면 Type.Fire, 숫자를 적어도 Type 중 0번으로 변환.
        {
            return Enum.Parse(type, value);
        }
        return value; // string
    }
}