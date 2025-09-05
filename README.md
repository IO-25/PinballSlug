# PinBallSlug 
Run & Gun X Brick Out Ver 1.0.0

## 📖 목차
1. [프로젝트 소개](#-프로젝트-소개)
2. [팀소개](#-팀소개)
3. [조작법](#-조작법)
4. [주요기능](#-주요기능)
5. [개발기간](#-개발기간)
6. [기술스택](#-기술스택)
7. [와이어프레임](#-와이어프레임)
8. [프로젝트 파일 구조](#-프로젝트-파일-구조)
9. [Trouble Shooting](#️-trouble-shooting)

---

## 👨‍🏫 프로젝트 소개
![Image](https://github.com/user-attachments/assets/b6002ec3-87d6-46a8-a72f-a7751811b03f)
  <br />
- 제목 : 핀볼슬러그 ( PinballSlug )
- 장르 : 2D 횡스크롤 탄막 액션 슈팅
- 핵심 컨셉 : 메탈 슬러그의 속도감 넘치는 액션과 벽돌깨기의 튕기는 탄환을 결합한 아케이드 슈팅 게임
- 플랫폼 : PC
- 플레이 인원 : 1인

---

## 🧑‍🤝‍🧑 팀소개
### 🔸기획
- 오경석
- 김현수
### 🔹개발
- 김혜현 : 플레이어 조작, 무기
- 정희찬 : 플랫폼, 시스템, UI
- 문장원 : 몬스터, 레벨 디자인


---

## 🎮 조작법

| 기능               | 키           |
|--------------------|--------------|
| **이동**           |   A, S, D    |
| **점프**           |    Space     |
| **기본 공격**      |    Mouse Left      |
| **특수 공격**      |     Mouse Right      |
| **무기 변경**      |      Q       |
| **옵션 토글**      |      ESC       |
---

### 🧪 테스트용 단축키 (Unity Editor 에서만 동작)

| 기능                           | 키    |
|--------------------------------|--------|
| 스테이지 클리어                 | `K`    |
| 랜덤 드롭 아이템 생성                 | `H`    |
---

## 💜 주요기능

### 1. 러닝 & 건 플레이 (Run & Gun)
캐릭터 이동 + 기본 사격 지원  
다양한 무기 아이템 드랍 및 교체 시스템  

### 2. 벽돌 깨기(Brick Out) 요소 결합
탄환 반사 / 튕김 시스템   
파괴 가능한 적 블록 구조   

---

## ⏲️ 개발기간

- **2025.09.01(월) ~ 2025.09.05(금)**

---

## 📚 기술스택

### ✔️ Language
- C#

### ✔️ Version Control
- Git
- GitHub

### ✔️ Framework / Engine
- Unity 2022.3 LTS

### ✔️ IDE
- Unity Editor
- Visual Studio 2022

### ✔️ 협업 툴
- Notion (기획 및 문서)
- Figma (UI 설계)

---

## 🖼 와이어프레임


---

## 📁 프로젝트 파일 구조
```
Assets
 ┣ 📂 Animations              # 애니메이션 리소스 (Animator, AnimationClip 등)
 ┣ 📂 Externals               # 외부 라이브러리 및 플러그인
 ┣ 📂 Materials               # 머티리얼(Material) 리소스
 ┣ 📂 Prefabs                 # 프리팹(재사용 가능한 게임 오브젝트 템플릿)
 ┣ 📂 Resources               # 런타임 로드(Resources.Load)용 리소스
 ┣ 📂 Scenes                  # Unity 씬(Scene) 파일
 ┣ 📂 ScriptableObjects       # ScriptableObject 에셋 (데이터 관리용)
 ┗ 📂 Scripts                 # C# 스크립트 코드
```
---

## 🛠️ Trouble Shooting

# 드랍 아이템 구현 및 생성 (김혜현)
# 1차 목표

- 몬스터 처치 시 플레이어가 획득할 수 있는 **드랍 아이템** 구현
- 드랍 아이템 속성(스프라이트, 획득 무기 등)을 **유연하게 관리**
    
    **1차 시도: 무기별 프리팹 생성**
    
    - 무기별로 **개별 드랍 아이템 프리팹** 생성

      <img width="197" height="43" alt="Img_01" src="https://github.com/user-attachments/assets/6c5bf97c-fafd-4958-a964-e36eb43df551" />
        
    
    ## **문제점**
    
    - 새로운 무기 추가 시 **매번 프리팹 생성 필요**
    - 공통 로직/속성 변경 시 **모든 프리팹을 일일이 수정해야 함**
    - **재사용성 낮음**
    
    ## **2차 시도: 데이터 중심 설계**
    
    - 드랍 아이템 **프리팹 1개만 사용**
    - 무기별 속성(스프라이트, 획득 무기 등)을 **ScriptableObject**(DropItemData)로 분리
    
    <img width="395" height="172" alt="Img_02" src="https://github.com/user-attachments/assets/9fb783d5-f69e-42ce-a065-3ab5c43e861e" />
    <img width="706" height="289" alt="Img_03" src="https://github.com/user-attachments/assets/7a06534b-6f60-4097-a318-4c2665ae2499" />
    
    ## **성과**
    
    - **새로운 무기 추가 시 데이터만 생성하면 됨** → 프리팹 재사용 가능
    - 공통 로직 변경 시 **프리팹 하나만 수정** → 유지보수 용이

    ![DropItemGIF](https://github.com/user-attachments/assets/e7a2a81c-8939-42fe-95bf-785c8d82ce5b)


# **2차 목표**

- 몬스터가 처치될 때 드랍 아이템 생성 로직을 단순화

## **시도**

- **DropItemFactory** 구현
- Resources 폴더를 통해 **드랍 아이템 프리팹**과 **DropItemData**를 동적으로 로드
- Factory 내부에서 **아이템 생성과 초기화 로직을 한 곳에서 관리**
    
    <img width="836" height="163" alt="Img_04" src="https://github.com/user-attachments/assets/a2343884-6c27-4782-9fed-7eb1faf6007e" />
    <img width="630" height="147" alt="Img_05" src="https://github.com/user-attachments/assets/c9f04166-c358-48f9-95ba-ad06b257a8bc" />

## **성과**

몬스터가 죽었을 때 **간단히 Factory 호출만으로 드랍 아이템 생성 가능**

이전

<img width="495" height="208" alt="Img_06" src="https://github.com/user-attachments/assets/548ee85b-0206-4ce1-b9be-6249aab60179" />

이후

<img width="692" height="121" alt="Img_07" src="https://github.com/user-attachments/assets/5f655e93-0167-4c49-84de-63afa867fbe4" />

# 끊임없는 배경화면 생성 (정희찬)

## **문제 상황**
씬 전환시 게임씬의 배경화면에서 오류가 발생

  <br />  <img width="418" height="129" alt="Image" src="https://github.com/user-attachments/assets/ad53efd6-baaf-4d64-b808-04010bcd2679" />
        
## **원인파악**
배경화면 프리팹을 일정한 위치에 도착하면 파괴되도록 만들었는데, 다시 게임씬으로 돌아가려하니 파괴된 배경화면 프리팹에 접근하려 해서 오류가 발생
    
## **기각된 방식**
기존에 쓰던 프리팹을 특정지점에서 파괴, 생성하는 방식

## **해결 방안**
특정지점을 넘어가면 맨 앞에 있는 배경을 맨 뒤에 배치하는 '오브젝트 풀링(Object Pooling)' 방식을 사용

## **추가 트러블슈팅**
첫 번째 배경 조각이 재활용되는 순간부터 배경 간격이 벌어지는 문제 발생
lastSpawnPositionX라는 변수를 사용해 위치를 추적했는데, 이 변수와 실제로 마지막에 배치된 배경 조각의 위치가 정확하게 일치하지 않아 간격이 벌어지는 문제 발생
프리팹의 길이를 자동으로 간격을 계산하는 방법 대신, 수동으로 입력하여 프리팹끼리의 간격을 조정

## **이번 기회를 통해 배운점**
오브젝트 풀링(Object Pooling)방식의 장점을 확실히 알 수 있는 기회
반복되는 오브젝트의 경우 삭제, 재생성하는 방식을 하지 말고 오브젝트 풀링을 통해 성능 저하를 줄이고 게임의 안정성을 높여야 한다는 점을 깨달음

# Scriptable Object  확률 계산 및 데이터 관리 (문장원)

## **구현 내용**

- 아이템 드롭 확률 테이블을 Scriptable Object로 관리
- 확률을 정규화 시킨 후 확률의 합을 구해 Random 값이 해당 값 사이라면 해당 인덱스를 반환

## **문제 상황**

- 시간이 흐를수록 마지막 인덱스 항목의  등장 확률이 높아짐

![Fig1.png](https://raw.githubusercontent.com/IO-25/PinballSlug/refs/heads/main/ReadmeSourceFiles/JangwonMoon/Fig1.png)

## 원인 파악

- Scriptable Object의 확률표가 계속해서 변함
- 이 변동된 확률표를 모든곳에서 참조하다 보니, 마지막 인덱스 확률이 높아짐

![Fig2.png](https://github.com/IO-25/PinballSlug/blob/main/ReadmeSourceFiles/JangwonMoon/Fig2.png)

## 기각한 방식

- 확률표를 깊은복사하여 사용하고, 기존 데이터를 유지시키기
- 문제 : 확률표 내용이 많아지면 매번 깊은 복사를 하는데도 시간이 많이 소요됨

## 해결 방향

<구현 알고리즘의 변경>

변동이 되면 안되는 확률표는 정규화만 하고 유지

매번 새로 뽑아 낼 Random값을 변경하여  판별하는 식으로 알고리즘 변경

(Random 값에 현제 확률을 빼고, 랜덤값이 0 이하라면 해당 인덱스를 반환)

## 추가사안

Scriptable Object내부에서 직접 동일 확률을 가진 확률표를 만드는 경우

해당 데이터들을 복제할 시 서로 참조를 하게 되면 Destroy시 참조가 같이 사라짐

<해결방식> 동일 확률의 경우 개수만이 중요하기 때문에 따로 메소드를 제작

Scriptable Object가 아닌 해당 메소드 실행시 내부 변수로만 확률 처리

## **이번 기회를 통해 배운점**

Scriptable Object의 경우 값이 변동되지 않을 고정값들만 저장

Scriptable Object의 값을 직접 변경은 지양

변동 값이 존재할 시 Monobehaviour등 외부에서  값을 복사하여 사용
