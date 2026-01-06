export interface Student {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    documentNumber?: string;
    age?: number;
    createdAt: string;
}

export interface StudentCreate {
    firstName: string;
    lastName: string;
    email: string;
    documentNumber?: string;
    age?: number;
}
