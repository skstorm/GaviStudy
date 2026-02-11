/**
 * GameLoop 클래스
 * requestAnimationFrame 기반 게임 루프를 관리합니다.
 * - 60FPS 목표
 * - deltaTime 정확한 계산 (ms → s)
 */
class GameLoop {
  /**
   * @param {HTMLCanvasElement} canvas - 캔버스 요소
   */
  constructor(canvas) {
    this.canvas = canvas;
    this.ctx = canvas.getContext('2d');

    // 게임 객체 생성
    this.game = new Game(canvas.width, canvas.height);
    this.input = new Input();

    // 시간 관리
    this.lastTime = 0;
    this.animationId = null;

    // 게임 초기화
    this.game.init();
  }

  /**
   * 게임 루프 시작
   */
  start() {
    // 플레이어 이미지 로드 (투명도가 있는 PNG)
    const playerImg = new Image();
    playerImg.onload = () => {
      playerImage = playerImg;
    };
    playerImg.src = 'resources/player.png';

    // 적 이미지 로드 (투명도가 있는 PNG)
    const enemyImg = new Image();
    enemyImg.onload = () => {
      enemyImage = enemyImg;
    };
    enemyImg.src = 'resources/enemy.png';

    this.lastTime = performance.now();
    this.loop();
  }

  /**
   * 메인 게임 루프
   * @param {number} currentTime - 현재 시간 (ms)
   */
  loop(currentTime = 0) {
    // deltaTime 계산 (ms → s)
    const deltaTime = (currentTime - this.lastTime) / 1000;
    this.lastTime = currentTime;

    // 게임 업데이트
    this.update(deltaTime);

    // 게임 렌더링
    this.render();

    // 다음 프레임 요청
    this.animationId = requestAnimationFrame((time) => this.loop(time));
  }

  /**
   * 게임 업데이트
   * @param {number} deltaTime - 프레임 간격 (초 단위)
   */
  update(deltaTime) {
    // deltaTime이 너무 크면 제한 (최대 0.1초 = 10FPS 최소)
    if (deltaTime > 0.1) {
      deltaTime = 0.1;
    }

    // 게임 상태에 따른 업데이트
    if (this.game.gameState === 'TITLE') {
      // 타이틀 화면: SpaceBar 또는 클릭으로 게임 시작
      if (this.input.isShooting()) {
        this.game.init();
      }
    } else if (this.game.gameState === 'PLAYING') {
      // 게임 플레이 중
      this.game.update(deltaTime, this.input);
    } else if (this.game.gameState === 'GAMEOVER') {
      // 게임 오버: R 키 또는 클릭으로 재시작
      if (this.input.isShooting()) {
        this.game.reset();
      }
    }
  }

  /**
   * 게임 렌더링
   */
  render() {
    // deltaTime 계산 (별 애니메이션용)
    const deltaTime = 1 / 60; // 약 60FPS 기준

    // 배경 렌더링 (별 애니메이션 포함)
    drawBackground(this.ctx, this.canvas, deltaTime);

    // 게임 상태에 따른 렌더링
    if (this.game.gameState === 'TITLE') {
      this.renderTitle();
    } else if (this.game.gameState === 'PLAYING') {
      this.renderGame();
    } else if (this.game.gameState === 'GAMEOVER') {
      this.renderGameOver();
    }
  }

  /**
   * 타이틀 화면 렌더링
   */
  renderTitle() {
    const highScore = this.game.highScores.length > 0 ? this.game.highScores[0] : 0;
    drawStartScreen(this.ctx, this.canvas, highScore);
  }

  /**
   * 게임 플레이 화면 렌더링
   */
  renderGame() {
    // UI 렌더링 (상단)
    drawUI(this.ctx, this.game, this.canvas);

    // 플레이어 렌더링
    if (this.game.player) {
      drawPlayer(this.ctx, this.game.player);
    }

    // 적 렌더링
    for (const enemy of this.game.enemies) {
      drawEnemy(this.ctx, enemy);
    }

    // 플레이어 총알 렌더링
    for (const bullet of this.game.playerBullets) {
      drawBullet(this.ctx, bullet);
    }

    // 적 총알 렌더링
    for (const bullet of this.game.enemyBullets) {
      drawBullet(this.ctx, bullet);
    }

    // 파티클 렌더링 (게임에 파티클 배열이 있는 경우)
    if (this.game.particles) {
      for (const particle of this.game.particles) {
        drawParticle(this.ctx, particle);
      }
    }
  }


  /**
   * 게임 오버 화면 렌더링
   */
  renderGameOver() {
    drawGameOverScreen(this.ctx, this.canvas, this.game, this.game.highScores);
  }

  /**
   * 게임 루프 중지
   */
  stop() {
    if (this.animationId) {
      cancelAnimationFrame(this.animationId);
      this.animationId = null;
    }
  }
}

// 페이지 로드 완료 시 게임 시작
window.addEventListener('DOMContentLoaded', () => {
  const canvas = document.getElementById('gameCanvas');
  if (canvas) {
    const gameLoop = new GameLoop(canvas);
    gameLoop.start();
  } else {
    console.error('Canvas element not found!');
  }
});
