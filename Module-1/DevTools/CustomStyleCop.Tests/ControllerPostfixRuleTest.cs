using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StyleCop;

namespace CustomStyleCop.Tests
{
    public class ControllerPostfixRuleTest
    {
        private readonly string CONTROLLER_NAME_TEST_PATH = @"./ControllerTest/NameTest/";

        private StyleCopConsole scConsole;
        private List<Violation> violationList;

        [SetUp]
        public void Setup()
        {
            scConsole = new StyleCopConsole(null, true, null, null, true, null);
            violationList = new List<Violation>();
            scConsole.ViolationEncountered += ScConsoleOnViolationEncountered;
            scConsole.OutputGenerated += ScConsoleOnOutputGenerated;
        }

        [TearDown]
        public void Destroy()
        {
            scConsole.ViolationEncountered -= ScConsoleOnViolationEncountered;
            scConsole.OutputGenerated -= ScConsoleOnOutputGenerated;
            scConsole = null;
        }

        private void ScConsoleOnOutputGenerated(object sender, OutputEventArgs e)
        {
            Console.WriteLine(e.Output);
        }

        private void ScConsoleOnViolationEncountered(object sender, ViolationEventArgs e)
        {
            violationList.Add(e.Violation);
        }

        [Test]
        public void RightControllerName()
        {
            var project = new CodeProject(1, CONTROLLER_NAME_TEST_PATH, new Configuration(new [] { "DEBUG" }));
            scConsole.Core.Environment.AddSourceCode(project,
                Path.Combine(CONTROLLER_NAME_TEST_PATH, "RightController.cs"), null);

            scConsole.Start(new List<CodeProject>() {project}, true);

            Assert.AreEqual(0, violationList.Count);
        }

        [Test]
        public void WrongControllerName()
        {
            var project = new CodeProject(1, CONTROLLER_NAME_TEST_PATH, new Configuration(new[] { "DEBUG" }));
            scConsole.Core.Environment.AddSourceCode(project,
                Path.Combine(CONTROLLER_NAME_TEST_PATH, "WrongController.cs"), null);

            scConsole.Start(new List<CodeProject>() { project }, true);

            Assert.AreEqual(1, violationList.Count);
            var violation = violationList.First();
            Assert.AreEqual("ACR1000", violation.Rule.CheckId);
        }
    }
}