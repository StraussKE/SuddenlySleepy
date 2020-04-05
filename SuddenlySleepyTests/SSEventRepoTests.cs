using System;
using System.Linq;
using Xunit;
using SuddenlySleepy.Models;
using SuddenlySleepy.Repositories;

namespace SuddenlySleepyTests
{
    public class SSEventRepoTests
    {
        [Fact]
        public void TestDirectEventCreation()
        {
            // Arrange
            var testSSEventId = new Guid();
            var testMeetingDate = DateTime.Now;
            var testLocation = "Here";
            var testSSEventName = "My Test Event";
            var testDescription = "This is where I am describing my event.";
            
            // Act
            var testSSEvent = new SSEvent
            {
                SSEventId = testSSEventId,
                MeetingDate= testMeetingDate,
                Location = testLocation,
                SSEventName = testSSEventName,
                Description = testDescription
            };

            // Assert
            Assert.NotNull(testSSEvent); // checks that the SSEvent was created
            Assert.Equal("Here", testSSEvent.Location);
            Assert.Equal("My Test Event", testSSEvent.SSEventName);
        }

        [Fact]
        public void TestDirectSSEventRepoAddEvent()
        {
            // Arrange
            var testSSEvent1 = new SSEvent
            {
                SSEventId = new Guid(),
                MeetingDate = DateTime.Now,
                Location = "Here",
                SSEventName = "My Test Event",
                Description = "This is where I am describing my test event"
            };
            var testSSEvent2 = new SSEvent
            {
                SSEventId = new Guid(),
                MeetingDate = DateTime.Now,
                Location = "There",
                SSEventName = "My Test Event",
                Description = "This is where I am describing my test event"
            };

            var testSSEventRepo = new FakeSSEventRepo();

            // Act
            testSSEventRepo.AddSSEvent(testSSEvent1);
            testSSEventRepo.AddSSEvent(testSSEvent2);

            // Assert
            Assert.Equal(2, testSSEventRepo.SSEvents.Count());
            Assert.Equal("There", testSSEventRepo.SSEvents.ToList().Last().Location);
        }

        [Fact]
        public void TestQueryable()
        {
            // Arrange
            var testSSEventRepo = new FakeSSEventRepo();

            testSSEventRepo.AddSSEvent( new SSEvent
            {
                SSEventId = new Guid(),
                MeetingDate = DateTime.Now,
                Location = "Here",
                SSEventName = "My Test Event",
                Description = "This is where I am describing my test event"
            });
            testSSEventRepo.AddSSEvent( new SSEvent
            {
                SSEventId = new Guid(),
                MeetingDate = DateTime.Now,
                Location = "There",
                SSEventName = "My Test Event",
                Description = "This is where I am describing my test event"
            });

            // Act
            var countTestUniqueLocation = testSSEventRepo.SSEvents.Where(e => e.Location == "Here").ToList().Count();
            var countTestLikeEventName = testSSEventRepo.SSEvents.Where(e => e.SSEventName == "My Test Event").ToList().Count();

            // Assert
            Assert.Equal(1, countTestUniqueLocation);
            Assert.Equal(2, countTestLikeEventName);
        }
    }
}
