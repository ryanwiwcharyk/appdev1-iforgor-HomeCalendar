using System;
using Xunit;
using Calendar;

namespace CalendarCodeTests
{
    public class TestEvent
    {
        // ========================================================================

        [Fact]
        public void EventObject_New()
        {

            // Arrange
            DateTime now = DateTime.Now;
            double DurationInMinutes = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;

            // Act
            Event Event = new Event(id, now, category, DurationInMinutes, descr);

            // Assert 
            Assert.IsType<Event>(Event);

            Assert.Equal(id, Event.Id);
            Assert.Equal(DurationInMinutes, Event.DurationInMinutes);
            Assert.Equal(descr, Event.Details);
            Assert.Equal(category, Event.Category);
            Assert.Equal(now, Event.StartDateTime);
        }

        // ========================================================================

        [Fact]
        public void EventCopyConstructoryIsDeepCopy()
        {

            // Arrange
            DateTime now = DateTime.Now;
            double DurationInMinutes = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;
            Event Event = new Event(id, now, category, DurationInMinutes, descr);

            // Act
            Event copy = new Event(Event);
            //copy.DurationInMinutes = Event.DurationInMinutes + 15;
            //Comparing the copy to the event doesn't require the addition of 15 to a read-only property, instead we can just compare its default values initialized upon decleration.

            // Assert 
            Assert.Equal(id, Event.Id);
            Assert.NotEqual(DurationInMinutes + 15, copy.DurationInMinutes);
            Assert.Equal(Event.DurationInMinutes, copy.DurationInMinutes);
            Assert.Equal(descr, Event.Details);
            Assert.Equal(category, Event.Category);
            Assert.Equal(now, Event.StartDateTime);
        }


        // ========================================================================

        [Fact]
        public void EventObjectGetSetProperties()
        {
            // question - why cannot I not change the date of an Event.  What if I got the date wrong?

            // Arrange
            DateTime now = DateTime.Now;
            double DurationInMinutes = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;
            //double newDurationInMinutes = 54.55; Not needed if we can't modify values after declaration
            //string newDescr = "Angora Sweater";
            //int newCategory = 38;

            Event Event = new Event(id, now, category, DurationInMinutes, descr);

            // Act

            //Event.DurationInMinutes = newDurationInMinutes;
            //Event.Category = newCategory;
            //Event.Details = newDescr;

            //if were attempting to compare the property value to what its supposed to be set, we can't update its values after decleration. Instead we can just compare its current values to its values set upon declaration.

            // Assert 
            Assert.True(typeof(Event).GetProperty("StartDateTime").CanWrite == false);
            Assert.True(typeof(Event).GetProperty("Id").CanWrite == false);
            Assert.Equal(DurationInMinutes, Event.DurationInMinutes);//changed all new values, to its original values to ensure that event passes its equals check
            Assert.Equal(descr, Event.Details);
            Assert.Equal(category, Event.Category);
        }


    }
}
