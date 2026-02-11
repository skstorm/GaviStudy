/**
 * Game 클래스
 * 메인 게임 로직을 관리합니다.
 * - 게임 상태: TITLE, PLAYING, GAMEOVER
 * - 적 생성, 충돌 처리, 점수 계산, Wave 시스템
 */
class Game {
  /**
   * @param {number} canvasWidth - 캔버스 너비
   * @param {number} canvasHeight - 캔버스 높이
   */
  constructor(canvasWidth, canvasHeight) {
    this.canvasWidth = canvasWidth;
    this.canvasHeight = canvasHeight;

    // 게임 상태: 'TITLE', 'PLAYING', 'GAMEOVER'
    this.gameState = 'TITLE';

    // 게임 데이터
    this.score = 0;
    this.wave = 1;
    this.elapsedTime = 0;

    // 게임 객체들
    this.player = null;
    this.enemies = [];
    this.playerBullets = [];
    this.enemyBullets = [];
    this.particles = []; // 파티클 배열

    // 적 생성 관련
    this.enemySpawnTimer = 0;
    this.enemySpawnInterval = 1.2; // 초기값: 1.2초

    // 하이스코어
    this.highScores = this.loadHighScores();

    // 오디오 시스템
    this.audio = new Audio();
  }

  /**
   * 게임 초기화
   */
  init() {
    // 플레이어 생성 (화면 하단 중앙)
    const playerX = this.canvasWidth / 2 - 15; // 30px 너비 / 2
    const playerY = this.canvasHeight - 60;
    this.player = new Player(playerX, playerY);

    // 게임 상태 초기화
    this.score = 0;
    this.wave = 1;
    this.elapsedTime = 0;

    // 모든 객체 배열 초기화
    this.enemies = [];
    this.playerBullets = [];
    this.enemyBullets = [];
    this.particles = [];

    // 적 생성 타이머 초기화
    this.enemySpawnTimer = 0;
    this.enemySpawnInterval = 1.2; // Wave 1-2: 1.2초

    // 게임 상태를 PLAYING으로 변경
    this.gameState = 'PLAYING';
  }

  /**
   * 게임 업데이트
   * @param {number} deltaTime - 프레임 간격 (초 단위)
   * @param {Input} input - 입력 객체
   */
  update(deltaTime, input) {
    if (this.gameState !== 'PLAYING') {
      return;
    }

    // 경과 시간 증가
    this.elapsedTime += deltaTime;

    // Wave 계산 및 업데이트
    this.wave = this.getWave();
    this.enemySpawnInterval = this.getSpawnInterval();

    // 플레이어 업데이트
    this.player.update(input, deltaTime, this.canvasWidth, this.canvasHeight);

    // 플레이어 발사 처리
    if (input.isShooting() && this.player.shoot(this.elapsedTime)) {
      // 플레이어 총알 생성 (기체 중앙 상단에서 발사)
      const bulletX = this.player.x + this.player.width / 2 - 2.5; // 5px 너비 / 2
      const bulletY = this.player.y - 10;
      const bullet = new Bullet(bulletX, bulletY, 0, -400, 'player'); // 400px/s 위로
      this.playerBullets.push(bullet);
      this.audio.play('shoot');
    }

    // 적 생성 처리
    this.enemySpawnTimer += deltaTime;
    if (this.enemySpawnTimer >= this.enemySpawnInterval) {
      this.spawn();
      this.enemySpawnTimer = 0;
    }

    // 적 업데이트 및 발사
    const waveMultiplier = this.getWaveMultiplier();
    for (let i = this.enemies.length - 1; i >= 0; i--) {
      const enemy = this.enemies[i];
      enemy.update(deltaTime, waveMultiplier);

      // 강화 적 발사 처리
      if (enemy.shoot(this.elapsedTime)) {
        // 2개의 총알을 30도 좌우로 분산
        const bulletX = enemy.x + enemy.width / 2 - 4; // 8px 너비 / 2
        const bulletY = enemy.y + enemy.height;

        // 왼쪽 총알 (-30도)
        const angle1 = -Math.PI / 6; // -30도
        const vx1 = Math.sin(angle1) * 200; // 200px/s
        const vy1 = Math.cos(angle1) * 200;
        this.enemyBullets.push(new Bullet(bulletX, bulletY, vx1, vy1, 'enemy'));

        // 오른쪽 총알 (+30도)
        const angle2 = Math.PI / 6; // +30도
        const vx2 = Math.sin(angle2) * 200;
        const vy2 = Math.cos(angle2) * 200;
        this.enemyBullets.push(new Bullet(bulletX, bulletY, vx2, vy2, 'enemy'));

        this.audio.play('shoot');
      }

      // 화면 이탈 확인 (하단 벗어남)
      if (enemy.y > this.canvasHeight) {
        this.enemies.splice(i, 1);
      }
    }

    // 플레이어 총알 업데이트
    for (let i = this.playerBullets.length - 1; i >= 0; i--) {
      const bullet = this.playerBullets[i];
      bullet.update(deltaTime);

      // 화면 이탈 확인
      if (bullet.isOutOfBounds(this.canvasWidth, this.canvasHeight)) {
        this.playerBullets.splice(i, 1);
      }
    }

    // 적 총알 업데이트
    for (let i = this.enemyBullets.length - 1; i >= 0; i--) {
      const bullet = this.enemyBullets[i];
      bullet.update(deltaTime);

      // 화면 이탈 확인
      if (bullet.isOutOfBounds(this.canvasWidth, this.canvasHeight)) {
        this.enemyBullets.splice(i, 1);
      }
    }

    // 충돌 처리
    this.handleCollisions();

    // 파티클 업데이트
    for (let i = this.particles.length - 1; i >= 0; i--) {
      const particle = this.particles[i];
      particle.update(deltaTime);

      // 파티클이 죽으면 제거
      if (!particle.isAlive()) {
        this.particles.splice(i, 1);
      }
    }

    // 게임 오버 확인
    if (!this.player.isAlive()) {
      this.gameOver();
    }
  }

