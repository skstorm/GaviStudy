/**
 * renderer.js
 * Canvas 렌더링 시스템
 * - 배경, UI, 객체, 화면 렌더링
 * - 60FPS 최적화
 * - 시각적으로 매력적인 그래픽
 */

// ============================================
// 배경 렌더링
// ============================================

// 별 데이터 (한 번 생성하여 재사용)
let stars = null;

// 플레이어 이미지
let playerImage = null;

// 적 이미지
let enemyImage = null;

/**
 * 이미지에서 흰색 배경을 투명하게 처리 (콜백 방식)
 * @param {HTMLImageElement} img - 처리할 이미지
 * @param {Function} callback - 처리 완료 후 호출될 콜백함수
 */
function removeWhiteBackground(img, callback) {
  try {
    const canvas = document.createElement('canvas');
    canvas.width = img.width;
    canvas.height = img.height;
    const ctx = canvas.getContext('2d');

    // 이미지를 캔버스에 그리기
    ctx.drawImage(img, 0, 0);

    // 픽셀 데이터 가져오기
    const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    const data = imageData.data;

    // 각 픽셀을 순회하며 흰색(또는 거의 흰색)을 투명하게 처리
    for (let i = 0; i < data.length; i += 4) {
      const r = data[i];     // Red
      const g = data[i + 1]; // Green
      const b = data[i + 2]; // Blue

      // 흰색 또는 거의 흰색인 픽셀 (RGB 각각 200 이상)을 투명하게 처리
      if (r > 200 && g > 200 && b > 200) {
        data[i + 3] = 0; // 알파값을 0(투명)으로 설정
      }
    }

    // 처리된 픽셀 데이터를 캔버스에 적용
    ctx.putImageData(imageData, 0, 0);

    // Canvas를 Image로 변환하여 반환
    const processedImg = new Image();
    processedImg.onload = () => {
      console.log('플레이어 이미지 처리 완료:', processedImg.width, 'x', processedImg.height);
      callback(processedImg);
    };
    processedImg.onerror = () => {
      console.error('플레이어 이미지 처리 실패');
      callback(null);
    };
    processedImg.src = canvas.toDataURL();
  } catch (error) {
    console.error('이미지 처리 중 오류:', error);
    callback(null);
  }
}

/**
 * 별 초기화
 * @param {number} canvasWidth - 캔버스 너비
 * @param {number} canvasHeight - 캔버스 높이
 */
function initStars(canvasWidth, canvasHeight) {
  stars = [];
  const starCount = 100; // 별 개수

  for (let i = 0; i < starCount; i++) {
    stars.push({
      x: Math.random() * canvasWidth,
      y: Math.random() * canvasHeight,
      size: Math.random() * 2 + 0.5, // 0.5 ~ 2.5 px
      speed: Math.random() * 20 + 10, // 10 ~ 30 px/s
      brightness: Math.random() * 0.5 + 0.5 // 0.5 ~ 1.0
    });
  }
}

/**
 * 배경 렌더링 (검은색 + 별 애니메이션)
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {HTMLCanvasElement} canvas - 캔버스 요소
 * @param {number} deltaTime - 프레임 간격 (초 단위, 선택사항)
 */
function drawBackground(ctx, canvas, deltaTime = 0) {
  // 어두운 우주 배경
  ctx.fillStyle = 'rgb(10, 10, 30)';
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  // 별이 초기화되지 않았으면 초기화
  if (!stars) {
    initStars(canvas.width, canvas.height);
  }

  // 별 그리기 및 애니메이션
  for (const star of stars) {
    // 별 이동 (아래로)
    star.y += star.speed * deltaTime;

    // 화면 하단을 벗어나면 상단으로 재배치
    if (star.y > canvas.height) {
      star.y = 0;
      star.x = Math.random() * canvas.width;
    }

    // 별 그리기 (흰색, 투명도 적용)
    ctx.save();
    ctx.globalAlpha = star.brightness;
    ctx.fillStyle = 'white';
    ctx.beginPath();
    ctx.arc(star.x, star.y, star.size, 0, Math.PI * 2);
    ctx.fill();
    ctx.restore();
  }
}

