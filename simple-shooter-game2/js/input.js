/**
 * Input 클래스
 * 키보드 및 마우스 입력을 처리합니다.
 * - 좌우 이동: A/D, 화살표 키
 * - 발사: SpaceBar, 마우스 클릭
 */
class Input {
  constructor() {
    // 키 상태 저장 객체
    this.keys = {
      left: false,   // A 또는 ← 화살표
      right: false,  // D 또는 → 화살표
      up: false,     // W 또는 ↑ 화살표
      down: false,   // S 또는 ↓ 화살표
      shoot: false   // SpaceBar 또는 마우스 클릭
    };

    // 이벤트 리스너 등록
    this.setupEventListeners();
  }

  /**
   * 키보드 및 마우스 이벤트 리스너 설정
   */
  setupEventListeners() {
    // 키보드 눌림 이벤트
    window.addEventListener('keydown', (e) => {
      this.handleKeyDown(e);
    });

    // 키보드 뗌 이벤트
    window.addEventListener('keyup', (e) => {
      this.handleKeyUp(e);
    });

    // 마우스 클릭 이벤트
    window.addEventListener('mousedown', () => {
      this.keys.shoot = true;
    });

    // 마우스 클릭 해제 이벤트
    window.addEventListener('mouseup', () => {
      this.keys.shoot = false;
    });
  }

  /**
   * 키보드 눌림 처리
   * @param {KeyboardEvent} e - 키보드 이벤트 객체
   */
  handleKeyDown(e) {
    switch (e.key.toLowerCase()) {
      case 'a':
      case 'arrowleft':
        this.keys.left = true;
        break;
      case 'd':
      case 'arrowright':
        this.keys.right = true;
        break;
      case 'w':
      case 'arrowup':
        this.keys.up = true;
        break;
      case 's':
      case 'arrowdown':
        this.keys.down = true;
        break;
      case ' ': // SpaceBar
        this.keys.shoot = true;
        break;
    }
  }

  /**
   * 키보드 뗌 처리
   * @param {KeyboardEvent} e - 키보드 이벤트 객체
   */
  handleKeyUp(e) {
    switch (e.key.toLowerCase()) {
      case 'a':
      case 'arrowleft':
        this.keys.left = false;
        break;
      case 'd':
      case 'arrowright':
        this.keys.right = false;
        break;
      case 'w':
      case 'arrowup':
        this.keys.up = false;
        break;
      case 's':
      case 'arrowdown':
        this.keys.down = false;
        break;
      case ' ': // SpaceBar
        this.keys.shoot = false;
        break;
    }
  }

  /**
   * 왼쪽으로 이동 중인지 확인
   * @returns {boolean} - 왼쪽 이동 상태
   */
  isMovingLeft() {
    return this.keys.left;
  }

  /**
   * 오른쪽으로 이동 중인지 확인
   * @returns {boolean} - 오른쪽 이동 상태
   */
  isMovingRight() {
    return this.keys.right;
  }

  /**
   * 위로 이동 중인지 확인
   * @returns {boolean} - 위쪽 이동 상태
   */
  isMovingUp() {
    return this.keys.up;
  }

  /**
   * 아래로 이동 중인지 확인
   * @returns {boolean} - 아래쪽 이동 상태
   */
  isMovingDown() {
    return this.keys.down;
  }

  /**
   * 발사 중인지 확인
   * @returns {boolean} - 발사 상태
   */
  isShooting() {
    return this.keys.shoot;
  }
}
