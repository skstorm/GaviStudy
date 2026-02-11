export interface TodoItem {
  id: string;
  text: string;
  isCompleted: boolean;
  createdAt: Date;
}

export const createTodoItem = (text: string): TodoItem => {
  return {
    id: crypto.randomUUID(),
    text: text.trim(),
    isCompleted: false,
    createdAt: new Date(),
  };
};

export interface SerializedTodoItem {
  id: string;
  text: string;
  isCompleted: boolean;
  createdAt: string;
}
