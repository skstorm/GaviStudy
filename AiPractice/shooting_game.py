#!/usr/bin/env python3
"""
CLI 슈팅 게임 - Space Shooter
조작: A/D - 좌우 이동, SPACE - 발사, Q - 종료
"""

import os
import sys
import time
import random
import threading
from collections import deque

# Windows 호환성을 위한 설정
if os.name == 'nt':
    import msvcrt
else:
    import tty
    import termios

class Game:
    def __init__(self, width=40, height=20):
        self.width = width
        self.height = height
        self.player_pos = width // 2
        self.player_char = "▲"
        self.enemy_char = "▼"
        self.bullet_char = "|"
        self.score = 0
        self.game_over = False

        # 게임 오브젝트들
        self.bullets = []  # [(x, y), ...]
        self.enemies = []  # [(x, y), ...]

        # 입력 버퍼
        self.input_buffer = deque()
        self.lock = threading.Lock()

    def clear_screen(self):
        """화면 클리어"""
        os.system('cls' if os.name == 'nt' else 'clear')

    def get_key_windows(self):
        """Windows에서 키 입력 받기"""
        if msvcrt.kbhit():
            key = msvcrt.getch()
            if key == b'\xe0':  # 특수키
                key = msvcrt.getch()
            return key.decode('utf-8', errors='ignore').lower()
        return None

    def get_key_unix(self):
        """Unix/Linux/Mac에서 키 입력 받기"""
        fd = sys.stdin.fileno()
        old_settings = termios.tcgetattr(fd)
        try:
            tty.setraw(sys.stdin.fileno())
            ch = sys.stdin.read(1)
            return ch.lower()
        finally:
            termios.tcsetattr(fd, termios.TCSADRAIN, old_settings)

    def input_thread(self):
        """별도 스레드에서 키 입력 받기"""
        while not self.game_over:
            try:
                if os.name == 'nt':
                    key = self.get_key_windows()
                else:
                    key = self.get_key_unix()

                if key:
                    with self.lock:
                        self.input_buffer.append(key)
            except:
                pass
            time.sleep(0.01)

    def process_input(self):
        """입력 처리"""
        with self.lock:
            while self.input_buffer:
                key = self.input_buffer.popleft()

                if key == 'a' and self.player_pos > 0:
                    self.player_pos -= 1
                elif key == 'd' and self.player_pos < self.width - 1:
                    self.player_pos += 1
                elif key == ' ':
                    self.shoot()
                elif key == 'q':
                    self.game_over = True

    def shoot(self):
        """총알 발사"""
        self.bullets.append([self.player_pos, self.height - 2])

    def spawn_enemy(self):
        """적 생성"""
        if random.random() < 0.3:  # 30% 확률로 적 생성
            x = random.randint(0, self.width - 1)
            self.enemies.append([x, 0])

    def update_bullets(self):
        """총알 업데이트"""
        for bullet in self.bullets[:]:
            bullet[1] -= 1
            if bullet[1] < 0:
                self.bullets.remove(bullet)

    def update_enemies(self):
        """적 업데이트"""
        for enemy in self.enemies[:]:
            enemy[1] += 1
            if enemy[1] >= self.height:
                self.enemies.remove(enemy)
                self.game_over = True  # 적이 바닥에 도달하면 게임 오버

    def check_collisions(self):
        """충돌 체크"""
        for bullet in self.bullets[:]:
            for enemy in self.enemies[:]:
                if bullet[0] == enemy[0] and bullet[1] == enemy[1]:
                    # 충돌!
                    self.bullets.remove(bullet)
                    self.enemies.remove(enemy)
                    self.score += 10
                    break

    def render(self):
        """화면 렌더링"""
        self.clear_screen()

        # 게임 보드 생성
        board = [[' ' for _ in range(self.width)] for _ in range(self.height)]

        # 플레이어 그리기
        if self.player_pos < self.width:
            board[self.height - 1][self.player_pos] = self.player_char

        # 총알 그리기
        for x, y in self.bullets:
            if 0 <= y < self.height and 0 <= x < self.width:
                board[y][x] = self.bullet_char

        # 적 그리기
        for x, y in self.enemies:
            if 0 <= y < self.height and 0 <= x < self.width:
                board[y][x] = self.enemy_char

        # 상단 경계
        print("┌" + "─" * self.width + "┐")

        # 게임 보드 출력
        for row in board:
            print("│" + "".join(row) + "│")

        # 하단 경계
        print("└" + "─" * self.width + "┘")

        # 점수 및 정보 출력
        print(f"\n점수: {self.score}")
        print("조작: A/D - 좌우 이동, SPACE - 발사, Q - 종료")

    def run(self):
        """게임 메인 루프"""
        print("CLI 슈팅 게임을 시작합니다!")
        print("3초 후 시작...")
        time.sleep(3)

        # 입력 스레드 시작
        input_thread = threading.Thread(target=self.input_thread, daemon=True)
        input_thread.start()

        frame_count = 0

        try:
            while not self.game_over:
                # 입력 처리
                self.process_input()

                # 게임 로직 업데이트
                if frame_count % 2 == 0:  # 총알은 빠르게
                    self.update_bullets()

                if frame_count % 3 == 0:  # 적은 조금 느리게
                    self.update_enemies()

                if frame_count % 5 == 0:  # 적 생성
                    self.spawn_enemy()

                # 충돌 체크
                self.check_collisions()

                # 화면 렌더링
                self.render()

                # 프레임 카운트 증가
                frame_count += 1

                # FPS 제어 (약 30 FPS)
                time.sleep(0.033)

        except KeyboardInterrupt:
            pass
        finally:
            self.game_over = True
            self.clear_screen()
            print("\n게임 오버!")
            print(f"최종 점수: {self.score}")
            print("\n게임을 플레이해 주셔서 감사합니다!\n")

if __name__ == "__main__":
    game = Game(width=40, height=20)
    game.run()
