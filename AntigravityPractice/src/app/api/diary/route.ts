import { NextResponse } from 'next/server';

const QUESTIONS = [
    {
        id: 1,
        question: "오늘의 전체적인 기분은 어떠신가요?",
        options: ["기쁨 & 행복", "우울 & 피곤", "상쾌 & 의욕충만", "차분 & 평온"]
    },
    {
        id: 2,
        question: "오늘 아침은 무엇을 드셨나요?",
        options: ["든든한 한식", "가벼운 샐러드/과일", "커피와 빵", "바빠서 걸렀어요"]
    },
    {
        id: 3,
        question: "오늘 하루 중 가장 기억에 남는 순간은?",
        options: ["친구/가족과의 대화", "나만의 휴식 시간", "몰입했던 업무/공부", "뜻밖의 행운"]
    }
];

export async function POST(req: Request) {
    const { type, answers } = await req.json();

    if (type === 'get_question') {
        const nextIndex = answers.length;
        if (nextIndex < QUESTIONS.length) {
            return NextResponse.json(QUESTIONS[nextIndex]);
        } else {
            return NextResponse.json({ done: true });
        }
    }

    if (type === 'generate_diary') {
        // 시뮬레이션된 AI 일기 생성 로직
        const summary = answers.map((a: any) => a.answer).join(', ');
        const diaryContent = `오늘 하루는 ${answers[0].answer} 기분으로 시작했어요. 아침으로는 ${answers[1].answer}을(를) 먹으며 에너지를 채웠죠. 특히 ${answers[2].answer} 순간이 가장 기억에 남네요. 사소하지만 소중한 기록들이 모여 나의 하루를 완성한 것 같아 뿌듯한 날입니다.`;

        return NextResponse.json({ diary: diaryContent });
    }

    return NextResponse.json({ error: 'Invalid request' }, { status: 400 });
}
