export enum CourseStatus {
    Draft = 'Draft',
    Published = 'Published'
}

export interface Course {
    id: string;
    title: string;
    description?: string;
    resourceUrl?: string;
    status: CourseStatus;
    isDeleted: boolean;
    createdAt: Date;
    updatedAt: Date;
}

export interface CourseCreate {
    title: string;
    description?: string;
    resourceUrl?: string;
}

export interface CourseUpdate {
    title: string;
    description?: string;
    resourceUrl?: string;
}

export interface CourseSummary {
    id: string;
    title: string;
    description?: string;
    resourceUrl?: string;
    status: CourseStatus;
    totalLessons: number;
    lastModified: Date;
}

export interface CourseSearchParams {
    q?: string;
    status?: CourseStatus;
    page?: number;
    pageSize?: number;
}

export interface PaginatedResponse<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
}