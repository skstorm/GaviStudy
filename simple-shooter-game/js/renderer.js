// 렌더러 클래스 (Canvas 렌더링)
class Renderer {
  constructor(canvas) {
    this.canvas = canvas;
    this.ctx = canvas.getContext('2d');
  }

  // 배경 그리기
  drawBackground(ctx) {
    // 검은색 배경
    ctx.fillStyle = '#000000';
    ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

    // 별 배경 (선택사항)
    this.drawStars(ctx);
  }

  // 별 그리기
  drawStars(ctx) {
    ctx.fillStyle = '#FFFFFF';
    ctx.globalAlpha = 0.5;

    // 고정된 위치의 별들
    const stars = [
      { x: 100, y: 50, size: 1 },
      { x: 200, y: 100, size: 0.5 },
      { x: 300, y: 80, size: 1 },
      { x: 400, y: 120, size: 0.5 },
      { x: 500, y: 60, size: 1 },
      { x: 600, y: 90, size: 0.5 },
      { x: 700, y: 110, size: 1 },
      { x: 150, y: 200, size: 0.5 },
      { x: 350, y: 250, size: 1 },
      { x: 550, y: 180, size: 0.5 },
      { x: 750, y: 220, size: 1 },
    ];

    for (let star of stars) {
      ctx.beginPath();
      ctx.arc(star.x, star.y, star.size, 0, Math.PI * 2);
      ctx.fill();
    }

    ctx.globalAlpha = 1.0;
  }

  // 플레이어 그리기
  drawPlayer(ctx, player) {
    player.render(ctx);
  }

  // 적 그리기
  drawEnemy(ctx, enemy) {
    enemy.render(ctx);
  }

  // 총알 그리기
  drawBullet(ctx, bullet) {
    bullet.render(ctx);
  }

  // 파티클 그리기
  drawParticle(ctx, particle) {
    particle.render(ctx);
  }

  // UI 그리기 (점수, 체력, Wave 등)
  drawUI(ctx, game) {
    ctx.fillStyle = '#FFFFFF';
    ctx.font = '16px Arial';

    // 좌측 상단: 점수
    ctx.textAlign = 'left';
    ctx.fillText('Score: ' + game.score, 10, 20);

    // 중앙 상단: 체력
    ctx.textAlign = 'center';
    const heartX = this.canvas.width / 2;
    const heartText = '❤️'.repeat(Math.max(0, game.player.hp)) + ' '.repeat(Math.max(0, game.player.maxHp - game.player.hp));
    ctx.fillText('Life: ' + heartText, heartX, 20);

    // 우측 상단: Wave
    ctx.textAlign = 'right';
    ctx.fillText('Wave: ' + game.getWave(), this.canvas.width - 10, 20);

    // 경과 시간 표시 (선택사항)
    ctx.textAlign = 'right';
    const minutes = Math.floor(game.elapsedTime / 60);
    const seconds = Math.floor(game.elapsedTime % 60);
    const timeStr = minutes + ':' + (seconds < 10 ? '0' : '') + seconds;
    ctx.fillText('Time: ' + timeStr, this.canvas.width - 10, 40);

    // 무적 상태 표시
    if (game.player.isInvulnerable()) {
      ctx.fillStyle = 'rgba(255, 255, 0, 0.5)';
      ctx.font = 'bold 14px Arial';
      ctx.textAlign = 'center';
      ctx.fillText('INVULNERABLE', this.canvas.width / 2, this.canvas.height / 2);
    }
  }

  // 시작 화면 그리기
  drawStartScreen(ctx, game) {
    // 배경
    this.drawBackground(ctx);

    // 제목
    ctx.fillStyle = '#FFD700';
    ctx.font = 'bold 60px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('Sky Defender', this.canvas.width / 2, 100);

    // 설명 텍스트
    ctx.fillStyle = '#FFFFFF';
    ctx.font = '16px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('Defend Earth from enemy fighters!', this.canvas.width / 2, 160);

    // 조작 설명
    ctx.font = '14px Arial';
    ctx.fillStyle = '#AAAAAA';
    ctx.fillText('A / Left Arrow - Move Left', this.canvas.width / 2, 220);
    ctx.fillText('D / Right Arrow - Move Right', this.canvas.width / 2, 245);
    ctx.fillText('Space Bar / Click - Shoot', this.canvas.width / 2, 270);

    // 하이스코어
    ctx.fillStyle = '#FFD700';
    ctx.font = 'bold 18px Arial';
    const highScore = game.getHighScore();
    ctx.fillText('High Score: ' + highScore, this.canvas.width / 2, 330);

    // 시작 버튼
    const buttonWidth = 150;
    const buttonHeight = 50;
    const buttonX = this.canvas.width / 2 - buttonWidth / 2;
    const buttonY = 400;

    ctx.fillStyle = '#0099FF';
    ctx.fillRect(buttonX, buttonY, buttonWidth, buttonHeight);

    ctx.strokeStyle = '#0066CC';
    ctx.lineWidth = 2;
    ctx.strokeRect(buttonX, buttonY, buttonWidth, buttonHeight);

    ctx.fillStyle = '#FFFFFF';
    ctx.font = 'bold 18px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('START', this.canvas.width / 2, buttonY + 32);

    // 시작 안내
    ctx.fillStyle = '#AAAAAA';
    ctx.font = '12px Arial';
    ctx.fillText('Press SPACE or Click to Start', this.canvas.width / 2, 480);
  }

