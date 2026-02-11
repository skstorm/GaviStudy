// 키보드 입력 처리 클래스
class Input {
  constructor() {
    // 현재 눌러진 키 상태를 저장하는 객체
    this.keys = {
      left: false,      // A 키 또는 왼쪽 화살표
      right: false,     // D 키 또는 오른쪽 화살표
      shoot: false      // SpaceBar
    };

    // 마우스 클릭 상태
    this.mouseClicked = false;

    // 이전 프레임의 마우스 상태 (한 번만 발생하도록)
    this.lastMouseClicked = false;

    this.setupEventListeners();
  }

  // 이벤트 리스너 설정
  setupEventListeners() {
    // 키보드 다운 이벤트
    document.addEventListener('keydown', (e) => {
      const key = e.key.toLowerCase();

      // 좌측 이동 (A 또는 왼쪽 화살표)
      if (key === 'a' || e.code === 'ArrowLeft') {
        this.keys.left = true;
        e.preventDefault();
      }

      // 우측 이동 (D 또는 오른쪽 화살표)
      if (key === 'd' || e.code === 'ArrowRight') {
        this.keys.right = true;
        e.preventDefault();
      }

      // 발사 (SpaceBar)
      if (key === ' ') {
        this.keys.shoot = true;
        e.preventDefault();
      }
    });

    // 키보드 업 이벤트
    document.addEventListener('keyup', (e) => {
      const key = e.key.toLowerCase();

      // 좌측 이동 해제
      if (key === 'a' || e.code === 'ArrowLeft') {
        this.keys.left = false;
        e.preventDefault();
      }

      // 우측 이동 해제
      if (key === 'd' || e.code === 'ArrowRight') {
        this.keys.right = false;
        e.preventDefault();
      }

      // 발사 해제
      if (key === ' ') {
        this.keys.shoot = false;
        e.preventDefault();
      }
    });

    // 마우스 클릭 이벤트
    document.addEventListener('click', (e) => {
      // Canvas 영역인지 확인
      const canvas = document.getElementById('gameCanvas');
      if (canvas && e.target === canvas) {
        this.mouseClicked = true;
      }
    });
  }

  // 프레임 업데이트 (마우스 클릭 상태 리셋)
  update() {
    this.lastMouseClicked = this.mouseClicked;
    this.mouseClicked = false;
  }

  // 현재 왼쪽 이동 중인지 확인
  isMovingLeft() {
    return this.keys.left;
  }

  // 현재 오른쪽 이동 중인지 확인
  isMovingRight() {
    return this.keys.right;
  }

  // 현재 발사 중인지 확인
  isShooting() {
    return this.keys.shoot;
  }

  // 마우스 클릭 여부 확인 (한 번만 true)
  wasMouseClicked() {
    return this.mouseClicked && !this.lastMouseClicked;
  }
}
