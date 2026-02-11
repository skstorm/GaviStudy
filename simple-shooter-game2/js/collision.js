/**
 * 충돌 감지 함수
 * AABB (Axis-Aligned Bounding Box) 방식으로 충돌을 감지합니다.
 * 플레이어 총알의 경우 충돌 범위를 확대합니다.
 *
 * @param {Object} rectA - 첫 번째 사각형 객체 (x, y, width, height 속성 필요)
 * @param {Object} rectB - 두 번째 사각형 객체 (x, y, width, height 속성 필요)
 * @returns {boolean} - 충돌 여부 (true: 충돌, false: 비충돌)
 */
function checkCollision(rectA, rectB) {
  // 플레이어 총알의 경우 충돌 범위를 3px 확대
  let padding = 0;
  if (rectA.owner === 'player' || rectB.owner === 'player') {
    padding = 3;
  }

  return (
    rectA.x - padding < rectB.x + rectB.width &&
    rectA.x + rectA.width + padding > rectB.x &&
    rectA.y - padding < rectB.y + rectB.height &&
    rectA.y + rectA.height + padding > rectB.y
  );
}
