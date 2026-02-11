// 게임 메인 클래스
class Game {
  constructor(canvasWidth, canvasHeight) {
    // Canvas 크기
    this.canvasWidth = canvasWidth;
    this.canvasHeight = canvasHeight;

    // 게임 상태
    this.gameState = 'TITLE'; // 'TITLE', 'PLAYING', 'GAMEOVER'

    // 점수 및 파동
    this.score = 0;
    this.elapsedTime = 0;
    this.lastEnemySpawnTime = 0;

    // 플레이어
    this.player = null;

    // 적, 총알, 파티클
    this.enemies = [];
    this.playerBullets = [];
    this.enemyBullets = [];
    this.particles = [];

    // 하이스코어
    this.highScores = [];
    this.loadHighScores();

    // 오디오
    this.audio = new Audio();
    this.audio.setLogging(false); // 로깅 비활성화 (필요시 활성화 가능)

    // 초기화
    this.init();
  }

  // 게임 초기화
  init() {
    // 플레이어 생성 (화면 하단 중앙)
    const playerX = this.canvasWidth / 2 - 15;
    const playerY = this.canvasHeight - 60;
    this.player = new Player(playerX, playerY);

    // 배열 초기화
    this.enemies = [];
    this.playerBullets = [];
    this.enemyBullets = [];
    this.particles = [];

    // 점수 초기화
    this.score = 0;
    this.elapsedTime = 0;
    this.lastEnemySpawnTime = 0;

    // 게임 상태 설정
    this.gameState = 'PLAYING';
  }

  // 게임 업데이트
  update(deltaTime, input) {
    if (this.gameState === 'TITLE') {
      // 타이틀 화면에서는 SpaceBar나 클릭으로 게임 시작
      if (input.isShooting() || input.wasMouseClicked()) {
        this.init();
      }
    } else if (this.gameState === 'PLAYING') {
      // 게임 진행 중
      this.updatePlaying(deltaTime, input);
    } else if (this.gameState === 'GAMEOVER') {
      // 게임 오버 화면에서는 R키나 클릭으로 재시작
      if (input.isShooting() || input.wasMouseClicked()) {
        this.init();
      }
    }
  }

  // 게임 진행 중 업데이트
  updatePlaying(deltaTime, input) {
    // 경과 시간 증가
    this.elapsedTime += deltaTime;

    // 플레이어 업데이트
    this.player.update(input, deltaTime, this.canvasWidth);

    // 플레이어 발사 처리
    if (input.isShooting() || input.wasMouseClicked()) {
      const bullet = this.player.shoot();
      if (bullet) {
        this.playerBullets.push(bullet);
        this.audio.play('shoot');
      }
    }

    // 적 생성
    this.spawn();

    // 적 업데이트
    for (let i = 0; i < this.enemies.length; i++) {
      const enemy = this.enemies[i];
      const waveMultiplier = this.getWaveMultiplier();
      enemy.update(deltaTime, waveMultiplier);

      // 강화 적의 발사
      if (enemy.type === 'strong') {
        const newBullets = enemy.shoot(this.player.y);
        this.enemyBullets.push(...newBullets);
      }

      // 화면 이탈 확인
      if (enemy.y > this.canvasHeight) {
        this.enemies.splice(i, 1);
        i--;
      }
    }

    // 플레이어 총알 업데이트
    for (let i = 0; i < this.playerBullets.length; i++) {
      const bullet = this.playerBullets[i];
      bullet.update(deltaTime);

      // 화면 이탈 확인
      if (bullet.isOutOfBounds(this.canvasWidth, this.canvasHeight)) {
        this.playerBullets.splice(i, 1);
        i--;
      }
    }

    // 적 총알 업데이트
    for (let i = 0; i < this.enemyBullets.length; i++) {
      const bullet = this.enemyBullets[i];
      bullet.update(deltaTime);

      // 화면 이탈 확인
      if (bullet.isOutOfBounds(this.canvasWidth, this.canvasHeight)) {
        this.enemyBullets.splice(i, 1);
        i--;
      }
    }

    // 파티클 업데이트
    for (let i = 0; i < this.particles.length; i++) {
      const particle = this.particles[i];
      particle.update(deltaTime);

      if (!particle.isAlive()) {
        this.particles.splice(i, 1);
        i--;
      }
    }

    // 충돌 처리
    this.handleCollisions();

    // 플레이어 생존 확인
    if (!this.player.isAlive()) {
      this.gameOver();
    }
  }

  // 적 생성 로직
  spawn() {
    const waveNumber = this.getWave();
    let spawnInterval = 1.2; // 기본 스폰 간격
    let basicRatio = 1.0; // 기본 적 비율

    // Wave에 따른 스폰 간격과 적 비율 설정
    if (waveNumber <= 2) {
      spawnInterval = 1.2;
      basicRatio = 1.0;
    } else if (waveNumber <= 4) {
      spawnInterval = 1.0;
      basicRatio = 1.0;
    } else if (waveNumber <= 6) {
      spawnInterval = 0.8;
      basicRatio = 0.8;
    } else if (waveNumber <= 10) {
      spawnInterval = 0.6;
      basicRatio = 0.6;
    } else {
      spawnInterval = 0.5;
      basicRatio = 0.4;
    }

    // 스폰 시간 확인
    if (this.elapsedTime - this.lastEnemySpawnTime >= spawnInterval) {
      this.lastEnemySpawnTime = this.elapsedTime;

      // 적 종류 결정
      const randomValue = Math.random();
      let enemyType = 'basic';

      if (waveNumber >= 5 && randomValue > basicRatio) {
        enemyType = 'strong';
      }

      // 적 생성 위치 (화면 상단, 좌우 랜덤)
      const enemyX = Math.random() * (this.canvasWidth - 35);
      const enemyY = -30;

      this.addEnemy(enemyType, enemyX, enemyY);
    }
  }

