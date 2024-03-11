using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Calendar;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;

namespace CalendarCodeTests
{
    public class TestEvents
    {
        int numberOfEventsInFile = TestConstants.numberOfEventsInFile;
        String testInputFile = TestConstants.testEventsInputFile;
        int maxIDInEventFile = TestConstants.maxIDInEventFile;
        Event firstEventInFile = new Event(1, new DateTime(2021, 1, 10), 3, 40, "App Dev Homework");


        // ========================================================================

        [Fact]
        public void EventsObject_New()
        {
            // Arrange

            // Act
            Events Events = new Events();

            // Assert 
            Assert.IsType<Events>(Events);

            Assert.True(typeof(Events).GetProperty("FileName").CanWrite == false);
            Assert.True(typeof(Events).GetProperty("DirName").CanWrite == false);

        }


        // ========================================================================

        [Fact]
        public void EventsMethod_ReadFromFile_NotExist_ThrowsException()
        {
            // Arrange
            String badFile = "abc.txt";
            Events Events = new Events();

            // Act and Assert
            Assert.Throws<System.IO.FileNotFoundException>(() => Events.ReadFromFile(badFile));

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_ReadFromFile_ValidateCorrectDataWasRead()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Events Events = new Events();

            // Act
            Events.ReadFromFile(dir + "\\" + testInputFile);
            List<Event> list = Events.List();
            Event firstEvent = list[0];

            // Assert
            Assert.Equal(numberOfEventsInFile, list.Count);
            Assert.Equal(firstEventInFile.Id, firstEvent.Id);
            Assert.Equal(firstEventInFile.DurationInMinutes, firstEvent.DurationInMinutes);
            Assert.Equal(firstEventInFile.Details, firstEvent.Details);
            Assert.Equal(firstEventInFile.Category, firstEvent.Category);

            String fileDir = Path.GetFullPath(Path.Combine(Events.DirName, ".\\"));
            Assert.Equal(dir, fileDir);
            Assert.Equal(testInputFile, Events.FileName);

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_List_ReturnsListOfEvents()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);

            // Act
            List<Event> list = events.List();

            // Assert
            Assert.Equal(numberOfEventsInFile, list.Count);

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_List_ModifyListDoesNotModifyEventsInstance()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Events Events = new Events();
            Events.ReadFromFile(dir + "\\" + testInputFile);
            List<Event> list = Events.List();

            DateTime now = DateTime.Now;
            double DurationInMinutes = 28.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 2;

            // Act
            //list[0].DurationInMinutes = list[0].DurationInMinutes + 21.03;
            //Instead of changing properties of an existing event(Because its properties are read-only), I can add a new event and compare the new index.
            Event newEvent = new Event(id, now, category, DurationInMinutes, descr);
            list.Add(newEvent);
            

            // Assert
            Assert.NotEqual(list.Count, Events.List().Count); //comparing the lengths of both lists after adding an event to list

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_Add()
        {

            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);

            DateTime date = DateTime.Now;
            int category = 2;
            double duration = 50;
            string details = "New event";

            // Act
            events.Add(date,category,duration,details);
            List<Event> eventList = events.List();
            int sizeOfList = events.List().Count;

            Assert.Equal(details, eventList[sizeOfList - 1].Details);
            Assert.Equal(numberOfEventsInFile + 1, sizeOfList);
            Assert.Equal(maxIDInEventFile + 1, eventList[sizeOfList - 1].Id);
            Assert.Equal(duration, eventList[sizeOfList - 1].DurationInMinutes);

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_Delete()
        {

            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);

            // Arrange
            int IdToDelete = 3;

            // Act
            events.Delete(IdToDelete);
            List<Event> EventsList = events.List();
            int sizeOfList = EventsList.Count;

            // Assert
            Assert.Equal(numberOfEventsInFile - 1, sizeOfList);
            Assert.False(EventsList.Exists(e => e.Id == IdToDelete), "correct Event item deleted");

        }

        // ========================================================================

        [Fact]
        public void EventsMethod_Delete_InvalidIDDoesntCrash()
        {
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);
    

            // Arrange
            int IdToDelete = 1006;
            List<Event> EventsList = events.List();
            int sizeOfList = EventsList.Count;

            // Act
            try
            {
                events.Delete(IdToDelete);
                Assert.Equal(sizeOfList, events.List().Count);
            }

            // Assert
            catch
            {
                Assert.True(false, "Invalid ID causes Delete to break");
            }
        }


        // ========================================================================

        [Fact]
        public void EventMethod_WriteToFile()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Events Events = new Events();
            Events.ReadFromFile(dir + "\\" + testInputFile);
            string fileName = TestConstants.EventOutputTestFile;
            String outputFile = dir + "\\" + fileName;
            File.Delete(outputFile);

            // Act
            Events.SaveToFile(outputFile);

            // Assert
            Assert.True(File.Exists(outputFile), "output file created");
            Assert.True(TestConstants.FileEquals(dir + "\\" + testInputFile, outputFile), "Input /output files are the same");
            String fileDir = Path.GetFullPath(Path.Combine(Events.DirName, ".\\"));
            Assert.Equal(dir, fileDir);
            Assert.Equal(fileName, Events.FileName);

            // Cleanup
            if (TestConstants.FileEquals(dir + "\\" + testInputFile, outputFile))
            {
                File.Delete(outputFile);
            }

        }

        // ========================================================================

        [Fact]
        public void EventMethod_WriteToFile_VerifyNewEventWrittenCorrectly()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Events Events = new Events();
            Events.ReadFromFile(dir + "\\" + testInputFile);
            string fileName = TestConstants.EventOutputTestFile;
            String outputFile = dir + "\\" + fileName;
            File.Delete(outputFile);

            // Act
            Events.Add(DateTime.Now, 14, 35.27, "McDonalds");
            List<Event> listBeforeSaving = Events.List();
            Events.SaveToFile(outputFile);
            Events.ReadFromFile(outputFile);
            List<Event> listAfterSaving = Events.List();

            Event beforeSaving = listBeforeSaving[listBeforeSaving.Count - 1];
            Event afterSaving = listAfterSaving.Find(e => e.Id == beforeSaving.Id);

            // Assert
            Assert.Equal(beforeSaving.Id, afterSaving.Id);
            Assert.Equal(beforeSaving.Category, afterSaving.Category);
            Assert.Equal(beforeSaving.Details, afterSaving.Details);
            Assert.Equal(beforeSaving.DurationInMinutes, afterSaving.DurationInMinutes);

        }

        // ========================================================================

        [Fact]
        public void EventMethod_WriteToFile_WriteToLastFileWrittenToByDefault()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Events Events = new Events();
            Events.ReadFromFile(dir + "\\" + testInputFile);
            string fileName = TestConstants.EventOutputTestFile;
            String outputFile = dir + "\\" + fileName;
            File.Delete(outputFile);
            Events.SaveToFile(outputFile); // output file is now last file that was written to.
            File.Delete(outputFile);  // Delete the file

            // Act
            Events.SaveToFile(); // should write to same file as before

            // Assert
            Assert.True(File.Exists(outputFile), "output file created");
            String fileDir = Path.GetFullPath(Path.Combine(Events.DirName, ".\\"));
            Assert.Equal(dir, fileDir);
            Assert.Equal(fileName, Events.FileName);

            // Cleanup
            if (TestConstants.FileEquals(dir + "\\" + testInputFile, outputFile))
            {
                File.Delete(outputFile);
            }

        }
    }
}

