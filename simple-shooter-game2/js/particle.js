/**
 * Particle 클래스
 * 폭발 효과 등 파티클 이펙트를 관리합니다.
 * - 색상별 폭발 효과: yellow (기본 적), orange (강화 적), red (플레이어 피격)
 * - 투명도 페이드 아웃 효과
 * - 중력 적용
 */
class Particle {
  /**
   * @param {number} x - 초기 X 좌표
   * @param {number} y - 초기 Y 좌표
   * @param {number} vx - X 방향 속도 (px/s)
   * @param {number} vy - Y 방향 속도 (px/s)
   * @param {string} color - 파티클 색상
   * @param {number} size - 파티클 크기 (px)
   * @param {number} lifetime - 생명 시간 (초)
   */
  constructor(x, y, vx, vy, color, size, lifetime) {
    this.x = x;
    this.y = y;
    this.vx = vx;
    this.vy = vy;
    this.color = color;
    this.size = size;
    this.lifetime = lifetime;
    this.maxLifetime = lifetime;
    this.alpha = 0.8; // 초기 투명도
  }

  /**
   * 파티클 업데이트
   * @param {number} deltaTime - 프레임 간격 (초 단위)
   */
  update(deltaTime) {
    // 위치 업데이트
    this.x += this.vx * deltaTime;
    this.y += this.vy * deltaTime;

    // 중력 적용 (약간의 하강 효과)
    this.vy += 200 * deltaTime; // 중력 가속도: 200 px/s²

    // 생명 시간 감소
    this.lifetime -= deltaTime;

    // 투명도 감소 (페이드 아웃)
    this.alpha = Math.max(0, (this.lifetime / this.maxLifetime) * 0.8);
  }

  /**
   * 파티클 렌더링
   * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
   */
  render(ctx) {
    ctx.save();
    ctx.globalAlpha = this.alpha;
    ctx.fillStyle = this.color;

    // 원형 파티클 그리기
    ctx.beginPath();
    ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);
    ctx.fill();

    ctx.restore();
  }

  /**
   * 파티클이 살아있는지 확인
   * @returns {boolean} - 생존 여부
   */
  isAlive() {
    return this.lifetime > 0;
  }
}

/**
 * 폭발 효과 생성 함수
 * @param {number} x - 폭발 중심 X 좌표
 * @param {number} y - 폭발 중심 Y 좌표
 * @param {string} colorType - 색상 타입 ('yellow', 'orange', 'red')
 * @param {number} count - 생성할 파티클 개수 (기본값: 8)
 * @returns {Array<Particle>} - 생성된 파티클 배열
 */
function createExplosion(x, y, colorType = 'yellow', count = 8) {
  const particles = [];

  // 색상 타입에 따른 색상 설정
  let colors = [];
  if (colorType === 'yellow') {
    // 기본 적 격추: 황색 계열
    colors = [
      'rgb(255, 220, 0)',
      'rgb(255, 200, 0)',
      'rgb(255, 180, 0)',
      'rgb(255, 160, 0)'
    ];
  } else if (colorType === 'orange') {
    // 강화 적 격추: 주황색 계열
    colors = [
      'rgb(255, 140, 0)',
      'rgb(255, 120, 0)',
      'rgb(255, 100, 0)',
      'rgb(255, 80, 0)'
    ];
  } else if (colorType === 'red') {
    // 플레이어 피격: 적색 계열
    colors = [
      'rgb(255, 50, 50)',
      'rgb(255, 30, 30)',
      'rgb(255, 10, 10)',
      'rgb(200, 0, 0)'
    ];
  }

  // 파티클 생성
  for (let i = 0; i < count; i++) {
    // 랜덤한 방향 (360도)
    const angle = (Math.PI * 2 * i) / count + (Math.random() - 0.5) * 0.5;

    // 랜덤한 속도 (150 ~ 300 px/s)
    const speed = 150 + Math.random() * 150;
    const vx = Math.cos(angle) * speed;
    const vy = Math.sin(angle) * speed;

    // 랜덤한 색상 선택
    const color = colors[Math.floor(Math.random() * colors.length)];

    // 랜덤한 크기 (2 ~ 5 px)
    const size = 2 + Math.random() * 3;

    // 랜덤한 생명 시간 (0.3 ~ 0.6초)
    const lifetime = 0.3 + Math.random() * 0.3;

    particles.push(new Particle(x, y, vx, vy, color, size, lifetime));
  }

  return particles;
}

/**
 * 타격 스파크 생성 함수 (강화 적 타격 시)
 * @param {number} x - 스파크 중심 X 좌표
 * @param {number} y - 스파크 중심 Y 좌표
 * @returns {Array<Particle>} - 생성된 파티클 배열
 */
function createSpark(x, y) {
  const particles = [];
  const count = 3; // 스파크는 적은 개수

  const colors = [
    'rgb(255, 255, 255)',
    'rgb(255, 200, 100)',
    'rgb(255, 150, 50)'
  ];

  for (let i = 0; i < count; i++) {
    // 랜덤한 방향
    const angle = Math.random() * Math.PI * 2;

    // 작은 속도 (50 ~ 150 px/s)
    const speed = 50 + Math.random() * 100;
    const vx = Math.cos(angle) * speed;
    const vy = Math.sin(angle) * speed;

    // 랜덤한 색상
    const color = colors[Math.floor(Math.random() * colors.length)];

    // 작은 크기 (1 ~ 3 px)
    const size = 1 + Math.random() * 2;

    // 짧은 생명 시간 (0.2 ~ 0.4초)
    const lifetime = 0.2 + Math.random() * 0.2;

    particles.push(new Particle(x, y, vx, vy, color, size, lifetime));
  }

  return particles;
}
