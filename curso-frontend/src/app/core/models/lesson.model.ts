export interface Lesson {
    id: string;
    courseId: string;
    title: string;
    order: number;
    isDeleted: boolean;
    createdAt: Date;
    updatedAt: Date;
}

export interface LessonCreate {
    courseId: string;
    title: string;
    order: number;
}

export interface LessonUpdate {
    title: string;
    order: number;
}

export interface ReorderLesson {
    lessonId: string;
    newOrder: number;
}