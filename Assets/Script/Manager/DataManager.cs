using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public struct NoteInfo {
    public int lane;            // 0, 1, 2, 3번 레인 (x좌표 64, 192, 320, 448 기반)
    public int startTime;       // 판정선에 닿아야 하는 시간 (ms)
    public int endTime;         // 롱노트가 끝나는 시간 (ms) - 일반 노트는 startTime과 동일
    public bool isLongNote;     // 128 타입인지 확인용
    public string hitSoundFile; // kick1.wav 같은 개별 샘플 이름
    public int volume;          // 0~100 사이의 개별 노트 볼륨
}

[System.Serializable]
public class TimingPointData
{
    public float startTime;    // 1번째: 시작 시간 (ms)
    public double msPerBeat;   // 2번째: 비트 간격 (ms) 또는 배속
    public int beatSignature;  // 3번째: 박자 (4/4 등)
    public int sampleSet;      // 4번째: 효과음 종류
    public int volume;         // 6번째: 볼륨
    public int uninherited;    // 7번째: 1이면 BPM, 0이면 배속
}

public class DataManager : Singleton<DataManager> {
    public string fileName = "";

    // 파싱된 결과물을 담을 리스트
    public List<NoteInfo> noteList = new List<NoteInfo>();
    public List<TimingPointData> timingPoints = new List<TimingPointData>();

    void LoadOsuData(string path) {
        string[] lines = File.ReadAllLines(path);
        string currentSection = "";

        foreach (string line in lines) {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine)) continue;

            // 섹션 변경 확인
            if (trimmedLine.StartsWith("[")) {
                currentSection = trimmedLine;
                continue;
            }

            // 1. 타이밍 포인트 파싱
            if (currentSection == "[TimingPoints]") {
                ParseTimingPoint(trimmedLine);
            }
            // 2. 히트 오브젝트(노트) 파싱
            else if (currentSection == "[HitObjects]") {
                ParseHitObject(trimmedLine);
            }
        }
        
        // 파싱 완료 후 GameManager에 전달
        GameManager.Instance.SetParsedData(noteList, timingPoints);
        Debug.Log($"파싱 완료: 노트 {noteList.Count}개, 타이밍포인트 {timingPoints.Count}개");
    }

    public void StartParsing(string songName)
    {
        // 경로 : Assets/StreamingAssets/곡이름/
        string folderPath = Path.Combine(Application.streamingAssetsPath, songName);
        string fullPath = Path.Combine(folderPath, "test.osu"); 

        Debug.Log($"[파싱 시도] 전체 경로: {fullPath}");
        
        if (File.Exists(fullPath))
        {
            noteList.Clear();
            timingPoints.Clear();
            
            LoadOsuData(fullPath);
        }
        else
        {
            Debug.LogError($"[파싱 실패] 파일을 찾을 수 없습니다: {fullPath}");
        }
    }
    
    void ParseTimingPoint(string line) {
        string[] data = line.Split(',');
        if (data.Length < 8) return;

        TimingPointData tp = new TimingPointData {
            startTime = float.Parse(data[0]),
            msPerBeat = double.Parse(data[1]),
            uninherited = int.Parse(data[6])
        };
        timingPoints.Add(tp);
    }

    void ParseHitObject(string line) {
        string[] data = line.Split(',');
        if (data.Length < 5) return;

        NoteInfo note = new NoteInfo();

        // 1. 레인 계산 (x좌표 기반)
        int x = int.Parse(data[0]);
        note.lane = Mathf.Clamp(x * 4 / 512, 0, 3);

        // 2. 시작 시간
        note.startTime = int.Parse(data[2]);

        // 3. 타입 분석 (1=단타, 128=롱노트)
        int type = int.Parse(data[3]);
        note.isLongNote = (type & 128) != 0;

        // 4. 롱노트 여부에 따른 종료 시간 및 사운드 파일 파싱
        if (note.isLongNote) 
        {
            // 롱노트 데이터 예: "8981:0:0:0:40:kick1.wav"
            string[] extraData = data[5].Split(':');
            note.endTime = int.Parse(extraData[0]); // 첫 번째 값이 종료 시간
        
            if (extraData.Length > 5) 
                note.hitSoundFile = extraData[5];
        } 
        else 
        {
            // 일반 노트 데이터 예: "0:0:0:40:kick1.wav"
            note.endTime = note.startTime;
        
            string[] extraData = data[5].Split(':');
            if (extraData.Length > 4) 
                note.hitSoundFile = extraData[4];
        }

        noteList.Add(note);
    }
}