// ============================================
// UI 렌더링
// ============================================

/**
 * 게임 플레이 중 UI 렌더링
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Game} game - 게임 객체
 * @param {HTMLCanvasElement} canvas - 캔버스 요소
 */
function drawUI(ctx, game, canvas) {
  // UI 배경 (반투명 검은색 바)
  ctx.fillStyle = 'rgba(0, 0, 0, 0.3)';
  ctx.fillRect(0, 0, canvas.width, 80);

  // 좌상단: 점수 표시
  ctx.fillStyle = 'white';
  ctx.font = 'bold 20px Arial';
  ctx.textAlign = 'left';
  ctx.fillText(`점수: ${game.score}`, 15, 30);

  // 좌측: 체력 표시 (하트)
  ctx.fillStyle = 'white';
  ctx.fillText('체력: ', 15, 60);

  // 하트 이모지 렌더링
  for (let i = 0; i < game.player.hp; i++) {
    ctx.fillStyle = 'red';
    ctx.font = '20px Arial';
    ctx.fillText('❤️', 75 + i * 30, 60);
  }

  // 우상단: Wave 표시
  ctx.fillStyle = 'rgb(255, 200, 0)';
  ctx.font = 'bold 24px Arial';
  ctx.textAlign = 'right';
  ctx.fillText(`Wave: ${game.wave}`, canvas.width - 15, 30);

  // 우측: 경과 시간 표시
  const minutes = Math.floor(game.elapsedTime / 60);
  const seconds = Math.floor(game.elapsedTime % 60);
  const timeText = `${minutes}:${seconds.toString().padStart(2, '0')}`;
  ctx.fillStyle = 'white';
  ctx.font = '18px Arial';
  ctx.fillText(`시간: ${timeText}`, canvas.width - 15, 60);
}

// ============================================
// 객체 렌더링
// ============================================

/**
 * 플레이어 렌더링 (이미지 또는 파란색 삼각형)
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Player} player - 플레이어 객체
 */
function drawPlayer(ctx, player) {
  // 무적 상태일 때 깜빡임 효과
  if (player.isInvulnerable()) {
    const blinkInterval = 0.1;
    const blinkPhase = Math.floor(player.invulnerableTime / blinkInterval) % 2;
    if (blinkPhase === 0) {
      return; // 깜빡임: 렌더링 생략
    }
  }

  ctx.save();

  // 이미지가 로드되었으면 이미지 그리기
  if (playerImage) {
    try {
      ctx.drawImage(playerImage, player.x, player.y, player.width, player.height);
    } catch (e) {
      console.error('플레이어 이미지 렌더링 오류:', e);
      drawPlayerFallback(ctx, player);
    }
  } else {
    // 이미지 미로드 시 기본 삼각형 렌더링
    drawPlayerFallback(ctx, player);
  }

  ctx.restore();
}

/**
 * 플레이어 기본 렌더링 (파란색 삼각형)
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Player} player - 플레이어 객체
 */
function drawPlayerFallback(ctx, player) {
  const centerX = player.x + player.width / 2;
  const centerY = player.y + player.height / 2;

  // 삼각형 그리기
  ctx.fillStyle = 'rgb(100, 150, 255)'; // 파란색
  ctx.beginPath();
  ctx.moveTo(centerX, player.y); // 상단 꼭지점
  ctx.lineTo(player.x, player.y + player.height); // 좌하단
  ctx.lineTo(player.x + player.width, player.y + player.height); // 우하단
  ctx.closePath();
  ctx.fill();

  // 테두리 (진한 파란색)
  ctx.strokeStyle = 'rgb(50, 100, 200)';
  ctx.lineWidth = 2;
  ctx.stroke();

  // 중앙 엔진 표시 (흰색 원)
  ctx.fillStyle = 'white';
  ctx.beginPath();
  ctx.arc(centerX, centerY + 5, 4, 0, Math.PI * 2);
  ctx.fill();
}

