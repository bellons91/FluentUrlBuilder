using FluentAssertions;
using NUnit.Framework;
using System;

namespace FluentUrlBuilder
{
    public class FluentUrlBuilderTests
    {

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

        #region Initialize

        [Test]
        public void Initialize_Should_InitializeCorrectly_When_InitialUrlIsSingleAndClean()
        {
            //Arrange
            var baseUrl = "https://www.code4it.dev";

            //Act
            var builder = FluentUrlBuilder.Initialize(baseUrl);

            //Assert
            builder.GetAsString().Should().Be(baseUrl);
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
            return builder.GetAsString();
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

        #endregion

        #region AddPart
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
            return builder.GetAsString();
        }

        #endregion

        #region SetHashFragment
        [Test]
        [TestCase("https://www.code4it.dev/blog", "fragment", ExpectedResult = "https://www.code4it.dev/blog#fragment")]
        public string SetHashFragment_Should_InsertFragmentIfValid(string baseUrl, string fragment)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.SetHashFragment(fragment);
            return builder.GetAsString();
        }


        [Test]
        [TestCase("https://www.code4it.dev/blog", "", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/blog", (string)null, ExpectedResult = "https://www.code4it.dev/blog")]
        public string SetHashFragment_Should_NotInsertFragmentIfInvalid(string baseUrl, string fragment)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.SetHashFragment(fragment);
            return builder.GetAsString();
        }

        [Test]
        [TestCase("https://www.code4it.dev/blog", "fragment", "other-fragment", ExpectedResult = "https://www.code4it.dev/blog#other-fragment")]
        [TestCase("https://www.code4it.dev/blog", "fragment", "",ExpectedResult = "https://www.code4it.dev/blog")]
        public string SetHashFragment_Should_OverwriteFragmentIfValid(string baseUrl, string fragment, string secondFragment)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.SetHashFragment(fragment);
            builder.SetHashFragment(secondFragment);
            return builder.GetAsString();
        }

        #endregion

        #region Query String
        [Test]
        [TestCase("https://www.code4it.dev/blog", "key", "val", ExpectedResult = "https://www.code4it.dev/blog?key=val")]
        [TestCase("https://www.code4it.dev/blog", "", "val", ExpectedResult = "https://www.code4it.dev/blog")]
        public string UpsertQueryString_Should_InsertValue_When_KeyIsValid(string baseUrl, string key, string value)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.UpsertQueryStringPair(key, value);
            return builder.GetAsString();
        }

       
        [Test]
        [TestCase("https://www.code4it.dev/blog", "key", "val", "val2", ExpectedResult = "https://www.code4it.dev/blog?key=val2")]
        [TestCase("https://www.code4it.dev/blog", "", "val", "val2", ExpectedResult = "https://www.code4it.dev/blog")]
        public string UpsertQueryString_Should_ReplaceValue_When_KeyIsValid(string baseUrl, string key, string value, string secondValue)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.UpsertQueryStringPair(key, value);
            builder.UpsertQueryStringPair(key, secondValue);
            return builder.GetAsString();
        }


        [Test]
        [TestCase("https://www.code4it.dev/blog", "key", "val", "key2", "val2", ExpectedResult = "https://www.code4it.dev/blog?key=val&key2=val2")]
        [TestCase("https://www.code4it.dev/blog", "", "val", "key2", "val2", ExpectedResult = "https://www.code4it.dev/blog?key2=val2")]
        public string UpsertQueryString_Should_AddMultipleKeys(string baseUrl, string key, string value, string secondKey, string secondValue)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.UpsertQueryStringPair(key, value);
            builder.UpsertQueryStringPair(secondKey, secondValue);
            return builder.GetAsString();
        }


        [Test]
        [TestCase("https://www.code4it.dev/blog", "key", "val", "key2", "val2", "key", ExpectedResult = "https://www.code4it.dev/blog?key2=val2")]
        [TestCase("https://www.code4it.dev/blog", "key", "val", "key2", "val2", "key2", ExpectedResult = "https://www.code4it.dev/blog?key=val")]

        [TestCase("https://www.code4it.dev/blog", "", "val", "key2", "val2", "key2", ExpectedResult = "https://www.code4it.dev/blog")]
        [TestCase("https://www.code4it.dev/blog", "", "val", "key2", "val2", "not-existing", ExpectedResult = "https://www.code4it.dev/blog?key2=val2")]
        public string RemoveQueryString_Should_RemovePair(string baseUrl, string key, string value, string secondKey, string secondValue, string keyToRemove)
        {
            var builder = FluentUrlBuilder.Initialize(baseUrl);
            builder.UpsertQueryStringPair(key, value);
            builder.UpsertQueryStringPair(secondKey, secondValue);
            builder.RemoveQueryStringPair(keyToRemove);
            return builder.GetAsString();
        }

        #endregion

        #region ReturnAsUri
        [Test]
        public void RemoveQueryString_Should_RemovePair()
        {
            var stringUrl = "https://www.code4it.dev";
            (string q,string  s) = ("key", "val");
            var expectedUri = new Uri("https://www.code4it.dev?key=val");
            var builder = FluentUrlBuilder.Initialize(stringUrl);
            builder.UpsertQueryStringPair(q, s);
            var actual = builder.GetAsUri();

            Assert.AreEqual(expectedUri, actual);
        }
        #endregion

    }
}