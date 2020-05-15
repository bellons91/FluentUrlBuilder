using FluentAssertions;
using NUnit.Framework;

namespace UrlBuilder
{
    public class UrlBuilderTests

    {

        [Test]
        public void UrlBuilder_Should_InitializeCorrectly_When_InitialUrlIsSingleAndClean()
        {
            //Arrange
            var baseUrl = "https://www.code4it.dev";

            //Act
            var builder = UrlBuilder.Initialize(baseUrl);

            //Assert
            builder.GetResult().Should().Be(baseUrl);

        }
    }
}