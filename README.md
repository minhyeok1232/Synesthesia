# Synesthsia
Unity 엔진을 활용하여 개발된 Built-In(PC) 2D 리듬게임 입니다.

## 📌 목차
1. [🔎 프로젝트 소개](#-프로젝트-소개)
2. [🕒 프로젝트 기간](#-프로젝트-기간)
3. [🔗 클래스 다이어그램](#-클래스-다이어그램) 
4. [🔄 진행 및 개선 사항](#-진행-및-개선-사항)
5. [⚡ 프로젝트 최적화 과정](#-프로젝트-최적화-과정)
6. [📝 개발 관점에서의 느낀 점](#-개발-관점에서의-느낀-점)
    
--- 
 
## 🔎 프로젝트 소개
- **장르** : Built-In(PC) 2D Rhythm Game
- **IDE** : Unity 6000.1f, GetBrains Rider
- **목적** :  
  - OSU 게임 데이터를 파싱하여, ~~ 

<details>
  <summary>🎇 프로젝트 실행 방법</summary>
 
### 1️⃣ Git Clone 
  ```bash
  git clone https://github.com/minhyeok1232/Synesthesia.git 
```
### 2️⃣ 실행 파일
  Unity Hub 실행 후, 클론한 프로젝트 폴더를 선택 후 "Open" 클릭!
</details>

## 🎯 프로젝트 기간
- MVP 개발 기간 : 2025.12.07 ~ 2025.12.21 
- 프로젝트 인원 : 1인 (개인)

## 🔗 클래스 다이어그램
### 객체지향 설계를 반영한 클래스 구조도
<img width="1325" height="430" alt="image" src="https://github.com/user-attachments/assets/cc489af7-e180-422e-afba-dfad924d322d" />

## 🔄 진행 및 개선 사항
### ✨ 사용자 데이터 저장 및 곡 파싱 시스템
#### 📑 OSU 데이터 파싱 및 연동 (DataManager & StageMenu)
현재 시스템은 **OSU 파일 형식(.osu)**을 분석하여 게임 내 노트 데이터로 변환하고, 이를 UI와 연동하는 구조를 갖추고 있습니다.

데이터 추출 (DataManager):

섹션별 파싱: .osu 파일의 [TimingPoints]와 [HitObjects] 섹션을 구분하여 읽어들입니다.

노트 정보 변환: 파일에 기록된 x좌표를 기반으로 레인(0~3번)을 계산하고, 단타 및 롱노트의 시작/종료 시간을 ms 단위로 추출하여 NoteInfo 리스트에 저장합니다.

개별 사운드 매칭: 각 노트에 할당된 히트 사운드(예: kick1.wav)와 볼륨 데이터를 파싱하여 사운드 피드백을 준비합니다.

곡 선택 및 명령 (StageMenu):

곡 정보 관리: Song 클래스를 통해 곡 제목, 작곡가, 프리뷰 시간 등을 관리합니다.

파싱 트리거: 플레이 버튼(BtnPlay)을 누르면 현재 선택된 곡의 이름을 DataManager.StartParsing()으로 전달하여 해당 곡의 데이터를 즉시 로드합니다.

사운드 비동기 로드: 곡 데이터 로드와 동시에 SoundManager를 통해 StreamingAssets 폴더 내의 MP3 파일과 효과음들을 비동기로 불러와 메모리 효율을 높입니다.

데이터 흐름 요약:

StageMenu: 사용자가 곡을 선택하고 플레이 클릭.

DataManager: StreamingAssets 내 해당 곡의 폴더를 찾아 .osu 파일을 한 줄씩 읽어 데이터화.

GameManager: 파싱된 NoteList와 TimingPoints를 수신하여 게임 플레이 씬에 전달.

TimingController: 전달받은 데이터를 바탕으로 음악 시간에 맞춰 실시간 노트 생성.


### 🔀 카메라 시스템 개선
#### 사용자에게 몰입감 있는 연출을 제공
- 기존의 평면적인 2D UI 레이아웃에서 벗어나, 유니티의 Perspective Camera와 3D 공간 배치를 활용하여 시각적 요소를 추가하였습니다.
<img width="1242" height="857" alt="image" src="https://github.com/user-attachments/assets/22a45712-5f8d-47fa-9bc6-47218e98c091" />

<br><br>

## ⚡ 프로젝트 최적화 과정

