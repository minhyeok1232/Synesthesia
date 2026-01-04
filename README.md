# Synesthsia
### Unity 엔진을 활용하여 개발된 Built-In(PC) 2D 리듬게임 입니다.
<img width="1212" height="865" alt="image" src="https://github.com/user-attachments/assets/c4d15e14-e882-423f-8cae-fe606111d06d" />


## 📌 목차
1. [🔎 프로젝트 소개](#-프로젝트-소개)
2. [🕒 프로젝트 기간](#-프로젝트-기간)
3. [🔗 클래스 다이어그램](#-클래스-다이어그램) 
4. [🔄 진행 및 개선 사항](#-진행-및-개선-사항)
5. [💵 객체지향적 사고 관점 설명](#-객체지향적-사고-관점-설명)
    
--- 
 
## 🔎 프로젝트 소개
- **장르** : Built-In (PC) 2D Rhythm Game (3D Perspective View)
- **IDE** : Unity 6000.1f, GetBrains Rider
- **목적** :
  - 정밀한 비트맵 파싱 시스템 구축: 외부 .osu 파일의 HitObjects와 TimingPoints 데이터를 정밀하게 분석하여 게임 내 노트 정보로 실시간 동기화하는 파싱 엔진 구현
  - 객체지향적 게임 엔진 설계: SOLID 원칙과 디자인 패턴(Singleton)을 적용하여 데이터 처리, 판정 로직, 시각 연출이 독립적으로 작동하는 확장성 높은 리듬 게임 프레임워크 개발
  - 몰입감 있는 3D 연출 구현: 2D 리듬 게임의 메커니즘을 3D 원근 카메라 시스템(Perspective View)과 결합하여 시각적 공간감과 박진감 넘치는 타격감 제공
  - 성능 최적화 및 자원 관리: 오브젝트 풀링(Object Pooling)을 통한 런타임 성능 최적화와 비동기 리소스 로딩 시스템을 활용한 효율적인 메모리 관리 실현
  - 롱노트 및 다중 판정 로직 고도화: 단순 단타 노트를 넘어 틱(Tick) 기반의 롱노트 홀딩 판정과 정밀한 타이밍 박스 시스템을 통한 완성도 높은 게임플레이 로직 구축
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
현재 시스템은 OSU 파일 형식(.osu)을 분석하여 게임 내 노트 데이터로 변환하고, 이를 UI와 연동하는 구조를 갖추고 있습니다.

#### 데이터 추출 (DataManager):
- 섹션별 파싱: .osu 파일의 [TimingPoints]와 [HitObjects] 섹션을 구분하여 읽어들입니다.
- 노트 정보 변환: 파일에 기록된 x좌표를 기반으로 레인(0~3번)을 계산하고, 단타 및 롱노트의 시작/종료 시간을 ms 단위로 추출하여 NoteInfo 리스트에 저장합니다.
- 개별 사운드 매칭: 각 노트에 할당된 히트 사운드(예: kick1.wav)와 볼륨 데이터를 파싱하여 사운드 피드백을 준비합니다.
- 곡 선택 및 명령 (StageMenu):
- 곡 정보 관리: Song 클래스를 통해 곡 제목, 작곡가, 프리뷰 시간 등을 관리합니다.
- 파싱 트리거: 플레이 버튼(BtnPlay)을 누르면 현재 선택된 곡의 이름을 DataManager.StartParsing()으로 전달하여 해당 곡의 데이터를 즉시 로드합니다.
- 사운드 비동기 로드: 곡 데이터 로드와 동시에 SoundManager를 통해 StreamingAssets 폴더 내의 MP3 파일과 효과음들을 비동기로 불러와 메모리 효율을 높입니다.

#### 데이터 흐름 요약:

- StageMenu: 사용자가 곡을 선택하고 플레이 클릭.
- DataManager: StreamingAssets 내 해당 곡의 폴더를 찾아 .osu 파일을 한 줄씩 읽어 데이터화.
- GameManager: 파싱된 NoteList와 TimingPoints를 수신하여 게임 플레이 씬에 전달.
- TimingController: 전달받은 데이터를 바탕으로 음악 시간에 맞춰 실시간 노트 생성.


### 🔀 카메라 시스템 개선
#### 사용자에게 몰입감 있는 연출을 제공
- 기존의 평면적인 2D UI 레이아웃에서 벗어나, 유니티의 Perspective Camera와 3D 공간 배치를 활용하여 시각적 요소를 추가하였습니다.
<img width="1242" height="857" alt="image" src="https://github.com/user-attachments/assets/22a45712-5f8d-47fa-9bc6-47218e98c091" />

<br><br>

## 💵 객체지향적 사고 관점 설명
### 객체지향의 특징
<details>
  <summary> 객체지향의 특징 </summary>

### 1️⃣ `캡슐화'
<img width="1121" height="793" alt="image" src="https://github.com/user-attachments/assets/76e964b2-fabe-4d41-9e8c-47785647f2a2" />
- 정보 은닉 및 상태 보호: ComboController나 ScoreController의 점수($currentScore$)와 콤보($currentCombo$) 변수를 private으로 선언하여 외부에서 직접 수정하지 못하도록 보호하였습니다.
- 메서드를 통한 접근: 오직 IncreaseScore()나 IncreaseCombo()와 같은 공개된 메서드를 통해서만 상태가 변경되도록 설계하여 데이터의 무결성을 유지합니다.


### 2️⃣ `추상화`
<img width="1115" height="582" alt="image" src="https://github.com/user-attachments/assets/2ef4d5e7-26b8-42b0-87e7-98094d55e88b" />
- 리듬 게임의 복잡한 정보를 NoteInfo, TimingPointData, Song 클래스/구조체로 정의하여, 필요한 속성(시간, 위치, 타입 등)만 추려내어 관리합니다.
<img width="1122" height="687" alt="image" src="https://github.com/user-attachments/assets/965e9223-0d13-4f55-890f-80c6d3940f6c" />
- Note.cs에서 일반 노트와 롱노트의 물리적 차이를 Setup()이라는 하나의 초기화 메서드 안에 추상화하여, 외부에서는 노트 종류와 상관없이 동일한 방식으로 생성할 수 있게 하였습니다.

### 3️⃣ '상속성'
<img width="1121" height="260" alt="image" src="https://github.com/user-attachments/assets/9c629633-6740-4146-91ab-986427186279" />
- 모든 매니저 클래스(GameManager, SoundManager 등)가 Singleton<T>을 상속받도록 하여, 싱글톤 패턴을 위한 중복 코드를 작성하지 않고 관리 기능을 재사용합니다.

### 4️⃣ '다형성'
<img width="969" height="757" alt="image" src="https://github.com/user-attachments/assets/13f3e421-5991-42d0-bd29-e36af2be7e13" />
Note 객체는 단일 클래스이지만, isLongNote 플래그에 따라 SetLongNoteLayout() 또는 SetNormalInitializeNote()가 실행되어 서로 다른 형태와 물리 크기를 가집니다.
이는 클래스를 세분화를 하여, 확장 설계가 가능합니다.

</details>

### 객체지향의 원칙
<details>
  <summary> 객체지향의 원칙 </summary>

### 1️⃣ 단일 책임 원칙(SRP)
<img width="1120" height="838" alt="image" src="https://github.com/user-attachments/assets/bc2bca64-fa6c-476b-8da2-105e4689990b" />
- 역할 분리: 각 매니저들은 각자의 역할을 담당합니다. (DataManager, SoundManager, ... )

### 2️⃣ 개방-폐쇄 원칙(OCP)
<img width="828" height="857" alt="image" src="https://github.com/user-attachments/assets/ab76bb20-c4b9-4128-b167-2d7c27c42ef7" />
- 확장성: ObjectPool은 ObjectInfo 배열을 통해 새로운 종류의 프리팹(예: 골드 노트, 장애물 등)이 추가되어도 내부 코드를 수정할 필요 없이 인스펙터에서의 설정만으로 대응이 가능합니다.

### 3️⃣ 리스코프 치환 원칙(LSP)
<img width="992" height="833" alt="image" src="https://github.com/user-attachments/assets/c69e2943-7e00-41c8-b222-67039f87edad" />
- 싱글톤 확장: Singleton<T>을 상속받은 어떠한 클래스(T)라도 Instance 속성을 통해 부모의 기능을 완벽하게 수행하며 전역적으로 안전하게 접근 가능합니다.

### 4️⃣ 인터페이스 분리 원칙(ISP)
<img width="1118" height="433" alt="image" src="https://github.com/user-attachments/assets/641c7848-bba6-4619-b9d3-0705d1af414b" />
- GameManager는 각 컨트롤러를 Get 메서드로 제공하여, 필요한 클래스가 자신에게 필요한 컨트롤러 기능만 호출하여 사용할 수 있도록 통로를 분리하였습니다.

### 5️⃣ 의존 역전 원칙(DIP)
<img width="1123" height="374" alt="image" src="https://github.com/user-attachments/assets/758b7c42-cec4-4ec2-b8e1-16ff199a152f" />
- 중앙 집중 참조: 각 컨트롤러(Player, Timing 등)는 서로를 직접 참조하지 않습니다. 대신 GameManager라는 상위 중계자를 통해 통신함으로써 객체 간의 결합도를 낮추고 유연성을 확보했습니다.

</details>

---
#### 플레이 영상 : [https://www.youtube.com/watch?v=Wb8Qj6Dlg80](https://www.youtube.com/watch?v=Wb8Qj6Dlg80)
<img width="846" height="864" alt="image" src="https://github.com/user-attachments/assets/42f96e2e-eede-455e-a021-526788661b74" />

