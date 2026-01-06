namespace CoursePlatform.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class CourseNotFoundException : DomainException
{
    public CourseNotFoundException(Guid id) 
        : base($"Course with ID {id} not found") { }
}

public class LessonNotFoundException : DomainException
{
    public LessonNotFoundException(Guid id) 
        : base($"Lesson with ID {id} not found") { }
}

public class CannotPublishCourseException : DomainException
{
    public CannotPublishCourseException() 
        : base("Cannot publish course without active lessons") { }
}

public class DuplicateLessonOrderException : DomainException
{
    public DuplicateLessonOrderException(int order) 
        : base($"A lesson with order {order} already exists in this course") { }
}