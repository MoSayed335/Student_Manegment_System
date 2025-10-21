namespace Student_Management_System
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Course> Courses { get; set; }

        public Student(int id, string name, int age)
        {
            StudentId = id;
            Name = name;
            Age = age;
            Courses = new List<Course>();
        }
        public bool Enroll(Course course)
        {
            if (course == null)
                return false;
            if (Courses.Any(c => c.CourseId == course.CourseId))
                return false;
            Courses.Add(course);
            return true;
        }

        public string PrintDetails()
        {
            String courseList = Courses.Count == 0 ? "No courses" :
                string.Join(", ", Courses.Select(c => $"{c.Title} (ID:{c.CourseId})"));
            return $"StudentId: {StudentId} | Name: {Name} | Age: {Age} | Courses: {courseList}";
        }
    }

    public class Instructor
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public Instructor(int id, string name, string specialization)
        {
            InstructorId = id;
            Name = name;
            Specialization = specialization;
        }

        public string PrintDetails()
        {
            return $"InstructorId: {InstructorId} | Name: {Name} | Specialization: {Specialization}";
        }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public Instructor Instructor { get; set; }

        public Course(int id, string title, Instructor instructor)
        {
            CourseId = id;
            Title = title;
            Instructor = instructor;
        }

        public string PrintDetails()
        {
            var instr = Instructor != null ? $"{Instructor.Name} (ID:{Instructor.InstructorId})" : "No Instructor";
            return $"CourseId: {CourseId} | Title: {Title} | Instructor: {instr}";
        }
    }

    public class StudentMangment
    {
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<Instructor> Instructors { get; set; }

        public StudentMangment()
        {
            Students = new List<Student>();
            Courses = new List<Course>();
            Instructors = new List<Instructor>();
        }

        public bool AddStudent(Student student)
        {
            if (student == null)
                return false;
            if (Students.Any(s => s.StudentId == student.StudentId))
                return false;
            Students.Add(student);
            return true;
        }

        public bool AddCourse(Course course)
        {
            if (course == null) return false;
            if (Courses.Any(c => c.CourseId == course.CourseId))
                return false;
            if (Courses.Any(c => c.Title == course.Title))
                return false;
            Courses.Add(course);
            return true;
        }

        public bool AddInstructor(Instructor instructor)
        {
            if (instructor == null)
                return false;
            if (Instructors.Any(i => i.InstructorId == instructor.InstructorId))
                return false;
            Instructors.Add(instructor);
            return true;
        }

        public Student FindStudent(int studentId)
        {
            return Students.FirstOrDefault(s => s.StudentId == studentId);
        }

        public Course FindCourse(int courseId)
        {
            return Courses.FirstOrDefault(c => c.CourseId == courseId);
        }

        public Instructor FindInstructor(int instructorId)
        {
            return Instructors.FirstOrDefault(i => i.InstructorId == instructorId);
        }

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = FindStudent(studentId);
            var course = FindCourse(courseId);
            if (student == null || course == null)
                return false;
            return student.Enroll(course);
        }

        public bool IsStudentEnrolledInCourse(int studentId, int courseId)
        {
            var student = FindStudent(studentId);
            if (student == null)
                return false;
            return student.Courses.Any(c => c.CourseId == courseId);
        }

        public string GetInstructorNameByCourseName(string courseName)
        {
            var course = Courses.FirstOrDefault(c => c.Title.Equals(courseName, StringComparison.OrdinalIgnoreCase));
            if (course == null) return null;
            return course.Instructor?.Name;
        }

        public List<Student> FindStudentsByName(string namePart)
        {
            return Students.Where(s => s.Name.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        public List<Course> FindCoursesByName(string namePart)
        {
            return Courses.Where(c => c.Title.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var manager = new StudentMangment();
            SeedSampleData(manager);

            while (true)
            {
                ShowMenu();
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();
                Console.WriteLine();
                if (!int.TryParse(choice, out int option))
                {
                    Console.WriteLine("Invalid input, try again. ");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        AddStudentFlow(manager);
                        break;
                    case 2:
                        AddInstructorFlow(manager);
                        break;
                    case 3:
                        AddCourseFlow(manager);
                        break;
                    case 4:
                        EnrollStudentFlow(manager);
                        break;
                    case 5:
                        ShowAllStudents(manager);
                        break;
                    case 6:
                        ShowAllCourses(manager);
                        break;
                    case 7:
                        ShowAllInstructors(manager);
                        break;
                    case 8:
                        FindStudentByIdOrName(manager);
                        break;
                    case 9:
                        FindCourseByIdOrName(manager);
                        break;
                    case 10:
                        CheckStudentEnrollmentFlow(manager);
                        break;
                    case 11:
                        GetInstructorByCourseNameFlow(manager);
                        break;
                    case 12:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exit. Bye!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.WriteLine("Option not found. Try again. ");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("=== Student Management System ===");
            Console.WriteLine("1. Add student");
            Console.WriteLine("2. Add instructor");
            Console.WriteLine("3. Add course");
            Console.WriteLine("4. Enroll student in course");
            Console.WriteLine("5. Show all students");
            Console.WriteLine("6. Show all courses");
            Console.WriteLine("7. Show all instructors");
            Console.WriteLine("8. Find student by ID or name");
            Console.WriteLine("9. Find course by ID or name");
            Console.WriteLine("10. Is student enrolled in a specific course?");
            Console.WriteLine("11. Get instructor by course name");
            Console.WriteLine("12. Exit");
            Console.WriteLine("=================================");
            Console.ResetColor();
        }

        #region Flows (UI helpers)
        static void AddStudentFlow(StudentMangment manager)
        {
            Console.Write("Enter Student ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID. ");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter Student name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name. ");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter student age: ");

            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid age. ");
                Console.ResetColor();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            var student = new Student(id, name, age);
            if (manager.AddStudent(student)) Console.WriteLine("Student added successfully.\n");
            else Console.WriteLine("Add failed — maybe a student with the same ID already exists.\n");
            Console.ResetColor();
        }

        static void AddInstructorFlow(StudentMangment manager)
        {
            Console.Write("Enter Instructor ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID. ");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter instructor name: ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name. ");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter instructor specialization: ");
            var spec = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(spec))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid specialization. ");
                Console.ResetColor();
                return;
            }

            var instr = new Instructor(id, name, spec);
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (manager.AddInstructor(instr))
                Console.WriteLine("Instructor added successfully.");
            else
                Console.WriteLine("Add failed — maybe an instructor with the same ID already exists. ");
            Console.ResetColor();
        }

        static void AddCourseFlow(StudentMangment manager)
        {

            Console.Write("Enter Course ID (number): ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID.");
                Console.ResetColor();

                return;
            }

            Console.Write("Enter course title: ");
            string title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid title. ");
                Console.ResetColor();
                return;
            }


            Console.Write("Enter Instructor ID for the course (or leave empty for none): ");
            var instrInput = Console.ReadLine();
            Instructor instructor = null;
            if (!string.IsNullOrWhiteSpace(instrInput))
            {
                if (!int.TryParse(instrInput, out int instrId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Instructor ID.");
                    Console.ResetColor();
                    return;
                }
                instructor = manager.FindInstructor(instrId);
                if (instructor == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Instructor not found with that ID.");
                    Console.ResetColor();
                    return;
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            var course = new Course(id, title, instructor);
            if (manager.AddCourse(course))
            {
                Console.WriteLine("Course added successfully.");
                Console.ResetColor();
            }


            else
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Add failed — maybe a course with the same ID already exists.");
            Console.ResetColor();
        }

        static void EnrollStudentFlow(StudentMangment manager)
        {
            Console.Write("Enter Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int sid))
            {
                Console.WriteLine("Invalid ID. ");
                return;
            }

            Console.Write("Enter Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int cid))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ID. ");
                return;
                Console.ResetColor();

            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            var success = manager.EnrollStudentInCourse(sid, cid);
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Student enrolled in course successfully. ");
                Console.ResetColor();

            }
            else if (!success)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enrollment failed — make sure the student and course exist and the student is not already enrolled.");
                Console.ResetColor();
            }
        }

        static void ShowAllStudents(StudentMangment manager)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=== All Students ===");
            if (manager.Students.Count == 0) Console.WriteLine("No students found. ");
            else
            {
                foreach (var s in manager.Students)
                    Console.WriteLine(s.PrintDetails());
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static void ShowAllCourses(StudentMangment manager)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=== All Courses ===");
            if (manager.Courses.Count == 0)
                Console.WriteLine("No courses found. ");
            else
            {
                foreach (var c in manager.Courses) Console.WriteLine(c.PrintDetails());
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static void ShowAllInstructors(StudentMangment manager)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=== All Instructors ===");
            if (manager.Instructors.Count == 0) Console.WriteLine("No instructors found.");
            else
            {
                foreach (var i in manager.Instructors) Console.WriteLine(i.PrintDetails());
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static void FindStudentByIdOrName(StudentMangment manager)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Search by ID or part of name: ");
            var q = Console.ReadLine();
            if (int.TryParse(q, out int id))
            {
                var s = manager.FindStudent(id);
                if (s != null) Console.WriteLine(s.PrintDetails());
                else Console.WriteLine("No student found with that ID. ");
            }
            else
            {
                var list = manager.FindStudentsByName(q);
                if (list.Count == 0) Console.WriteLine("No students found with that name.");
                else
                {
                    foreach (var s in list) Console.WriteLine(s.PrintDetails());
                    Console.WriteLine();
                }
            }
            Console.ResetColor();
        }

        static void FindCourseByIdOrName(StudentMangment manager)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.Write("Search by ID or part of course title: ");
            var q = Console.ReadLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (int.TryParse(q, out int id))
            {
                var c = manager.FindCourse(id);
                if (c != null)
                    Console.WriteLine(c.PrintDetails());

                else
                    Console.WriteLine("No course found with that ID. ");

            }

            else
            {
                var list = manager.FindCoursesByName(q);
                if (list.Count == 0) Console.WriteLine("No courses found with that name. ");
                else
                {
                    foreach (var c in list) Console.WriteLine(c.PrintDetails());
                    Console.WriteLine();
                }
            }
        }

        static void CheckStudentEnrollmentFlow(StudentMangment manager)
        {
            Console.Write("Enter Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int sid))
            {
                Console.WriteLine("Invalid ID. ");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Enter Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int cid))
            {
                Console.WriteLine("Invalid ID. ");
                return;
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            var enrolled = manager.IsStudentEnrolledInCourse(sid, cid);
            Console.WriteLine(enrolled ? "Yes — the student is enrolled in the course. " : "No — the student is not enrolled in the course. ");
            Console.ResetColor();
        }

        static void GetInstructorByCourseNameFlow(StudentMangment manager)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter course name (full or part): ");
            var name = Console.ReadLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            var matched = manager.FindCoursesByName(name);
            if (matched.Count == 0)
            {
                Console.WriteLine("No course found with that name. ");
                return;
            }
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var c in matched)
            {
                var instrName = c.Instructor?.Name ?? "No instructor assigned for this course";
                Console.WriteLine($"Course: {c.Title} (ID:{c.CourseId}) => Instructor: {instrName}");
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        #endregion

        static void SeedSampleData(StudentMangment manager)
        {
            var ins1 = new Instructor(1, "Dr. Ahmed", "Computer Science");
            var ins2 = new Instructor(2, "Dr. Mona", "Mathematics");
            manager.AddInstructor(ins1);
            manager.AddInstructor(ins2);

            var c1 = new Course(101, "Programming 101", ins1);
            var c2 = new Course(102, "Linear Algebra", ins2);
            manager.AddCourse(c1);
            manager.AddCourse(c2);

            var s1 = new Student(1001, "Mohamed Ali", 20);
            var s2 = new Student(1002, "Sara Mahmoud", 22);
            manager.AddStudent(s1);
            manager.AddStudent(s2);

            s1.Enroll(c1);
        }
    }
}
