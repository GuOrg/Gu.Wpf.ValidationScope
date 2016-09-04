namespace Gu.Wpf.ValidationScope.Tests
{
    using NUnit.Framework;

    public partial class ErrorCollectionTests
    {
        [Test]
        public void EmptyValidationErrors()
        {
            Assert.NotNull(ErrorCollection.EmptyValidationErrors);
            CollectionAssert.IsEmpty(ErrorCollection.EmptyValidationErrors);
        }
    }
}