'use client';

import { useState, useEffect } from 'react';
import styles from './DiaryApp.module.css';

interface Question {
    id: number;
    question: string;
    options: string[];
    done?: boolean;
}

interface Answer {
    questionId: number;
    answer: string;
}

export default function DiaryApp() {
    const [currentQuestion, setCurrentQuestion] = useState<Question | null>(null);
    const [answers, setAnswers] = useState<Answer[]>([]);
    const [diary, setDiary] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        fetchNextQuestion([]);
    }, []);

    const fetchNextQuestion = async (currentAnswers: Answer[]) => {
        setLoading(true);
        try {
            const res = await fetch('/api/diary', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ type: 'get_question', answers: currentAnswers }),
            });
            const data = await res.json();
            if (data.done) {
                generateDiary(currentAnswers);
            } else {
                setCurrentQuestion(data);
            }
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const handleOptionClick = (option: string) => {
        if (!currentQuestion) return;
        const newAnswer = { questionId: currentQuestion.id, answer: option };
        const newAnswers = [...answers, newAnswer];
        setAnswers(newAnswers);
        fetchNextQuestion(newAnswers);
    };

    const generateDiary = async (finalAnswers: Answer[]) => {
        setLoading(true);
        try {
            const res = await fetch('/api/diary', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ type: 'generate_diary', answers: finalAnswers }),
            });
            const data = await res.json();
            setDiary(data.diary);
            setCurrentQuestion(null);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const reset = () => {
        setAnswers([]);
        setDiary(null);
        fetchNextQuestion([]);
    };

    if (loading) return <div className={styles.loading}>AI가 생각 중입니다...</div>;

    if (diary) {
        return (
            <div className={styles.diaryResult}>
                <h3 className={styles.diaryTitle}>📖 오늘의 일기</h3>
                <p className={styles.diaryText}>{diary}</p>
                <button className="primary-btn" onClick={reset} style={{ marginTop: '2rem', width: '100%' }}>다시 쓰기</button>
            </div>
        );
    }

    if (!currentQuestion) return null;

    return (
        <div className={styles.questionContainer}>
            <h2 className={styles.questionTitle}>{currentQuestion.question}</h2>
            <div className={styles.optionsGrid}>
                {currentQuestion.options.map((option) => (
                    <button
                        key={option}
                        className={styles.optionBtn}
                        onClick={() => handleOptionClick(option)}
                    >
                        {option}
                    </button>
                ))}
            </div>
        </div>
    );
}
