using FluentAssertions;
using NUnit.Framework;
using System;

namespace FluentUrlBuilder
{
    public class FluentUrlBuilderTests
    {

        [Test]
        public void Initialize_Should_InitializeCorrectly_When_InitialUrlIsSingleAndClean()
        {
            //Arrange
            var baseUrl = "https://www.code4it.dev";

            //Act
            var builder = FluentUrlBuilder.Initialize(baseUrl);

            //Assert
            builder.GetResult().Should().Be(baseUrl);
        }


        [Test]
        [TestCase("https://www.code4it.dev", "blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "/blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "/blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "blog", "an-article", ExpectedResult = "https://www.code4it.dev/blog/an-article")]
        [TestCase("https://www.code4it.dev/", "", ExpectedResult = "https://www.code4it.dev")]
        [TestCase("https://www.code4it.dev/", "   ", ExpectedResult = "https://www.code4it.dev")]
        public string Initialize_Should_InitializeCorrectly_When_InitialUrlIsMultiple(params string[] varie)
        {
            //Act
            var builder = FluentUrlBuilder.Initialize(varie);

            //Assert
            return builder.GetResult();
        }


        [Test]
        [TestCase("", "")]
        [TestCase((string)null, "")]
        [TestCase("/", "")]
        [TestCase(" /  /", "")]
        [TestCase(" /hello/", "hello")]
        [TestCase(" hi/hello/", "hi/hello")]
        public void TrimString_Should_TrimCorrectly(string initial, string expected)
        {
            //Act
            var actualString = FluentUrlBuilder.TrimString(initial);

            //Assert
            Assert.AreEqual(expected, actualString);
        }

        [Test]
        [TestCase((string)null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("   ", (string)null)]
        [TestCase("   /  ", "")]

        public void Initialize_Should_ThowException_When_InputIsNotValid(params string[] parts)
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => FluentUrlBuilder.Initialize(parts));
        }


        [Test]
        [TestCase("https://www.code4it.dev", "blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "/blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "/blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        public string AddPathPart_Should_CorrectlyAddPart(string baseUrl, string part)
        {
            //Arrange
            var builder = FluentUrlBuilder.Initialize(baseUrl);

            //Act
            builder.AddPathPart(part);
            //Assert
            return builder.GetResult();
        }

    }
}