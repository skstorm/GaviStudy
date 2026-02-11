/**
 * Enemy 클래스
 * 적 기체를 관리합니다.
 * - 기본 적: 25×25px, 1HP, 직선 하강
 * - 강화 적: 35×35px, 3HP, 진동 이동, 발사 가능
 */
class Enemy {
  /**
   * @param {string} type - 적 타입 ('basic' 또는 'strong')
   * @param {number} x - 초기 X 좌표
   * @param {number} y - 초기 Y 좌표
   * @param {number} baseSpeed - 기본 속도 (px/s)
   */
  constructor(type, x, y, baseSpeed) {
    this.type = type; // 'basic' 또는 'strong'
    this.x = x;
    this.y = y;
    this.initialX = x; // 진동 패턴을 위한 초기 X 좌표

    // 타입에 따른 속성 설정
    if (type === 'basic') {
      // 기본 적: 25×25px, 1HP
      this.width = 25;
      this.height = 25;
      this.maxHp = 1;
      this.speed = baseSpeed;
    } else if (type === 'strong') {
      // 강화 적: 35×35px, 3HP, 80% 속도
      this.width = 35;
      this.height = 35;
      this.maxHp = 3;
      this.speed = baseSpeed * 0.8; // 사양서: 기본 적의 80% 속도
    }

    this.hp = this.maxHp;

    // 발사 관련 (강화 적만)
    this.lastShootTime = 0;
    this.shootInterval = 1.5; // 사양서: 1.5초마다 발사

    // 진동 패턴용 경과 시간
    this.elapsedTime = 0;
  }

  /**
   * 적 업데이트
   * @param {number} deltaTime - 프레임 간격 (초 단위)
   * @param {number} waveMultiplier - Wave 난이도 배수
   */
  update(deltaTime, waveMultiplier) {
    this.elapsedTime += deltaTime;

    // 강화 적의 경우 좌우 진동 패턴 적용
    if (this.type === 'strong') {
      // 사양서: X좌표 = 초기X + sin(경과시간 * 2) * 30
      this.x = this.initialX + Math.sin(this.elapsedTime * 2) * 30;
    }

    // 아래로 이동 (Wave 배수 적용)
    this.y += this.speed * waveMultiplier * deltaTime;
  }

  /**
   * 발사 가능 여부 확인 (강화 적만)
   * @param {number} currentTime - 현재 시간 (초 단위)
   * @returns {boolean} - 발사 가능 여부
   */
  shoot(currentTime) {
    // 기본 적은 발사 불가
    if (this.type !== 'strong') {
      return false;
    }

    // 발사 간격 확인 (1.5초)
    if (currentTime - this.lastShootTime >= this.shootInterval) {
      this.lastShootTime = currentTime;
      return true;
    }
    return false;
  }

  /**
   * 데미지를 받습니다.
   * @param {number} damage - 받을 데미지 (기본값: 1)
   * @returns {boolean} - 적이 파괴되었는지 여부
   */
  takeDamage(damage = 1) {
    this.hp -= damage;
    return this.hp <= 0; // 체력이 0 이하면 파괴됨
  }

  /**
   * 적이 살아있는지 확인
   * @returns {boolean} - 생존 여부
   */
  isAlive() {
    return this.hp > 0;
  }

  /**
   * 적 렌더링
   * - 기본 적: 적색
   * - 강화 적: 주황색
   * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
   */
  render(ctx) {
    if (this.type === 'basic') {
      // 기본 적: 적색
      ctx.fillStyle = 'rgb(255, 50, 50)';
    } else if (this.type === 'strong') {
      // 강화 적: 주황색
      ctx.fillStyle = 'rgb(255, 140, 0)';
    }

    // 적 기체 그리기
    ctx.fillRect(this.x, this.y, this.width, this.height);

    // 테두리
    ctx.strokeStyle = 'rgb(150, 0, 0)';
    ctx.lineWidth = 2;
    ctx.strokeRect(this.x, this.y, this.width, this.height);

    // 강화 적의 경우 체력 표시 (체력 바)
    if (this.type === 'strong' && this.maxHp > 1) {
      const barWidth = this.width - 4;
      const barHeight = 4;
      const barX = this.x + 2;
      const barY = this.y - 8;

      // 체력 바 배경 (어두운 회색)
      ctx.fillStyle = 'rgb(50, 50, 50)';
      ctx.fillRect(barX, barY, barWidth, barHeight);

      // 체력 바 전경 (초록색)
      ctx.fillStyle = 'rgb(0, 255, 0)';
      const currentBarWidth = (this.hp / this.maxHp) * barWidth;
      ctx.fillRect(barX, barY, currentBarWidth, barHeight);
    }
  }
}
