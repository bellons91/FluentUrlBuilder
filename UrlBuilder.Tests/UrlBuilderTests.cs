using FluentAssertions;
using NUnit.Framework;

namespace UrlBuilder
{
    public class UrlBuilderTests
    {

        [Test]
        public void Constructor_Should_InitializeCorrectly_When_InitialUrlIsSingleAndClean()
        {
            //Arrange
            var baseUrl = "https://www.code4it.dev";

            //Act
            var builder = UrlBuilder.Initialize(baseUrl);

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
        public string Constructor_Should_InitializeCorrectly_When_InitialUrlIsMultiple(params string[] varie)
        {
            //Act
            var builder = UrlBuilder.Initialize(varie);

            //Assert
            return builder.GetResult();
        }


        [Test]
        [TestCase("https://www.code4it.dev", "blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "/blog", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev", "/blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/", "blog/", ExpectedResult = "https://www.code4it.dev/blog")]
        public string AddPart_Should_CorrectlyAddPart(string baseUrl, string part)
        {
            //Arrange
            var builder = UrlBuilder.Initialize(baseUrl);

            //Act
            builder.AddPart(part);
            //Assert
            return builder.GetResult();
        }

    }
}