/**
 * 적 렌더링 (이미지 또는 삼각형)
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Enemy} enemy - 적 객체
 */
function drawEnemy(ctx, enemy) {
  ctx.save();

  // 이미지가 로드되었으면 이미지 그리기
  if (enemyImage) {
    try {
      ctx.drawImage(enemyImage, enemy.x, enemy.y, enemy.width, enemy.height);
    } catch (e) {
      console.error('적 이미지 렌더링 오류:', e);
      drawEnemyFallback(ctx, enemy);
    }
  } else {
    // 이미지 미로드 시 기본 삼각형 렌더링
    drawEnemyFallback(ctx, enemy);
  }

  // 강화 적: 체력 바 표시 (이미지/삼각형 위에 표시)
  if (enemy.type === 'strong' && enemy.maxHp > 1) {
    const barWidth = enemy.width - 4;
    const barHeight = 4;
    const barX = enemy.x + 2;
    const barY = enemy.y - 8;

    // 체력 바 배경 (어두운 회색)
    ctx.fillStyle = 'rgb(50, 50, 50)';
    ctx.fillRect(barX, barY, barWidth, barHeight);

    // 체력 바 전경 (체력 비율에 따라 색상 변경)
    const hpRatio = enemy.hp / enemy.maxHp;
    if (hpRatio > 0.6) {
      ctx.fillStyle = 'rgb(0, 255, 0)'; // 초록색
    } else if (hpRatio > 0.3) {
      ctx.fillStyle = 'rgb(255, 200, 0)'; // 황색
    } else {
      ctx.fillStyle = 'rgb(255, 50, 50)'; // 적색
    }

    const currentBarWidth = hpRatio * barWidth;
    ctx.fillRect(barX, barY, currentBarWidth, barHeight);
  }

  ctx.restore();
}

/**
 * 적 기본 렌더링 (역삼각형)
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Enemy} enemy - 적 객체
 */
function drawEnemyFallback(ctx, enemy) {
  const centerX = enemy.x + enemy.width / 2;

  // 타입에 따른 색상 설정
  if (enemy.type === 'basic') {
    ctx.fillStyle = 'rgb(255, 50, 50)'; // 적색
    ctx.strokeStyle = 'rgb(180, 0, 0)';
  } else if (enemy.type === 'strong') {
    ctx.fillStyle = 'rgb(255, 140, 0)'; // 주황색
    ctx.strokeStyle = 'rgb(200, 80, 0)';
  }

  // 역삼각형 그리기 (아래를 향하는 삼각형)
  ctx.beginPath();
  ctx.moveTo(centerX, enemy.y + enemy.height); // 하단 꼭지점
  ctx.lineTo(enemy.x, enemy.y); // 좌상단
  ctx.lineTo(enemy.x + enemy.width, enemy.y); // 우상단
  ctx.closePath();
  ctx.fill();

  // 테두리
  ctx.lineWidth = 2;
  ctx.stroke();
}

/**
 * 총알 렌더링 (플레이어: 황색, 적: 적색)
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Bullet} bullet - 총알 객체
 */
function drawBullet(ctx, bullet) {
  ctx.save();

  if (bullet.owner === 'player') {
    // 플레이어 총알: 황색, 길쭉한 모양
    ctx.fillStyle = 'rgb(255, 200, 0)';
    ctx.fillRect(bullet.x, bullet.y, bullet.width, bullet.height);

    // 테두리 (밝은 황색)
    ctx.strokeStyle = 'rgb(255, 220, 50)';
    ctx.lineWidth = 1;
    ctx.strokeRect(bullet.x, bullet.y, bullet.width, bullet.height);

    // 발광 효과 (그라데이션)
    const gradient = ctx.createRadialGradient(
      bullet.x + bullet.width / 2,
      bullet.y + bullet.height / 2,
      0,
      bullet.x + bullet.width / 2,
      bullet.y + bullet.height / 2,
      bullet.width
    );
    gradient.addColorStop(0, 'rgba(255, 255, 200, 0.8)');
    gradient.addColorStop(1, 'rgba(255, 200, 0, 0)');

    ctx.fillStyle = gradient;
    ctx.fillRect(bullet.x - 2, bullet.y - 2, bullet.width + 4, bullet.height + 4);
  } else if (bullet.owner === 'enemy') {
    // 적 총알: 적색, 정사각형
    ctx.fillStyle = 'rgb(255, 50, 50)';
    ctx.fillRect(bullet.x, bullet.y, bullet.width, bullet.height);

    // 테두리 (진한 적색)
    ctx.strokeStyle = 'rgb(180, 0, 0)';
    ctx.lineWidth = 1;
    ctx.strokeRect(bullet.x, bullet.y, bullet.width, bullet.height);
  }

  ctx.restore();
}

