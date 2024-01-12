//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//


using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microsoft.SqlTools.ServiceLayer.IntegrationTests.Scripting
{
    public class AsyncScriptAsScriptingOperationTests
    {
        public static IEnumerable<TestCaseData> ScriptAsTestCases
        {
            get
            {
                yield return new TestCaseData(
                   @"
                        CREATE TABLE selectTable (c1 int)
                   ",
                    new ScriptingParams()
                    {
                        ScriptDestination = "ToEditor",
                        ScriptingObjects = new List<ScriptingObject>()
                        {
                            new ScriptingObject()
                            {
                                Name = "selectTable",
                                Schema = "dbo",
                                Type = "Table",
                            }
                        },
                        Operation = ScriptingOperationType.Select,
                        ScriptOptions = new ScriptOptions()
                        {
                            ScriptCreateDrop = "ScriptSelect"
                        }
                    },
                    new List<string>() { "SELECT TOP (1000) [c1]" });

                yield return new TestCaseData(
                    @"
                    CREATE TABLE dropTable (c1 int)
                    ",
                    new ScriptingParams()
                    {
                        ScriptDestination = "ToEditor",
                        ScriptingObjects = new List<ScriptingObject>()
                        {
                            new ScriptingObject()
                            {
                                Name = "dropTable",
                                Schema = "dbo",
                                Type = "Table"
                            }
                        },
                        Operation = ScriptingOperationType.Delete,
                        ScriptOptions = new ScriptOptions()
                        {
                            ScriptCreateDrop = "ScriptDrop"
                        }
                    },
                    new List<string> { "DROP TABLE [dbo].[dropTable]" }
                    );

                yield return new TestCaseData(
                    "CREATE TABLE createTable (c1 int)",
                    new ScriptingParams()
                    {
                        ScriptDestination = "ToEditor",
                        ScriptingObjects = new List<ScriptingObject>()
                        {
                            new ScriptingObject()
                            {
                                Name = "createTable",
                                Schema = "dbo",
                                Type = "Table"
                            }
                        },
                        Operation = ScriptingOperationType.Create,
                        ScriptOptions = new ScriptOptions()
                        {
                            ScriptCreateDrop = "ScriptCreate"
                        }
                    },
                    new List<string> { "CREATE TABLE [dbo].[createTable]" }
                );

                yield return new TestCaseData(
                    @"
                    CREATE TABLE testTable1 (c1 int)
                    GO
                    CREATE INDEX idx ON testTable1 (c1)
                    GO
                    ",
                    new ScriptingParams()
                    {
                        ScriptDestination = "ToEditor",
                        ScriptingObjects = new List<ScriptingObject>()
                        {
                            new ScriptingObject()
                            {
                                Name = "idx",
                                Schema = "dbo",
                                Type = "Index",
                                ParentName = "testTable1",
                                ParentTypeName = "Table",
                            }
                        },
                        Operation = ScriptingOperationType.Create,
                        ScriptOptions = new ScriptOptions()
                        {
                            ScriptCreateDrop = "ScriptCreate"
                        }
                    },
                    new List<string> { "CREATE NONCLUSTERED INDEX [idx] ON [dbo].[testTable1]" }
                );
            }
        }

        [Test]
        [TestCaseSource("ScriptAsTestCases")]
        public async Task TestCommonScenarios(
            string query, ScriptingParams scriptingParams, List<string> expectedScriptContents)
        {
            var testDb = await SqlTestDb.CreateNewAsync(TestServerType.OnPrem, false, null, query, "ScriptingTests");
            scriptingParams.ConnectionString = testDb.ConnectionString;

            var actualScript = await AsyncScriptAsScriptingOperation.GetScriptAsScript(scriptingParams);

            foreach (var expectedStr in expectedScriptContents)
            {
                Assert.That(actualScript, Does.Contain(expectedStr));
            }

            await testDb.CleanupAsync();
        }
    }
}