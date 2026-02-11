/**
 * Bullet 클래스
 * 플레이어 및 적의 총알을 관리합니다.
 * - 플레이어 총알: 5×12px, 400px/s 위로, 황색
 * - 적 총알: 8×8px, 200px/s 아래로, 적색
 */
class Bullet {
  /**
   * @param {number} x - 초기 X 좌표
   * @param {number} y - 초기 Y 좌표
   * @param {number} velocityX - X 방향 속도 (px/s)
   * @param {number} velocityY - Y 방향 속도 (px/s, 음수면 위로)
   * @param {string} owner - 총알 소유자 ('player' 또는 'enemy')
   */
  constructor(x, y, velocityX, velocityY, owner) {
    this.x = x;
    this.y = y;
    this.velocityX = velocityX;
    this.velocityY = velocityY;
    this.owner = owner; // 'player' 또는 'enemy'

    // 소유자에 따른 크기 설정
    if (owner === 'player') {
      // 플레이어 총알: 8×15px (충돌 판정 확대)
      this.width = 8;
      this.height = 15;
      this.color = 'rgb(255, 200, 0)'; // 황색
    } else if (owner === 'enemy') {
      // 적 총알: 8×8px
      this.width = 8;
      this.height = 8;
      this.color = 'rgb(255, 50, 50)'; // 적색
    }
  }

  /**
   * 총알 업데이트
   * @param {number} deltaTime - 프레임 간격 (초 단위)
   */
  update(deltaTime) {
    this.x += this.velocityX * deltaTime;
    this.y += this.velocityY * deltaTime;
  }

  /**
   * 총알이 화면을 벗어났는지 확인
   * @param {number} canvasWidth - 캔버스 너비
   * @param {number} canvasHeight - 캔버스 높이
   * @returns {boolean} - 화면 이탈 여부
   */
  isOutOfBounds(canvasWidth, canvasHeight) {
    return (
      this.x + this.width < 0 ||
      this.x > canvasWidth ||
      this.y + this.height < 0 ||
      this.y > canvasHeight
    );
  }

  /**
   * 총알 렌더링
   * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
   */
  render(ctx) {
    ctx.fillStyle = this.color;
    ctx.fillRect(this.x, this.y, this.width, this.height);

    // 테두리 (선택사항)
    if (this.owner === 'player') {
      ctx.strokeStyle = 'rgb(255, 150, 0)';
      ctx.lineWidth = 1;
      ctx.strokeRect(this.x, this.y, this.width, this.height);
    }
  }
}
