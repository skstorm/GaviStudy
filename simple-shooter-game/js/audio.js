// 오디오 관리 클래스 (v1.0에서는 로직만 구현, 실제 음성 없음)
class Audio {
  constructor() {
    // 음량 (0.0 ~ 1.0)
    this.volume = 0.7;

    // 음성 로그 활성화 여부
    this.enableLogging = true;
  }

  // 사운드 재생
  play(soundName) {
    if (this.enableLogging) {
      // 콘솔에 로그 출력 (실제 음성 재생 대신)
      switch (soundName) {
        case 'shoot':
          console.log('🔊 발사음 재생 (Volume: ' + this.volume + ')');
          break;
        case 'explosion':
          console.log('💥 폭발음 재생 (Volume: ' + this.volume + ')');
          break;
        case 'hit':
          console.log('⚡ 타격음 재생 (Volume: ' + this.volume + ')');
          break;
        case 'damage':
          console.log('💔 피격음 재생 (Volume: ' + this.volume + ')');
          break;
        case 'crash':
          console.log('💢 충돌음 재생 (Volume: ' + this.volume + ')');
          break;
        default:
          console.log('🔊 ' + soundName + ' 재생 (Volume: ' + this.volume + ')');
      }
    }

    // v1.0에서는 실제 오디오 API를 사용하지 않음
    // 추후 버전에서는 Web Audio API 또는 HTML5 Audio 태그 사용 가능
  }

  // 음량 설정
  setVolume(volume) {
    // 0.0 ~ 1.0 범위로 제한
    this.volume = Math.max(0, Math.min(1, volume));
    console.log('🔊 음량 설정: ' + Math.round(this.volume * 100) + '%');
  }

  // 음량 반환
  getVolume() {
    return this.volume;
  }

  // 로깅 활성화/비활성화
  setLogging(enabled) {
    this.enableLogging = enabled;
  }
}
