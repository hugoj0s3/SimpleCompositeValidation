using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using SimpleCompositeValidation.Validations.String.Urls;
using Xunit;

namespace SimpleCompositeValidation.UnitTests.Validations.Urls
{
    public class UrlValidationTests
    {
        [Theory]
        [InlineData("https://www.google.com")]
        [InlineData("http://facebook.com")]
        [InlineData("http://www.test.com")]
        [InlineData("http://www.test123.com")]
        public void Update_PassingValidsUrl_TheFailureListRemainEmpty(string url)
        {
            // Arrange
            var target = new UrlValidation("Test");

            // Act
            target.Update(url);

            // Arrange
            target.Failures.ShouldBeEmpty();
        }


        [Theory]
        [InlineData("https://www.google.com/maps/asdasdasd/", "www.google.com")]
        [InlineData("http://facebook.com/profile/asdasdasd/", "facebook.com")]
        [InlineData("http://www.test.com/test/asdasd/aewe?asdf", "www.test.com")]
        [InlineData("http://www.test123.com/asdasd/asdqewqe", "www.test123.com")]
        public void Update2_PassingValidsUrl_TheFailureListRemainEmpty(string url, string host)
        {
            // Arrange
            var target = new HostUrlValidation("Test", host);

            // Act
            target.Update(url);

            // Arrange
            target.Failures.ShouldBeEmpty();
        }
    }
}
