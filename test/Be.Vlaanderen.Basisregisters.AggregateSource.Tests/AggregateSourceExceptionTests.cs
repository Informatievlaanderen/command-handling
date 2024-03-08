#pragma warning disable SYSLIB0011
namespace Be.Vlaanderen.Basisregisters.AggregateSource.Tests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class AggregateSourceExceptionTests
    {
        [Test]
        public void IsAnException()
        {
            Assert.That(new AggregateSourceException(), Is.InstanceOf<Exception>());
        }

        [Test]
        public void UsingTheDefaultConstructorReturnsExceptionWithExpectedProperties()
        {
            var sut = new AggregateSourceException();

            //Use of Contains.Substring due to differences in OS.
            Assert.That(sut.Message, Contains.Substring(typeof (AggregateSourceException).Name));
            Assert.That(sut.InnerException, Is.Null);
        }

        [Test]
        public void UsingTheConstructorWithMessageReturnsExceptionWithExpectedProperties()
        {
            var sut = new AggregateSourceException("Message");

            Assert.That(sut.Message, Is.EqualTo("Message"));
            Assert.That(sut.InnerException, Is.Null);
        }

        [Test]
        public void UsingTheConstructorWithMessageAndInnerExceptionReturnsExceptionWithExpectedProperties()
        {
            var innerException = new Exception();
            var sut = new AggregateSourceException("Message", innerException);

            Assert.That(sut.Message, Is.EqualTo("Message"));
            Assert.That(sut.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        public void CanBeSerialized()
        {
            var innerException = new Exception("InnerMessage");
            var sut = new AggregateSourceException("Message", innerException);

            using (var stream = new MemoryStream())
            {
                // Write the properties of the AggregateSourceException object to the stream
                using (var writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
                {
                    writer.Write(sut.Message);
                    writer.Write(sut.InnerException.Message);
                }

                stream.Position = 0;

                // Read the properties of the AggregateSourceException object from the stream
                using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true))
                {
                    var message = reader.ReadString();
                    var innerMessage = reader.ReadString();
                    var result = new AggregateSourceException(message, new Exception(innerMessage));

                    Assert.That(sut.Message, Is.EqualTo(result.Message));
                    Assert.That(sut.InnerException.Message, Is.EqualTo(result.InnerException.Message));
                }
            }
        }
    }
}
