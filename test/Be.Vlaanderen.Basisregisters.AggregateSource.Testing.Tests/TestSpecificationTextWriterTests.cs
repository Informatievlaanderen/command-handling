namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class TestSpecificationTextWriterTests : TestSpecificationDataPointFixture
    {
        [Test]
        public void TextWriterCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestSpecificationTextWriter(null));
        }

        [TestCaseSource("ExceptionCentricTestSpecificationCases")]
        public void WriteExceptionCentricTestSpecificationResultsInExpectedOutput(
            ExceptionCentricTestSpecification specification, string result)
        {
            using (var writer = new StringWriter())
            {
                var sut = new TestSpecificationTextWriter(writer);

                sut.Write(specification);

                Assert.That(writer.ToString(), Is.EqualTo(result));
            }
        }

        public static IEnumerable<TestCaseData> ExceptionCentricTestSpecificationCases
        {
            get
            {
                yield return new TestCaseData(
                    new ExceptionCentricTestSpecification(OneEvent, Message, Exception),
                    string.Format("Given{0}  System.Object{0}When{0}  System.Object{0}Throws{0}  [Exception] Message{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new ExceptionCentricTestSpecification(TwoEventsOfTheSameSource, Message, Exception),
                    string.Format("Given{0}  System.Object,{0}  System.Object{0}When{0}  System.Object{0}Throws{0}  [Exception] Message{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new ExceptionCentricTestSpecification(NoEvents, Message, Exception),
                    string.Format("When{0}  System.Object{0}Throws{0}  [Exception] Message{0}", Environment.NewLine)
                    );
            }
        }

        [TestCaseSource("EventCentricTestSpecificationCases")]
        public void WriteEventCentricTestSpecificationResultsInExpectedOutput(
            EventCentricTestSpecification specification, string result)
        {
            using (var writer = new StringWriter())
            {
                var sut = new TestSpecificationTextWriter(writer);

                sut.Write(specification);

                Assert.That(writer.ToString(), Is.EqualTo(result));
            }
        }

        public static IEnumerable<TestCaseData> EventCentricTestSpecificationCases
        {
            get
            {
                yield return new TestCaseData(
                    new EventCentricTestSpecification(NoEvents, Message, NoEvents),
                    string.Format("When{0}  System.Object{0}Then{0}  nothing happened{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new EventCentricTestSpecification(OneEvent, Message, NoEvents),
                    string.Format("Given{0}  System.Object{0}When{0}  System.Object{0}Then{0}  nothing happened{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new EventCentricTestSpecification(TwoEventsOfTheSameSource, Message, NoEvents),
                    string.Format("Given{0}  System.Object,{0}  System.Object{0}When{0}  System.Object{0}Then{0}  nothing happened{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new EventCentricTestSpecification(NoEvents, Message, OneEvent),
                    string.Format("When{0}  System.Object{0}Then{0}  System.Object{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new EventCentricTestSpecification(NoEvents, Message, TwoEventsOfTheSameSource),
                    string.Format("When{0}  System.Object{0}Then{0}  System.Object,{0}  System.Object{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new EventCentricTestSpecification(OneEvent, Message, OneEvent),
                    string.Format("Given{0}  System.Object{0}When{0}  System.Object{0}Then{0}  System.Object{0}", Environment.NewLine)
                    );

                yield return new TestCaseData(
                    new EventCentricTestSpecification(TwoEventsOfTheSameSource, Message, TwoEventsOfTheSameSource),
                    string.Format("Given{0}  System.Object,{0}  System.Object{0}When{0}  System.Object{0}Then{0}  System.Object,{0}  System.Object{0}", Environment.NewLine)
                    );
            }
        }
    }
}