  /**
   * 적 생성
   */
  spawn() {
    // 랜덤 X 좌표 (화면 내에서)
    const maxX = this.canvasWidth - 40; // 적 크기 고려
    const x = Math.random() * maxX;
    const y = -40; // 화면 위쪽에서 생성
    const baseSpeed = 100; // 기본 속도: 100px/s

    // Wave에 따른 적 타입 결정
    const enemyType = this.determineEnemyType();

    const enemy = new Enemy(enemyType, x, y, baseSpeed);
    this.enemies.push(enemy);
  }

  /**
   * Wave에 따른 적 타입 결정
   * @returns {string} - 'basic' 또는 'strong'
   */
  determineEnemyType() {
    // Wave 1-4: 기본 적만
    if (this.wave <= 4) {
      return 'basic';
    }

    // Wave 5-6: 기본 적 80%, 강화 적 20%
    if (this.wave <= 6) {
      return Math.random() < 0.8 ? 'basic' : 'strong';
    }

    // Wave 7-10: 기본 적 60%, 강화 적 40%
    if (this.wave <= 10) {
      return Math.random() < 0.6 ? 'basic' : 'strong';
    }

    // Wave 11+: 기본 적 40%, 강화 적 60%
    return Math.random() < 0.4 ? 'basic' : 'strong';
  }

