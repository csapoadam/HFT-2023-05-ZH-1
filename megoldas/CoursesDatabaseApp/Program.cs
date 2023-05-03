using System.Reflection;

namespace CoursesDatabaseApp
{

    [AttributeUsage(AttributeTargets.Property)]
    // a fenti sor nelkul barmire, pl. osztalyra vagy konstruktorra is
    // alkalmazni lehetne...
    public class NeptunAttribute : Attribute
    {}

    public interface IValidation
    {
        public bool Validate(object instance, PropertyInfo prop);
    }

    public class NeptunValidation: IValidation
    {
        private NeptunAttribute attr;

        public NeptunValidation(NeptunAttribute attr)
        {

            this.attr = attr;

        }

        public bool Validate(object instance, PropertyInfo prop) {
            if (prop.PropertyType == typeof(string))
            {
                var value = prop.GetValue(instance) as string;
                if (value != null)
                {
                    bool isLen6 = value.Length == 6;

                    bool isAllCapitalOrNumeric = true;
                    for (int i = 0; i < 6; i++)
                    {
                        if (!char.IsUpper(value[i]) && !char.IsDigit(value[i]))
                        {
                            isAllCapitalOrNumeric = false;
                        }
                    }

                    return isLen6 && isAllCapitalOrNumeric;
                }
            }
            throw new InvalidOperationException();
        }
    }

    public class Validator
    {
        public bool Validate(object instance)
        {
            foreach (var prop in instance.GetType().GetProperties())
            {
                foreach (var attr in prop.GetCustomAttributes(false))
                {
                    if (attr is NeptunAttribute)
                    {
                        NeptunValidation validation = new NeptunValidation((NeptunAttribute)attr);
                        if (!validation.Validate(instance, prop))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            CoursesDbContext db = new CoursesDbContext();

            var listOfCourses = db.InstructorCourses.Select(x => x.Course.Title).Distinct();
            Console.WriteLine("List of all courses: ");
            foreach (var course in listOfCourses)
            {
                Console.WriteLine("\t" + course);
            }

            var coursesTaughtByJaneDoe = db.InstructorCourses.Where(ic => ic.Instructor.Name.Equals("Jane Doe")).Select(ic => ic.Course.Title);

            Console.WriteLine("Courses taught by Jane Doe: ");
            foreach (var course in coursesTaughtByJaneDoe)
            {
                Console.WriteLine("\t" + course);
            }

            var teachersWithMoreThan2Courses = db.InstructorCourses.GroupBy(ic => ic.Instructor.Name).Select(t => new
            {
                name = t.Key,
                numTimes = t.Count()
            }).Where(t => t.numTimes > 2);
                

            Console.WriteLine("Instructors with more than 2 courses: ");
            foreach (var instructor in teachersWithMoreThan2Courses)
            {
                Console.WriteLine("\t" + instructor);
            }

            var validInstructor = new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABX21J" };
            var invalidInstructor1 = new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABx21J" };
            var invalidInstructor2 = new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABX21JQ" };

            Validator validator = new Validator();
            Console.WriteLine("validInstructor is valid: " + validator.Validate(validInstructor));
            Console.WriteLine("invalidInstructor1 is valid: " + validator.Validate(invalidInstructor1));
            Console.WriteLine("invalidInstructor2 is valid: " + validator.Validate(invalidInstructor2));
        }
    }
}