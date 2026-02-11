// 적 클래스
class Enemy {
  constructor(type, x, y) {
    // 종류: 'basic' 또는 'strong'
    this.type = type;

    // 위치
    this.x = x;
    this.y = y;
    this.initialX = x; // 초기 X좌표 (강화 적의 진동 패턴용)

    // 크기 및 기본 통계
    if (type === 'strong') {
      this.width = 35;
      this.height = 35;
      this.maxHp = 3;
      this.hp = 3;
      this.baseSpeed = 80; // 기본 적의 80%
    } else {
      // 'basic' 타입
      this.width = 25;
      this.height = 25;
      this.maxHp = 1;
      this.hp = 1;
      this.baseSpeed = 100; // 초기 속도 100 px/s
    }

    // 생성 시간 (진동 패턴과 발사 패턴용)
    this.createdTime = 0;

    // 강화 적 발사 관련
    this.lastShootTime = 0;
    this.shootInterval = 1.5; // 1.5초 마다 발사
    this.lastPlayerY = 0;
  }

  // 게임 업데이트
  update(deltaTime, waveMultiplier) {
    // 생성 시간 증가
    this.createdTime += deltaTime;

    // 현재 속도 계산 (Wave 배수에 따라 증가)
    const currentSpeed = this.baseSpeed * waveMultiplier;

    // Y좌표 업데이트 (아래로 이동)
    this.y += currentSpeed * deltaTime;

    // 강화 적의 경우 좌우 진동 적용
    if (this.type === 'strong') {
      this.x = this.initialX + Math.sin(this.createdTime * 2) * 30;
    }

    // 강화 적의 경우 발사 시간 증가
    if (this.type === 'strong') {
      this.lastShootTime += deltaTime;
    }
  }

  // 발사 (강화 적만)
  shoot(playerY) {
    // 강화 적만 발사 가능
    if (this.type !== 'strong') {
      return [];
    }

    // 플레이어와 Y좌표가 가까울 때만 발사
    const yDiff = Math.abs(this.y - playerY);
    if (yDiff > 150) {
      // 플레이어가 너무 멀면 발사하지 않음
      return [];
    }

    // 발사 간격 확인
    if (this.lastShootTime < this.shootInterval) {
      return [];
    }

    this.lastShootTime = 0;

    // 2개의 총알을 30도 좌우 분산으로 발사
    const bullets = [];
    const baseX = this.x + this.width / 2;
    const baseY = this.y + this.height;

    // 왼쪽 총알 (약간 왼쪽 아래로)
    const leftBullet = new Bullet(baseX - 10, baseY, false);
    leftBullet.velocityY = 200; // 적 총알 속도
    bullets.push(leftBullet);

    // 오른쪽 총알 (약간 오른쪽 아래로)
    const rightBullet = new Bullet(baseX + 10, baseY, false);
    rightBullet.velocityY = 200; // 적 총알 속도
    bullets.push(rightBullet);

    return bullets;
  }

  // 데미지 처리
  takeDamage(damage = 1) {
    this.hp -= damage;
    if (this.hp < 0) {
      this.hp = 0;
    }
  }

  // 생존 여부 확인
  isAlive() {
    return this.hp > 0;
  }

  // Canvas에 그리기
  render(ctx) {
    if (this.type === 'strong') {
      // 강화 적 (주황색)
      ctx.fillStyle = '#FF9900';
      ctx.fillRect(this.x, this.y, this.width, this.height);

      // 테두리
      ctx.strokeStyle = '#FF6600';
      ctx.lineWidth = 2;
      ctx.strokeRect(this.x, this.y, this.width, this.height);

      // 체력 표시 바
      const barWidth = this.width;
      const barHeight = 4;
      ctx.fillStyle = '#FF0000';
      ctx.fillRect(this.x, this.y - 8, barWidth, barHeight);

      ctx.fillStyle = '#00FF00';
      const hpPercent = this.hp / this.maxHp;
      ctx.fillRect(this.x, this.y - 8, barWidth * hpPercent, barHeight);

      ctx.strokeStyle = '#FFFFFF';
      ctx.lineWidth = 1;
      ctx.strokeRect(this.x, this.y - 8, barWidth, barHeight);

      // 무기 표시 (2개의 작은 원)
      ctx.fillStyle = '#FFD700';
      ctx.beginPath();
      ctx.arc(this.x + 8, this.y + this.height, 2, 0, Math.PI * 2);
      ctx.fill();

      ctx.beginPath();
      ctx.arc(this.x + this.width - 8, this.y + this.height, 2, 0, Math.PI * 2);
      ctx.fill();
    } else {
      // 기본 적 (빨간색)
      ctx.fillStyle = '#FF3333';
      ctx.fillRect(this.x, this.y, this.width, this.height);

      // 테두리
      ctx.strokeStyle = '#CC0000';
      ctx.lineWidth = 1;
      ctx.strokeRect(this.x, this.y, this.width, this.height);

      // 조종석 표시 (작은 원)
      ctx.fillStyle = '#FFFF00';
      ctx.beginPath();
      ctx.arc(
        this.x + this.width / 2,
        this.y + this.height / 2,
        2,
        0,
        Math.PI * 2
      );
      ctx.fill();
    }
  }
}