  /**
   * 충돌 처리
   */
  handleCollisions() {
    // 1. 플레이어 총알 vs 적
    for (let i = this.playerBullets.length - 1; i >= 0; i--) {
      const bullet = this.playerBullets[i];
      let bulletHit = false;

      for (let j = this.enemies.length - 1; j >= 0; j--) {
        const enemy = this.enemies[j];

        if (checkCollision(bullet, enemy)) {
          // 충돌 발생
          bulletHit = true;

          // 적에게 데미지
          const isDestroyed = enemy.takeDamage(1);

          if (isDestroyed) {
            // 적 파괴됨
            this.enemies.splice(j, 1);

            // 점수 추가 (사양서 기준)
            if (enemy.type === 'basic') {
              this.score += 10; // 기본 적: +10점
              this.audio.play('explosion');

              // 황색 폭발 파티클 생성
              const explosionParticles = createExplosion(
                enemy.x + enemy.width / 2,
                enemy.y + enemy.height / 2,
                'yellow',
                8
              );
              this.particles.push(...explosionParticles);
            } else if (enemy.type === 'strong') {
              this.score += 20; // 강화 적 완전 격추: +20점
              this.audio.play('explosion');

              // 주황색 폭발 파티클 생성 (더 많이)
              const explosionParticles = createExplosion(
                enemy.x + enemy.width / 2,
                enemy.y + enemy.height / 2,
                'orange',
                12
              );
              this.particles.push(...explosionParticles);
            }
          } else {
            // 적이 아직 살아있음 (강화 적)
            this.score += 5; // 강화 적 타격: +5점
            this.audio.play('hit');

            // 스파크 파티클 생성
            const sparkParticles = createSpark(
              bullet.x + bullet.width / 2,
              bullet.y + bullet.height / 2
            );
            this.particles.push(...sparkParticles);
          }

          break;
        }
      }

      // 총알이 적에 맞았으면 제거
      if (bulletHit) {
        this.playerBullets.splice(i, 1);
      }
    }

    // 2. 적 총알 vs 플레이어
    for (let i = this.enemyBullets.length - 1; i >= 0; i--) {
      const bullet = this.enemyBullets[i];

      if (checkCollision(bullet, this.player)) {
        // 총알 제거
        this.enemyBullets.splice(i, 1);

        // 플레이어 데미지
        if (this.player.takeDamage()) {
          this.audio.play('damage');

          // 적색 파티클 생성 (플레이어 피격)
          const damageParticles = createExplosion(
            bullet.x + bullet.width / 2,
            bullet.y + bullet.height / 2,
            'red',
            6
          );
          this.particles.push(...damageParticles);
        }
      }
    }

    // 3. 적 vs 플레이어 (직접 충돌)
    for (let i = this.enemies.length - 1; i >= 0; i--) {
      const enemy = this.enemies[i];

      if (checkCollision(enemy, this.player)) {
        // 적 제거
        this.enemies.splice(i, 1);

        // 플레이어 데미지
        if (this.player.takeDamage()) {
          this.audio.play('crash');

          // 큰 적색 폭발 파티클 생성 (충돌)
          const crashParticles = createExplosion(
            enemy.x + enemy.width / 2,
            enemy.y + enemy.height / 2,
            'red',
            10
          );
          this.particles.push(...crashParticles);
        }
      }
    }
  }

  /**
   * Wave 계산
   * 사양서: 누적 점수 / 1000 = Wave 번호 (최대 Wave 15)
   * @returns {number} - 현재 Wave
   */
  getWave() {
    const wave = Math.floor(this.score / 1000) + 1;
    return Math.min(wave, 15); // 최대 Wave 15
  }

  /**
   * Wave별 난이도 배수 계산
   * @returns {number} - 난이도 배수 (1.0 ~ 1.8)
   */
  getWaveMultiplier() {
    if (this.wave <= 2) return 1.0;
    if (this.wave <= 4) return 1.2;
    if (this.wave <= 6) return 1.4;
    if (this.wave <= 10) return 1.6;
    return 1.8;
  }

  /**
   * Wave별 적 생성 간격 계산
   * @returns {number} - 적 생성 간격 (초)
   */
  getSpawnInterval() {
    if (this.wave <= 2) return 1.2;
    if (this.wave <= 4) return 1.0;
    if (this.wave <= 6) return 0.8;
    if (this.wave <= 10) return 0.6;
    return 0.5;
  }

  /**
   * 게임 오버 처리
   */
  gameOver() {
    this.gameState = 'GAMEOVER';

    // 하이스코어 저장
    this.saveHighScores(this.score);
  }

  /**
   * 게임 리셋
   */
  reset() {
    this.init();
  }

  /**
   * 하이스코어 로드 (localStorage)
   * @returns {Array<number>} - 하이스코어 배열
   */
  loadHighScores() {
    const saved = localStorage.getItem('skyDefenderHighScores');
    if (saved) {
      try {
        return JSON.parse(saved);
      } catch (e) {
        return [];
      }
    }
    return [];
  }

  /**
   * 하이스코어 저장 (localStorage)
   * @param {number} newScore - 새로운 점수
   */
  saveHighScores(newScore) {
    this.highScores.push(newScore);
    this.highScores.sort((a, b) => b - a); // 내림차순 정렬
    this.highScores = this.highScores.slice(0, 5); // 상위 5개만 유지
    localStorage.setItem('skyDefenderHighScores', JSON.stringify(this.highScores));
  }
}
