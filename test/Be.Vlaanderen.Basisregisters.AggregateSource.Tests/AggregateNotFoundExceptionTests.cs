#pragma warning disable SYSLIB0011
namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class AggregateNotFoundExceptionTests
    {
        private static readonly string AggregateIdentifier = Guid.NewGuid().ToString();
        private static readonly Type AggregateType = typeof (object);

        [Test]
        public void IsAnAggregateSourceException()
        {
            Assert.That(new AggregateNotFoundException(AggregateIdentifier, AggregateType),
                        Is.InstanceOf<AggregateSourceException>());
        }

        [Test]
        public void UsingTheDefaultContstructorReturnsExceptionWithExpectedProperties()
        {
            var sut = new AggregateNotFoundException(AggregateIdentifier, AggregateType);

            Assert.That(sut.Identifier, Is.EqualTo(AggregateIdentifier));
            Assert.That(sut.ClrType, Is.EqualTo(AggregateType));
            Assert.That(sut.Message, Is.EqualTo(
                "The Object aggregate with identifier " + AggregateIdentifier +
                " could not be found. Please make sure the call site is indeed passing in an identifier for an Object aggregate."));
            Assert.That(sut.InnerException, Is.Null);
        }

        [Test]
        public void UsingTheDefaultContstructorAggregateIdentifierCannotBeNull()
        {
            Assert.That(
                Assert.Throws<ArgumentNullException>(() => new AggregateNotFoundException(null, AggregateType))
                      .ParamName,
                Is.EqualTo("identifier"));
        }

        [Test]
        public void UsingTheDefaultContstructorAggregateTypeCannotBeNull()
        {
            Assert.That(
                Assert.Throws<ArgumentNullException>(() => new AggregateNotFoundException(AggregateIdentifier, null))
                      .ParamName,
                Is.EqualTo("clrType"));
        }

        [Test]
        public void UsingTheConstructorWithMessageReturnsExceptionWithExpectedProperties()
        {
            var sut = new AggregateNotFoundException(AggregateIdentifier, AggregateType, "Message");

            Assert.That(sut.Identifier, Is.EqualTo(AggregateIdentifier));
            Assert.That(sut.ClrType, Is.EqualTo(AggregateType));
            Assert.That(sut.Message, Is.EqualTo("Message"));
            Assert.That(sut.InnerException, Is.Null);
        }

        [Test]
        public void UsingTheConstructorWithMessageAggregateIdentifierCannotBeNull()
        {
            Assert.That(
                Assert.Throws<ArgumentNullException>(
                    () => new AggregateNotFoundException(null, AggregateType, "Message"))
                      .ParamName,
                Is.EqualTo("identifier"));
        }

        [Test]
        public void UsingTheConstructorWithMessageAggregateTypeCannotBeNull()
        {
            Assert.That(
                Assert.Throws<ArgumentNullException>(
                    () => new AggregateNotFoundException(AggregateIdentifier, null, "Message"))
                      .ParamName,
                Is.EqualTo("clrType"));
        }

        [Test]
        public void UsingTheConstructorWithMessageAndInnerExceptionReturnsExceptionWithExpectedProperties()
        {
            var innerException = new Exception();
            var sut = new AggregateNotFoundException(AggregateIdentifier, AggregateType, "Message", innerException);

            Assert.That(sut.Identifier, Is.EqualTo(AggregateIdentifier));
            Assert.That(sut.ClrType, Is.EqualTo(AggregateType));
            Assert.That(sut.Message, Is.EqualTo("Message"));
            Assert.That(sut.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        public void UsingTheConstructorWithMessageAndInnerExceptionAggregateIdentifierCannotBeNull()
        {
            Assert.That(
                Assert.Throws<ArgumentNullException>(
                    () => new AggregateNotFoundException(null, AggregateType, "Message", new Exception())).ParamName,
                Is.EqualTo("identifier"));
        }

        [Test]
        public void UsingTheConstructorWithMessageAndInnerExceptionAggregateTypeCannotBeNull()
        {
            Assert.That(
                Assert.Throws<ArgumentNullException>(
                    () => new AggregateNotFoundException(AggregateIdentifier, null, "Message", new Exception()))
                      .ParamName,
                Is.EqualTo("clrType"));
        }

        [Test]
        public void CanBeSerialized()
        {
            var innerException = new Exception("InnerMessage");
            var sut = new AggregateNotFoundException(AggregateIdentifier, AggregateType, "Message", innerException);

            using (var stream = new MemoryStream())
            {
                // Write the properties of the AggregateSourceException object to the stream
                using (var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
                {
                    writer.Write(sut.Identifier);
                    writer.Write(sut.ClrType.AssemblyQualifiedName);
                    writer.Write(sut.Message);
                    writer.Write(sut.InnerException.Message);
                }

                stream.Position = 0;

                // Read the properties of the AggregateSourceException object from the stream
                using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true))
                {
                    var identifier = reader.ReadString();
                    var clrType = Type.GetType(reader.ReadString());
                    var message = reader.ReadString();
                    var innerMessage = reader.ReadString();
                    var result = new AggregateNotFoundException(identifier, clrType, message, new Exception(innerMessage));

                    Assert.That(result.Identifier, Is.EqualTo(AggregateIdentifier));
                    Assert.That(result.ClrType, Is.EqualTo(AggregateType));
                    Assert.That(result.Message, Is.EqualTo(result.Message));
                    Assert.That(result.InnerException.Message, Is.EqualTo(result.InnerException.Message));
                }
            }
        }

        //TODO: Test that type could not be resolved upon deserialization.
    }
}