/**
 * 파티클 렌더링
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {Particle} particle - 파티클 객체
 */
function drawParticle(ctx, particle) {
  particle.render(ctx);
}

// ============================================
// 화면 렌더링
// ============================================

/**
 * 시작 화면 렌더링
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {HTMLCanvasElement} canvas - 캔버스 요소
 * @param {number} highScore - 최고 점수
 */
function drawStartScreen(ctx, canvas, highScore) {
  const centerX = canvas.width / 2;
  const centerY = canvas.height / 2;

  // 배경 그라데이션
  const gradient = ctx.createLinearGradient(0, 0, 0, canvas.height);
  gradient.addColorStop(0, 'rgb(10, 10, 40)');
  gradient.addColorStop(1, 'rgb(30, 10, 50)');
  ctx.fillStyle = gradient;
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  // 별 배경
  drawBackground(ctx, canvas, 0.016);

  // 제목: "Sky Defender"
  ctx.save();
  ctx.shadowColor = 'rgba(100, 150, 255, 0.8)';
  ctx.shadowBlur = 20;
  ctx.fillStyle = 'white';
  ctx.font = 'bold 56px Arial';
  ctx.textAlign = 'center';
  ctx.fillText('Sky Defender', centerX, centerY - 100);
  ctx.restore();

  // 부제목
  ctx.fillStyle = 'rgb(200, 200, 200)';
  ctx.font = '22px Arial';
  ctx.fillText('횡스크롤 슈팅 게임', centerX, centerY - 60);

  // 하이스코어 표시
  if (highScore > 0) {
    ctx.fillStyle = 'rgb(255, 200, 0)';
    ctx.font = 'bold 26px Arial';
    ctx.fillText(`최고 점수: ${highScore}`, centerX, centerY - 10);
  }

  // 조작법 안내
  ctx.fillStyle = 'rgb(150, 150, 150)';
  ctx.font = '18px Arial';
  ctx.fillText('조작법', centerX, centerY + 40);

  ctx.font = '16px Arial';
  ctx.fillText('이동: A / D 또는 ← / →', centerX, centerY + 70);
  ctx.fillText('발사: SpaceBar 또는 마우스 클릭', centerX, centerY + 95);

  // 시작 버튼 (깜빡이는 효과)
  const blinkPhase = Math.floor(Date.now() / 500) % 2;

  ctx.fillStyle = 'rgb(100, 150, 255)';
  ctx.fillRect(centerX - 120, centerY + 130, 240, 50);
  ctx.strokeStyle = 'white';
  ctx.lineWidth = 2;
  ctx.strokeRect(centerX - 120, centerY + 130, 240, 50);

  ctx.fillStyle = 'white';
  ctx.font = 'bold 24px Arial';
  ctx.fillText('게임 시작', centerX, centerY + 162);

  // 깜빡이는 안내 문구
  if (blinkPhase === 0) {
    ctx.fillStyle = 'rgb(255, 200, 0)';
    ctx.font = 'bold 20px Arial';
    ctx.fillText('SpaceBar 또는 클릭으로 시작', centerX, centerY + 210);
  }
}