  // 게임 오버 화면 그리기
  drawGameOverScreen(ctx, game) {
    // 배경 (반투명)
    ctx.fillStyle = 'rgba(0, 0, 0, 0.7)';
    ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

    // "GAME OVER" 텍스트
    ctx.fillStyle = '#FF3333';
    ctx.font = 'bold 50px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('GAME OVER', this.canvas.width / 2, 80);

    // 최종 점수
    ctx.fillStyle = '#FFFFFF';
    ctx.font = 'bold 24px Arial';
    ctx.fillText('Score: ' + game.score, this.canvas.width / 2, 150);

    // 순위
    ctx.font = '18px Arial';
    ctx.fillStyle = '#FFD700';
    const rank = game.getCurrentRank();
    ctx.fillText('Rank: ' + rank, this.canvas.width / 2, 190);

    // 하이스코어 상위 5
    ctx.fillStyle = '#FFFFFF';
    ctx.font = 'bold 16px Arial';
    ctx.fillText('Top Scores', this.canvas.width / 2, 250);

    ctx.font = '14px Arial';
    ctx.textAlign = 'center';
    for (let i = 0; i < Math.min(5, game.highScores.length); i++) {
      const yPos = 280 + i * 25;
      ctx.fillText((i + 1) + '. ' + game.highScores[i], this.canvas.width / 2, yPos);
    }

    if (game.highScores.length === 0) {
      ctx.fillStyle = '#AAAAAA';
      ctx.fillText('No scores yet', this.canvas.width / 2, 280);
    }

    // 재시작 버튼
    const buttonWidth = 120;
    const buttonHeight = 40;
    const buttonX = this.canvas.width / 2 - buttonWidth / 2 - 80;
    const buttonY = 450;

    ctx.fillStyle = '#0099FF';
    ctx.fillRect(buttonX, buttonY, buttonWidth, buttonHeight);
    ctx.strokeStyle = '#0066CC';
    ctx.lineWidth = 2;
    ctx.strokeRect(buttonX, buttonY, buttonWidth, buttonHeight);

    ctx.fillStyle = '#FFFFFF';
    ctx.font = 'bold 16px Arial';
    ctx.textAlign = 'center';
    ctx.fillText('RESTART', this.canvas.width / 2 - 80, buttonY + 27);

    // 메뉴 버튼
    const menuX = this.canvas.width / 2 + 80 - buttonWidth / 2;
    ctx.fillStyle = '#666666';
    ctx.fillRect(menuX, buttonY, buttonWidth, buttonHeight);
    ctx.strokeStyle = '#333333';
    ctx.lineWidth = 2;
    ctx.strokeRect(menuX, buttonY, buttonWidth, buttonHeight);

    ctx.fillStyle = '#FFFFFF';
    ctx.fillText('MENU', this.canvas.width / 2 + 80, buttonY + 27);

    // 버튼 안내
    ctx.fillStyle = '#AAAAAA';
    ctx.font = '12px Arial';
    ctx.fillText('Press SPACE or Click to Continue', this.canvas.width / 2, 540);
  }

  // 전체 프레임 렌더링
  render(game) {
    if (game.gameState === 'TITLE') {
      this.drawStartScreen(this.ctx, game);
    } else if (game.gameState === 'PLAYING') {
      // 배경
      this.drawBackground(this.ctx);

      // 적 그리기
      for (let enemy of game.enemies) {
        this.drawEnemy(this.ctx, enemy);
      }

      // 플레이어 총알 그리기
      for (let bullet of game.playerBullets) {
        this.drawBullet(this.ctx, bullet);
      }

      // 적 총알 그리기
      for (let bullet of game.enemyBullets) {
        this.drawBullet(this.ctx, bullet);
      }

      // 파티클 그리기
      for (let particle of game.particles) {
        this.drawParticle(this.ctx, particle);
      }

      // 플레이어 그리기
      this.drawPlayer(this.ctx, game.player);

      // UI 그리기
      this.drawUI(this.ctx, game);
    } else if (game.gameState === 'GAMEOVER') {
      // 배경
      this.drawBackground(this.ctx);

      // 게임 오버 화면
      this.drawGameOverScreen(this.ctx, game);
    }
  }
}
