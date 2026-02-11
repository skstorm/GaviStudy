import './globals.css';
import type { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'AI 선택형 일기장',
  description: 'AI의 질문에 답하며 만드는 나만의 일기장',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="ko">
      <body>
        <main>{children}</main>
      </body>
    </html>
  );
}
