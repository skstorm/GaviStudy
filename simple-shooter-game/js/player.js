// 플레이어 클래스
class Player {
  constructor(x, y) {
    // 위치
    this.x = x;
    this.y = y;

    // 크기
    this.width = 30;
    this.height = 30;

    // 이동 속성
    this.speed = 300; // px/s
    this.velocityX = 0; // 현재 속도

    // 체력
    this.hp = 3;
    this.maxHp = 3;

    // 무적 시간 (초 단위)
    this.invulnerableTime = 0;
    this.invulnerableDuration = 2.0; // 2초 무적

    // 발사 관련
    this.lastShootTime = 0;
    this.shootInterval = 0.15; // 0.15초 마다 발사 가능
  }

  // 게임 업데이트
  update(input, deltaTime, canvasWidth) {
    // 입력에 따른 속도 설정
    if (input.isMovingLeft()) {
      this.velocityX = -this.speed;
    } else if (input.isMovingRight()) {
      this.velocityX = this.speed;
    } else {
      this.velocityX = 0;
    }

    // 위치 업데이트
    this.x += this.velocityX * deltaTime;

    // 화면 경계 처리 (플레이어는 화면을 벗어날 수 없음)
    if (this.x < 0) {
      this.x = 0;
    }
    if (this.x + this.width > canvasWidth) {
      this.x = canvasWidth - this.width;
    }

    // 무적 시간 감소
    if (this.invulnerableTime > 0) {
      this.invulnerableTime -= deltaTime;
    }

    // 발사 시간 증가
    this.lastShootTime += deltaTime;
  }

  // 발사 가능한지 확인 및 발사 처리
  shoot() {
    // 발사 간격이 지났는지 확인
    if (this.lastShootTime >= this.shootInterval) {
      this.lastShootTime = 0;

      // 플레이어 위치의 중앙에서 발사
      const bulletX = this.x + this.width / 2 - 2.5; // 총알 너비가 5px이므로
      const bulletY = this.y - 10; // 플레이어 위쪽에서 발사

      return new Bullet(bulletX, bulletY, true); // true = 플레이어 총알
    }
    return null;
  }

  // 데미지 처리
  takeDamage(damage = 1) {
    // 무적 상태가 아닐 때만 데미지 처리
    if (!this.isInvulnerable()) {
      this.hp -= damage;
      this.invulnerableTime = this.invulnerableDuration;

      // 체력이 0 이하로 내려가지 않도록
      if (this.hp < 0) {
        this.hp = 0;
      }
    }
  }

  // 무적 상태 확인
  isInvulnerable() {
    return this.invulnerableTime > 0;
  }

  // 생존 여부 확인
  isAlive() {
    return this.hp > 0;
  }

  // Canvas에 그리기
  render(ctx) {
    // 무적 상태일 때는 깜빡이는 효과
    if (this.isInvulnerable()) {
      // 무적 시간에 따라 투명도 변경 (깜빡이는 효과)
      const alpha = Math.sin(this.invulnerableTime * Math.PI * 3) * 0.5 + 0.5;
      ctx.globalAlpha = alpha;
    }

    // 플레이어 기체를 사각형으로 그리기 (파란색)
    ctx.fillStyle = '#0099FF';
    ctx.fillRect(this.x, this.y, this.width, this.height);

    // 플레이어 기체 테두리
    ctx.strokeStyle = '#0066CC';
    ctx.lineWidth = 2;
    ctx.strokeRect(this.x, this.y, this.width, this.height);

    // 조종석 표시 (작은 원)
    ctx.fillStyle = '#FFD700';
    ctx.beginPath();
    ctx.arc(
      this.x + this.width / 2,
      this.y + this.height / 2,
      3,
      0,
      Math.PI * 2
    );
    ctx.fill();

    // 알파값 복원
    ctx.globalAlpha = 1.0;
  }
}
