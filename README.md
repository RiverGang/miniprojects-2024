# miniprojects-2024
IoT 개발자 미니프로젝트

## 1일차
- 조별 자리배치
- IoT 프로젝트 개요

    ![IoT 프로젝트](https://raw.githubusercontent.com/RiverGang/miniprojects-2024/main/images/mp001.png)

    1. IoT기기 구성 - 아두이노, 라즈베리파이 등 IoT장비들과 연결해서 사용
    2. 서버 구성 - IoT기기와 통신, DB구성, 데이터 수집 앱 개발
    3. 모니터링 구성 - 실시간 모니터링/제어 앱, 전체 연결
    
- 조별 미니프로젝트
    - 5월 28일 (40시간)
    - 구체적으로 어떤 디바이스 구성, 데이터 수집, 모니터링 계획
    - 8월말 정도에 끝나는 프로젝트 일정 계획
    - 요구사항 리스트, 기능명세, UI/UX 디자인, DB설계 문서하나로 통합
    - 5월 28일 오후 각 조별 파워포인트/프레젠테이션 약 10분 발표
    - 요구사항 리스트(Excel 작성)
    - 기능명세 문서
    - DB설계 ERD 또는 SSMS 물리적DB설계
    - UI/UX디자인 16일(목) 전달
    
## 2일차
- 미니프로젝트
    - 프로젝트 문서
    - UI/UX 디자인 툴 설명
        - 피그마: https://www.figma.com/
        - 목업: https://app.moqups.com/
    - 프레젠테이션
    - Notion ...
    - 라즈베리파이(RPi) 리셋팅, 네트워크 설정, VNC(OS UI작업)
    
- 스마트홈 연동 클래스 미니프로젝트
    1. 요구사항 정의, 기능명세, 일정정리
    2. UI/UX 디자인
        - RPi는 디자인 없음(콘솔)
        - 데이터 수신앱
        - 모니터링 앱
    3. DB설계
    4. RPi 세팅(Network)
    5. RPi GPIO, IoT 디바이스 연결(카메라, DHT 센서, RGB LED, ...)
    6. RPi 데이터 전송 파이썬 프로그래밍
    7. PC(Server) 데이터 수신 C# 프로그래밍
    8. 모니터링 앱 개발(수신 및 송신)

## 3일차
- 미니프로젝트
    - 실무 프로젝트 문서
    - Jira 사용법
    - 조별로 진행

- 라즈베리파이 셋팅
    1. RPi 기본 구성 - RPi + MicroSD + Power
    2. RPi 기본 셋팅
        - 한글화
        - 키보드 변경
        - 화면사이즈 변경(RealVNC)
        - Pi Apps 앱설치 도우미 앱
        - Github Desktop, VS Code
        - 네트워크 확인
        - RealVNC Server 자동실행 설정


- 스마트홈 연동 클래스 미니프로젝트
    - RPi 셋팅... 진행

## 4일차
- 라즈베리파이 IoT장비 설치
    [x] 라즈베리파이 카메라
    - 테스트 방법
        1. 카메라센서 연결()
        2. 실행창(명령프롬프터)
        3. sudo libcamera-hello -t 0
    [x] GPIO HAT
    [x] 브레드보드와 연결
    [x] RGB LED 모듈
        - V -> 5V 연결 
        - R -> GPIO4
        - B -> GPI05
        - G -> GPIO6 연결
- VS Code 한글설치: Korean
- VS code Python 설치
    - Python -> Python-Bank 폴더 추가

## 5일차
- 라즈베리파이 IoT장비 설치
    [x] DHT11 센서
        - GND(-) -> GND 8개중 아무 곳이나 연결
        - VCC(+) -> 5V 연결
        - S(OUT) -> GPIO18 연결
        
- 미니프로젝트
    - 팀별 구매목록 작성
    - 프로젝트 결정사항 공유
    - 발표자료 준비
    
- 마우스 감도 설정
    - sudo nano /boot/firmware/cmdline.txt
    - 맨 뒤에 usbhid.mousepoll=0 추가
    - 저장한 뒤, 라즈베리파이 리부팅(sudo reboot)

- wifi connection 문제해결
    - sudo /etc/rc.local
        - exit 0 바로 위
        - sudo iw reg set KR
        - sudo iwconfig wlan0 power off
        - 위의 두문장 추가
