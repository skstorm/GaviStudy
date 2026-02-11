// 메인 게임 루프
class GameLoop {
  constructor() {
    // Canvas 설정
    this.canvas = document.getElementById('gameCanvas');
    if (!this.canvas) {
      console.error('Canvas element not found!');
      return;
    }

    this.canvas.width = 800;
    this.canvas.height = 600;

    // 게임 초기화
    this.game = new Game(this.canvas.width, this.canvas.height);
    this.input = new Input();
    this.renderer = new Renderer(this.canvas);

    // 시간 관련
    this.lastTime = performance.now();
    this.deltaTime = 0;
    this.fps = 0;
    this.frameCount = 0;

    // 게임 루프 시작
    this.start();
  }

  // 게임 루프 시작
  start() {
    console.log('Game started!');
    this.loop();
  }

  // 게임 루프
  loop = () => {
    // 현재 시간 계산
    const currentTime = performance.now();
    this.deltaTime = (currentTime - this.lastTime) / 1000; // ms -> s로 변환
    this.lastTime = currentTime;

    // 프레임 속도 제한 (최대 0.1초 = 10fps 최소)
    if (this.deltaTime > 0.1) {
      this.deltaTime = 0.1;
    }

    // 입력 업데이트
    this.input.update();

    // 게임 업데이트
    this.game.update(this.deltaTime, this.input);

    // 렌더링
    this.renderer.render(this.game);

    // FPS 계산 (선택사항)
    this.frameCount++;
    this.fps = 1 / this.deltaTime;

    // 다음 프레임 요청
    requestAnimationFrame(this.loop);
  };
}

// 페이지 로드 후 게임 시작
window.addEventListener('DOMContentLoaded', () => {
  new GameLoop();
});
