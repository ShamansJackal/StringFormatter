using StringFormatter.Core;

namespace StringFormatter.Tests
{
    public class UnitTest1
    {
        private readonly ClassForTests _target = new();

        [Fact]
        public void SimpleTest()
        {
            var expected = $"Hi {_target.Name} with id={_target.Id}";
            var actual = StringFormatter1.Shared.Format("Hi {Name} with id={Id}", _target);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DateTest()
        {
            var expected = $"Now {_target.Date} in Minsk";
            var actual = StringFormatter1.Shared.Format("Now {Date} in Minsk", _target);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void EscapeTest()
        {
            var expected = $"{{Name}}={_target.Name}";
            var actual = StringFormatter1.Shared.Format("{{Name}}={Name}", _target);

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void PropertyTest()
        {
            var expected = $"Hi {_target.Name} with password={_target.Password}";
            var actual = StringFormatter1.Shared.Format("Hi {Name} with password={Password}", _target);

            Assert.Equal(expected, actual);
        }
    }
}