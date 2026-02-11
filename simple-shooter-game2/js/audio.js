/**
 * Audio 클래스
 * 게임 사운드를 관리합니다.
 * v1.0: 콘솔 로그로 사운드 재생 표시
 * 추후 Web Audio API 또는 <audio> 태그로 구현 가능
 */
class Audio {
  constructor() {
    this.volume = 0.5; // 기본 음량: 50%
    this.enabled = true; // 사운드 활성화 여부

    // 사운드 이름 목록
    this.soundNames = ['shoot', 'explosion', 'hit', 'damage', 'crash'];
  }

  /**
   * 사운드 재생
   * @param {string} soundName - 사운드 이름 ('shoot', 'explosion', 'hit', 'damage', 'crash')
   */
  play(soundName) {
    if (!this.enabled) {
      return;
    }

    // v1.0: 콘솔 로그로 출력
    console.log(`[Audio] Playing sound: ${soundName} (volume: ${this.volume})`);

    // TODO: 추후 실제 사운드 재생 구현
    // 예시:
    // const audio = new Audio(`assets/sounds/${soundName}.mp3`);
    // audio.volume = this.volume;
    // audio.play();
  }

  /**
   * 음량 설정
   * @param {number} volume - 음량 (0.0 ~ 1.0)
   */
  setVolume(volume) {
    this.volume = Math.max(0, Math.min(1, volume)); // 0~1 범위로 제한
  }

  /**
   * 사운드 활성화/비활성화
   * @param {boolean} enabled - 활성화 여부
   */
  setEnabled(enabled) {
    this.enabled = enabled;
  }
}
