// 총알 클래스
class Bullet {
  constructor(x, y, isPlayerBullet) {
    // 위치
    this.x = x;
    this.y = y;

    // 총알 종류
    this.isPlayerBullet = isPlayerBullet;

    // 크기
    if (isPlayerBullet) {
      // 플레이어 총알: 5 × 12 px
      this.width = 5;
      this.height = 12;
    } else {
      // 적 총알: 8 × 8 px
      this.width = 8;
      this.height = 8;
    }

    // 속도
    if (isPlayerBullet) {
      // 플레이어 총알은 위로 이동 (음수 Y)
      this.velocityY = -400; // 400 px/s 위로
    } else {
      // 적 총알은 아래로 이동 (양수 Y)
      // 기본값은 200 px/s이지만 Enemy.shoot()에서 설정됨
      this.velocityY = 200; // 200 px/s 아래로
    }
  }

  // 게임 업데이트
  update(deltaTime) {
    // Y좌표 업데이트
    this.y += this.velocityY * deltaTime;
  }

  // 화면 이탈 확인
  isOutOfBounds(canvasWidth, canvasHeight) {
    // 위쪽 이탈 (플레이어 총알)
    if (this.isPlayerBullet && this.y + this.height < 0) {
      return true;
    }

    // 아래쪽 이탈 (적 총알)
    if (!this.isPlayerBullet && this.y > canvasHeight) {
      return true;
    }

    // 좌우 이탈
    if (this.x < -this.width || this.x > canvasWidth) {
      return true;
    }

    return false;
  }

  // Canvas에 그리기
  render(ctx) {
    if (this.isPlayerBullet) {
      // 플레이어 총알 (황색)
      ctx.fillStyle = '#FFD700';
      ctx.fillRect(this.x, this.y, this.width, this.height);

      // 테두리
      ctx.strokeStyle = '#FFAA00';
      ctx.lineWidth = 1;
      ctx.strokeRect(this.x, this.y, this.width, this.height);

      // 반짝이는 효과
      ctx.fillStyle = '#FFFFFF';
      ctx.fillRect(this.x + 1, this.y + 2, 2, 4);
    } else {
      // 적 총알 (빨간색)
      ctx.fillStyle = '#FF3333';
      ctx.fillRect(this.x, this.y, this.width, this.height);

      // 테두리
      ctx.strokeStyle = '#FF0000';
      ctx.lineWidth = 1;
      ctx.strokeRect(this.x, this.y, this.width, this.height);
    }
  }
}