  // 충돌 처리
  handleCollisions() {
    // 플레이어 총알 vs 적
    for (let i = 0; i < this.playerBullets.length; i++) {
      const bullet = this.playerBullets[i];

      for (let j = 0; j < this.enemies.length; j++) {
        const enemy = this.enemies[j];

        if (checkCollision(bullet, enemy)) {
          // 충돌 발생
          enemy.takeDamage(1);

          // 점수 추가
          if (enemy.type === 'basic') {
            this.score += 10;
            this.audio.play('explosion');

            // 파티클 생성 (황색)
            this.createExplosion(
              enemy.x + enemy.width / 2,
              enemy.y + enemy.height / 2,
              'yellow'
            );
          } else {
            // 강화 적 타격
            this.score += 5;
            this.audio.play('hit');

            // 파티클 생성 (주황색)
            this.createExplosion(
              enemy.x + enemy.width / 2,
              enemy.y + enemy.height / 2,
              'orange'
            );

            // 강화 적이 죽었는지 확인
            if (!enemy.isAlive()) {
              this.score += 20; // 추가 보너스
            }
          }

          // 총알 제거
          this.playerBullets.splice(i, 1);
          i--;

          // 적이 죽었으면 제거
          if (!enemy.isAlive()) {
            this.enemies.splice(j, 1);
          }

          break; // 다음 총알로
        }
      }
    }

    // 적 총알 vs 플레이어
    for (let i = 0; i < this.enemyBullets.length; i++) {
      const bullet = this.enemyBullets[i];

      if (checkCollision(bullet, this.player)) {
        // 충돌 발생
        this.player.takeDamage(1);
        this.audio.play('damage');

        // 파티클 생성 (빨간색)
        this.createExplosion(
          this.player.x + this.player.width / 2,
          this.player.y + this.player.height / 2,
          'red'
        );

        // 총알 제거
        this.enemyBullets.splice(i, 1);
        i--;
      }
    }

    // 적 vs 플레이어 (접촉)
    for (let i = 0; i < this.enemies.length; i++) {
      const enemy = this.enemies[i];

      if (checkCollision(this.player, enemy)) {
        // 충돌 발생
        this.player.takeDamage(1);
        this.audio.play('crash');

        // 파티클 생성 (큰 폭발)
        this.createExplosion(
          this.player.x + this.player.width / 2,
          this.player.y + this.player.height / 2,
          'red',
          8 // 파티클 개수
        );

        // 적 제거
        this.enemies.splice(i, 1);
        i--;
      }
    }
  }

  // 폭발 파티클 생성
  createExplosion(x, y, color, count = 5) {
    for (let i = 0; i < count; i++) {
      const angle = (Math.PI * 2 * i) / count + (Math.random() - 0.5);
      const speed = 100 + Math.random() * 100;
      const particle = new Particle(x, y, angle, speed, color);
      this.particles.push(particle);
    }
  }

  // 적 추가
  addEnemy(type, x, y) {
    const enemy = new Enemy(type, x, y);
    this.enemies.push(enemy);
  }

  // 게임 오버 처리
  gameOver() {
    this.gameState = 'GAMEOVER';
    this.audio.play('explosion');

    // 하이스코어 저장
    this.saveHighScore(this.score);
  }

  // 게임 리셋
  reset() {
    this.init();
  }

  // 현재 Wave 계산
  getWave() {
    const wave = Math.floor(this.score / 1000) + 1;
    return Math.min(wave, 15); // 최대 Wave 15
  }

  // Wave 배수 계산
  getWaveMultiplier() {
    const waveNumber = this.getWave();

    if (waveNumber <= 2) {
      return 1.0;
    } else if (waveNumber <= 4) {
      return 1.2;
    } else if (waveNumber <= 6) {
      return 1.4;
    } else if (waveNumber <= 10) {
      return 1.6;
    } else {
      return 1.8;
    }
  }

  // 하이스코어 로드
  loadHighScores() {
    const stored = localStorage.getItem('skyDefenderHighScores');
    if (stored) {
      this.highScores = JSON.parse(stored);
    } else {
      this.highScores = [];
    }
  }

  // 하이스코어 저장
  saveHighScore(score) {
    this.highScores.push(score);
    this.highScores.sort((a, b) => b - a);
    this.highScores = this.highScores.slice(0, 10); // 상위 10개만 유지
    localStorage.setItem('skyDefenderHighScores', JSON.stringify(this.highScores));
  }

  // 최고 점수 반환
  getHighScore() {
    return this.highScores.length > 0 ? this.highScores[0] : 0;
  }

  // 현재 점수 순위 반환
  getCurrentRank() {
    // 현재 점수보다 높은 점수의 개수 + 1
    let rank = 1;
    for (let score of this.highScores) {
      if (score > this.score) {
        rank++;
      }
    }
    return rank;
  }
}
