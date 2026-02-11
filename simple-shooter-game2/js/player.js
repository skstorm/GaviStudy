/**
 * Player 클래스
 * 플레이어 기체를 관리합니다.
 * - 이동, 발사, 데미지 처리
 * - 사양서 기준: 30×30px, 속도 300px/s, 체력 3, 발사 간격 0.15초
 */
class Player {
  /**
   * @param {number} x - 초기 X 좌표
   * @param {number} y - 초기 Y 좌표
   */
  constructor(x, y) {
    this.x = x;
    this.y = y;
    this.width = 30;   // 사양서: 30px
    this.height = 30;  // 사양서: 30px
    this.speed = 300;  // 사양서: 300px/s
    this.maxHp = 3;    // 사양서: 초기 체력 3
    this.hp = this.maxHp;

    // 발사 관련
    this.lastShootTime = 0;        // 마지막 발사 시간
    this.shootInterval = 0.15;     // 사양서: 0.15초 간격

    // 무적 관련
    this.invulnerableTime = 0;     // 남은 무적 시간
    this.invulnerableDuration = 2; // 사양서: 2초 무적
  }

  /**
   * 플레이어 업데이트
   * @param {Input} input - 입력 객체
   * @param {number} deltaTime - 프레임 간격 (초 단위)
   * @param {number} canvasWidth - 캔버스 너비
   * @param {number} canvasHeight - 캔버스 높이
   */
  update(input, deltaTime, canvasWidth, canvasHeight) {
    // 좌우 이동 처리 (즉시 최대 속도로 이동)
    if (input.isMovingLeft()) {
      this.x -= this.speed * deltaTime;
    }
    if (input.isMovingRight()) {
      this.x += this.speed * deltaTime;
    }

    // 상하 이동 처리
    if (input.isMovingUp()) {
      this.y -= this.speed * deltaTime;
    }
    if (input.isMovingDown()) {
      this.y += this.speed * deltaTime;
    }

    // 화면 경계 처리 (좌우)
    if (this.x < 0) {
      this.x = 0;
    }
    if (this.x + this.width > canvasWidth) {
      this.x = canvasWidth - this.width;
    }

    // 화면 경계 처리 (상하)
    if (this.y < 0) {
      this.y = 0;
    }
    if (this.y + this.height > canvasHeight) {
      this.y = canvasHeight - this.height;
    }

    // 무적 시간 감소
    if (this.invulnerableTime > 0) {
      this.invulnerableTime -= deltaTime;
      if (this.invulnerableTime < 0) {
        this.invulnerableTime = 0;
      }
    }
  }

  /**
   * 발사 가능 여부 확인 및 발사 시간 갱신
   * @param {number} currentTime - 현재 시간 (초 단위)
   * @returns {boolean} - 발사 가능 여부
   */
  shoot(currentTime) {
    // 발사 간격 확인
    if (currentTime - this.lastShootTime >= this.shootInterval) {
      this.lastShootTime = currentTime;
      return true;
    }
    return false;
  }

  /**
   * 데미지를 받습니다.
   * - 무적 상태에서는 데미지를 받지 않습니다.
   * - 데미지를 받으면 2초간 무적 상태가 됩니다.
   * @returns {boolean} - 데미지를 실제로 받았는지 여부
   */
  takeDamage() {
    // 무적 상태면 데미지 무시
    if (this.invulnerableTime > 0) {
      return false;
    }

    // 체력 감소
    this.hp--;

    // 무적 시간 설정 (2초)
    this.invulnerableTime = this.invulnerableDuration;

    return true;
  }

  /**
   * 무적 상태 확인
   * @returns {boolean} - 무적 상태 여부
   */
  isInvulnerable() {
    return this.invulnerableTime > 0;
  }

  /**
   * 플레이어가 살아있는지 확인
   * @returns {boolean} - 생존 여부
   */
  isAlive() {
    return this.hp > 0;
  }

  /**
   * 플레이어 렌더링
   * - 무적 상태일 때 깜빡임 효과
   * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
   */
  render(ctx) {
    // 무적 상태일 때 깜빡임 효과 (0.2초마다)
    if (this.isInvulnerable()) {
      const blinkInterval = 0.2;
      const blinkPhase = Math.floor(this.invulnerableTime / blinkInterval) % 2;
      if (blinkPhase === 0) {
        return; // 깜빡임: 렌더링 생략
      }
    }

    // 플레이어 기체 그리기 (청색)
    ctx.fillStyle = 'rgb(100, 150, 255)';
    ctx.fillRect(this.x, this.y, this.width, this.height);

    // 테두리
    ctx.strokeStyle = 'rgb(50, 100, 200)';
    ctx.lineWidth = 2;
    ctx.strokeRect(this.x, this.y, this.width, this.height);

    // 기체 중앙 표시 (흰색 원)
    ctx.fillStyle = 'white';
    ctx.beginPath();
    ctx.arc(this.x + this.width / 2, this.y + this.height / 2, 5, 0, Math.PI * 2);
    ctx.fill();
  }
}
