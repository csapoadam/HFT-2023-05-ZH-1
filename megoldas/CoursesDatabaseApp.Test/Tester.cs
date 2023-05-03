using NUnit.Framework;

namespace CoursesDatabaseApp.Test
{
    [TestFixture]
    public class Tester
    {
        [Test]
        public void NeptunFormatTest1()
        {
            var validInstructor = new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABX21J" };

            Validator validator = new Validator();

            Assert.That(validator.Validate(validInstructor), Is.True);
        }

        [Test]
        public void NeptunFormatTest2()
        {
            var invalidInstructor1 = new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABx21J" };

            Validator validator = new Validator();

            Assert.That(validator.Validate(invalidInstructor1), Is.False);
        }

        [Test]
        public void NeptunFormatTest3()
        {
            var invalidInstructor2 = new Instructor { Id = 1, Name = "Jane Doe", Neptun = "ABX21JQ" };

            Validator validator = new Validator();

            Assert.That(validator.Validate(invalidInstructor2), Is.False);
        }
    }
}