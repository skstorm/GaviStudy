// 파티클 클래스 (폭발 효과)
class Particle {
  constructor(x, y, angle, speed, color) {
    // 위치
    this.x = x;
    this.y = y;

    // 속도
    this.velocityX = Math.cos(angle) * speed;
    this.velocityY = Math.sin(angle) * speed;

    // 색상
    this.color = color;

    // 크기
    this.size = 3;

    // 생존 시간
    this.lifetime = 0.5; // 0.5초
    this.age = 0;
  }

  // 업데이트
  update(deltaTime) {
    // 나이 증가
    this.age += deltaTime;

    // 위치 업데이트
    this.x += this.velocityX * deltaTime;
    this.y += this.velocityY * deltaTime;

    // 속도 감소 (공기 저항)
    this.velocityX *= 0.98;
    this.velocityY *= 0.98;

    // 중력 적용
    this.velocityY += 300 * deltaTime;
  }

  // 생존 여부
  isAlive() {
    return this.age < this.lifetime;
  }

  // Canvas에 그리기
  render(ctx) {
    // 투명도 계산 (시간이 지날수록 사라짐)
    const alpha = 1 - this.age / this.lifetime;
    ctx.globalAlpha = alpha;

    // 색상 설정
    switch (this.color) {
      case 'yellow':
        ctx.fillStyle = '#FFD700';
        break;
      case 'orange':
        ctx.fillStyle = '#FF9900';
        break;
      case 'red':
        ctx.fillStyle = '#FF3333';
        break;
      default:
        ctx.fillStyle = '#FFFFFF';
    }

    // 원형 파티클 그리기
    ctx.beginPath();
    ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);
    ctx.fill();

    // 알파값 복원
    ctx.globalAlpha = 1.0;
  }
}