/**
 * 게임 오버 화면 렌더링
 * @param {CanvasRenderingContext2D} ctx - 캔버스 컨텍스트
 * @param {HTMLCanvasElement} canvas - 캔버스 요소
 * @param {Game} game - 게임 객체
 * @param {Array<number>} highScores - 하이스코어 배열
 */
function drawGameOverScreen(ctx, canvas, game, highScores) {
  const centerX = canvas.width / 2;
  const centerY = canvas.height / 2;

  // 어두운 배경 (반투명)
  ctx.fillStyle = 'rgba(0, 0, 0, 0.7)';
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  // "GAME OVER" 타이틀
  ctx.save();
  ctx.shadowColor = 'rgba(255, 0, 0, 0.8)';
  ctx.shadowBlur = 30;
  ctx.fillStyle = 'rgb(255, 50, 50)';
  ctx.font = 'bold 64px Arial';
  ctx.textAlign = 'center';
  ctx.fillText('GAME OVER', centerX, centerY - 120);
  ctx.restore();

  // 최종 점수
  ctx.fillStyle = 'white';
  ctx.font = 'bold 32px Arial';
  ctx.fillText(`최종 점수: ${game.score}`, centerX, centerY - 50);

  // 현재 점수의 순위 계산
  let currentRank = highScores.findIndex(score => score === game.score) + 1;
  if (currentRank === 0) {
    // 새로운 점수인 경우 순위 계산
    currentRank = highScores.filter(score => score > game.score).length + 1;
  }

  // 순위 표시 (상위 5위 안에 들었을 경우)
  if (currentRank <= 5) {
    ctx.fillStyle = 'rgb(255, 200, 0)';
    ctx.font = 'bold 24px Arial';
    ctx.fillText(`🏆 ${currentRank}위 달성! 🏆`, centerX, centerY - 10);
  }

  // 상위 5 하이스코어 표시
  ctx.fillStyle = 'rgb(255, 200, 0)';
  ctx.font = 'bold 22px Arial';
  ctx.fillText('최고 기록 (Top 5)', centerX, centerY + 30);

  ctx.font = '20px Arial';
  for (let i = 0; i < Math.min(5, highScores.length); i++) {
    const rank = i + 1;
    const score = highScores[i];

    // 현재 점수 강조
    if (score === game.score && rank === currentRank) {
      ctx.fillStyle = 'rgb(255, 255, 100)';
      ctx.font = 'bold 22px Arial';
    } else {
      ctx.fillStyle = 'white';
      ctx.font = '20px Arial';
    }

    ctx.fillText(`${rank}. ${score}점`, centerX, centerY + 65 + i * 30);
  }

  // 버튼들
  const buttonY = centerY + 220;

  // [다시 시작] 버튼
  ctx.fillStyle = 'rgb(100, 150, 255)';
  ctx.fillRect(centerX - 180, buttonY, 150, 50);
  ctx.strokeStyle = 'white';
  ctx.lineWidth = 2;
  ctx.strokeRect(centerX - 180, buttonY, 150, 50);

  ctx.fillStyle = 'white';
  ctx.font = 'bold 20px Arial';
  ctx.fillText('다시 시작', centerX - 105, buttonY + 32);

  // [메뉴로] 버튼
  ctx.fillStyle = 'rgb(80, 80, 80)';
  ctx.fillRect(centerX + 30, buttonY, 150, 50);
  ctx.strokeStyle = 'white';
  ctx.lineWidth = 2;
  ctx.strokeRect(centerX + 30, buttonY, 150, 50);

  ctx.fillStyle = 'white';
  ctx.fillText('메뉴로', centerX + 105, buttonY + 32);

  // 깜빡이는 안내 문구
  const blinkPhase = Math.floor(Date.now() / 500) % 2;
  if (blinkPhase === 0) {
    ctx.fillStyle = 'rgb(255, 200, 0)';
    ctx.font = 'bold 18px Arial';
    ctx.fillText('SpaceBar 또는 클릭으로 재시작', centerX, buttonY + 80);
  }
}
